using Dimasyechka.Code.BattleSystem.EnemiesSystem;
using Dimasyechka.Code.BattleSystem.GlobalWindow;
using Dimasyechka.Code.BattleSystem.PlayerSystem;
using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.GlobalWindows.Controllers;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.ZenjectFactories;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

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
namespace Dimasyechka.Code.BattleSystem.Controllers
{
    public class BattleController : MonoBehaviour
    {
        public event Action<bool> onBattleStatusChanged;
        public event Action<TurnStatus> onBattleTurnStatusChanged;


        public enum TurnStatus
        {
            PlayerTurn,
            EnemiesTurn
        }

        [SerializeField]
        private RuntimeBattleCharacter _defaultRuntimeBattleCharacterPrefab;

        [SerializeField]
        private Transform _enemiesSpawnArea;


        public List<RuntimeBattleCharacter> EnemiesInBattle { get; } = new();

        public List<CharacterProfile> DefeatedEnemiesInBattle { get; } = new();

        public bool IsBattle { get; set; }

        public bool IsWin { get; set; }

        public TurnStatus CurrentTurnStatus { get; private set; }

        public int CurrentBattleStep { get; private set; }


        private RuntimePlayer _runtimePlayer;
        private GameController _gameController;
        private GameTimeFlowController _gameTimeFlowController;
        private RuntimeBattlePlayerController _runtimeBattlePlayerController;


        private RuntimeBattleCharacterFactory _runtimeBattleCharacterFactory;


        [Inject]
        public void Construct(
            RuntimePlayer runtimePlayer,
            RuntimeBattlePlayerController runtimeBattlePlayerController,
            GameTimeFlowController gameTimeFlowController,
            GameController gameController,
            RuntimeBattleCharacterFactory runtimeBattleCharacterFactory)
        {
            _runtimeBattlePlayerController = runtimeBattlePlayerController;
            _gameTimeFlowController = gameTimeFlowController;
            _runtimePlayer = runtimePlayer;
            _gameController = gameController;

            _runtimeBattleCharacterFactory = runtimeBattleCharacterFactory;
        }


        public void AddEnemyToBattle(CharacterProfile characterProfile)
        {
            RuntimeBattleCharacter createdEnemy;

            if (characterProfile.CharacterPrefab != null)
                createdEnemy = _runtimeBattleCharacterFactory.InstantiateCharacter(characterProfile.CharacterPrefab, _enemiesSpawnArea);
            else
                createdEnemy = _runtimeBattleCharacterFactory.InstantiateCharacter(_defaultRuntimeBattleCharacterPrefab, _enemiesSpawnArea);

            createdEnemy.CreateBattleCharacter(characterProfile);
            EnemiesInBattle.Add(createdEnemy);
        }


        public void StartPlayerTurn()
        {
            if (CheckEndBattleConditions()) return;

            //if (!_runtimePlayer.RuntimePlayerStats.IsStun)
            //{
            UpdateAllEnemiesUI();

            CurrentTurnStatus = TurnStatus.PlayerTurn;
            onBattleTurnStatusChanged?.Invoke(CurrentTurnStatus);
            //}
            //else
            //{
            //    _gameController.AddEventText("Вы оглушены и не можете атаковать.");
            //    StartEnemiesTurn();
            //}
        }

        public void EndPlayerTurn()
        {
            _gameController.AddEventText("Вы закончили свой ход.");

            StartEnemiesTurn();
        }


        public void StartEnemiesTurn()
        {
            if (CheckEndBattleConditions()) return;

            CurrentTurnStatus = TurnStatus.EnemiesTurn;
            onBattleTurnStatusChanged?.Invoke(CurrentTurnStatus);

            for (var i = 0; i < EnemiesInBattle.Count; i++) EnemiesInBattle[i].ProcessCharacterActions();

            EndEnemiesTurn();
        }

        public void UpdateAllEnemiesUI()
        {
            foreach (var runtimeBattleCharacter in EnemiesInBattle) runtimeBattleCharacter.UpdateCharacterView();
        }

