using System;
using System.Collections.Generic;
using UnityEngine;

/*
    Суть системы боя такова
    У нас есть персонаж, которым мы управляем
    У персонажа есть определённые удары, магия и т.п.
    Игра идёт по ходам, у игрока есть ход с определённым количеством действий
    Каждое действие тратит очко действий
    Когда у игрока заканчиваются очки действий начинается ход противников
    Противники это стак персонажей друг над другом
    Активный противник всегда тот, который поверх всех
    Противники лежащие под ним могут использовать только второстепенные навыки прикрытия и поддержки
    А активный противник может делать всё, что угодно
 */
public class BattleController : MonoBehaviour
{
    public enum TurnStatus
    {
        PlayerTurn,
        EnemiesTurn
    }

    [Header("Кнопка ожидания")]
    public GameObject waitButton;

    [Header("Аниматор начала битвы")]
    public GameObject startBattleImage;
    [Header("Аниматор конца битвы")]
    public GameObject endBattleImage;


    // ======
    private static BattleController instance;
    public static BattleController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BattleController>();
            }
            return instance;
        }
    }


    public event Action<bool> onBattleStatusChanged;

    public event Action<TurnStatus> onBattleTurnStatusChanged;


    [SerializeField]
    private RuntimeBattlePlayerController m_runtimeBattlePlayerController;

    [SerializeField]
    private RuntimeBattleCharacter m_defaultRuntimeBattleCharacterPrefab;

    [SerializeField]
    private Transform m_enemiesSpawnArea;


    private List<RuntimeBattleCharacter> enemiesInBattle = new List<RuntimeBattleCharacter>();
    public List<RuntimeBattleCharacter> EnemiesInBattle => enemiesInBattle;


    private List<CharacterProfile> defeatedEnemiesInBattle = new List<CharacterProfile>();
    public List<CharacterProfile> DefeatedEnemiesInBattle => defeatedEnemiesInBattle;


    private bool isBattle;
    public bool IsBattle { get => isBattle; set => isBattle = value; }

    private bool isWin;
    public bool IsWin { get => isWin; set => isWin = value; }


    private TurnStatus m_currentTurnStatus;
    public TurnStatus CurrentTurnStatus => m_currentTurnStatus;


    private int currentBattleStep = 0;
    public int CurrentBattleStep => currentBattleStep;


    public void AddEnemyToBattle(CharacterProfile character)
    {
        RuntimeBattleCharacter createdEnemy;

        if (character.CharacterPrefab != null)
        {
            createdEnemy = Instantiate(character.CharacterPrefab, m_enemiesSpawnArea);
        }
        else
        {
            createdEnemy = Instantiate(m_defaultRuntimeBattleCharacterPrefab, m_enemiesSpawnArea);
        }

        createdEnemy.CreateBattleCharacter(character);
        enemiesInBattle.Add(createdEnemy);
    }


    public void StartPlayerTurn()
    {
        if (CheckEndBattleConditions()) return;

        if (!RuntimePlayer.Instance.RuntimePlayerStats.isStun)
        {
            UpdateAllEnemiesUI();

            m_currentTurnStatus = TurnStatus.PlayerTurn;
            onBattleTurnStatusChanged?.Invoke(m_currentTurnStatus);
        }
        else
        {
            GameController.Instance.AddEventText("Вы оглушены и не можете атаковать.");
            StartEnemiesTurn();
        }
    }

    public void EndPlayerTurn()
    {
        GameController.Instance.AddEventText("Вы закончили свой ход.");

        StartEnemiesTurn();
    }


    public void StartEnemiesTurn()
    {
        if (CheckEndBattleConditions()) return;

        m_currentTurnStatus = TurnStatus.EnemiesTurn;
        onBattleTurnStatusChanged?.Invoke(m_currentTurnStatus);

        for (int i = 0; i < enemiesInBattle.Count; i++)
        {
            enemiesInBattle[i].ProcessCharacterActions();
        }

        EndEnemiesTurn();
    }

    public void UpdateAllEnemiesUI()
    {
        foreach (RuntimeBattleCharacter runtimeBattleCharacter in enemiesInBattle)
        {
            runtimeBattleCharacter.UpdateCharacterView();
        }
    }

    // Фактически здесь конец хода, потому что и игрок сходил и все противники
    private void EndEnemiesTurn()
    {
        GameTimeFlowController.Instance.AddTime(1);

        currentBattleStep++;

        StartPlayerTurn();
    }


    public bool CheckEndBattleConditions()
    {
        // Здесь все проверки после каждого действия, чтобы понять умер противник или нет
        if (RuntimePlayer.Instance.RuntimePlayerStats.Health <= 0)
        {
            RuntimePlayer.Instance.RuntimePlayerStats.isDeath = true;

            isWin = false;
            EndBattle();
            GameController.Instance.ShowDeathBox("Вы умерли во время битвы.");

            return true;
        }
        

        for (int i = 0; i < enemiesInBattle.Count; i++)
        {
            if (enemiesInBattle[i].Health <= 0)
            {
                //Debug.Log("Противник: " + enemiesInBattle[i].characterProfile.Name + " - Повержен.");
                // Добавляем противника в побеждённых
                defeatedEnemiesInBattle.Add(enemiesInBattle[i].characterProfile);
                Destroy(enemiesInBattle[i].gameObject);
                // Удаляем противника из списка
                enemiesInBattle.RemoveAt(i);
            }
        }
        
        
        // Если противников не осталось, заканчиваем бой
        if (enemiesInBattle.Count == 0)
        {
            //Debug.Log("Противники закончились, мы выйграли!");
            isWin = true;
            GameController.Instance.Blocker.SetActive(true);
            endBattleImage.SetActive(true);
            EndBattle();
            return true;
        }

        return false;
    }


    public void TryStartBattle(List<CharacterProfile> characters)
    {
        if (isBattle) return;

        ClearBattleData();

        //FindObjectOfType<SavingManager>().SaveGame("AutoSave");

        foreach (CharacterProfile characterProfile in characters)
        {
            AddEnemyToBattle(characterProfile);
        }

        isBattle = true;
        isWin = false;
        currentBattleStep = 0;

        m_runtimeBattlePlayerController.InitializeBattlePlayer();

        FindObjectOfType<PanelsManager>().OpenHideActionPanel(0, true);

        onBattleStatusChanged?.Invoke(true);
        StartPlayerTurn();
    }

    public void EndBattle()
    {
        GameController.Instance.Blocker.SetActive(false);
        endBattleImage.SetActive(false);

        if (isWin)
        {
            WinCondition();
            SaveLoadSystemController.Instance.TrySaveGameState("AutoSave");
            //FindObjectOfType<SavingManager>().SaveGame("AutoSave");
        }

        isWin = false;
        isBattle = false;

        ClearBattleData();

        waitButton.SetActive(true);

        // Закрываем панели
        FindObjectOfType<PanelsManager>().CloseAllActionPanels();
    }

    // Выдача предметов за победу
    public void WinCondition()
    {
        RuntimePlayer.Instance.RuntimePlayerStats.isStun = false;

        // Деньги и опыт с каждого моба
        for (int i = 0; i < defeatedEnemiesInBattle.Count; i++)
        {
            RuntimePlayer.Instance.GiveMoney(defeatedEnemiesInBattle[i].Gold);
            RuntimePlayer.Instance.GiveExperience(defeatedEnemiesInBattle[i].Exp);
        }

        // Добавляем игроку индивидуальные вещи моба
        //for (int j = 0; j < defeatedEnemiesInBattle.Count; j++)
        //{
        //    for (int i = 0; i < defeatedEnemiesInBattle[j].DropItems.Length; i++)
        //    {
        //        int stack;
        //        try
        //        {
        //            Debug.Log(defeatedEnemiesInBattle[j].DropItems[i].Name + " - " + defeatedEnemiesInBattle[j].DropItems[i].DropStack + " - " + defeatedEnemiesInBattle[j].DropItems[i].ItemID);
        //            stack = UnityEngine.Random.Range(1, defeatedEnemiesInBattle[j].DropItems[i].DropStack);
        //            //Debug.Log("Стак - " + stack);
        //            FindObjectOfType<Inventory>().AddItem(defeatedEnemiesInBattle[j].DropItems[i].ItemID, stack);
        //        }
        //        catch (System.Exception e)
        //        {
        //            Debug.Log("Произошла ошибка при дропе вещи с моба - " + e.Message);
        //        }
        //    }
        //}


        // Дроп с локации
        //if (LocationsController.Instance.CurrentLocation != null)
        //{
        //    for (int i = 0; i < LocationsController.Instance.CurrentLocation.DroppableItems.Count; i++)
        //    {
        //        if (UnityEngine.Random.Range(0, 101) <= LocationsController.Instance.CurrentLocation.DroppableItems[i].ChanceToFind + RuntimePlayer.Instance.RuntimePlayerStats.Luck)
        //        {
        //            FindObjectOfType<Inventory>().AddItem(LocationsController.Instance.CurrentLocation.DroppableItems[i].ItemID, 1);
        //        }
        //    }
        //}
    }

    public void ForceKillAllEnemies()
    {
        foreach (RuntimeBattleCharacter enemy in enemiesInBattle)
        {
            enemy.Health = -999999999;
        }

        CheckEndBattleConditions();
    }

    private void ClearBattleData()
    {
        foreach (RuntimeBattleCharacter character in enemiesInBattle)
        {
            Destroy(character.gameObject);
        }

        enemiesInBattle.Clear();
        defeatedEnemiesInBattle.Clear();
    }

    // ====

    // Атака противника - ДОДЕЛАТЬ КУЧУ ПРОВЕРОК !!!!!!
    public void EnemyAIAttack()
    {
        //if (allEnemies.Count == 0)
        //    return;

        //if (allEnemies[allEnemies.Count - 1].Health <= 0)
        //    CheckEnemyDeath();
        //else
        //{
        //    for (int c = 0; c < allEnemies.Count; c++)
        //    {
        //        if (!allEnemies[c].isStun)
        //        {
        //            // Если противник ближник и стоит первый
        //            // Или если противник дальник, маг, бросатель и стоит на любой линии
        //            if ((allEnemies[c].AttackType == CharactersLibrary.CharacterAttackType.Melee && c == allEnemies.Count - 1)
        //                || allEnemies[c].AttackType == CharactersLibrary.CharacterAttackType.Ranged
        //                || allEnemies[c].AttackType == CharactersLibrary.CharacterAttackType.Throwable
        //                || allEnemies[c].AttackType == CharactersLibrary.CharacterAttackType.Magic)
        //            {
        //                // То противник атакует
        //                for (int i = 0; i < allEnemies[c].AttackSpeed; i++)
        //                {
        //                    EnemyAICalculateChances(c);

        //                    if (Player.Health <= 0)
        //                        break;
        //                }
        //            }

        //            CharacterAfterAttackEffect(1);

        //            if (Player.Health <= 0)
        //                break;
        //        }
        //        else
        //        {
        //            GameController.Instance.AddEventText("Противник был оглушён и не смог атаковать.");
        //        }
        //    }

        //    CheckPlayerDeath();

        //    CurrentRound++;
        //}

        //UIScript.Instance.UpdateEnemyUIText();
    }


    // Расчёт приоритета атаки противника
    private void EnemyAICalculateChances(int c)
    {
        //bool canUseMagic = false;
        //bool isAttack = false;

        //// Может ли противник использовать магию?
        //if (allEnemies[c].MaxMana > 0 && allEnemies[c].Spells.Length > 0)
        //{
        //    canUseMagic = true;
        //}

        //// Самое первое - проверка можно ли убить СЛАБЫМ ударом и приблизит ли это к победе?
        //if (allEnemies[c].Damage / 2 >= Player.Health + Player.Armor)
        //{
        //    EnemyAttack(c, 0);
        //    isAttack = true;
        //}
        //// Самое первое - проверка можно ли убить СРЕДНИМ ударом и приблизит ли это к победе?
        //else if (allEnemies[c].Damage >= Player.Health + Player.Armor)
        //{
        //    EnemyAttack(c, 1);
        //    isAttack = true;
        //}
        //// Самое первое - проверка можно ли убить СИЛЬНЫМ ударом и приблизит ли это к победе?
        //else if (allEnemies[c].Damage * 2 >= Player.Health + Player.Armor)
        //{
        //    // Если мы можем использовать боевую магию, то используем её, 
        //    // т.к. шансы на сильный удар крайне малы и могут не удовлетворить нас
        //    for (int i = 0; i < allEnemies[c].Spells.Length; i++)
        //    {
        //        if (allEnemies[c].Mana >= allEnemies[c].Spells[i].manaCost)
        //        {
        //            // Если противнику хватает маны и заклинание наносит больше, чем ...
        //            // ... здоровье игрока, то используем боевую магию
        //            if (allEnemies[c].Spells[i].spellPower >= Player.Health ||
        //               (allEnemies[c].Spells[i].spellPower >= Player.Health * 0.7f))
        //            {
        //                switch (allEnemies[c].Spells[i].currentSpell)
        //                {
        //                    case CharacterMagic.spellType.MagicFireball:
        //                        UseEnemyMagic(CharacterMagic.spellType.MagicFireball, c, i);
        //                        isAttack = true;
        //                        break;
        //                    case CharacterMagic.spellType.MagicThunder:
        //                        UseEnemyMagic(CharacterMagic.spellType.MagicThunder, c, i);
        //                        isAttack = true;
        //                        break;
        //                }
        //                break;
        //            }
        //        }
        //    }

        //    // Если магию не использовали, то бьём сильный удар
        //    if (!isAttack)
        //    {
        //        EnemyAttack(c, 2);
        //        isAttack = true;
        //    }
        //}

        //// Если убили игрока, то выходим из битвы
        //if (Player.Health <= 0)
        //{
        //    return;
        //}

        //// Используем магию по приоритетам у противника
        //// Если противник может использовать магию, то используем это...
        //if (canUseMagic)
        //{
        //    for(int i = 0; i < allEnemies[c].Spells.Length; i++)
        //    {
        //        if (allEnemies[c].lastMagicPriority == allEnemies[c].Spells[i].usePriority)
        //        {
        //            if (allEnemies[c].Mana >= allEnemies[c].Spells[i].manaCost)
        //            {
        //                allEnemies[c].Mana -= allEnemies[c].Spells[i].manaCost;
        //                UseEnemyMagic(allEnemies[c].Spells[i].currentSpell, c, i);
        //                canUseMagic = false;
        //                allEnemies[c].lastMagicPriority++;
        //                // Использовали магию - уходим из цикла ...
        //                break;
        //            }
        //        }
        //        else
        //        {
        //            for (int l = 0; l < allEnemies[c].Spells.Length; l++)
        //            {
        //                if (allEnemies[c].Mana >= allEnemies[c].Spells[l].manaCost)
        //                {
        //                    allEnemies[c].Mana -= allEnemies[c].Spells[l].manaCost;
        //                    UseEnemyMagic(allEnemies[c].Spells[l].currentSpell, c, l);
        //                    canUseMagic = false;
        //                    allEnemies[c].lastMagicPriority++;
        //                    // Использовали магию - уходим из цикла ...
        //                    break;
        //                }
        //            }
        //        }

        //        // Если это последнее заклинание в списке, то обнуляем приоритеты
        //        if (i == allEnemies[c].Spells.Length)
        //            allEnemies[c].lastMagicPriority = 0;
        //    }
        //}

        //// Если ничего так и не сделали, то просто атакуем случайной атакой
        //if(!isAttack && !canUseMagic)
        //{
        //    EnemyAttack(c, Random.Range(0, 3));
        //    isAttack = true;
        //}
    }
    public void UseEnemyMagic(CharacterMagic.spellType useSpell, int c, int s)
    {
        //GameController.Instance.AddEventText(CurrentRound + " - Противник использовал магию: " + magicName(useSpell, allEnemies[c].Spells[s].spellPower));
        //switch(useSpell)
        //{
        //    case CharacterMagic.spellType.MagicFireball:
        //        Player.Health -= (int)allEnemies[c].Spells[s].spellPower;
        //        break;
        //    case CharacterMagic.spellType.MagicHealthHeal:
        //        allEnemies[c].Health += allEnemies[c].Spells[s].spellPower;
        //        if(allEnemies[c].Health > allEnemies[c].MaxHealth)
        //        {
        //            allEnemies[c].Health = allEnemies[c].MaxHealth;
        //        }
        //        break;
        //    case CharacterMagic.spellType.MagicHealthSteal:
        //        allEnemies[c].Health += allEnemies[c].Spells[s].spellPower;
        //        if (allEnemies[c].Health > allEnemies[c].MaxHealth)
        //        {
        //            allEnemies[c].Health = allEnemies[c].MaxHealth;
        //        }
        //        Player.Health -= (int)allEnemies[c].Spells[s].spellPower;
        //        break;
        //    case CharacterMagic.spellType.MagicManaHeal:
        //        allEnemies[c].Mana += (int)allEnemies[c].Spells[s].spellPower;
        //        if (allEnemies[c].Mana > allEnemies[c].MaxMana)
        //        {
        //            allEnemies[c].Mana = allEnemies[c].MaxMana;
        //        }
        //        break;
        //    case CharacterMagic.spellType.MagicManaSteal:
        //        if(Player.Mana > 0)
        //        {
        //            if (allEnemies[c].Spells[s].spellPower >= Player.Mana)
        //            {
        //                allEnemies[c].Mana += (int)allEnemies[c].Spells[s].spellPower;
        //                Player.Mana -= (int)allEnemies[c].Spells[s].spellPower;
        //            }
        //            else
        //            {
        //                allEnemies[c].Mana += Player.Mana;
        //                Player.Mana = 0;
        //            }
        //            // Защита
        //            if (allEnemies[c].Mana > allEnemies[c].MaxMana)
        //            {
        //                allEnemies[c].Mana = allEnemies[c].MaxMana;
        //            }
        //        }
        //        break;
        //    case CharacterMagic.spellType.MagicShield:
        //        FindObjectOfType<BuffManager>().SetBuffToEnemy(Buff.BuffType.Magic_Shield);
        //        break;
        //    case CharacterMagic.spellType.MagicSpawn:
        //        for(int i = 0; i < allEnemies[c].Spells[s].spellPower; i++)
        //            AddEnemyToBattle(CharactersLibrary.CharacterType.SkeletWarrior);
        //        break;
        //    case CharacterMagic.spellType.MagicStun:
        //        FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Magic_Stun);
        //        break;
        //    case CharacterMagic.spellType.MagicThunder:
        //        Player.Health -= (int)allEnemies[c].Spells[s].spellPower;
        //        break;
        //}
    }
    public void EnemyAttack(int c, int attackType)
    {
        //int rndAtk = attackType;
        //if (UnityEngine.Random.Range(0, 101) > Player.DodgeChance + Player.Luck)
        //{
        //    CharacterBeforeAttackEffect(1);
        //    switch (rndAtk)
        //    {
        //        // Слабый удар
        //        case 0:
        //            if (UnityEngine.Random.Range(0, 101) <= 95)
        //            {
        //                double dmg = allEnemies[c].Damage / 2 - Player.Armor;
        //                if (allEnemies[c].Damage / 2 > Player.Armor)
        //                {
        //                    Player.Health -= dmg;
        //                    GameController.Instance.AddEventText(CurrentBattleStep + " - Противник атаковал слабым ударом и нанёс " + dmg + " ед. урона.");
        //                }
        //                else
        //                    GameController.Instance.AddEventText(CurrentBattleStep + " - Броня защитила Вас.");
        //            }
        //            else
        //                GameController.Instance.AddEventText(CurrentBattleStep + " - Противник промахнулся");
        //            break;
        //        // Средний удар
        //        case 1:
        //            if (UnityEngine.Random.Range(0, 101) <= 65)
        //            {
        //                double dmg = allEnemies[c].Damage - Player.Armor;
        //                if (allEnemies[c].Damage > Player.Armor)
        //                {
        //                    Player.Health -= dmg;
        //                    GameController.Instance.AddEventText(CurrentBattleStep + " - Противник атаковал средним ударом и нанёс " + dmg + " ед. урона.");
        //                }
        //                else
        //                    GameController.Instance.AddEventText(CurrentBattleStep + " - Броня защитила Вас.");
        //            }
        //            else
        //                GameController.Instance.AddEventText(CurrentBattleStep + " - Противник промахнулся");
        //            break;
        //        // Сильный удар
        //        case 2:
        //            if (UnityEngine.Random.Range(0, 101) <= 30)
        //            {
        //                double dmg = allEnemies[c].Damage * 2 - Player.Armor;
        //                if (allEnemies[c].Damage * 2 > Player.Armor)
        //                {
        //                    Player.Health -= dmg;
        //                    GameController.Instance.AddEventText(CurrentBattleStep + " - Противник атаковал сильным ударом и нанёс " + dmg + " ед. урона.");
        //                }
        //                else
        //                    GameController.Instance.AddEventText(CurrentBattleStep + " - Броня защитила Вас.");
        //            }
        //            else
        //                GameController.Instance.AddEventText(CurrentBattleStep + " - Противник промахнулся");
        //            break;
        //    }
        //}
        //else
        //{
        //    GameController.Instance.AddEventText(CurrentBattleStep + " - Вы уклонились от атаки противника.");
        //    FindObjectOfType<HeroAbilityManager>().ChargeHeroAbility(Player.HeroAbility.Ninja);
        //}
    }
}