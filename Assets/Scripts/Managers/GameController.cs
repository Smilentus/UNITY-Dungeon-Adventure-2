using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance
    { 
        get 
        { 
            if (instance == null)
            {
                instance = FindObjectOfType<GameController>();
            }

            return instance; 
        }
    }


    [Header("Ссылка на LocationsManager")]
    public LocationsController m_locationManager;
    [Header("Ссылка на Inventory")]
    public Inventory m_inventory;


    [Header("Префаб информационного текста")]
    public GameObject EventText;
    [Header("Родитель для евентов")]
    public Transform EventParent;
    [Header("Панель скролов")]
    public ScrollRect eventScroll;


    [Header("Окно информации")]
    public GameObject Blocker;
    public GameObject DeathBox;


    private void Start()
    {
        instance = this;

        FindObjectOfType<faderScript>().FadeScreenOut();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GlobalWindowsController.Instance.IsWindowShown(typeof(AcceptGlobalWindow)))
            {
                Blocker.SetActive(false);
                GlobalWindowsController.Instance.TryHideGlobalWindow(typeof(AcceptGlobalWindow));
            }
            else
            {
                Blocker.SetActive(true);
                GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(AcceptGlobalWindow), new AcceptGlobalWindowData() {
                    GlobalWindowTitle = "Информация",
                    GlobalWindowDescription = "Вы действительно хотите выйти в меню?",
                    ApplyButtonText = "Принять",
                    CancelButtonText = "Отменить",
                    OnApply = ExitToMainMenu
                });
            }
        }
    }

    // Добавление текста в панель ивентов
    public void AddEventText(string text)
    {
        // Если текстовых полей >= Х, то удаляем
        if (EventParent.childCount >= 256)
        {
            for (int i = 256; i < EventParent.childCount; i++)
            {
                Destroy(EventParent.GetChild(i).gameObject);
            }
        }

        GameObject newText = Instantiate(EventText, EventParent);
        newText.GetComponent<Text>().text = "[" + GameTimeFlowController.Instance.DateNow() + "]\n" + text + "\n";
        newText.transform.SetAsFirstSibling();

        eventScroll.normalizedPosition = new Vector2(eventScroll.normalizedPosition.x, 1);
    }

    // Отображение информационного окна
    /// <summary>
    /// 0 - Событие. 1 - Информация.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="action"></param>
    public void ShowMessageText(string message, string title = "[Информация]")
    {
        Blocker.SetActive(true);

        GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(InfoGlobalWindow), new InfoGlobalWindowData() { 
            GlobalWindowTitle = title,
            InfoMessage = message,
            ApplyButtonText = "Принять",
            OnApply = HideMessageText
        });
    }
    // Закрытие информационного окна
    public void HideMessageText()
    {
        Blocker.SetActive(false);
    }

    // DeathBox открытие
    public void ShowDeathBox(string deathText)
    {
        Player.isDeath = true;
        DeathBox.SetActive(true);
        DeathBox.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = deathText;
    }


    public void ExitToMainMenu()
    {
        FindObjectOfType<SavingManager>().SaveGame("AutoSave");
        FindObjectOfType<faderScript>().FadeScreen("MenuScene");
        Blocker.SetActive(true);
    }


    public void LoadAutoSave()
    {
        FindObjectOfType<SaveButtonsLoader>().LoadGame("AutoSave");
        Blocker.SetActive(true);
    }

    // Действие при нажатии на кнопку локации
    public void PressDirectionButton(LocationProfile locationToTravel)
    {
        LocationsController.Instance.TravelToLocation(locationToTravel, 0);
    }

    // Действие при нажатии на кнопку игрового ивента
    public void PressActionButton(BaseGameEventProfile baseGameEventProfile)
    {
        //switch(act)
        //{
        //    case LocationsController.Action.None:
        //        // Ничего не будет
        //        break;
        //    case LocationsController.Action.TalkToBarmen:
        //        FindObjectOfType<StorytellerManager>().StartDialog("TalkToBarmen_01");
        //        break;
        //    case LocationsController.Action.YF_Fight:
        //        YellowForestLocation();
        //        break;
        //    case LocationsController.Action.RF_Fight:
        //        RedForestLocation();
        //        break;
        //    case LocationsController.Action.GF_Fight:
        //        GreenForestLocation();
        //        break;
        //    case LocationsController.Action.BF_Fight:
        //        BlueForestLocation();
        //        break;
        //    case LocationsController.Action.WF_Fight:
        //        WhiteForestLocation();
        //        break;
        //    case LocationsController.Action.Ocean_Fight:
        //        OceanHoleLocation();
        //        break;
        //    case LocationsController.Action.LowMountain_Fight:
        //        LowMountainLocation();
        //        break;
        //    case LocationsController.Action.HighMountain_Fight:
        //        HighMountainLocation();
        //        break;
        //    case LocationsController.Action.HellPath_Fight:
        //        HellPathLocation();
        //        break;
        //    case LocationsController.Action.HellDiablo_Fight:
        //        HellDiabloLocation();
        //        break;
        //    case LocationsController.Action.HellCastle_Fight:
        //        HellCastleLocation();
        //        break;
        //    case LocationsController.Action.Dungeon_Fight:
        //        DungeonLocation();
        //        break;
        //    case LocationsController.Action.RH_WatchRoom:
        //        if (m_inventory.IsContainItem("RabbitInCage") && !FindObjectOfType<BuffManager>().isBuffOnAction(Buff.BuffType.Happiness, 0))
        //        {
        //            // Удаляем клетку из инвентаря (Выпускаем зверя)
        //            m_inventory.CheckItemForDelete("RabbitInCage", 1);
        //            FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Happiness);
        //        }
        //        else
        //        {
        //            ShowMessageText("Подсказка: Найдите в КРАСНОМ лесу КРАСНОГО кролика и поймайте его в клетку.");
        //        }
        //        break;
        //    case LocationsController.Action.UM_MineResources:
        //        MineLocation();
        //        break;
        //    case LocationsController.Action.GF_CutResources:
        //        ForestCutterLocation();
        //        break;
        //}
    }

    // Различные функции V V V
    // Получение урона
    public void TakeDamage(int dmg, bool ignoreArmor)
    {
        if (ignoreArmor)
        {
            Player.Health -= dmg;
            AddEventText("Вы получили урон через броню: " + dmg);
        }
        else
        {
            if (dmg > Player.Armor)
            {
                Player.Health -= (dmg - Player.Armor);
                AddEventText("Вы получили урон: " + (dmg - Player.Armor));
            }
            else
                AddEventText("Броня заблокировала урон.");
        }
    }
    // Получение опыта
    public void GiveExp(double exp)
    {
        // Подсчёт доп. процентного опыта
        double extraExp = exp * (Player.ExtraExpMod / 100);
        // Опыт, который дадим
        exp += extraExp;
        Player.Exp += exp;

        string info = "Получено: " + exp + " ед. опыта!";
        if (Player.ExtraExpMod > 0)
        {
            info = "Получено: " + exp + " + (" + extraExp + ") ед. опыта.";
        }

        AddEventText(info);

        while (Player.Exp >= Player.MaxExp)
        {
            Player.Lvl++;
            Player.SkillPoints += 5;
            Player.Exp -= Player.MaxExp;
            Player.MaxExp += Player.ExpMulty;
            Player.ExpMulty += 1;
            AddEventText("Новый уровень - " + Player.Lvl + "!");
        }
    }
    // Получение монет
    public void GiveMoney(double money)
    {
        double extraMoney = money * Player.ExtraMoneyMod / 100;

        money += extraMoney;
        Player.Money += (int)money;

        string info = "Получено: " + money + " ед. золота!";
        if(Player.ExtraMoneyMod > 0)
        {
            info = "Получено: " + money + " + (" + extraMoney + ") ед. золота.";
        }

        AddEventText(info);
    }
    // Ожидание X часов
    public void WaitSomeTime(int timeToWait)
    {
        if (!BattleController.Instance.IsBattle)
        {
            // Время баффов
            FindObjectOfType<BuffManager>().ChangeBuffActionTime(timeToWait);

            // Добавляем время
            if (timeToWait < 0)
                timeToWait *= -1;
            FindObjectOfType<GameTimeFlowController>().AddTime(timeToWait);
        }
    }
    // --------------

    // Различные локации V V V
    public void RedForestLocation()
    {
        int rndEvent = Random.Range(0, 14);
        int rnd = Random.Range(0, 101);
        //if (rnd <= 10)
        //{
        //    FindObjectOfType<LocationsController>().AddNewExtraLocation(LocationsController.Location.RF_House);
        //}
        switch (rndEvent)
        {
            case 0:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.RedOrc);
                break;
            case 1:
                AddEventText("[Ничего не произошло]");
                break;
            case 2:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.GreenOrc);
                break;
            case 3:
                AddEventText("[Ничего не произошло]");
                break;
            case 4:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.WolfOrc);
                break;
            case 5:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.Enchantress);
                break;
            case 6:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.FireElemental);
                break;
            case 7:
                AddEventText("[Ничего не произошло]");
                break;
            case 8:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.FireGolem);
                break;
            case 9:
                AddEventText("[Ничего не произошло]");
                break;
            case 10:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.ForestElf);
                break;
            case 11:
                AddEventText("[Ничего не произошло]");
                break;
            case 12:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.Vampire);
                break;
            case 13:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.RedRabbit);
                break;
        }
    }
    public void GreenForestLocation()
    {
        int rnd = Random.Range(0, 101);
        //if(rnd <= 10)
        //{
        //    FindObjectOfType<LocationsController>().AddNewExtraLocation(LocationsController.Location.GF_Village);
        //}
        //if(rnd <= 15)
        //{
        //    FindObjectOfType<LocationsController>().AddNewExtraLocation(LocationsController.Location.GF_ForestCutter);
        //}
        int rndEvent = Random.Range(0, 19);
        switch (rndEvent)
        {
            case 0:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.EntWarrior);
                break;
            case 1:
                AddEventText("[Ничего не произошло]");
                break;
            case 2:
                AddEventText("[Ничего не произошло]");
                break;
            case 3:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.EntDefender);
                break;
            case 4:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.EntKing);
                break;
            case 5:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.EntKiller);
                break;
            case 6:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.EntHealer);
                break;
            case 7:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.GreenOrc);
                break;
            case 8:
                AddEventText("[Ничего не произошло]");
                break;
            case 9:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.RedOrc);
                break;
            case 10:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.OrcBetrayer);
                break;
            case 11:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.OrcGienas);
                break;
            case 12:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.WolfOrc);
                break;
            case 13:
                AddEventText("[Ничего не произошло]");
                break;
            case 14:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.Wolverine);
                break;
            case 15:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.Barsuk);
                break;
            case 16:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.GoblinsSquad);
                break;
            case 17:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.ForestElf);
                break;
            case 18:
                AddEventText("[Ничего не произошло]");
                break;
        }
    }
    public void ForestCutterLocation()
    {
        int rndResource = Random.Range(0, 6);

        AddEventText("[Лесопилка] \nДобыто: Древесина " + rndResource + " шт.");
        m_inventory.AddItem("Log", rndResource);
        FindObjectOfType<GameTimeFlowController>().AddTime(rndResource);

        GiveExp(rndResource / 2);
    }
    public void BlueForestLocation()
    {
        int rndEvent = Random.Range(0, 11);
        switch (rndEvent)
        {
            case 0:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.DinosaurMonster);
                break;
            case 1:
                AddEventText("[Ничего не произошло]");
                break;
            case 2:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.DragonMutant);
                break;
            case 3:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.GorgouleMutant);
                break;
            case 4:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.EyeSaurMutant);
                break;
            case 5:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.GrasshopperMutant);
                break;
            case 6:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.HomaMutant);
                break;
            case 7:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.WaterDragonMutant);
                break;
            case 8:
                AddEventText("[Ничего не произошло]");
                break;
            case 9:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.WaterElemental);
                break;
            case 10:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.ForestElf);
                break;
        }
    }
    public void YellowForestLocation()
    {
        int rndEvent = Random.Range(0, 11);
        switch (rndEvent)
        {
            case 0:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.Thief);
                break;
            case 1:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.Enchantress);
                break;
            case 2:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.BirdMage);
                break;
            case 3:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.LightMage);
                break;
            case 4:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.ShadowMage);
                break;
            case 5:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.ForestElf);
                break;
            case 6:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.DamagedMage);
                break;
            case 7:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.SpiraleMage);
                break;
            case 8:
                AddEventText("[Ничего не произошло]");
                break;
            case 9:
                AddEventText("[Ничего не произошло]");
                break;
            case 10:
                AddEventText("[Ничего не произошло]");
                break;
        }
    }
    public void WhiteForestLocation()
    {
        int rndEvent = Random.Range(0, 9);
        switch (rndEvent)
        {
            case 0:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.Ghost);
                break;
            case 1:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.CursedTigers);
                break;
            case 2:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.ForestMonster);
                break;
            case 3:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.ForestScares);
                break;
            case 4:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.SomethingInTheFar);
                break;
            case 5:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.FrozenWolf);
                break;
            case 6:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.WindElemental);
                break;
            case 7:
                AddEventText("[Ничего не произошло]");
                break;
            case 8:
                AddEventText("[Ничего не произошло]");
                break;
        }
    }

    public void MineLocation()
    {
        int rndResource = Random.Range(0, 101);
        int rndAmount = Random.Range(1, 11);
        int oreMultiply = 0;

        if(rndResource <= 5)
        {
            // Титан
            AddEventText("[Шахта] \nДобыто: Титановая руда " + rndAmount + "шт....");
            m_inventory.AddItem("Ore_Titan", rndAmount);
            oreMultiply = 45;
        }
        else if (rndResource <= 12)
        {
            // Лазурит
            AddEventText("[Шахта] \nДобыто: Лазуритовая руда " + rndAmount + "шт....");
            m_inventory.AddItem("Ore_Lazuli", rndAmount);
            oreMultiply = 40;
        }
        else if (rndResource <= 18)
        {
            // Малахит
            AddEventText("[Шахта] \nДобыто: Малахитовая руда " + rndAmount + "шт....");
            m_inventory.AddItem("Ore_Malahit", rndAmount);
            oreMultiply = 35;
        }
        else if (rndResource <= 20)
        {
            // Бронза
            AddEventText("[Шахта] \nДобыто: Бронзовая руда " + rndAmount + "шт....");
            m_inventory.AddItem("Ore_Bronze", rndAmount);
            oreMultiply = 30;
        }
        else if (rndResource <= 26)
        {
            // Золото
            AddEventText("[Шахта] \nДобыто: Золотая руда " + rndAmount + "шт....");
            m_inventory.AddItem("Ore_Gold", rndAmount);
            oreMultiply = 25;
        }
        else if (rndResource <= 36)
        {
            // Железо
            AddEventText("[Шахта] \nДобыто: Железная руда " + rndAmount + "шт....");
            m_inventory.AddItem("Ore_Iron", rndAmount);
            oreMultiply = 20;
        }
        else if(rndResource <= 50)
        {
            // Медь
            AddEventText("[Шахта] \nДобыто: Медная руда " + rndAmount + "шт....");
            m_inventory.AddItem("Ore_Cooper", rndAmount);
            oreMultiply = 15;
        }
        else if(rndResource <= 80)
        {
            // Уголь
            AddEventText("[Шахта] \nДобыто: Уголь " + rndAmount + "шт....");
            m_inventory.AddItem("Coal", rndAmount);
            oreMultiply = 10;
        }
        else
        {
            // Ничего не добыто
            AddEventText("[Шахта] \nНичего не добыто ...");
        }

        GiveExp(rndAmount * oreMultiply);
        FindObjectOfType<GameTimeFlowController>().AddTime(4);
        // Накладываем бафф усталости когда находимся в шахте
    }
    public void DungeonLocation()
    {
        int rndEvent = Random.Range(0, 21);

        int rnd = Random.Range(0, 101);
        //if (rnd <= 15)
        //{
        //    FindObjectOfType<LocationsController>().AddNewExtraLocation(LocationsController.Location.UM_Mine);
        //}
        switch (rndEvent)
        {
            case 0:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.StoneGolem);
                break;
            case 1:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.IronGolem);
                break;
            case 2:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.CrystalGolem);
                break;
            case 3:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.ReptileHealer);
                break;
            case 4:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.ReptileMage);
                break;
            case 5:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.ReptileReaper);
                break;
            case 6:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.ReptileWarrior);
                break;
            case 7:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.Skelet);
                break;
            case 8:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.SkeletMage);
                break;
            case 9:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.SkeletWarrior);
                break;
            case 10:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.Mimick);
                break;
            case 11:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.StoneScorpion);
                break;
            case 12:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.PoisonSlime);
                break;
            case 13:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.StoneMonster);
                break;
            case 14:
                ShowMessageText("Вы наткнулись на ядовитую ловушку!");
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Poison);
                break;
            case 15:
                ShowMessageText("Вы наткнулись на огненную ловушку!");
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Fire);
                break;
            case 16:
                ShowMessageText("Вы наткнулись на ловушку с кольями!");
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Bleeding);
                break;
            case 17:
                AddEventText("[Ничего не произошло]");
                break;
            case 18:
                AddEventText("[Ничего не произошло]");
                break;
            case 19:
                AddEventText("[Ничего не произошло]");
                break;
            case 20:
                AddEventText("[Ничего не произошло]");
                break;
            case 21:
                AddEventText("[Ничего не произошло]");
                break;
        }
    }

    public void HellCastleLocation()
    {
        int rndEvent = Random.Range(0, 9);
        switch (rndEvent)
        {
            case 0:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.FireGolem);
                break;
            case 1:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.DarkGolem);
                break;
            case 2:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.FireElemental);
                break;
            case 3:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.HellHound);
                break;
            case 4:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.HellDeath);
                break;
            case 5:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.HellScareDemon);
                break;
            case 6:
                ShowMessageText("Вы наткнулись на огненную ловушку!");
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Fire);
                break;
            case 7:
                AddEventText("[Ничего не произошло]");
                break;
            case 8:
                AddEventText("[Ничего не произошло]");
                break;
        }
    }
    public void HellDiabloLocation()
    {
        int rndEvent = Random.Range(0, 8);
        switch (rndEvent)
        {
            case 0:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.FireGolem);
                break;
            case 1:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.DarkGolem);
                break;
            case 2:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.FireElemental);
                break;
            case 3:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.DarkElementalOne);
                break;
            case 4:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.Satana);
                break;
            case 5:
                ShowMessageText("Вы наткнулись на огненную ловушку!");
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Fire);
                break;
            case 6:
                AddEventText("[Ничего не произошло]");
                break;
            case 7:
                AddEventText("[Ничего не произошло]");
                break;
        }
    }
    public void HellPathLocation()
    {
        int rndEvent = Random.Range(0, 11);
        switch (rndEvent)
        {
            case 0:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.FireGolem);
                break;
            case 1:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.DarkGolem);
                break;
            case 2:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.FireElemental);
                break;
            case 3:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.HellHound);
                break;
            case 4:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.HellExecuter);
                break;
            case 5:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.HellDemon);
                break;
            case 6:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.HellScareDemon);
                break;
            case 7:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.HellDeath);
                break;
            case 8:
                ShowMessageText("Вы наткнулись на огненную ловушку!");
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Fire);
                break;
            case 9:
                AddEventText("[Ничего не произошло]");
                break;
            case 10:
                AddEventText("[Ничего не произошло]");
                break;
        }
    }

    public void HighMountainLocation()
    {
        int rndEvent = Random.Range(0, 6);
        switch (rndEvent)
        {
            case 0:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.GreenDragon);
                break;
            case 1:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.RedDragon);
                break;
            case 2:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.BlueDragon);
                break;
            case 3:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.LightDragon);
                break;
            case 4:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.WindElemental);
                break;
            case 5:
                AddEventText("[Ничего не произошло]");
                break;
        }
    }
    public void LowMountainLocation()
    {
        int rndEvent = Random.Range(0, 7);
        switch(rndEvent)
        {
            case 0:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.GoldenDragon);
                break;
            case 1:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.DarkDragon);
                break;
            case 2:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.SpectralDragon);
                break;
            case 3:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.ShadowDragon);
                break;
            case 4:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.JadeDragon);
                break;
            case 5:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.EarthElemental);
                break;
            case 6:
                AddEventText("[Ничего не произошло]");
                break;
        }
    }

    public void OceanHoleLocation()
    {
        int rndEvent = Random.Range(0, 5);
        switch(rndEvent)
        {
            case 0:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.WaterElemental);
                break;
            case 1:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.Kraken);
                break;
            case 2:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.WaterDragonMutant);
                break;
            case 3:
                BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.FlyingGollandec);
                break;
            case 4:
                AddEventText("[Ничего не произошло]");
                break;
        }
    }
    // Конец локаций ^ ^ ^
}