        // Фактически здесь конец хода, потому что и игрок сходил и все противники
        private void EndEnemiesTurn()
        {
            _gameTimeFlowController.AddTime(1);

            CurrentBattleStep++;

            StartPlayerTurn();
        }


        public bool CheckEndBattleConditions()
        {
            // Здесь все проверки после каждого действия, чтобы понять умер противник или нет
            if (_runtimePlayer.RuntimePlayerStats.Health.Value <= 0)
            {
                IsWin = false;
                EndBattle();
                _gameController.ShowDeathBox("Вы умерли во время битвы.");

                return true;
            }


            for (var i = 0; i < EnemiesInBattle.Count; i++)
                if (EnemiesInBattle[i].Health <= 0)
                {
                    //Debug.Log("Противник: " + enemiesInBattle[i].CharacterProfile.Name + " - Повержен.");
                    // Добавляем противника в побеждённых
                    DefeatedEnemiesInBattle.Add(EnemiesInBattle[i].CharacterProfile);
                    Destroy(EnemiesInBattle[i].gameObject);
                    // Удаляем противника из списка
                    EnemiesInBattle.RemoveAt(i);
                }


            // Если противников не осталось, заканчиваем бой
            if (EnemiesInBattle.Count == 0)
            {
                //Debug.Log("Противники закончились, мы выйграли!");
                IsWin = true;
                _gameController.Blocker.SetActive(true);
                EndBattle();
                return true;
            }

            return false;
        }


        public void TryStartBattle(List<CharacterProfile> characters)
        {
            if (IsBattle) return;

            ClearBattleData();

            foreach (var characterProfile in characters) AddEnemyToBattle(characterProfile);

            IsBattle = true;
            IsWin = false;
            CurrentBattleStep = 0;

            _runtimeBattlePlayerController.InitializeBattlePlayer();

            GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(BattleGlobalWindow));

            onBattleStatusChanged?.Invoke(true);
            StartPlayerTurn();
        }

        public void EndBattle()
        {
            GameController.Instance.Blocker.SetActive(false);

            if (IsWin)
            {
                WinCondition();
                SaveLoadSystemController.Instance.TrySaveGameState("AutoSave");
            }

            IsWin = false;
            IsBattle = false;

            ClearBattleData();

            GlobalWindowsController.Instance.TryHideGlobalWindow(typeof(BattleGlobalWindow));
        }

        // Выдача предметов за победу
        public void WinCondition()
        {
            // Деньги и опыт с каждого моба
            for (var i = 0; i < DefeatedEnemiesInBattle.Count; i++)
            {
                _runtimePlayer.GiveMoney(DefeatedEnemiesInBattle[i].Gold);
                _runtimePlayer.GiveExperience(DefeatedEnemiesInBattle[i].Exp);
            }

            // Добавляем игроку индивидуальные вещи моба
            // Дроп с локации
        }

        public void ForceKillAllEnemies()
        {
            foreach (var enemy in EnemiesInBattle) enemy.Health = -999999999;

            CheckEndBattleConditions();
        }

        private void ClearBattleData()
        {
            foreach (var character in EnemiesInBattle) Destroy(character.gameObject);

            EnemiesInBattle.Clear();
            DefeatedEnemiesInBattle.Clear();
        }

        // ====

        // Атака противника - ДОДЕЛАТЬ КУЧУ ПРОВЕРОК !!!!!!
        //public void EnemyAIAttack()
        //{
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
        //}


        // Расчёт приоритета атаки противника
        //private void EnemyAICalculateChances(int c)
        //{
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
        //}

        //public void UseEnemyMagic(CharacterMagic.spellType useSpell, int c, int s)
        //{
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
        //}

        //public void EnemyAttack(int c, int attackType)
        //{
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
        //}
    }


    public class RuntimeBattleCharacterFactory : DiContainerFactory
    {
        public RuntimeBattleCharacter InstantiateCharacter(RuntimeBattleCharacter characterPrefab, Transform spawnPoint)
        {
            return _diContainer.InstantiatePrefabForComponent<RuntimeBattleCharacter>(characterPrefab, spawnPoint);
        }
    }
}