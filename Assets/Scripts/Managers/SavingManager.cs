using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SavingManager : MonoBehaviour
{
    private static SavingManager SM;
    public static SavingManager _SM
    { get { return SM; } }

    [System.Serializable]
    public class Saver
    {
        // Название сохранения
        public string saveName;
        // Дата сохранения
        public string saveData;

        public bool isFirstEnter;
        public string PlayerName;
        public double MaxHealth, Health;
        public double HealthRegen;
        public double Armor;
        public double MaxMana, Mana;
        public double ManaRegen;
        public double AttackSpeed;
        public double Attack;

        public double tempHealth, tempMaxHealth, tempDamage;

        public bool AntiHole;
        public double Luck;
        public double Money;

        public double ChanceNotToDelete;
        public double ChanceToCraftTwice;

        public double HAChargePower;
        public double HACharge;
        public double HAMaxCharge;
        public int HALvl;
        public int HAMaxLvl;
        public Player.HeroAbility Ability;

        public double Lvl;
        public double Exp;
        public double MaxExp;
        public double ExpMulty;
        public int SkillPoints;

        public double ExtraExpMod;
        public double ExtraExpMulty;
        public double ExtraMoneyMod;
        public double ExtraMoneyMulty;

        public double DodgeChance;
        public double LightChance;
        public double MediumChance;
        public double HeavyChance;
        public double CriticalStrikeChance;
        public double CriticalStrikeMulty;

        public bool isHadVillage;

        public int manaPotionCounter;
        public int openedInvCases;
        public int openedRuneCases;

        public int CurrentHour;
        public int CurrentDay;
        public int CurrentMonth;
        public int CurrentYear;

        public string villageName;
        public double villagePopularity;
        public int villageCapacity;
        public int villageMaxCapacity;
        public List<producingItem> villageProducedItems;
        public List<SVillagePart> villageParts;

        public List<GameTimeFlowEvent> savedTimeEvents;

        public CharacterManager.CharacterArmorType ArmorType;
        public CharacterManager.CharacterAttackType AttackType;
        public CharacterManager.CharacterElement Element;

        public LocationManager.Location Location;

        public List<LocationManager.Location> savedLocations = new List<LocationManager.Location>();

        public List<SSkill> savedSkills = new List<SSkill>();
        public List<SItem> savedInv = new List<SItem>();
        public List<SBuff> savedBuffs = new List<SBuff>();
        public List<SMagic> savedMagic = new List<SMagic>();
    }

    [System.Serializable]
    public class SVillagePart
    {
        public PlayerVillageActivity.villagePart vilPart;
        public int currentLvl;
        public double upgradeCost;
        public double upgradeMulty;
        public int currentWorkers;
        public double currentIncome;
        public int currentPopularity;
    }

    [System.Serializable]
    public class SItem
    {
        public int Stack;
        public string ItemID;
        public int ItemPos;
    }

    [System.Serializable]
    public class SMagic
    {
        public MagicManager.MagicType magicType;
        public double actionVar;
        public int cooldownTimeMax;
        public int currentCooldown;
    }

    [System.Serializable]
    public class SSkill
    {
        public int Lvl;
        public int Cost;
        public double Var;
    }

    [System.Serializable]
    public class SBuff
    {
        public int Duration;
        public Buff.BuffType sType;
    }

    [Header("Ссылки")]
    public BattleHelper BH;
    public GameController GH;
    public CharacterManager CM;
    public LocationManager LM;
    public Inventory INV;
    public PanelsManager PM;
    public SkillsManager SMr;
    public BuffManager BM;
    public MagicManager MM;
    public PlayerVillageActivity PVA;

    [Header("Панель сохранений")]
    public GameObject savingPanel;

    // Открытие меню сохранений 
    public void OpenSavingMenu()
    {
        savingPanel.SetActive(true);
    }

    // Закрытие меню сохранений
    public void CloseSavingMenu()
    {
        savingPanel.SetActive(false);
    }

    // Сохранение игры
    public void SaveGame(string saveName)
    {
        Saver save = new Saver();

        save.isFirstEnter = Player.isFirstEnter;

        // Сохранение даты
        save.CurrentHour = GameTimeFlowController.Instance.CurrentHour;
        save.CurrentDay = GameTimeFlowController.Instance.CurrentDay;
        save.CurrentMonth = GameTimeFlowController.Instance.CurrentMonth;
        save.CurrentYear = GameTimeFlowController.Instance.CurrentYear;
        save.savedTimeEvents = FindObjectOfType<GameTimeFlowController>().CurrentGameTimeFlowEvents;

        // СОХРАНЯТЬ ЗДЕСЬ, НЕ ЗАБУДЬ!
        save.PlayerName = Player.Name;
        save.Health = Player.Health;
        save.MaxHealth = Player.MaxHealth;
        save.HealthRegen = Player.HealthRegen;
        save.Mana = Player.Mana;
        save.MaxMana = Player.MaxMana;
        save.ManaRegen = Player.ManaRegen;
        save.Attack = Player.Damage;
        save.AttackSpeed = Player.AttackSpeed;
        save.Armor = Player.Armor;

        // Сохраняем действие баффа лягушки
        save.tempMaxHealth = Player.tempMaxHealth;
        save.tempHealth = Player.tempHealth;
        save.tempDamage = Player.tempDamage;

        // Скиллы
        save.AntiHole = Player.AntiHole;
        save.Luck = Player.Luck;
        save.SkillPoints = Player.SkillPoints;
        save.ChanceToCraftTwice = Player.ChanceToCraftTwice;
        save.ChanceNotToDelete = Player.ChanceNotToDelete;
        save.openedInvCases = Player.openedInvCases;
        save.openedRuneCases = Player.openedRuneCases;

        // Индивидуальная абилка
        save.HACharge = Player.HACharge;
        save.HALvl = Player.HALvl;
        save.HAMaxCharge = Player.HAMaxCharge;
        save.HAMaxLvl = Player.HAMaxLvl;
        save.Ability = Player.Ability;
        save.HAChargePower = Player.HAChargePower;

        // Уровень и деньги
        save.Lvl = Player.Lvl;
        save.MaxExp = Player.MaxExp;
        save.Exp = Player.Exp;
        save.ExpMulty = Player.ExpMulty;
        save.Money = Player.Money;
        save.ExtraExpMod = Player.ExtraExpMod;
        save.ExtraExpMulty = Player.ExtraExpMulty;
        save.ExtraMoneyMod = Player.ExtraMoneyMod;
        save.ExtraMoneyMulty = Player.ExtraMoneyMulty;

        // Шансы
        save.LightChance = Player.LightStrikeChance;
        save.MediumChance = Player.MediumStrikeChance;
        save.HeavyChance = Player.HeavyStrikeChance;
        save.DodgeChance = Player.DodgeChance;
        save.CriticalStrikeMulty = Player.CriticalStrikeMulty;
        save.CriticalStrikeChance = Player.CriticalStrikeChance;

        save.isHadVillage = Player.isHadVillage;

        // Различные типы
        save.ArmorType = Player.ArmorType;
        save.AttackType = Player.AttackType;
        save.Element = Player.Element;

        // Сохранение использования зелий маны
        save.manaPotionCounter = INV.manaPotionCounter;

        // Локация
        save.Location = LocationManager.CurrentLocation;

        // Сохранение деревни
        save.villageName = PVA.villageName;
        save.villagePopularity = PVA.villagePopularity;
        save.villageCapacity = PVA.villageCapacity;
        save.villageMaxCapacity = PVA.villageMaxCapacity;

        save.villageProducedItems = new List<producingItem>();
        for(int i = 0; i < PVA.currentProducingItems.Count; i++)
        {
            save.villageProducedItems.Add(PVA.currentProducingItems[i]);
        }

        save.villageParts = new List<SVillagePart>();
        for (int i = 0; i < PVA.VillageParts.Length; i++)
        {
            save.villageParts.Add(new SVillagePart()
            {
                vilPart = PVA.VillageParts[i].vilPart,
                currentIncome = PVA.VillageParts[i].currentIncome,
                currentLvl = PVA.VillageParts[i].currentLvl,
                currentPopularity = PVA.VillageParts[i].currentPopularity,
                currentWorkers = PVA.VillageParts[i].currentWorkers,
                upgradeCost = PVA.VillageParts[i].upgradeCost,
                upgradeMulty = PVA.VillageParts[i].upgradeMulty,
            });
        }

        var locList = FindObjectOfType<LocationManager>();
        // Сохранение найденных локаций
        for (int i = 0; i < locList.foundedLocations.Count; i++)
        {
            save.savedLocations.Add(locList.foundedLocations[i]);
        }

        // Сохранение скиллов
        for(int skill = 0; skill < SMr.AllSkills.Length; skill++)
        {
            save.savedSkills.Add(new SSkill() {
                Lvl = SMr.AllSkills[skill].Lvl,
                Cost = SMr.AllSkills[skill].Cost,
                Var = SMr.AllSkills[skill].currentVariable,
            });
        }

        // Сохранение инвентаря
        for(int i = 0; i < INV.inventory.Length; i++)
        {
            if (INV.inventory[i].Icon != null)
            {
                save.savedInv.Add(new SItem() { ItemID = INV.inventory[i].ItemID, Stack = INV.inventory[i].Stack, ItemPos = i });
            }
        }

        // Сохранение баффов
        for(int buff = 0; buff < BM.activeBuffs.Count; buff++)
        {
            save.savedBuffs.Add(new SBuff() { Duration = BM.activeBuffs[buff].Duration, sType = BM.activeBuffs[buff].Type });
        }

        // Сохранение магии
        for (int mag = 0; mag < MM.playerMagic.Count; mag++)
        {
            save.savedMagic.Add(new SMagic() {
                magicType = MM.playerMagic[mag].magicType,
                actionVar = MM.playerMagic[mag].actionVar,
                cooldownTimeMax = MM.playerMagic[mag].cooldownTimeMax,
                currentCooldown = MM.playerMagic[mag].currentCooldown
            });
        }

        if(saveName == "AutoSave")
        {
            FindObjectOfType<SaveButtonsLoader>().UpdateAutoSaveSlot();
        }

        try
        {
            if (!Directory.Exists(Application.persistentDataPath + "/files"))
                Directory.CreateDirectory(Application.persistentDataPath + "/files");
            FileStream fs = new FileStream(Application.persistentDataPath + "/files/" + saveName + ".sv", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, save);
            fs.Close();
            // ----- Запись данных закончена -------
            Debug.Log("Игра сохранена!");
        }
        catch (Exception e)
        {
            Debug.Log("Ошибка при сохранении");
            Debug.Log(e.Message);
        }
    }
    // Загрузка игры
    public void LoadGame(string saveName)
    {
        if (File.Exists(Application.persistentDataPath + "/files/" + saveName + ".sv"))
        {
            FileStream fs = new FileStream(Application.persistentDataPath + "/files/" + saveName + ".sv", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                Saver save = (Saver)formatter.Deserialize(fs);

                // Загрузка даты
                GameTimeFlowController.Instance.CurrentHour = save.CurrentHour;
                GameTimeFlowController.Instance.CurrentDay = save.CurrentDay;
                GameTimeFlowController.Instance.CurrentMonth = save.CurrentMonth;
                GameTimeFlowController.Instance.CurrentYear = save.CurrentYear;
                FindObjectOfType<GameTimeFlowController>().SetTimeFlowEvents(save.savedTimeEvents);

                Player.isFirstEnter = save.isFirstEnter;

                // Загружать здесь, НЕ ЗАБУДЬ!
                Player.Name = save.PlayerName;
                Player.Health = save.Health;
                Player.MaxHealth = save.MaxHealth;
                Player.HealthRegen = save.HealthRegen;
                Player.Mana = save.Mana;
                Player.MaxMana = save.MaxMana;
                Player.ManaRegen = save.ManaRegen;
                Player.Damage = save.Attack;
                Player.AttackSpeed = save.AttackSpeed;
                Player.Armor = save.Armor;

                // Загружаем действие баффа лягушки
                Player.tempMaxHealth = save.tempMaxHealth;
                Player.tempHealth = save.tempHealth;
                Player.tempDamage = save.tempDamage;

                // Скиллы
                Player.AntiHole = save.AntiHole;
                Player.Luck = save.Luck;
                Player.SkillPoints = save.SkillPoints;
                Player.ChanceToCraftTwice = save.ChanceToCraftTwice;
                Player.ChanceNotToDelete = save.ChanceNotToDelete;
                Player.openedInvCases = save.openedInvCases;
                Player.openedRuneCases = save.openedRuneCases;

                // Индивидуальная абилка
                Player.HACharge = save.HACharge;
                Player.HALvl = save.HALvl;
                Player.HAMaxCharge = save.HAMaxCharge;
                Player.HAMaxLvl = save.HAMaxLvl;
                Player.Ability = save.Ability;
                Player.HAChargePower = save.HAChargePower;

                // Уровень и деньги
                Player.Lvl = save.Lvl;
                Player.MaxExp = save.MaxExp;
                Player.Exp = save.Exp;
                Player.ExpMulty = save.ExpMulty;
                Player.Money = save.Money;
                Player.ExtraExpMod = save.ExtraExpMod;
                Player.ExtraMoneyMod = save.ExtraMoneyMod;
                Player.ExtraExpMulty = save.ExtraExpMulty;
                Player.ExtraMoneyMulty = save.ExtraMoneyMulty;

                // Различные шансы
                Player.DodgeChance = save.DodgeChance;
                Player.LightStrikeChance = save.LightChance;
                Player.MediumStrikeChance = save.MediumChance;
                Player.HeavyStrikeChance = save.HeavyChance;
                Player.CriticalStrikeMulty = save.CriticalStrikeMulty;
                Player.CriticalStrikeChance = save.CriticalStrikeChance;

                // Загрузка деревни игрока
                Player.isHadVillage = save.isHadVillage;

                // Типы
                Player.ArmorType = save.ArmorType;
                Player.AttackType = save.AttackType;
                Player.Element = save.Element;

                // Счётчик зелья маны
                INV.manaPotionCounter = save.manaPotionCounter;

                // Локация
                LocationManager.CurrentLocation = save.Location;
                LM.ChangeLocationAfterLoading(LocationManager.CurrentLocation);

                var locList = FindObjectOfType<LocationManager>();

                // Загрузка деревни
                PVA.SetDefault();
                
                PVA.villageName = save.villageName;
                PVA.villagePopularity = save.villagePopularity;
                PVA.villageCapacity = save.villageCapacity;
                PVA.villageMaxCapacity = save.villageMaxCapacity;

                for (int i = 0; i < PVA.currentProducingItems.Count; i++)
                {
                    PVA.currentProducingItems.Add(save.villageProducedItems[i]);
                }
                for (int i = 0; i < PVA.VillageParts.Length; i++)
                {
                    PVA.VillageParts[i].currentIncome = save.villageParts[i].currentIncome;
                    PVA.VillageParts[i].currentLvl = save.villageParts[i].currentLvl;
                    PVA.VillageParts[i].currentPopularity = save.villageParts[i].currentPopularity;
                    PVA.VillageParts[i].currentWorkers = save.villageParts[i].currentWorkers;
                    PVA.VillageParts[i].upgradeCost = save.villageParts[i].upgradeCost;
                    PVA.VillageParts[i].upgradeMulty = save.villageParts[i].upgradeMulty;
                }

                // Загрузка найденных локаций
                for (int i = 0; i < save.savedLocations.Count; i++)
                {
                    locList.foundedLocations.Add(save.savedLocations[i]);
                }
                if (save.savedLocations.Count > 0)
                    LM.buttonFoundedLocations.SetActive(true);
                else
                    LM.buttonFoundedLocations.SetActive(false);
                // Загрузка скиллов
                for (int skill = 0; skill < SMr.AllSkills.Length; skill++)
                {
                    SMr.AllSkills[skill].Cost = save.savedSkills[skill].Cost;
                    SMr.AllSkills[skill].Lvl = save.savedSkills[skill].Lvl;
                    SMr.AllSkills[skill].currentVariable = save.savedSkills[skill].Var;
                }
                // Загрузка инвентаря
                INV.InventoryInitialization();
                INV.OpenSavedCases();
                for (int i = 0; i < save.savedInv.Count; i++)
                {
                    INV.AddItemOnSlot(save.savedInv[i].ItemID, save.savedInv[i].Stack, save.savedInv[i].ItemPos);
                }
                INV.ReDrawInventory();
                // Загрузка баффов
                for(int buff = 0; buff < save.savedBuffs.Count; buff++)
                {
                    BM.LoadBuff(save.savedBuffs[buff].sType, save.savedBuffs[buff].Duration);
                }
                if (BM.activeBuffs.Count > 0)
                    BM.FullBuffPanelPlayer.SetActive(true);
                else
                    BM.FullBuffPanelPlayer.SetActive(false);
                // Загрузка магии
                for (int mag = 0; mag < save.savedMagic.Count; mag++)
                {
                    MM.AddMagicToPlayer(save.savedMagic[mag].magicType);

                    // Инициализируем новые значения
                    MM.playerMagic[mag].actionVar = save.savedMagic[mag].actionVar;
                    MM.playerMagic[mag].currentCooldown = save.savedMagic[mag].currentCooldown;
                    MM.playerMagic[mag].cooldownTimeMax = save.savedMagic[mag].cooldownTimeMax;
                }
                if (saveName == "AutoSave")
                {
                    FindObjectOfType<SaveButtonsLoader>().UpdateAutoSaveSlot();
                }

                // Добавление новых переменных
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);

                SetDefaultSettings();
            }
            finally
            {
                fs.Close();
            }

            Debug.Log("Игра загружена!");
        }
    }

    // Установка дефолтных значений переменных игрока при старте игры
    public void SetDefaultSettings()
    {
        // Обнуление всех бафов
        FindObjectOfType<BuffManager>().NullAllBuffs();

        // Переменные деревни игрока
        FindObjectOfType<PlayerVillageActivity>().SetDefault();

        // Переменные игрового времени
        GameTimeFlowController.Instance.CurrentDay = 1;
        GameTimeFlowController.Instance.CurrentHour = 10;
        GameTimeFlowController.Instance.CurrentMonth = 1;
        GameTimeFlowController.Instance.CurrentYear = 1;
        FindObjectOfType<GameTimeFlowController>().ForceFinishAllGameTimeFlowEvents();

        // Переменные игрока
        Player.isFirstEnter = true;
        Player.Name = "";
        Player.MaxHealth = 100;
        Player.Health = 100;
        Player.HealthRegen = 0;
        Player.MaxMana = 0;
        Player.ManaRegen = 0;
        Player.Mana = 0;
        Player.Armor = 0;
        Player.Damage = 10;
        Player.AttackSpeed = 1;
        Player.isStun = false;

        // Бафф лягушки
        Player.tempDamage = 0;
        Player.tempHealth = 0;
        Player.tempMaxHealth = 0;

        // Сброс абилки персонажа
        FindObjectOfType<HeroAbilityManager>().ResetHeroAbility();

        // Скиллы
        Player.AntiHole = false;
        Player.Luck = 0;
        Player.SkillPoints = 0;
        Player.ChanceNotToDelete = 0;
        Player.ChanceToCraftTwice = 0;
        Player.openedInvCases = 19;
        Player.openedRuneCases = 0;

        // Уровень и деньги
        Player.Lvl = 1;
        Player.Exp = 0;
        Player.MaxExp = 50;
        Player.ExpMulty = 50;
        Player.Money = 0;

        Player.ExtraExpMod = 0;
        Player.ExtraMoneyMod = 0;

        // Различные шансы
        Player.DodgeChance = 10;
        Player.LightStrikeChance = 95;
        Player.MediumStrikeChance = 65;
        Player.HeavyStrikeChance = 30;
        Player.CriticalStrikeChance = 1;
        Player.CriticalStrikeMulty = 2;

        Player.isHadVillage = false;

        // Счётчик зелья маны
        INV.manaPotionCounter = 0;

        // Типы
        Player.AttackType = CharacterManager.CharacterAttackType.Melee;
        Player.ArmorType = CharacterManager.CharacterArmorType.None;
        Player.Element = CharacterManager.CharacterElement.None;

        // Локация
        LocationManager.CurrentLocation = LocationManager.Location.GreenForest;
        LM.ChangeLocationAfterLoading(LocationManager.CurrentLocation);
        LM.foundedLocations = new List<LocationManager.Location>();
        LM.CheckLocations();

        // Функции
        SMr.NullSkills();
        INV.InventoryInitialization();
        INV.NullInventory();
        INV.NullMouseSlot();
        INV.ReDrawInventory();
        PM.CloseAllPlayerPanels(-1);
        PM.CloseAllActionPanels();
        FindObjectOfType<MagicManager>().NullAllMagic();
        MM.magicSpawnObject.SetActive(false);
    }

    // Загрузка состояния игры после запуска сцены
    public void LoadAfterLoading()
    {
        // Загружаю состояния игры 
        string loadGameState = FindObjectOfType<SaveButtonsLoader>().loadGameState;

        Debug.Log("Инициализация состояния: " + loadGameState);

        // Применяю настройки для состояния
        if (loadGameState == "NewGame" || loadGameState == "")
        {
            SetDefaultSettings();

            if(Player.isFirstEnter)
            {
                Player.isFirstEnter = false;
                Debug.Log("Первый вход в игру...");
                FindObjectOfType<ArtAndVideoManager>().StartArtShow("FirstEnter_01");
                FindObjectOfType<StorytellerManager>().StartDialog("Learning_01");
            }
        }
        else
        {
            LoadGame(loadGameState);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame("AutoSave");
    }
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            SaveGame("AutoSave");
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu))
        {
            if(!BattleHelper._BH.isBattle)
                SaveGame("AutoSave");
        }
    }
    private void Awake()
    {
        // Ищем все контроллеры ...
        BH = FindObjectOfType<BattleHelper>();
        GH = FindObjectOfType<GameController>();
        CM = FindObjectOfType<CharacterManager>();
        LM = FindObjectOfType<LocationManager>();
        INV = FindObjectOfType<Inventory>();
        SMr = FindObjectOfType<SkillsManager>();
        BM = FindObjectOfType<BuffManager>();
        MM = FindObjectOfType<MagicManager>();
        PVA = FindObjectOfType<PlayerVillageActivity>();
    }
    private void Start()
    {
        SM = this;
    }
}
