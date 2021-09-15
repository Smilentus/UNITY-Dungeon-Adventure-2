using UnityEngine;
using UnityEngine.UI;

public class GameHelper : MonoBehaviour
{
    #region Для обращения
    private static GameHelper GH;
    public static GameHelper _GH
    { get { return GH; } }
    #endregion

    [Header("Ссылка на LocationsManager")]
    public LocationManager LM;
    [Header("Ссылка на Inventory")]
    public Inventory Inv;

    [Header("Префаб информационного текста")]
    public GameObject EventText;
    [Header("Родитель для евентов")]
    public Transform EventParent;
    [Header("Панель скролов")]
    public ScrollRect eventScroll;

    [Header("Окно информации")]
    public GameObject MessageBox;
    public GameObject AcceptBox;
    public GameObject Blocker;
    public GameObject DeathBox;

    private void Start()
    {
        GH = this;

        FindObjectOfType<faderScript>().FadeScreenOut();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (!AcceptBox.activeSelf)
                ShowAcceptBox();

        if (Input.GetKeyDown(KeyCode.M))
        {
            FindObjectOfType<LocationManager>().AddNewExtraLocation(LocationManager.Location.GF_ForestCutter);
            FindObjectOfType<LocationManager>().AddNewExtraLocation(LocationManager.Location.UM_Mine);
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
        newText.GetComponent<Text>().text = "[" + GameTimeScript.DateNow() + "]\n" + text + "\n";
        newText.transform.SetAsFirstSibling();

        eventScroll.normalizedPosition = new Vector2(eventScroll.normalizedPosition.x, 1);
    }

    // Отображение информационного окна
    /// <summary>
    /// 0 - Событие. 1 - Информация.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="action"></param>
    public void ShowMessageText(string text, int action)
    {
        string beginText = "";

        if (action == 0)
            beginText = "[Событие]";
        if (action == 1)
            beginText = "[Информация]";
        MessageBox.GetComponentInChildren<Text>().text = beginText + "\n" + text;

        Blocker.SetActive(true);
        MessageBox.SetActive(true);
    }
    // Закрытие информационного окна
    public void HideMessageText()
    {
        Blocker.SetActive(false);
        MessageBox.SetActive(false);
    }

    // DeathBox открытие
    public void ShowDeathBox(string deathText)
    {
        Player.isDeath = true;
        DeathBox.SetActive(true);
        DeathBox.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = deathText;
    }

    // Открыть панель подтверждения
    public void ShowAcceptBox()
    {
        Blocker.SetActive(true);
        AcceptBox.SetActive(true);
    }
    // Закрыть панель подтверждения
    public void HideAcceptBox()
    {
        Blocker.SetActive(false);
        AcceptBox.SetActive(false);
    }
    // Подтверждение действия
    public void Accept()
    {
        FindObjectOfType<SavingManager>().SaveGame("AutoSave");
        FindObjectOfType<faderScript>().FadeScreen("MenuScene");
        HideAcceptBox();
        Blocker.SetActive(true);
    }

    public void LoadAutoSave()
    {
        FindObjectOfType<SaveButtonsLoader>().LoadGame("AutoSave");
        HideAcceptBox();
        Blocker.SetActive(true);
    }

    // Действие при нажатии на кнопку
    public void PressDirectionButton(LocationManager.Location locToGo)
    {
        FindObjectOfType<LocationManager>().DirectChangeLocation(locToGo, 0);
    }

    // Действие при нажатии на кнопку с действием
    public void PressActionButton(LocationManager.Action act)
    {
        switch(act)
        {
            case LocationManager.Action.None:
                // Ничего не будет
                break;
            case LocationManager.Action.TalkToBarmen:
                FindObjectOfType<StorytellerManager>().StartDialog("TalkToBarmen_01");
                break;
            case LocationManager.Action.YF_Fight:
                YellowForestLocation();
                break;
            case LocationManager.Action.RF_Fight:
                RedForestLocation();
                break;
            case LocationManager.Action.GF_Fight:
                GreenForestLocation();
                break;
            case LocationManager.Action.BF_Fight:
                BlueForestLocation();
                break;
            case LocationManager.Action.WF_Fight:
                WhiteForestLocation();
                break;
            case LocationManager.Action.Ocean_Fight:
                OceanHoleLocation();
                break;
            case LocationManager.Action.LowMountain_Fight:
                LowMountainLocation();
                break;
            case LocationManager.Action.HighMountain_Fight:
                HighMountainLocation();
                break;
            case LocationManager.Action.HellPath_Fight:
                HellPathLocation();
                break;
            case LocationManager.Action.HellDiablo_Fight:
                HellDiabloLocation();
                break;
            case LocationManager.Action.HellCastle_Fight:
                HellCastleLocation();
                break;
            case LocationManager.Action.Dungeon_Fight:
                DungeonLocation();
                break;
            case LocationManager.Action.RH_WatchRoom:
                if (Inv.IsContainItem("RabbitInCage") && !FindObjectOfType<BuffManager>().isBuffOnAction(Buff.BuffType.Happiness, 0))
                {
                    // Удаляем клетку из инвентаря (Выпускаем зверя)
                    Inv.CheckItemForDelete("RabbitInCage", 1);
                    FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Happiness);
                }
                else
                {
                    ShowMessageText("Подсказка: Найдите в КРАСНОМ лесу КРАСНОГО кролика и поймайте его в клетку.", 1);
                }
                break;
            case LocationManager.Action.UM_MineResources:
                MineLocation();
                break;
            case LocationManager.Action.GF_CutResources:
                ForestCutterLocation();
                break;
        }
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
        if (!BattleHelper._BH.isBattle)
        {
            // Время баффов
            FindObjectOfType<BuffManager>().ChangeBuffActionTime(timeToWait);

            // Добавляем время
            if (timeToWait < 0)
                timeToWait *= -1;
            FindObjectOfType<GameTimeScript>().AddTime(timeToWait);
        }
    }
    // --------------

