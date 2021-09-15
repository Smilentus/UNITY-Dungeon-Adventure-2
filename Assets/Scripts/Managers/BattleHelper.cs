using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHelper : MonoBehaviour
{
    private static BattleHelper BH;
    public static BattleHelper _BH
    { get { return BH; } }

    [Space(20)]
    [Header("Вещи падающие с каждого моба (Просто добавь сюда)")]
    public Item[] ItemsToDropFromAll;
    [Header("Вещи со всех лесов")]
    public Item[] ItemsToDropFromAllForests;
    [Header("Вещи с синего леса")]
    public Item[] ItemsToDropFromBlueForest;
    [Header("Вещи с красного леса")]
    public Item[] ItemsToDropFromRedForest;
    [Header("Вещи с жёлтого леса")]
    public Item[] ItemsToDropFromYellowForest;
    [Header("Вещи с зелёного леса")]
    public Item[] ItemsToDropFromGreenForest;
    [Header("Вещи с подземелья")]
    public Item[] ItemsToDropFromDungeon;

    [Space(20)]
    [Header("Все персонажи")]
    public Character[] AllCharacters;

    [Header("Проверка сражается ли сейчас игрок")]
    public bool isBattle;
    public bool isWin;

    [Header("Текущий раунд")]
    public int CurrentRound;

    [Header("Кнопки действия в битве")]
    public GameObject[] ActionButtons;

    [Header("Кнопка ожидания")]
    public GameObject waitButton;

    [Header("Аниматор начала битвы")]
    public GameObject startBattleImage;
    [Header("Аниматор конца битвы")]
    public GameObject endBattleImage;

    [Space(20)]
    [Header("Параметры битвы")]
    public Transform spawnEnemyParent;
    [Header("Префаб противника")]
    public GameObject enemyPrefab;
    [Header("Объекты всех противников")]
    public List<GameObject> allEnemiesObjects = new List<GameObject>();
    [Header("Список противников которые учавствуют в битве")]
    public List<Character> allEnemies = new List<Character>();
    [Header("Список противников, которых мы убили")]
    public List<Character> defeatedEnemies = new List<Character>();

    private void Start()
    {
        BH = this;
    }

    // Добавление противника в лист для сражения
    public void AddEnemyToBattle(CharacterManager.CharacterType EnemyType)
    {
        // Поиск позиции в общем листе
        int pos = FindEnemyInList(EnemyType);

        // Создаём нового персонажа
        Character Enemy = new Character();

        // Установка основных переменных
        Enemy.Name = AllCharacters[pos].Name;
        Enemy.MaxHealth = AllCharacters[pos].MaxHealth;
        Enemy.Health = AllCharacters[pos].Health;
        Enemy.HealthRegen = AllCharacters[pos].HealthRegen;
        Enemy.Mana = AllCharacters[pos].Mana;
        Enemy.MaxMana = AllCharacters[pos].MaxMana;
        Enemy.Attack = AllCharacters[pos].Attack;
        Enemy.AttackSpeed = AllCharacters[pos].AttackSpeed;
        Enemy.Armor = AllCharacters[pos].Armor;
        Enemy.DodgeChance = AllCharacters[pos].DodgeChance;
        Enemy.isStun = false;
        // Установка типа 
        Enemy.Type = AllCharacters[pos].Type;
        // Установка типа защиты 
        Enemy.ArmorType = AllCharacters[pos].ArmorType;
        // Установка типа атаки
        Enemy.AttackType = AllCharacters[pos].AttackType;
        // Установка элемента
        Enemy.Element = AllCharacters[pos].Element;
        // Установка картинки
        Enemy.CharacterImage = AllCharacters[pos].CharacterImage;
        // Установка дроп. вещей
        Enemy.DropItems = new Item[AllCharacters[pos].DropItems.Length];
        for (int i = 0; i < AllCharacters[pos].DropItems.Length; i++)
        {
            Enemy.DropItems[i] = AllCharacters[pos].DropItems[i];
        }
        // Установка магии противника
        Enemy.Spells = new CharacterMagic[AllCharacters[pos].Spells.Length];
        for (int i = 0; i < AllCharacters[pos].Spells.Length; i++)
        {
            Enemy.Spells[i] = AllCharacters[pos].Spells[i];
        }
        // Дроп. лута
        Enemy.Gold = AllCharacters[pos].Gold;
        Enemy.Exp = AllCharacters[pos].Exp;

        // Инициализируем объект
        GameObject newEnemy = Instantiate(enemyPrefab, spawnEnemyParent);
        // Добавляем объект в список
        allEnemiesObjects.Add(newEnemy);
        // Добавляем в список
        allEnemies.Add(Enemy);

        // Отображаем картинку
        allEnemiesObjects[allEnemiesObjects.Count - 1].transform.GetChild(0).GetComponent<RawImage>().texture = Enemy.CharacterImage;
        UIScript.Instance.UpdateEnemyUIText();
    }

    // Устанавливаем флаг первой встречи с монстром
    private void SetMeetingFlag(CharacterManager.CharacterType chType)
    {
        for(int i = 0; i < AllCharacters.Length; i++)
        {
            if(!AllCharacters[i].isMeeted)
            {
                AllCharacters[i].isMeeted = true;
                break;
            }
        }
    }

    // Начало сражения
    public void StartBattle(CharacterManager.CharacterType EnemyType)
    {
        if (isBattle == false)
        {
            Player.isStun = false;
            isBattle = true;

            CurrentRound = 0;

            // Прогружаем магию персонажа
            FindObjectOfType<MagicManager>().LoadMagicInBattle();

            // Выпускаем в битву
            AddEnemyToBattle(EnemyType);

            waitButton.SetActive(false);

            // Спавним окно с информацией о противнике и потом, в зависимости от выбора ...
            // Идём дальше и показываем анимацию битвы и инициализируем сражение
            SetMeetingFlag(EnemyType);

            FindObjectOfType<SavingManager>().SaveGame("AutoSave");

            GameHelper._GH.Blocker.SetActive(true);
            startBattleImage.SetActive(true);
        }
    }

    // Открытие битвы
    public void StartBattleAfterAnim()
    {
        GameHelper._GH.Blocker.SetActive(false);
        startBattleImage.SetActive(false);

        // Открытие панели битвы
        FindObjectOfType<PanelsManager>().OpenHideActionPanel(0, true);

        // Эффект спавна персонажа
        CharacterSpawnEffect(0);
        // Эффект спавна противника
        CharacterSpawnEffect(1);
    }

    // Поиск противника в листе
    public int FindEnemyInList(CharacterManager.CharacterType fType)
    {
        for (int i = 0; i < AllCharacters.Length; i++)
        {
            if (AllCharacters[i].Type == fType)
                return i;
        }
        return -1;
    }

    // Обнуление текущего противника
    private void NullCurrentEnemy()
    {
        // Очистка текущего списка противников ...
        for (int i = 0; i < allEnemies.Count; i++)
        {
            allEnemies.RemoveAt(i);
            allEnemiesObjects.RemoveAt(i);
            Destroy(spawnEnemyParent.GetChild(i).gameObject);
        }
        // ... и очистка побеждённых противников
        defeatedEnemies.Clear();
        // Удаление бафов противника, если они ещё действуют
        FindObjectOfType<BuffManager>().NullAllEnemyBuffs();
    }

    // Конец сражения
    public void EndBattle()
    {
        GameHelper._GH.Blocker.SetActive(false);
        endBattleImage.SetActive(false);

        if (isWin)
        {
            WinCondition();

            // Сохраняем игру, если выйграли
            FindObjectOfType<SavingManager>().SaveGame("AutoSave");
        }

        isWin = false;
        isBattle = false;

        try
        {
            // Обнуление противника
            NullCurrentEnemy();
        }
        catch (System.Exception e)
        {
            Debug.Log("Ошибка при обнулении противника - \n" + e.Message);
        }

        waitButton.SetActive(true);

        // Изменяем цвет всех кнопок на нормальный
        foreach (var go in ActionButtons)
        {
            go.GetComponent<Image>().color = go.GetComponent<ButtonInteract>().normalColor;
        }

        // Закрываем панели
        FindObjectOfType<PanelsManager>().CloseAllActionPanels();
    }

    // Выдача предметов за победу
    public void WinCondition()
    {
        Player.isStun = false;

        // Деньги и опыт с каждого моба
        for (int i = 0; i < defeatedEnemies.Count; i++)
        {
            GameHelper._GH.GiveMoney(defeatedEnemies[i].Gold);
            GameHelper._GH.GiveExp(defeatedEnemies[i].Exp);
        }

        // Добавляем игроку индивидуальные вещи моба
        for (int j = 0; j < defeatedEnemies.Count; j++)
        {
            for (int i = 0; i < defeatedEnemies[j].DropItems.Length; i++)
            {
                int stack;
                try
                {
                    Debug.Log(defeatedEnemies[j].DropItems[i].Name + " - " + defeatedEnemies[j].DropItems[i].DropStack + " - " + defeatedEnemies[j].DropItems[i].ItemID);
                    stack = Random.Range(1, defeatedEnemies[j].DropItems[i].DropStack);
                    Debug.Log("Стак - " + stack);
                    FindObjectOfType<Inventory>().AddItem(defeatedEnemies[j].DropItems[i].ItemID, stack);
                }
                catch (System.Exception e)
                {
                    Debug.Log("Произошла ошибка при дропе вещи с моба - " + e.Message);
                }
            }
        }

        // Вещи с каждого моба
        for (int i = 0; i < ItemsToDropFromAll.Length; i++)
        {
            // Если шанс выпал, то добавляем
            if (Random.Range(0, 101) <= ItemsToDropFromAll[i].ChanceToFind + Player.Luck)
                FindObjectOfType<Inventory>().AddItem(ItemsToDropFromAll[i].ItemID, 1);
        }

        /* Вещи по локациям */
        if (LocationManager.CurrentLocation.ToString().ToLower().Contains("forest"))
        {
            // Вещи из всех лесов 
            for (int i = 0; i < ItemsToDropFromAllForests.Length; i++)
            {
                // Если шанс выпал, то добавляем
                if (Random.Range(0, 101) <= ItemsToDropFromAllForests[i].ChanceToFind + Player.Luck)
                    FindObjectOfType<Inventory>().AddItem(ItemsToDropFromAllForests[i].ItemID, 1);
            }
        }
        if (LocationManager.CurrentLocation == LocationManager.Location.BlueForest)
        {
            // Вещи из синего леса
            for (int i = 0; i < ItemsToDropFromBlueForest.Length; i++)
            {
                // Если шанс выпал, то добавляем
                if (Random.Range(0, 101) <= ItemsToDropFromBlueForest[i].ChanceToFind + Player.Luck)
                    FindObjectOfType<Inventory>().AddItem(ItemsToDropFromBlueForest[i].ItemID, 1);
            }
        }
        if (LocationManager.CurrentLocation == LocationManager.Location.RedForest)
        {
            // Вещи их красного леса
            for (int i = 0; i < ItemsToDropFromRedForest.Length; i++)
            {
                // Если шанс выпал, то добавляем
                if (Random.Range(0, 101) <= ItemsToDropFromRedForest[i].ChanceToFind + Player.Luck)
                    FindObjectOfType<Inventory>().AddItem(ItemsToDropFromRedForest[i].ItemID, 1);
            }
        }
        if (LocationManager.CurrentLocation == LocationManager.Location.YellowForest)
        {
            // Вещи из жёлтого леса
            for (int i = 0; i < ItemsToDropFromYellowForest.Length; i++)
            {
                // Если шанс выпал, то добавляем
                if (Random.Range(0, 101) <= ItemsToDropFromYellowForest[i].ChanceToFind + Player.Luck)
                    FindObjectOfType<Inventory>().AddItem(ItemsToDropFromYellowForest[i].ItemID, 1);
            }
        }
        if (LocationManager.CurrentLocation == LocationManager.Location.GreenForest)
        {
            // Вещи из зелёного леса
            for (int i = 0; i < ItemsToDropFromGreenForest.Length; i++)
            {
                // Если шанс выпал, то добавляем
                if (Random.Range(0, 101) <= ItemsToDropFromGreenForest[i].ChanceToFind + Player.Luck)
                    FindObjectOfType<Inventory>().AddItem(ItemsToDropFromGreenForest[i].ItemID, 1);
            }
        }
        if (LocationManager.CurrentLocation == LocationManager.Location.Dungeon)
        {
            // Вещи из зелёного леса
            for (int i = 0; i < ItemsToDropFromDungeon.Length; i++)
            {
                // Если шанс выпал, то добавляем
                if (Random.Range(0, 101) <= ItemsToDropFromDungeon[i].ChanceToFind + Player.Luck)
                    FindObjectOfType<Inventory>().AddItem(ItemsToDropFromDungeon[i].ItemID, 1);
            }
        }
    }

    // Атака игрока
    /// <summary>
    /// Типы атак: 0 - слабая. 1 - средняя. 2 - сильная.
    /// </summary>
    /// <param name="type"></param>
    public void AttackEnemy(int type)
    {
        if (CurrentRound % 15 == 0)
            FindObjectOfType<GameTimeScript>().AddTime(1);

        if (!Player.isStun)
        {
            // Атакуем Х раз от скорости атаки
            for (int i = 0; i < Player.AttackSpeed; i++)
            {
                // Эффекты до удара
                CharacterBeforeAttackEffect(0);
                // Крит. удар - TODO: Переделать <--
                if (!CriticalStrike())
                {
                    switch (type)
                    {
                        case 0:
                            LightStrike();
                            break;
                        case 1:
                            MediumStrike();
                            break;
                        case 2:
                            HeavyStrike();
                            break;
                    }
                }
                // Эффекты после удара
                CharacterAfterAttackEffect(0);

                // Работа скиллов
                FindObjectOfType<SkillsManager>().SkillsAction();

                if (allEnemies[allEnemies.Count - 1].Health <= 0)
                    break;
            }
        }
        else
        {
            GameHelper._GH.AddEventText("Вы оглушены и не можете атаковать.");
        }

        // Проверяем баффы на урон и снимаем длительность всех баффов
        FindObjectOfType<BuffManager>().BuffsAction();

        UIScript.Instance.UpdateEnemyUIText();
        FindObjectOfType<MagicManager>().UpdateMagicCooldown();

        CheckEnemyDeath();

        if (Player.Health <= 0)
            CheckPlayerDeath();
        else
            EnemyAIAttack();
    }

    // Попытка сбежать
    public void TryToEscape()
    {
        if (Random.Range(0, 101) <= 50 + Player.Luck)
        {
            isBattle = false;
            isWin = false;
            GameHelper._GH.AddEventText("Вы сбежали.");
            EndBattle();
        }
        else
        {
            GameHelper._GH.AddEventText("Вы не смогли избежать боя.");
            EnemyAIAttack();
        }
    }

    #region Удары персонажа по противнику
    private void LightStrike()
    {
        double dmg = Player.Damage / 2;

        // Проверка уклонения противника
        if (Random.Range(0, 101) + Player.Luck > allEnemies[allEnemies.Count - 1].DodgeChance)
        {
            if (Random.Range(0, 101) <= Player.LightStrikeChance + Player.Luck)
            {
                if (dmg > allEnemies[allEnemies.Count - 1].Armor)
                {
                    allEnemies[allEnemies.Count - 1].Health -= dmg;
                    // Анимируем удар
                    GameHelper._GH.AddEventText(CurrentRound + " - Вы нанесли урон слабым ударом: " + dmg + " ед.");
                }
                else
                {
                    GameHelper._GH.AddEventText(CurrentRound + " - Броня защитила противника.");
                }
            }
            else
                GameHelper._GH.AddEventText(CurrentRound + " - Вы промахнулись.");
        }
        else
            GameHelper._GH.AddEventText(CurrentRound + " - Противник увернулся.");
    }
    private void MediumStrike()
    {
        double dmg = Player.Damage;

        if (Random.Range(0, 101) + Player.Luck > allEnemies[allEnemies.Count - 1].DodgeChance)
        {
            // Проверка уклонения противника
            if (Random.Range(0, 101) <= Player.MediumStrikeChance + Player.Luck)
            {
                if (dmg > allEnemies[allEnemies.Count - 1].Armor)
                {
                    allEnemies[allEnemies.Count - 1].Health -= dmg;
                    // Анимируем удар
                    GameHelper._GH.AddEventText(CurrentRound + " - Вы нанесли урон нормальным ударом: " + dmg + " ед.");
                }
                else
                {
                    GameHelper._GH.AddEventText(CurrentRound + " - Броня защитила противника.");
                }
            }
            else
                GameHelper._GH.AddEventText(CurrentRound + " - Вы промахнулись.");
        }
        else
            GameHelper._GH.AddEventText(CurrentRound + " - Противник увернулся.");
    }
    private void HeavyStrike()
    {
        double dmg = Player.Damage * 2;

        // Проверка уклонения противника       
        if (Random.Range(0, 101) + Player.Luck > allEnemies[allEnemies.Count - 1].DodgeChance)
        {
            if (Random.Range(0, 101) <= Player.HeavyStrikeChance + Player.Luck)
            {
                if (dmg > allEnemies[allEnemies.Count - 1].Armor)
                {
                    allEnemies[allEnemies.Count - 1].Health -= dmg;
                    // Анимируем удар
                    GameHelper._GH.AddEventText(CurrentRound + " - Вы нанесли урон сильным ударом: " + dmg + " ед.");
                }
                else
                {
                    GameHelper._GH.AddEventText(CurrentRound + " - Броня защитила противника.");
                }
            }
            else
                GameHelper._GH.AddEventText(CurrentRound + " - Вы промахнулись.");
        }
        else
            GameHelper._GH.AddEventText(CurrentRound + " - Противник увернулся.");
    }
    private bool CriticalStrike()
    {
        int rndChance = Random.Range(0, 101);
        double dmg = Player.Damage * Player.CriticalStrikeMulty;

        if (rndChance <= Player.CriticalStrikeChance)
        {
            allEnemies[allEnemies.Count - 1].Health -= dmg;
            GameHelper._GH.AddEventText(CurrentRound + " - Вы нанесли урон критическим ударом: " + dmg + " ед.");
            return true;
        }
        return false;
    }
    #endregion

    // Атака противника - ДОДЕЛАТЬ КУЧУ ПРОВЕРОК !!!!!!
    public void EnemyAIAttack()
    {
        if (allEnemies.Count == 0)
            return;

        if (allEnemies[allEnemies.Count - 1].Health <= 0)
            CheckEnemyDeath();
        else
        {
            for (int c = 0; c < allEnemies.Count; c++)
            {
                if (!allEnemies[c].isStun)
                {
                    // Если противник ближник и стоит первый
                    // Или если противник дальник, маг, бросатель и стоит на любой линии
                    if ((allEnemies[c].AttackType == CharacterManager.CharacterAttackType.Melee && c == allEnemies.Count - 1)
                        || allEnemies[c].AttackType == CharacterManager.CharacterAttackType.Ranged
                        || allEnemies[c].AttackType == CharacterManager.CharacterAttackType.Throwable
                        || allEnemies[c].AttackType == CharacterManager.CharacterAttackType.Magic)
                    {
                        // То противник атакует
                        for (int i = 0; i < allEnemies[c].AttackSpeed; i++)
                        {
                            EnemyAICalculateChances(c);

                            if (Player.Health <= 0)
                                break;
                        }
                    }

                    CharacterAfterAttackEffect(1);

                    if (Player.Health <= 0)
                        break;
                }
                else
                {
                    GameHelper._GH.AddEventText("Противник был оглушён и не смог атаковать.");
                }
            }

            CheckPlayerDeath();

            CurrentRound++;
        }

        UIScript.Instance.UpdateEnemyUIText();
    }

    // Расчёт приоритета атаки противника
    private void EnemyAICalculateChances(int c)
    {
        bool canUseMagic = false;
        bool isAttack = false;

        // Может ли противник использовать магию?
        if (allEnemies[c].MaxMana > 0 && allEnemies[c].Spells.Length > 0)
        {
            canUseMagic = true;
        }

        // Самое первое - проверка можно ли убить СЛАБЫМ ударом и приблизит ли это к победе?
        if (allEnemies[c].Attack / 2 >= Player.Health + Player.Armor)
        {
            EnemyAttack(c, 0);
            isAttack = true;
        }
        // Самое первое - проверка можно ли убить СРЕДНИМ ударом и приблизит ли это к победе?
        else if (allEnemies[c].Attack >= Player.Health + Player.Armor)
        {
            EnemyAttack(c, 1);
            isAttack = true;
        }
        // Самое первое - проверка можно ли убить СИЛЬНЫМ ударом и приблизит ли это к победе?
        else if (allEnemies[c].Attack * 2 >= Player.Health + Player.Armor)
        {
            // Если мы можем использовать боевую магию, то используем её, 
            // т.к. шансы на сильный удар крайне малы и могут не удовлетворить нас
            for (int i = 0; i < allEnemies[c].Spells.Length; i++)
            {
                if (allEnemies[c].Mana >= allEnemies[c].Spells[i].manaCost)
                {
                    // Если противнику хватает маны и заклинание наносит больше, чем ...
                    // ... здоровье игрока, то используем боевую магию
                    if (allEnemies[c].Spells[i].spellPower >= Player.Health ||
                       (allEnemies[c].Spells[i].spellPower >= Player.Health * 0.7f))
                    {
                        switch (allEnemies[c].Spells[i].currentSpell)
                        {
                            case CharacterMagic.spellType.MagicFireball:
                                UseEnemyMagic(CharacterMagic.spellType.MagicFireball, c, i);
                                isAttack = true;
                                break;
                            case CharacterMagic.spellType.MagicThunder:
                                UseEnemyMagic(CharacterMagic.spellType.MagicThunder, c, i);
                                isAttack = true;
                                break;
                        }
                        break;
                    }
                }
            }

            // Если магию не использовали, то бьём сильный удар
            if (!isAttack)
            {
                EnemyAttack(c, 2);
                isAttack = true;
            }
        }

        // Если убили игрока, то выходим из битвы
        if (Player.Health <= 0)
        {
            return;
        }

        // Используем магию по приоритетам у противника
        // Если противник может использовать магию, то используем это...
        if (canUseMagic)
        {
            for(int i = 0; i < allEnemies[c].Spells.Length; i++)
            {
                if (allEnemies[c].lastMagicPriority == allEnemies[c].Spells[i].usePriority)
                {
                    if (allEnemies[c].Mana >= allEnemies[c].Spells[i].manaCost)
                    {
                        allEnemies[c].Mana -= allEnemies[c].Spells[i].manaCost;
                        UseEnemyMagic(allEnemies[c].Spells[i].currentSpell, c, i);
                        canUseMagic = false;
                        allEnemies[c].lastMagicPriority++;
                        // Использовали магию - уходим из цикла ...
                        break;
                    }
                }
                else
                {
                    for (int l = 0; l < allEnemies[c].Spells.Length; l++)
                    {
                        if (allEnemies[c].Mana >= allEnemies[c].Spells[l].manaCost)
                        {
                            allEnemies[c].Mana -= allEnemies[c].Spells[l].manaCost;
                            UseEnemyMagic(allEnemies[c].Spells[l].currentSpell, c, l);
                            canUseMagic = false;
                            allEnemies[c].lastMagicPriority++;
                            // Использовали магию - уходим из цикла ...
                            break;
                        }
                    }
                }

                // Если это последнее заклинание в списке, то обнуляем приоритеты
                if (i == allEnemies[c].Spells.Length)
                    allEnemies[c].lastMagicPriority = 0;
            }
        }

        // Если ничего так и не сделали, то просто атакуем случайной атакой
        if(!isAttack && !canUseMagic)
        {
            EnemyAttack(c, Random.Range(0, 3));
            isAttack = true;
        }
    }
    public void UseEnemyMagic(CharacterMagic.spellType useSpell, int c, int s)
    {
        GameHelper._GH.AddEventText(CurrentRound + " - Противник использовал магию: " + magicName(useSpell, allEnemies[c].Spells[s].spellPower));
        switch(useSpell)
        {
            case CharacterMagic.spellType.MagicFireball:
                Player.Health -= (int)allEnemies[c].Spells[s].spellPower;
                break;
            case CharacterMagic.spellType.MagicHealthHeal:
                allEnemies[c].Health += allEnemies[c].Spells[s].spellPower;
                if(allEnemies[c].Health > allEnemies[c].MaxHealth)
                {
                    allEnemies[c].Health = allEnemies[c].MaxHealth;
                }
                break;
            case CharacterMagic.spellType.MagicHealthSteal:
                allEnemies[c].Health += allEnemies[c].Spells[s].spellPower;
                if (allEnemies[c].Health > allEnemies[c].MaxHealth)
                {
                    allEnemies[c].Health = allEnemies[c].MaxHealth;
                }
                Player.Health -= (int)allEnemies[c].Spells[s].spellPower;
                break;
            case CharacterMagic.spellType.MagicManaHeal:
                allEnemies[c].Mana += (int)allEnemies[c].Spells[s].spellPower;
                if (allEnemies[c].Mana > allEnemies[c].MaxMana)
                {
                    allEnemies[c].Mana = allEnemies[c].MaxMana;
                }
                break;
            case CharacterMagic.spellType.MagicManaSteal:
                if(Player.Mana > 0)
                {
                    if (allEnemies[c].Spells[s].spellPower >= Player.Mana)
                    {
                        allEnemies[c].Mana += (int)allEnemies[c].Spells[s].spellPower;
                        Player.Mana -= (int)allEnemies[c].Spells[s].spellPower;
                    }
                    else
                    {
                        allEnemies[c].Mana += Player.Mana;
                        Player.Mana = 0;
                    }
                    // Защита
                    if (allEnemies[c].Mana > allEnemies[c].MaxMana)
                    {
                        allEnemies[c].Mana = allEnemies[c].MaxMana;
                    }
                }
                break;
            case CharacterMagic.spellType.MagicShield:
                FindObjectOfType<BuffManager>().SetBuffToEnemy(Buff.BuffType.Magic_Shield);
                break;
            case CharacterMagic.spellType.MagicSpawn:
                for(int i = 0; i < allEnemies[c].Spells[s].spellPower; i++)
                    AddEnemyToBattle(CharacterManager.CharacterType.SkeletWarrior);
                break;
            case CharacterMagic.spellType.MagicStun:
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Magic_Stun);
                break;
            case CharacterMagic.spellType.MagicThunder:
                Player.Health -= (int)allEnemies[c].Spells[s].spellPower;
                break;
        }
    }
    public void EnemyAttack(int c, int attackType)
    {
        int rndAtk = attackType;
        if (Random.Range(0, 101) > Player.DodgeChance + Player.Luck)
        {
            CharacterBeforeAttackEffect(1);
            switch (rndAtk)
            {
                // Слабый удар
                case 0:
                    if (Random.Range(0, 101) <= 95)
                    {
                        double dmg = allEnemies[c].Attack / 2 - Player.Armor;
                        if (allEnemies[c].Attack / 2 > Player.Armor)
                        {
                            Player.Health -= dmg;
                            GameHelper._GH.AddEventText(CurrentRound + " - Противник атаковал слабым ударом и нанёс " + dmg + " ед. урона.");
                        }
                        else
                            GameHelper._GH.AddEventText(CurrentRound + " - Броня защитила Вас.");
                    }
                    else
                        GameHelper._GH.AddEventText(CurrentRound + " - Противник промахнулся");
                    break;
                // Средний удар
                case 1:
                    if (Random.Range(0, 101) <= 65)
                    {
                        double dmg = allEnemies[c].Attack - Player.Armor;
                        if (allEnemies[c].Attack > Player.Armor)
                        {
                            Player.Health -= dmg;
                            GameHelper._GH.AddEventText(CurrentRound + " - Противник атаковал средним ударом и нанёс " + dmg + " ед. урона.");
                        }
                        else
                            GameHelper._GH.AddEventText(CurrentRound + " - Броня защитила Вас.");
                    }
                    else
                        GameHelper._GH.AddEventText(CurrentRound + " - Противник промахнулся");
                    break;
                // Сильный удар
                case 2:
                    if (Random.Range(0, 101) <= 30)
                    {
                        double dmg = allEnemies[c].Attack * 2 - Player.Armor;
                        if (allEnemies[c].Attack * 2 > Player.Armor)
                        {
                            Player.Health -= dmg;
                            GameHelper._GH.AddEventText(CurrentRound + " - Противник атаковал сильным ударом и нанёс " + dmg + " ед. урона.");
                        }
                        else
                            GameHelper._GH.AddEventText(CurrentRound + " - Броня защитила Вас.");
                    }
                    else
                        GameHelper._GH.AddEventText(CurrentRound + " - Противник промахнулся");
                    break;
            }
        }
        else
        {
            GameHelper._GH.AddEventText(CurrentRound + " - Вы уклонились от атаки противника.");
            FindObjectOfType<HeroAbilityManager>().ChargeHeroAbility(Player.HeroAbility.Ninja);
        }
    }
    public string magicName(CharacterMagic.spellType spell, double spellPower)
    {
        string newName = "";

        switch (spell)
        {
            case CharacterMagic.spellType.MagicFireball:
                newName = "*Огненный шар* и нанёс " + spellPower.ToString("f0") + " ед. урона!";
                break;
            case CharacterMagic.spellType.MagicHealthHeal:
                newName = "*Магическая регенерация здоровья* и восстановил " + spellPower.ToString("f0") + " ОЗ";
                break;
            case CharacterMagic.spellType.MagicHealthSteal:
                newName = "*Похищение жизни* и восстановил " + spellPower.ToString("f0") + " ОЗ";
                break;
            case CharacterMagic.spellType.MagicManaHeal:
                newName = "*Магическое восстановление маны* и восстановил " + spellPower.ToString("f0") + " ОМ";
                break;
            case CharacterMagic.spellType.MagicManaSteal:
                newName = "*Похищение маны* и восстановил " + spellPower.ToString("f0") + " ОМ";
                break;
            case CharacterMagic.spellType.MagicShield:
                newName = "*Магическая защита* и усилил защиту на " + spellPower.ToString("f0") + " ед.";
                break;
            case CharacterMagic.spellType.MagicSpawn:
                newName = "*Призыв скелетов* и призвал " + spellPower.ToString("f0") + " скелетов!";
                break;
            case CharacterMagic.spellType.MagicStun:
                newName = "*Остолбенение* и Вы замерли!";
                break;
            case CharacterMagic.spellType.MagicThunder:
                newName = "*Разряд молнии* и нанёс " + spellPower.ToString("f0") + " ед. урона!";
                break;
            default:
                newName = "";
                break;
        }

        return newName;
    }

    #region Различные проверки в битве
    // Проверка смерти противника
    public void CheckEnemyDeath()
    {
        try
        {
            for (int i = 0; i < allEnemies.Count; i++)
            {
                if (allEnemies[i].Health <= 0)
                {
                    //CharacterAfterDeathEffect(1);
                    Debug.Log("Противник: " + allEnemies[i].Name + " - Повержен.");
                    // Добавляем противника в побеждённых
                    Character newCharacter = new Character();
                    newCharacter = allEnemies[i];
                    defeatedEnemies.Add(newCharacter);
                    Debug.Log("Добавили противника - " + defeatedEnemies[defeatedEnemies.Count - 1].Name);
                    // Удаляем противника из списка
                    allEnemies.RemoveAt(i);
                    // Удаляем противника из списка объектов
                    allEnemiesObjects.RemoveAt(i);
                    // Удаляем противника визуально
                    Destroy(spawnEnemyParent.GetChild(i).gameObject);
                }
            }
        }catch(System.Exception e)
        {
            Debug.Log("Ошибка при проверке смерти противника - " + e.Message);
        }

        // Если противников не осталось, заканчиваем бой
        if (allEnemies.Count == 0)
        {
            Debug.Log("Противники закончились, мы выйграли!");
            isWin = true;
            GameHelper._GH.Blocker.SetActive(true);
            endBattleImage.SetActive(true);
        }

        UIScript.Instance.UpdateEnemyUIText();
    }

    // Проверка смерти персонажа
    public void CheckPlayerDeath()
    {
        if (Player.Health <= 0)
        {
            Player.isDeath = true;
            CharacterAfterDeathEffect(0);

            isWin = false;
            EndBattle();
            GameHelper._GH.ShowDeathBox("Вы умерли во время битвы.");
        }
    }

    // Эффект после спавна
    public void CharacterSpawnEffect(int character)
    {
        if (character == 0)
        {

        }

        if (character == 1)
        {

        }
    }

    // Эффект перед КАЖДОЙ атакой
    public void CharacterBeforeAttackEffect(int character)
    {
        if (character == 0)
        {
            FindObjectOfType<HeroAbilityManager>().ChargeHeroAbility(Player.HeroAbility.Archer);
        }

        if (character == 1)
        {
            
        }
    }

    // Эффект после ВСЕХ атак
    public void CharacterAfterAttackEffect(int character)
    {
        if (character == 0)
        {
            // Проверяем баффы после атаки персонажа
            FindObjectOfType<BuffManager>().BuffsBattleAction();
        }

        if (character == 1)
        {
            FindObjectOfType<HeroAbilityManager>().ChargeHeroAbility(Player.HeroAbility.Warrior);
            // Регенерация здоровья противника
            allEnemies[allEnemies.Count - 1].Health += allEnemies[allEnemies.Count - 1].HealthRegen;
        }
    }

    // Эффект после смерти
    public void CharacterAfterDeathEffect(int character)
    {
        if(character == 0)
        { }

        //if(character == 1)
        //{
        //    switch (Enemy.Type)
        //    {
        //        case CharacterManager.CharacterType.DarkElementalOne:
        //            StartBattle(CharacterManager.CharacterType.DarkElementalTwo);
        //            break;
        //        case CharacterManager.CharacterType.DarkElementalTwo:
        //            StartBattle(CharacterManager.CharacterType.DarkElementalThree);
        //            break;
        //        case CharacterManager.CharacterType.HellHound:
        //            StartBattle(CharacterManager.CharacterType.HellTwoHeadsHound);
        //            break;
        //        case CharacterManager.CharacterType.HellTwoHeadsHound:
        //            StartBattle(CharacterManager.CharacterType.HellThreeHeadsHound);
        //            break;
        //    }
        //}
    }
    #endregion
}
