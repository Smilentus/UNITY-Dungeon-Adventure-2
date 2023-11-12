using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SavingManager : MonoBehaviour
{
    //private static SavingManager SM;
    //public static SavingManager _SM
    //{ get { return SM; } }

    //[System.Serializable]
    //public class Saver
    //{
    //    // Название сохранения
    //    public string saveName;
    //    // Дата сохранения
    //    public string saveData;

    //    public bool isFirstEnter;
    //    public string PlayerName;
    //    public double MaxHealth, Health;
    //    public double HealthRegen;
    //    public double Armor;
    //    public double MaxMana, Mana;
    //    public double ManaRegen;
    //    public double AttackSpeed;
    //    public double Attack;

    //    public double tempHealth, tempMaxHealth, tempDamage;

    //    public bool AntiHole;
    //    public double Luck;
    //    public double Money;

    //    public double ChanceNotToDelete;
    //    public double ChanceToCraftTwice;

    //    public double HAChargePower;
    //    public double HACharge;
    //    public double HAMaxCharge;
    //    public int HALvl;
    //    public int HAMaxLvl;
    //    public HeroAbility Ability;

    //    public double Lvl;
    //    public double Exp;
    //    public double MaxExp;
    //    public double ExpMulty;
    //    public int SkillPoints;

    //    public double ExtraExpMod;
    //    public double ExtraExpMulty;
    //    public double ExtraMoneyMod;
    //    public double ExtraMoneyMulty;

    //    public double DodgeChance;
    //    public double LightChance;
    //    public double MediumChance;
    //    public double HeavyChance;
    //    public double CriticalStrikeChance;
    //    public double CriticalStrikeMulty;

    //    public bool isHadVillage;

    //    public int manaPotionCounter;
    //    public int openedInvCases;
    //    public int openedRuneCases;

    //    public int CurrentHour;
    //    public int CurrentDay;
    //    public int CurrentMonth;
    //    public int CurrentYear;

    //    public string villageName;
    //    public double villagePopularity;
    //    public int villageCapacity;
    //    public int villageMaxCapacity;
    //    public List<producingItem> villageProducedItems;
    //    public List<SVillagePart> villageParts;

    //    public List<GameTimeFlowEvent> savedTimeEvents;

    //    public CharactersLibrary.CharacterArmorType ArmorType;
    //    public CharactersLibrary.CharacterAttackType AttackType;
    //    public CharactersLibrary.CharacterElement Element;

    //    public LocationProfile Location;

    //    public List<LocationProfile> savedLocations = new List<LocationProfile>();

    //    public List<SSkill> savedSkills = new List<SSkill>();
    //    public List<SItem> savedInv = new List<SItem>();
    //    public List<SBuff> savedBuffs = new List<SBuff>();
    //    public List<SMagic> savedMagic = new List<SMagic>();
    //}

    //[System.Serializable]
    //public class SVillagePart
    //{
    //    public PlayerVillageActivity.villagePart vilPart;
    //    public int currentLvl;
    //    public double upgradeCost;
    //    public double upgradeMulty;
    //    public int currentWorkers;
    //    public double currentIncome;
    //    public int currentPopularity;
    //}

    //[System.Serializable]
    //public class SItem
    //{
    //    public int Stack;
    //    public string ItemID;
    //    public int ItemPos;
    //}

    //[System.Serializable]
    //public class SMagic
    //{
    //    public MagicManager.MagicType magicType;
    //    public double actionVar;
    //    public int cooldownTimeMax;
    //    public int currentCooldown;
    //}

    //[System.Serializable]
    //public class SSkill
    //{
    //    public int Lvl;
    //    public int Cost;
    //    public double Var;
    //}

    //[System.Serializable]
    //public class SBuff
    //{
    //    public int Duration;
    //    public Buff.BuffType sType;
    //}

    //[Header("Ссылки")]
    //public BattleController BH;
    //public GameController GH;
    //public CharactersLibrary CM;
    //public LocationsController LM;
    //public Inventory INV;
    //public PanelsManager PM;
    //public SkillsManager SMr;
    //public BuffManager BM;
    //public MagicManager MM;
    //public PlayerVillageActivity PVA;

    //[Header("Панель сохранений")]
    //public GameObject savingPanel;

    //// Открытие меню сохранений 
    //public void OpenSavingMenu()
    //{
    //    savingPanel.SetActive(true);
    //}

    //// Закрытие меню сохранений
    //public void CloseSavingMenu()
    //{
    //    savingPanel.SetActive(false);
    //}

    //// Сохранение игры
    //public void SaveGame(string saveName)
    //{
    //    Saver save = new Saver();

    //    save.isFirstEnter = RuntimePlayer.isFirstEnter;

    //    // Сохранение даты
    //    save.CurrentHour = GameTimeFlowController.Instance.CurrentHour;
    //    save.CurrentDay = GameTimeFlowController.Instance.CurrentDay;
    //    save.CurrentMonth = GameTimeFlowController.Instance.CurrentMonth;
    //    save.CurrentYear = GameTimeFlowController.Instance.CurrentYear;
    //    save.savedTimeEvents = FindObjectOfType<GameTimeFlowController>().CurrentGameTimeFlowEvents;

    //    // СОХРАНЯТЬ ЗДЕСЬ, НЕ ЗАБУДЬ!
    //    save.PlayerName = RuntimePlayer.Name;
    //    save.Health = RuntimePlayer.Health;
    //    save.MaxHealth = RuntimePlayer.MaxHealth;
    //    save.HealthRegen = RuntimePlayer.HealthRegen;
    //    save.Mana = RuntimePlayer.Mana;
    //    save.MaxMana = RuntimePlayer.MaxMana;
    //    save.ManaRegen = RuntimePlayer.ManaRegen;
    //    save.Attack = RuntimePlayer.Damage;
    //    save.AttackSpeed = RuntimePlayer.AttackSpeed;
    //    save.Armor = RuntimePlayer.Armor;

    //    // Сохраняем действие баффа лягушки
    //    save.tempMaxHealth = RuntimePlayer.tempMaxHealth;
    //    save.tempHealth = RuntimePlayer.tempHealth;
    //    save.tempDamage = RuntimePlayer.tempDamage;

    //    // Скиллы
    //    save.AntiHole = RuntimePlayer.AntiHole;
    //    save.Luck = RuntimePlayer.Luck;
    //    save.SkillPoints = RuntimePlayer.SkillPoints;
    //    save.ChanceToCraftTwice = RuntimePlayer.ChanceToCraftTwice;
    //    save.ChanceNotToDelete = RuntimePlayer.ChanceNotToDelete;
    //    save.openedInvCases = RuntimePlayer.openedInvCases;
    //    save.openedRuneCases = RuntimePlayer.openedRuneCases;

    //    // Индивидуальная абилка
    //    save.HACharge = RuntimePlayer.HACharge;
    //    save.HALvl = RuntimePlayer.HALvl;
    //    save.HAMaxCharge = RuntimePlayer.HAMaxCharge;
    //    save.HAMaxLvl = RuntimePlayer.HAMaxLvl;
    //    save.Ability = RuntimePlayer.Ability;
    //    save.HAChargePower = RuntimePlayer.HAChargePower;

    //    // Уровень и деньги
    //    save.Lvl = RuntimePlayer.Lvl;
    //    save.MaxExp = RuntimePlayer.MaxExp;
    //    save.Exp = RuntimePlayer.Exp;
    //    save.ExpMulty = RuntimePlayer.ExpMulty;
    //    save.Money = RuntimePlayer.Money;
    //    save.ExtraExpMod = RuntimePlayer.ExtraExpMod;
    //    save.ExtraExpMulty = RuntimePlayer.ExtraExpMulty;
    //    save.ExtraMoneyMod = RuntimePlayer.ExtraMoneyMod;
    //    save.ExtraMoneyMulty = RuntimePlayer.ExtraMoneyMulty;

    //    // Шансы
    //    save.LightChance = RuntimePlayer.LightStrikeChance;
    //    save.MediumChance = RuntimePlayer.MediumStrikeChance;
    //    save.HeavyChance = RuntimePlayer.HeavyStrikeChance;
    //    save.DodgeChance = RuntimePlayer.DodgeChance;
    //    save.CriticalStrikeMulty = RuntimePlayer.CriticalStrikeMulty;
    //    save.CriticalStrikeChance = RuntimePlayer.CriticalStrikeChance;

    //    save.isHadVillage = RuntimePlayer.isHadVillage;

    //    // Различные типы
    //    save.ArmorType = RuntimePlayer.ArmorType;
    //    save.AttackType = RuntimePlayer.AttackType;
    //    save.Element = RuntimePlayer.Element;

    //    // Сохранение использования зелий маны
    //    save.manaPotionCounter = INV.manaPotionCounter;

    //    // Локация
    //    save.Location = LocationsController.Instance.CurrentLocation;

    //    // Сохранение деревни
    //    save.villageName = PVA.villageName;
    //    save.villagePopularity = PVA.villagePopularity;
    //    save.villageCapacity = PVA.villageCapacity;
    //    save.villageMaxCapacity = PVA.villageMaxCapacity;

    //    save.villageProducedItems = new List<producingItem>();
    //    for (int i = 0; i < PVA.currentProducingItems.Count; i++)
    //    {
    //        save.villageProducedItems.Add(PVA.currentProducingItems[i]);
    //    }

    //    save.villageParts = new List<SVillagePart>();
    //    for (int i = 0; i < PVA.VillageParts.Length; i++)
    //    {
    //        save.villageParts.Add(new SVillagePart()
    //        {
    //            vilPart = PVA.VillageParts[i].vilPart,
    //            currentIncome = PVA.VillageParts[i].currentIncome,
    //            currentLvl = PVA.VillageParts[i].currentLvl,
    //            currentPopularity = PVA.VillageParts[i].currentPopularity,
    //            currentWorkers = PVA.VillageParts[i].currentWorkers,
    //            upgradeCost = PVA.VillageParts[i].upgradeCost,
    //            upgradeMulty = PVA.VillageParts[i].upgradeMulty,
    //        });
    //    }

    //    //var locList = FindObjectOfType<LocationsController>();
    //    //// Сохранение найденных локаций
    //    //for (int i = 0; i < locList.foundedLocations.Count; i++)
    //    //{
    //    //    save.savedLocations.Add(locList.foundedLocations[i]);
    //    //}

    //    // Сохранение скиллов
    //    for (int skill = 0; skill < SMr.AllSkills.Length; skill++)
    //    {
    //        save.savedSkills.Add(new SSkill()
    //        {
    //            Lvl = SMr.AllSkills[skill].Lvl,
    //            Cost = SMr.AllSkills[skill].Cost,
    //            Var = SMr.AllSkills[skill].currentVariable,
    //        });
    //    }

    //    // Сохранение инвентаря
    //    for (int i = 0; i < INV.inventory.Length; i++)
    //    {
    //        if (INV.inventory[i].Icon != null)
    //        {
    //            save.savedInv.Add(new SItem() { ItemID = INV.inventory[i].ItemID, Stack = INV.inventory[i].Stack, ItemPos = i });
    //        }
    //    }

    //    // Сохранение баффов
    //    for (int buff = 0; buff < BM.activeBuffs.Count; buff++)
    //    {
    //        save.savedBuffs.Add(new SBuff() { Duration = BM.activeBuffs[buff].Duration, sType = BM.activeBuffs[buff].Type });
    //    }

    //    // Сохранение магии
    //    for (int mag = 0; mag < MM.playerMagic.Count; mag++)
    //    {
    //        save.savedMagic.Add(new SMagic()
    //        {
    //            magicType = MM.playerMagic[mag].magicType,
    //            actionVar = MM.playerMagic[mag].actionVar,
    //            cooldownTimeMax = MM.playerMagic[mag].cooldownTimeMax,
    //            currentCooldown = MM.playerMagic[mag].currentCooldown
    //        });
    //    }

    //    if (saveName == "AutoSave")
    //    {
    //        FindObjectOfType<SaveButtonsLoader>().UpdateAutoSaveSlot();
    //    }

    //    try
    //    {
    //        if (!Directory.Exists(Application.persistentDataPath + "/files"))
    //            Directory.CreateDirectory(Application.persistentDataPath + "/files");
    //        FileStream fs = new FileStream(Application.persistentDataPath + "/files/" + saveName + ".sv", FileMode.Create);
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        formatter.Serialize(fs, save);
    //        fs.Close();
    //        // ----- Запись данных закончена -------
    //        Debug.Log("Игра сохранена!");
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log("Ошибка при сохранении");
    //        Debug.Log(e.Message);
    //    }
    //}
    //// Загрузка игры
    //public void LoadGame(string saveName)
    //{
    //    if (File.Exists(Application.persistentDataPath + "/files/" + saveName + ".sv"))
    //    {
    //        FileStream fs = new FileStream(Application.persistentDataPath + "/files/" + saveName + ".sv", FileMode.Open);
    //        BinaryFormatter formatter = new BinaryFormatter();

    //        try
    //        {
    //            Saver save = (Saver)formatter.Deserialize(fs);

    //            // Загрузка даты
    //            GameTimeFlowController.Instance.CurrentHour = save.CurrentHour;
    //            GameTimeFlowController.Instance.CurrentDay = save.CurrentDay;
    //            GameTimeFlowController.Instance.CurrentMonth = save.CurrentMonth;
    //            GameTimeFlowController.Instance.CurrentYear = save.CurrentYear;
    //            FindObjectOfType<GameTimeFlowController>().SetTimeFlowEvents(save.savedTimeEvents);

    //            RuntimePlayer.isFirstEnter = save.isFirstEnter;

    //            // Загружать здесь, НЕ ЗАБУДЬ!
    //            RuntimePlayer.Name = save.PlayerName;
    //            RuntimePlayer.Health = save.Health;
    //            RuntimePlayer.MaxHealth = save.MaxHealth;
    //            RuntimePlayer.HealthRegen = save.HealthRegen;
    //            RuntimePlayer.Mana = save.Mana;
    //            RuntimePlayer.MaxMana = save.MaxMana;
    //            RuntimePlayer.ManaRegen = save.ManaRegen;
    //            RuntimePlayer.Damage = save.Attack;
    //            RuntimePlayer.AttackSpeed = save.AttackSpeed;
    //            RuntimePlayer.Armor = save.Armor;

    //            // Загружаем действие баффа лягушки
    //            RuntimePlayer.tempMaxHealth = save.tempMaxHealth;
    //            RuntimePlayer.tempHealth = save.tempHealth;
    //            RuntimePlayer.tempDamage = save.tempDamage;

    //            // Скиллы
    //            RuntimePlayer.AntiHole = save.AntiHole;
    //            RuntimePlayer.Luck = save.Luck;
    //            RuntimePlayer.SkillPoints = save.SkillPoints;
    //            RuntimePlayer.ChanceToCraftTwice = save.ChanceToCraftTwice;
    //            RuntimePlayer.ChanceNotToDelete = save.ChanceNotToDelete;
    //            RuntimePlayer.openedInvCases = save.openedInvCases;
    //            RuntimePlayer.openedRuneCases = save.openedRuneCases;

    //            // Индивидуальная абилка
    //            RuntimePlayer.HACharge = save.HACharge;
    //            RuntimePlayer.HALvl = save.HALvl;
    //            RuntimePlayer.HAMaxCharge = save.HAMaxCharge;
    //            RuntimePlayer.HAMaxLvl = save.HAMaxLvl;
    //            RuntimePlayer.Ability = save.Ability;
    //            RuntimePlayer.HAChargePower = save.HAChargePower;

    //            // Уровень и деньги
    //            RuntimePlayer.Lvl = save.Lvl;
    //            RuntimePlayer.MaxExp = save.MaxExp;
    //            RuntimePlayer.Exp = save.Exp;
    //            RuntimePlayer.ExpMulty = save.ExpMulty;
    //            RuntimePlayer.Money = save.Money;
    //            RuntimePlayer.ExtraExpMod = save.ExtraExpMod;
    //            RuntimePlayer.ExtraMoneyMod = save.ExtraMoneyMod;
    //            RuntimePlayer.ExtraExpMulty = save.ExtraExpMulty;
    //            RuntimePlayer.ExtraMoneyMulty = save.ExtraMoneyMulty;

    //            // Различные шансы
    //            RuntimePlayer.DodgeChance = save.DodgeChance;
    //            RuntimePlayer.LightStrikeChance = save.LightChance;
    //            RuntimePlayer.MediumStrikeChance = save.MediumChance;
    //            RuntimePlayer.HeavyStrikeChance = save.HeavyChance;
    //            RuntimePlayer.CriticalStrikeMulty = save.CriticalStrikeMulty;
    //            RuntimePlayer.CriticalStrikeChance = save.CriticalStrikeChance;

    //            // Загрузка деревни игрока
    //            RuntimePlayer.isHadVillage = save.isHadVillage;

    //            // Типы
    //            RuntimePlayer.ArmorType = save.ArmorType;
    //            RuntimePlayer.AttackType = save.AttackType;
    //            RuntimePlayer.Element = save.Element;

    //            // Счётчик зелья маны
    //            INV.manaPotionCounter = save.manaPotionCounter;

    //            // Локация
    //            LocationsController.Instance.SetLocationAfterLoading(save.Location);

    //            var locList = FindObjectOfType<LocationsController>();

    //            // Загрузка деревни
    //            PVA.SetDefault();

    //            PVA.villageName = save.villageName;
    //            PVA.villagePopularity = save.villagePopularity;
    //            PVA.villageCapacity = save.villageCapacity;
    //            PVA.villageMaxCapacity = save.villageMaxCapacity;

    //            for (int i = 0; i < PVA.currentProducingItems.Count; i++)
    //            {
    //                PVA.currentProducingItems.Add(save.villageProducedItems[i]);
    //            }
    //            for (int i = 0; i < PVA.VillageParts.Length; i++)
    //            {
    //                PVA.VillageParts[i].currentIncome = save.villageParts[i].currentIncome;
    //                PVA.VillageParts[i].currentLvl = save.villageParts[i].currentLvl;
    //                PVA.VillageParts[i].currentPopularity = save.villageParts[i].currentPopularity;
    //                PVA.VillageParts[i].currentWorkers = save.villageParts[i].currentWorkers;
    //                PVA.VillageParts[i].upgradeCost = save.villageParts[i].upgradeCost;
    //                PVA.VillageParts[i].upgradeMulty = save.villageParts[i].upgradeMulty;
    //            }

    //            // Загрузка найденных локаций
    //            //for (int i = 0; i < save.savedLocations.Count; i++)
    //            //{
    //            //    locList.foundedLocations.Add(save.savedLocations[i]);
    //            //}
    //            //if (save.savedLocations.Count > 0)
    //            //    LM.buttonFoundedLocations.SetActive(true);
    //            //else
    //            //    LM.buttonFoundedLocations.SetActive(false);
    //            // Загрузка скиллов
    //            for (int skill = 0; skill < SMr.AllSkills.Length; skill++)
    //            {
    //                SMr.AllSkills[skill].Cost = save.savedSkills[skill].Cost;
    //                SMr.AllSkills[skill].Lvl = save.savedSkills[skill].Lvl;
    //                SMr.AllSkills[skill].currentVariable = save.savedSkills[skill].Var;
    //            }
    //            // Загрузка инвентаря
    //            INV.InventoryInitialization();
    //            INV.OpenSavedCases();
    //            for (int i = 0; i < save.savedInv.Count; i++)
    //            {
    //                INV.AddItemOnSlot(save.savedInv[i].ItemID, save.savedInv[i].Stack, save.savedInv[i].ItemPos);
    //            }
    //            INV.ReDrawInventory();
    //            // Загрузка баффов
    //            for (int buff = 0; buff < save.savedBuffs.Count; buff++)
    //            {
    //                BM.LoadBuff(save.savedBuffs[buff].sType, save.savedBuffs[buff].Duration);
    //            }
    //            if (BM.activeBuffs.Count > 0)
    //                BM.FullBuffPanelPlayer.SetActive(true);
    //            else
    //                BM.FullBuffPanelPlayer.SetActive(false);
    //            // Загрузка магии
    //            for (int mag = 0; mag < save.savedMagic.Count; mag++)
    //            {
    //                MM.AddMagicToPlayer(save.savedMagic[mag].magicType);

    //                // Инициализируем новые значения
    //                MM.playerMagic[mag].actionVar = save.savedMagic[mag].actionVar;
    //                MM.playerMagic[mag].currentCooldown = save.savedMagic[mag].currentCooldown;
    //                MM.playerMagic[mag].cooldownTimeMax = save.savedMagic[mag].cooldownTimeMax;
    //            }
    //            if (saveName == "AutoSave")
    //            {
    //                FindObjectOfType<SaveButtonsLoader>().UpdateAutoSaveSlot();
    //            }

    //            // Добавление новых переменных
    //        }
    //        catch (System.Exception e)
    //        {
    //            Debug.Log(e.Message);

    //            SetDefaultSettings();
    //        }
    //        finally
    //        {
    //            fs.Close();
    //        }

    //        Debug.Log("Игра загружена!");
    //    }
    //}

    //// Установка дефолтных значений переменных игрока при старте игры
    //public void SetDefaultSettings()
    //{
    //    // Обнуление всех бафов
    //    FindObjectOfType<BuffManager>().NullAllBuffs();

    //    // Переменные деревни игрока
    //    FindObjectOfType<PlayerVillageActivity>().SetDefault();

    //    // Переменные игрового времени
    //    GameTimeFlowController.Instance.CurrentDay = 1;
    //    GameTimeFlowController.Instance.CurrentHour = 10;
    //    GameTimeFlowController.Instance.CurrentMonth = 1;
    //    GameTimeFlowController.Instance.CurrentYear = 1;
    //    FindObjectOfType<GameTimeFlowController>().ForceFinishAllGameTimeFlowEvents();

    //    // Переменные игрока
    //    RuntimePlayer.isFirstEnter = true;
    //    RuntimePlayer.Name = "";
    //    RuntimePlayer.MaxHealth = 100;
    //    RuntimePlayer.Health = 100;
    //    RuntimePlayer.HealthRegen = 0;
    //    RuntimePlayer.MaxMana = 0;
    //    RuntimePlayer.ManaRegen = 0;
    //    RuntimePlayer.Mana = 0;
    //    RuntimePlayer.Armor = 0;
    //    RuntimePlayer.Damage = 10;
    //    RuntimePlayer.AttackSpeed = 1;
    //    RuntimePlayer.isStun = false;

    //    // Бафф лягушки
    //    RuntimePlayer.tempDamage = 0;
    //    RuntimePlayer.tempHealth = 0;
    //    RuntimePlayer.tempMaxHealth = 0;

    //    // Сброс абилки персонажа
    //    FindObjectOfType<HeroAbilityManager>().ResetHeroAbility();

    //    // Скиллы
    //    RuntimePlayer.AntiHole = false;
    //    RuntimePlayer.Luck = 0;
    //    RuntimePlayer.SkillPoints = 0;
    //    RuntimePlayer.ChanceNotToDelete = 0;
    //    RuntimePlayer.ChanceToCraftTwice = 0;
    //    RuntimePlayer.openedInvCases = 19;
    //    RuntimePlayer.openedRuneCases = 0;

    //    // Уровень и деньги
    //    RuntimePlayer.Lvl = 1;
    //    RuntimePlayer.Exp = 0;
    //    RuntimePlayer.MaxExp = 50;
    //    RuntimePlayer.ExpMulty = 50;
    //    RuntimePlayer.Money = 0;

    //    RuntimePlayer.ExtraExpMod = 0;
    //    RuntimePlayer.ExtraMoneyMod = 0;

    //    // Различные шансы
    //    RuntimePlayer.DodgeChance = 10;
    //    RuntimePlayer.LightStrikeChance = 95;
    //    RuntimePlayer.MediumStrikeChance = 65;
    //    RuntimePlayer.HeavyStrikeChance = 30;
    //    RuntimePlayer.CriticalStrikeChance = 1;
    //    RuntimePlayer.CriticalStrikeMulty = 2;

    //    RuntimePlayer.isHadVillage = false;

    //    // Счётчик зелья маны
    //    INV.manaPotionCounter = 0;

    //    // Типы
    //    RuntimePlayer.AttackType = CharactersLibrary.CharacterAttackType.Melee;
    //    RuntimePlayer.ArmorType = CharactersLibrary.CharacterArmorType.None;
    //    RuntimePlayer.Element = CharactersLibrary.CharacterElement.None;

    //    // Локация
    //    //LocationsController.CurrentLocation = LocationsController.Location.GreenForest;
    //    //LM.ChangeLocationAfterLoading(LocationsController.CurrentLocation);
    //    //LM.foundedLocations = new List<LocationsController.Location>();
    //    //LM.CheckLocations();

    //    // Функции
    //    SMr.NullSkills();
    //    INV.InventoryInitialization();
    //    INV.NullInventory();
    //    INV.NullMouseSlot();
    //    INV.ReDrawInventory();
    //    PM.CloseAllPlayerPanels(-1);
    //    PM.CloseAllActionPanels();
    //    FindObjectOfType<MagicManager>().NullAllMagic();
    //    MM.magicSpawnObject.SetActive(false);
    //}

    //// Загрузка состояния игры после запуска сцены
    //public void LoadAfterLoading()
    //{
    //    // Загружаю состояния игры 
    //    string loadGameState = FindObjectOfType<SaveButtonsLoader>().loadGameState;

    //    Debug.Log("Инициализация состояния: " + loadGameState);

    //    // Применяю настройки для состояния
    //    if (loadGameState == "NewGame" || loadGameState == "")
    //    {
    //        SetDefaultSettings();

    //        if (RuntimePlayer.isFirstEnter)
    //        {
    //            RuntimePlayer.isFirstEnter = false;
    //            Debug.Log("Первый вход в игру...");
    //            FindObjectOfType<ArtAndVideoManager>().StartArtShow("FirstEnter_01");
    //            FindObjectOfType<StorytellerManager>().StartDialog("Learning_01");
    //        }
    //    }
    //    else
    //    {
    //        LoadGame(loadGameState);
    //    }
    //}

    //private void OnApplicationQuit()
    //{
    //    SaveGame("AutoSave");
    //}
    //private void OnApplicationPause(bool pause)
    //{
    //    if (pause)
    //    {
    //        SaveGame("AutoSave");
    //    }
    //}
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu))
    //    {
    //        if (!BattleController.Instance.IsBattle)
    //            SaveGame("AutoSave");
    //    }
    //}
    //private void Awake()
    //{
    //    // Ищем все контроллеры ...
    //    BH = FindObjectOfType<BattleController>();
    //    GH = FindObjectOfType<GameController>();
    //    CM = FindObjectOfType<CharactersLibrary>();
    //    LM = FindObjectOfType<LocationsController>();
    //    INV = FindObjectOfType<Inventory>();
    //    SMr = FindObjectOfType<SkillsManager>();
    //    BM = FindObjectOfType<BuffManager>();
    //    MM = FindObjectOfType<MagicManager>();
    //    PVA = FindObjectOfType<PlayerVillageActivity>();
    //}
    //private void Start()
    //{
    //    SM = this;
    //}
}