    // Различные локации V V V
    public void RedForestLocation()
    {
        int rndEvent = Random.Range(0, 14);
        int rnd = Random.Range(0, 101);
        if (rnd <= 10)
        {
            FindObjectOfType<LocationManager>().AddNewExtraLocation(LocationManager.Location.RF_House);
        }
        switch (rndEvent)
        {
            case 0:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.RedOrc);
                break;
            case 1:
                AddEventText("[Ничего не произошло]");
                break;
            case 2:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.GreenOrc);
                break;
            case 3:
                AddEventText("[Ничего не произошло]");
                break;
            case 4:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.WolfOrc);
                break;
            case 5:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.Enchantress);
                break;
            case 6:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.FireElemental);
                break;
            case 7:
                AddEventText("[Ничего не произошло]");
                break;
            case 8:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.FireGolem);
                break;
            case 9:
                AddEventText("[Ничего не произошло]");
                break;
            case 10:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.ForestElf);
                break;
            case 11:
                AddEventText("[Ничего не произошло]");
                break;
            case 12:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.Vampire);
                break;
            case 13:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.RedRabbit);
                break;
        }
    }
    public void GreenForestLocation()
    {
        int rnd = Random.Range(0, 101);
        if(rnd <= 10)
        {
            FindObjectOfType<LocationManager>().AddNewExtraLocation(LocationManager.Location.GF_Village);
        }
        if(rnd <= 15)
        {
            FindObjectOfType<LocationManager>().AddNewExtraLocation(LocationManager.Location.GF_ForestCutter);
        }
        int rndEvent = Random.Range(0, 19);
        switch (rndEvent)
        {
            case 0:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.EntWarrior);
                break;
            case 1:
                AddEventText("[Ничего не произошло]");
                break;
            case 2:
                AddEventText("[Ничего не произошло]");
                break;
            case 3:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.EntDefender);
                break;
            case 4:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.EntKing);
                break;
            case 5:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.EntKiller);
                break;
            case 6:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.EntHealer);
                break;
            case 7:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.GreenOrc);
                break;
            case 8:
                AddEventText("[Ничего не произошло]");
                break;
            case 9:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.RedOrc);
                break;
            case 10:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.OrcBetrayer);
                break;
            case 11:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.OrcGienas);
                break;
            case 12:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.WolfOrc);
                break;
            case 13:
                AddEventText("[Ничего не произошло]");
                break;
            case 14:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.Wolverine);
                break;
            case 15:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.Barsuk);
                break;
            case 16:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.GoblinsSquad);
                break;
            case 17:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.ForestElf);
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
        Inv.AddItem("Log", rndResource);
        FindObjectOfType<GameTimeScript>().AddTime(rndResource);

        GiveExp(rndResource / 2);
    }
    public void BlueForestLocation()
    {
        int rndEvent = Random.Range(0, 11);
        switch (rndEvent)
        {
            case 0:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.DinosaurMonster);
                break;
            case 1:
                AddEventText("[Ничего не произошло]");
                break;
            case 2:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.DragonMutant);
                break;
            case 3:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.GorgouleMutant);
                break;
            case 4:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.EyeSaurMutant);
                break;
            case 5:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.GrasshopperMutant);
                break;
            case 6:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.HomaMutant);
                break;
            case 7:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.WaterDragonMutant);
                break;
            case 8:
                AddEventText("[Ничего не произошло]");
                break;
            case 9:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.WaterElemental);
                break;
            case 10:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.ForestElf);
                break;
        }
    }
    public void YellowForestLocation()
    {
        int rndEvent = Random.Range(0, 11);
        switch (rndEvent)
        {
            case 0:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.Thief);
                break;
            case 1:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.Enchantress);
                break;
            case 2:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.BirdMage);
                break;
            case 3:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.LightMage);
                break;
            case 4:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.ShadowMage);
                break;
            case 5:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.ForestElf);
                break;
            case 6:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.DamagedMage);
                break;
            case 7:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.SpiraleMage);
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
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.Ghost);
                break;
            case 1:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.CursedTigers);
                break;
            case 2:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.ForestMonster);
                break;
            case 3:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.ForestScares);
                break;
            case 4:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.SomethingInTheFar);
                break;
            case 5:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.FrozenWolf);
                break;
            case 6:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.WindElemental);
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
            Inv.AddItem("Ore_Titan", rndAmount);
            oreMultiply = 45;
        }
        else if (rndResource <= 12)
        {
            // Лазурит
            AddEventText("[Шахта] \nДобыто: Лазуритовая руда " + rndAmount + "шт....");
            Inv.AddItem("Ore_Lazuli", rndAmount);
            oreMultiply = 40;
        }
        else if (rndResource <= 18)
        {
            // Малахит
            AddEventText("[Шахта] \nДобыто: Малахитовая руда " + rndAmount + "шт....");
            Inv.AddItem("Ore_Malahit", rndAmount);
            oreMultiply = 35;
        }
        else if (rndResource <= 20)
        {
            // Бронза
            AddEventText("[Шахта] \nДобыто: Бронзовая руда " + rndAmount + "шт....");
            Inv.AddItem("Ore_Bronze", rndAmount);
            oreMultiply = 30;
        }
        else if (rndResource <= 26)
        {
            // Золото
            AddEventText("[Шахта] \nДобыто: Золотая руда " + rndAmount + "шт....");
            Inv.AddItem("Ore_Gold", rndAmount);
            oreMultiply = 25;
        }
        else if (rndResource <= 36)
        {
            // Железо
            AddEventText("[Шахта] \nДобыто: Железная руда " + rndAmount + "шт....");
            Inv.AddItem("Ore_Iron", rndAmount);
            oreMultiply = 20;
        }
        else if(rndResource <= 50)
        {
            // Медь
            AddEventText("[Шахта] \nДобыто: Медная руда " + rndAmount + "шт....");
            Inv.AddItem("Ore_Cooper", rndAmount);
            oreMultiply = 15;
        }
        else if(rndResource <= 80)
        {
            // Уголь
            AddEventText("[Шахта] \nДобыто: Уголь " + rndAmount + "шт....");
            Inv.AddItem("Coal", rndAmount);
            oreMultiply = 10;
        }
        else
        {
            // Ничего не добыто
            AddEventText("[Шахта] \nНичего не добыто ...");
        }

        GiveExp(rndAmount * oreMultiply);
        FindObjectOfType<GameTimeScript>().AddTime(4);
        // Накладываем бафф усталости когда находимся в шахте
    }
    public void DungeonLocation()
    {
        int rndEvent = Random.Range(0, 21);

        int rnd = Random.Range(0, 101);
        if (rnd <= 15)
        {
            FindObjectOfType<LocationManager>().AddNewExtraLocation(LocationManager.Location.UM_Mine);
        }
        switch (rndEvent)
        {
            case 0:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.StoneGolem);
                break;
            case 1:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.IronGolem);
                break;
            case 2:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.CrystalGolem);
                break;
            case 3:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.ReptileHealer);
                break;
            case 4:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.ReptileMage);
                break;
            case 5:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.ReptileReaper);
                break;
            case 6:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.ReptileWarrior);
                break;
            case 7:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.Skelet);
                break;
            case 8:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.SkeletMage);
                break;
            case 9:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.SkeletWarrior);
                break;
            case 10:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.Mimick);
                break;
            case 11:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.StoneScorpion);
                break;
            case 12:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.PoisonSlime);
                break;
            case 13:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.StoneMonster);
                break;
            case 14:
                ShowMessageText("Вы наткнулись на ядовитую ловушку!", 1);
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Poison);
                break;
            case 15:
                ShowMessageText("Вы наткнулись на огненную ловушку!", 1);
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Fire);
                break;
            case 16:
                ShowMessageText("Вы наткнулись на ловушку с кольями!", 1);
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
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.FireGolem);
                break;
            case 1:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.DarkGolem);
                break;
            case 2:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.FireElemental);
                break;
            case 3:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.HellHound);
                break;
            case 4:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.HellDeath);
                break;
            case 5:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.HellScareDemon);
                break;
            case 6:
                ShowMessageText("Вы наткнулись на огненную ловушку!", 1);
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
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.FireGolem);
                break;
            case 1:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.DarkGolem);
                break;
            case 2:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.FireElemental);
                break;
            case 3:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.DarkElementalOne);
                break;
            case 4:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.Satana);
                break;
            case 5:
                ShowMessageText("Вы наткнулись на огненную ловушку!", 1);
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
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.FireGolem);
                break;
            case 1:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.DarkGolem);
                break;
            case 2:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.FireElemental);
                break;
            case 3:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.HellHound);
                break;
            case 4:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.HellExecuter);
                break;
            case 5:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.HellDemon);
                break;
            case 6:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.HellScareDemon);
                break;
            case 7:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.HellDeath);
                break;
            case 8:
                ShowMessageText("Вы наткнулись на огненную ловушку!", 1);
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
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.GreenDragon);
                break;
            case 1:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.RedDragon);
                break;
            case 2:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.BlueDragon);
                break;
            case 3:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.LightDragon);
                break;
            case 4:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.WindElemental);
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
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.GoldenDragon);
                break;
            case 1:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.DarkDragon);
                break;
            case 2:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.SpectralDragon);
                break;
            case 3:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.ShadowDragon);
                break;
            case 4:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.JadeDragon);
                break;
            case 5:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.EarthElemental);
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
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.WaterElemental);
                break;
            case 1:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.Kraken);
                break;
            case 2:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.WaterDragonMutant);
                break;
            case 3:
                BattleHelper._BH.StartBattle(CharacterManager.CharacterType.FlyingGollandec);
                break;
            case 4:
                AddEventText("[Ничего не произошло]");
                break;
        }
    }
    // Конец локаций ^ ^ ^
}
