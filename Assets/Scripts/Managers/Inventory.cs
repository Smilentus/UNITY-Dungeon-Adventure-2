using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public enum ItemType
    {
        None,
        Potion,
        Consumable,
        Material,
        Helmet,
        Chestplate,
        Greaves,
        Gloves,
        Necklace,
        Orb,
        RingRegen,
        RingExp,
        RingGold,
        Shield,
        MeleeWeapon,
        RangedWeapon,
        MagicWeapon,
        Rune,
    }

    [Header("Все вещи в игре")]
    public ItemProfile[] AllItems;

    [Space(20)]
    [Header("Вещи путешествующего странника")]
    public ItemProfile[] adventurerItems;
    [Header("Вещи путешествующего священника")]
    public ItemProfile[] preacherItems;
    [Header("Вещи из сундука")]
    public ItemProfile[] ChestItems;

    [Space(20)]
    [Header("Вещи в инвентаре")]
    public ItemProfile[] inventory;
    [Header("Кнопки инвентаря")]
    public GameObject[] Buttons;

    [Header("Слот инвентаря")]
    public GameObject invSlot;
    [Header("Иконка мусорки")]
    public Texture trashCanIcon;

    [Space(20)]
    [Header("Панели инвентарей")]
    public Transform invExtraParent;
    public Transform invParent;
    public Transform gearPanel1;
    public Transform gearPanel2;
    public Transform trashPanel;
    public Transform runePanel;

    [Space(20)]
    [Header("Слот мышки")]
    public Transform mouseIcon;
    public ItemProfile mouseSlot;

    [Header("Панель описания")]
    public GameObject DescrBox;

    [Header("Ссылка на PanelsManager")]
    public PanelsManager PM;

    [HideInInspector]
    [Header("Ёмкость инвентаря")]
    public int InvCapacity = 45;
    [HideInInspector]
    [Header("Счётчик зелья маны")]
    public int manaPotionCounter = 0;
    [HideInInspector]
    public bool isOpened = false;

    [Space(20)]
    [Header("Цвет подсветки")]
    public Color highlightedColor;
    [Header("Цвет обычного слота")]
    public Color normalColor;

    [Space(20)]
    [Header("Кнопка использовать")]
    public GameObject buttonUse;

    public bool isInfinite;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddItem("Potion_Health", 10);
            AddItem("Plank", 100);
            AddItem("Fliry_Sword", 1);
            AddItem("Cooper_Gloves", 1);
        }
    }

    public void OpenSavedCases()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (i < Player.openedInvCases)
            {
                Buttons[i].GetComponent<InvButton>().isActive = true;
                Buttons[i].GetComponent<InvButton>().UpdateState();
            }
            if (i > 70 && i < 71 + Player.openedRuneCases)
            {
                Buttons[i].GetComponent<InvButton>().isActive = true;
                Buttons[i].GetComponent<InvButton>().UpdateState();
            }
        }
    }

    public void InventoryInitialization()
    {
        Buttons = new GameObject[InvCapacity + 34];
        inventory = new ItemProfile[InvCapacity + 34];

        Debug.Log("Очищен инвентарь!");

        for (int i = 0; i < 9; i++)
        {
            GameObject newSlot = Instantiate(invSlot, invExtraParent);

            newSlot.GetComponent<InvButton>().slotNum = i;
            if (i < Player.openedInvCases)
                newSlot.GetComponent<InvButton>().isActive = true;
            else
                newSlot.GetComponent<InvButton>().isActive = false;
            newSlot.GetComponent<InvButton>().UpdateState();

            inventory[i] = new ItemProfile();

            Buttons[i] = newSlot;
        }
        for (int i = 9; i < InvCapacity + 9; i++)
        {
            GameObject newSlot = Instantiate(invSlot, invParent);

            newSlot.GetComponent<InvButton>().slotNum = i;

            if (i < Player.openedInvCases)
                newSlot.GetComponent<InvButton>().isActive = true;
            else
                newSlot.GetComponent<InvButton>().isActive = false;
            newSlot.GetComponent<InvButton>().UpdateState();

            inventory[i] = new ItemProfile();

            Buttons[i] = newSlot;
        }
        for (int i = InvCapacity + 9; i < InvCapacity + 17; i++)
        {
            GameObject newSlot = Instantiate(invSlot, gearPanel1);

            newSlot.GetComponent<InvButton>().slotNum = i;
            newSlot.GetComponent<InvButton>().isActive = true;
            newSlot.GetComponent<InvButton>().UpdateState();

            inventory[i] = new ItemProfile();

            Buttons[i] = newSlot;
        }
        for (int i = InvCapacity + 17; i < InvCapacity + 25; i++)
        {
            GameObject newSlot = Instantiate(invSlot, gearPanel2);

            newSlot.GetComponent<InvButton>().slotNum = i;
            newSlot.GetComponent<InvButton>().isActive = true;
            newSlot.GetComponent<InvButton>().UpdateState();

            inventory[i] = new ItemProfile();

            Buttons[i] = newSlot;
        }
        for (int i = InvCapacity + 25; i < InvCapacity + 34; i++)
        {
            if (i == 70)
                continue;

            GameObject newSlot = Instantiate(invSlot, runePanel);

            newSlot.GetComponent<InvButton>().slotNum = i;

            if (i < InvCapacity + 26 + Player.openedRuneCases)
                newSlot.GetComponent<InvButton>().isActive = true;
            else
                newSlot.GetComponent<InvButton>().isActive = false;
            newSlot.GetComponent<InvButton>().UpdateState();

            inventory[i] = new ItemProfile();

            Buttons[i] = newSlot;
        }

        // Создаём мусорку.
        GameObject Trash = Instantiate(invSlot, trashPanel);
        Trash.GetComponent<InvButton>().slotNum = 70;
        Trash.GetComponent<InvButton>().isActive = true;
        inventory[70] = new ItemProfile();
        Buttons[70] = Trash;
        // Закончили создавать мусорку.
        // РЕГИОНЫ ДЛЯ СЛАБАКОВ.

        Debug.Log("Заполнен инвентарь");

        HighlightSlots();
        ReDrawInventory();
    }

    // Показать описание
    public void ShowDescr(int ID)
    {
        if (inventory[ID].Icon != null && ID < 70)
        {
            string descrText = "Предмет: " + inventory[ID].Name + "\nОписание: " + inventory[ID].Descr + "\n";

            if (inventory[ID].Health != 0)
                descrText += "\nБонус ОЗ: " + inventory[ID].Health;
            if (inventory[ID].Mana != 0)
                descrText += "\nБонус ОМ: " + inventory[ID].Mana;
            if (inventory[ID].Armor != 0)
                descrText += "\nБонус защиты: " + inventory[ID].Armor;
            if (inventory[ID].Damage != 0)
                descrText += "\nБонус урона: " + inventory[ID].Damage;
            if (inventory[ID].AttackSpeed != 0)
                descrText += "\nБонус скорости атаки: " + inventory[ID].AttackSpeed;

            DescrBox.GetComponentInChildren<Text>().text = descrText;
            DescrBox.SetActive(true);
        }
    }

    // Скрыть описание
    public void HideDescr()
    {
        DescrBox.SetActive(false);
    }

    // Открытие и закрытие панели инвентаря
    public void ShowHideInventoryPanel()
    {
        PM.CloseAllPlayerPanels(0);
        PM.OpenHidePlayerPanel(0, !PM.PlayerPanels[0].activeSelf);

        isOpened = PM.PlayerPanels[0].activeSelf;

        EmptyMouseSlot();
        HideDescr();
        ReDrawInventory();
    }

    // Обнуление инвентаря
    public void NullInventory()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = new ItemProfile();
        }
    }

    // Очистка слота мыши
    public void EmptyMouseSlot()
    {
        if (mouseSlot.Icon != null)
        {
            AddItemOnSlot(mouseSlot.ItemID, mouseSlot.Stack, mouseSlot.takedSlot);
            NullMouseSlot();
            ReDrawInventory();
        }
    }

    // Обнуление слота мышки
    public void NullMouseSlot()
    {
        mouseSlot = new ItemProfile();
        ReDrawInventory();
    }

    // Проверка можно ли использовать вещь
    public void CheckItemForUse(int slot)
    {
        if (isOpened)
        {
            if (mouseSlot.Icon != null)
                UseItem(mouseSlot.ItemID);
        }
        else
        {
            UseItem(inventory[slot].ItemID);
        }
    }

    // Использование вещи
    public void UseItem(string ID)
    {
        switch (ID)
        {
            case "Potion_Health":
                if (Player.Health < Player.MaxHealth)
                {
                    Player.Health += 25;

                    CheckItemForDelete(ID, 1);
                }
                else
                    GameController.Instance.ShowMessageText("У Вас и так максимальный запас здоровья!");
                break;
            case "Potion_Mana":
                if (manaPotionCounter > 0)
                {
                    FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.FrogBuff);
                    manaPotionCounter = 0;
                }
                else
                {
                    if (Player.Mana < Player.MaxMana)
                    {
                        Player.Mana += 15;
                        manaPotionCounter = 0;

                        CheckItemForDelete(ID, 1);
                    }
                    else if (Player.Mana >= Player.MaxMana)
                    {
                        GameController.Instance.ShowMessageText("Если Вы продолжите пить эту бяку при максимальном запасе маны, то Вы превратитесь в лягушку. Я серьёзно!(нет)");
                        manaPotionCounter++;
                    }
                }
                break;
            case "Potion_Armor":
                if (!FindObjectOfType<BuffManager>().isBuffOnAction(Buff.BuffType.ArmorBonus, 0))
                {
                    FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.ArmorBonus);

                    CheckItemForDelete(ID, 1);
                }
                else
                    GameController.Instance.ShowMessageText("Ваше тело не выдержит большей нагрузки!");
                break;
            case "Potion_Regen":
                if (!FindObjectOfType<BuffManager>().isBuffOnAction(Buff.BuffType.RegenBonus, 0))
                {
                    FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.RegenBonus);

                    CheckItemForDelete(ID, 1);
                }
                else
                    GameController.Instance.ShowMessageText("Если выпить слишком много восстановительного зелья, то можно умереть!");
                break;
            case "Potion_ManaRegen":
                if (!FindObjectOfType<BuffManager>().isBuffOnAction(Buff.BuffType.ManaRegenBonus, 0))
                {
                    FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.ManaRegenBonus);

                    CheckItemForDelete(ID, 1);
                }
                else
                    GameController.Instance.ShowMessageText("Частое употребление зелья восстановления маны вредит здоровью!");
                break;
            case "Potion_Random": // Зелье случайности
                int rndAction = Random.Range(0, 8);

                CheckItemForDelete(ID, 1);

                switch (rndAction)
                {
                    case 0:
                        FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.FrogBuff);
                        GameController.Instance.ShowMessageText("Зелье изуродовало Вас ещё сильнее, чем Ваша жизнь. Вы стали лягушкой.", "[Событие]");
                        break;
                    case 1:
                        FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.HealthAndManaRegen);
                        GameController.Instance.ShowMessageText("Зелье придало Вам мощи! (Бафф: Усиленное восстановление маны и здоровья)", "[Событие]");
                        break;
                    case 2:
                        if (!Player.AntiHole)
                        {
                            GameController.Instance.ShowDeathBox("О, нет, зелье превратилось в чёрную дыру и засосало всё и вся в себя!");
                        }
                        else
                            GameController.Instance.ShowMessageText("О, нет, зелье просто упало и разбилось! (Защита от чёрной дыры)", "[Событие]");
                        break;
                    case 3:
                        GameController.Instance.TakeDamage(1000, true);
                        if (Player.Health <= 0)
                            GameController.Instance.ShowDeathBox("Зелье оказалось сильнодействующим токсином, который уничтожил Вас в ту же секунду, как Вы глотнули его!");
                        else
                            GameController.Instance.ShowMessageText("Зелье оказалось сильнодействующим токсином, который не смог Вас уничтожить!", "[Событие]");
                        break;
                    case 4:
                        int rndPotion = Random.Range(0, 5);
                        string potionID = "";
                        switch(rndPotion)
                        {
                            case 0:
                                potionID = "Potion_Health";
                                break;
                            case 1:
                                potionID = "Potion_Regen";
                                break;
                            case 2:
                                potionID = "Potion_Mana";
                                break;
                            case 3:
                                potionID = "Potion_ManaRegen";
                                break;
                            case 4:
                                potionID = "Potion_Armor";
                                break;
                        }
                        GameController.Instance.ShowMessageText(("На зелье была наклейка, это оказалось " + AllItems[FindItemPos(potionID)].Name) + "!", "[Событие]");
                        AddItem(potionID, 1);
                        break;
                    case 5:
                        GameController.Instance.ShowMessageText("Зелье превратилось в облако и улетело в небеса!", "[Событие]");
                        break;
                    case 6:
                        GameController.Instance.ShowMessageText("Вы нарушили баланс Вселенной, готовьтесь к худшему!", "[Событие]");
                        BattleController.Instance.StartBattle(CharactersLibrary.CharacterType.BalanceKeeper);
                        break;
                    case 7:
                        FindObjectOfType<ArtAndVideoManager>().StartArtShow("IlluminatiArt");
                        break;
                }
                break;
            case "Potion_Vampirism":
                CheckItemForDelete(ID, 1);
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Vampirism);
                break;
            case "Potion_Frog":
                CheckItemForDelete(ID, 1);
                FindObjectOfType<BuffManager>().DeleteBuffAction(Buff.BuffType.FrogBuff);
                break;
            case "Potion_Destroyer":
                // Зелье уничтожения -> Добавить возможность уничтожения противника
                if (FindObjectOfType<BattleController>().IsBattle)
                {
                    CheckItemForDelete(ID, 1);
                    //if (Random.Range(1, 101) <= 50)
                    //    BattleController.Instance.allEnemies[0].Health = -1;
                    //else
                    //    Player.Health = -1;

                    if (Player.Health < 0)
                    {
                        GameController.Instance.ShowDeathBox("Зелье уничтожило Вас!");
                    }
                    else
                    {
                        BattleController.Instance.CheckEnemyDeath();
                    }
                }
                else
                {
                    GameController.Instance.ShowMessageText("Это зелье можно использовать только в битве!");
                }
                break;
            case "Potion_Speed":
                // Зелье скорости -> Добавить бафф скорости
                break;
            case "Chest":
                CheckItemForDelete("Chest", 1);
                for (int i = 0; i < ChestItems.Length; i++)
                {
                    if (Random.Range(0, 101) <= ChestItems[i].ChanceToFind)
                    {
                        AddItem(ChestItems[i].ItemID, 1);
                    }
                }
                break;
            case "Cage":
                if (BattleController.Instance.IsBattle)
                {
                    //if (BattleController.Instance.allEnemies[0].Type == CharactersLibrary.CharacterType.RedRabbit)
                    //{
                    //    FindObjectOfType<Inventory>().CheckItemForDelete("Cage", 1);
                    //    FindObjectOfType<Inventory>().AddItem("RabbitInCage", 1);
                    //    BattleController.Instance.isWin = true;
                    //    BattleController.Instance.EndBattle();
                    //}
                    //else
                    //{
                    //    GameController.Instance.ShowMessageText("Это нужно использовать на другое существо!");
                    //}
                }
                else
                    GameController.Instance.ShowMessageText("Это нужно использовать на КРАСНОГО кролика в КРАСНОМ лесу!");
                break;
            case "RabbitInCage":
                //if (LocationsController.CurrentLocation == LocationsController.Location.RF_House)
                //{
                //    if (IsContainItem("RabbitInCage"))
                //    {
                //        FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Happiness);
                //        CheckItemForDelete("RabbitInCage", 1);
                //    }
                //}
                break;
            case "Book_Black":
                // Чёрная книга -> Случайный эффект, усиленная версия зелья
                break;
            case "Book_Blue":
                // Синяя книга -> Связанное с маной
                break;
            case "Book_Gray":
                // Серая книга -> Очки навыков
                break;
            case "Book_Green":
                // Зелёная книга -> Опыт
                break;
            case "Book_Purple":
                // Фиолетовая книга -> Возможно что-то магическое
                break;
            case "Book_Red":
                // Красная книга -> Увеличение здоровья
                break;
            case "Book_White":
                // Белая книга -> Призыв призрака
                break;
            case "Book_Yellow":
                // Жёлтая книга -> Усиление скорости
                break;
            case "Book_Orange":
                // Оранжевая книга -> Улучшение силы
                break;
            case "Book_Pink":
                // Розовая книга -> Увеличение регенерации
                break;
        }

        ReDrawInventory();
    }

    // Быстрое добавление в мусорку
    public void QuickDrop(int slot)
    {
        if (slot != 70 && inventory[slot].Icon != null)
        {
            inventory[70] = inventory[slot];
            inventory[slot] = new ItemProfile();
            HighlightSlots();
            ReDrawInventory();
        }
    }

    // Проверка вещи на удаление
    public void CheckItemForDelete(string itemID, int stack)
    {
        // Если инвентарь открыт
        if (isOpened)
        {
            if (mouseSlot.Icon != null && mouseSlot.Stack >= stack)
            {
                mouseSlot.Stack -= stack;
                if (mouseSlot.Stack <= 0)
                    NullMouseSlot();
            }
        }
        else
        {
            DeleteItemAmount(itemID, stack);
        }
    }

    // Удаление определённого количества вещей из инвентаря
    public void DeleteItemAmount(string itemID, int stack)
    {
        // Удаляем вещь
        for (int i = 0; i < 54; i++)
        {
            if (inventory[i].ItemID == itemID && inventory[i].Stack >= stack)
            {
                inventory[i].Stack -= stack;
                // Если кол-во равно 0, удаляем полностью
                if (inventory[i].Stack <= 0)
                    inventory[i] = new ItemProfile();
                break;
            }
        }

        ReDrawInventory();
    }

    #region Вспомогательные функции по поиску вещей в инвентаре
    /// <summary>
    /// Поиск предмета в инвентаре по ID
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public bool IsContainItem(string itemID)
    {
        for (int i = 0; i < 54; i++)
        {
            if (inventory[i].ItemID == itemID)
                return true;
        }
        return false;
    }
    /// <summary>
    /// Поиск вещи в инвентаре по ID и проверка кол-ва
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="stack"></param>
    /// <returns></returns>
    public bool IsContainItem(string itemID, int stack)
    {
        if (isInfinite)
            return true;

        for (int i = 0; i < 54; i++)
        {
            if (inventory[i].ItemID == itemID && inventory[i].Stack >= stack)
                return true;
        }
        return false;
    }
    /// <summary>
    /// Read-only возврат значения всех вещей данного типа
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public int GetItemStack(string itemID)
    {
        // Сумма всех стаков 
        int sum = 0;
        // Поиск всех вещей по ID 
        for (int i = 0; i < 54; i++)
        {
            if (isInfinite)
                return 999999;
            if (inventory[i].ItemID == itemID)
                sum += inventory[i].Stack;
        }
        return sum;
    }
    #endregion

    // Поиск предмета в общем списке
    private int FindItemPos(string itemID)
    {
        for (int i = 0; i < AllItems.Length; i++)
        {
            if (AllItems[i].ItemID == itemID)
                return i;
        }
        return -1;
    }

    // Добавление вещи на слот
    public void AddItemOnSlot(string itemID, int stack, int slot)
    {
        int pos = FindItemPos(itemID);

        if (Buttons[slot].GetComponent<InvButton>().isActive)
        {
            inventory[slot].Name = AllItems[pos].Name;
            inventory[slot].Descr = AllItems[pos].Descr;
            inventory[slot].Cost = AllItems[pos].Cost;
            inventory[slot].ChanceToFind = AllItems[pos].ChanceToFind;
            inventory[slot].ItemID = AllItems[pos].ItemID;

            inventory[slot].Damage = AllItems[pos].Damage;
            inventory[slot].AttackSpeed = AllItems[pos].AttackSpeed;
            inventory[slot].Armor = AllItems[pos].Armor;
            inventory[slot].Health = AllItems[pos].Health;
            inventory[slot].Mana = AllItems[pos].Mana;
            inventory[slot].equipBuffs = AllItems[pos].equipBuffs;

            inventory[slot].Stack = stack;
            inventory[slot].MaxStack = AllItems[pos].MaxStack;
            inventory[slot].Type = AllItems[pos].Type;
            inventory[slot].Icon = AllItems[pos].Icon;

            ReDrawInventory();
        }
    }

    // Добавление вещи в инвентарь
    public void AddItem(string itemID, int stack)
    {
        int pos = FindItemPos(itemID);
        bool isFound = false;

        // Проверяем есть ли в инвентаре такой предмет
        for (int i = 0; i < 54; i++)
        {
            // Проверка на наличие предмета
            if (inventory[i].ItemID == itemID && inventory[i].Stack < inventory[i].MaxStack)
            {
                // Если предмет нашли и добавляемый стак меньше разницы, то прибавляем стак
                if (stack <= inventory[i].MaxStack - inventory[i].Stack)
                    inventory[i].Stack += stack;
                else // Если предмет нашли и стак больше разницы, то просто добавляем разницу и новый предмет, пока не сможем забить
                {
                    stack -= inventory[i].MaxStack - inventory[i].Stack;
                    inventory[i].Stack = inventory[i].MaxStack;

                    int mStack = AllItems[pos].MaxStack;

                    while (stack > 0)
                    {
                        if (stack > mStack)
                            AddItem(itemID, mStack);
                        else
                            AddItem(itemID, stack);
                        stack -= mStack;
                    }
                }
                isFound = true;
                break;
            }
        }

        if (!isFound)
        {
            for (int i = 0; i < 54; i++)
            {
                // Если слот активен
                if (Buttons[i].GetComponent<InvButton>().isActive)
                {
                    int mStack = AllItems[pos].MaxStack;

                    // Если слот пустой
                    if (inventory[i].Icon == null)
                    {
                        inventory[i].Name = AllItems[pos].Name;
                        inventory[i].Descr = AllItems[pos].Descr;
                        inventory[i].Cost = AllItems[pos].Cost;
                        inventory[i].ChanceToFind = AllItems[pos].ChanceToFind;
                        inventory[i].ItemID = AllItems[pos].ItemID;

                        inventory[i].Damage = AllItems[pos].Damage;
                        inventory[i].AttackSpeed = AllItems[pos].AttackSpeed;
                        inventory[i].Armor = AllItems[pos].Armor;
                        inventory[i].Health = AllItems[pos].Health;
                        inventory[i].Mana = AllItems[pos].Mana;
                        inventory[i].equipBuffs = AllItems[pos].equipBuffs;

                        inventory[i].MaxStack = AllItems[pos].MaxStack;
                        inventory[i].Type = AllItems[pos].Type;
                        inventory[i].Icon = AllItems[pos].Icon;

                        if (stack > mStack)
                        {
                            inventory[i].Stack = mStack;
                            stack -= mStack;

                            while (stack > 0)
                            {
                                if (stack > mStack)
                                    AddItem(itemID, mStack);
                                else
                                    AddItem(itemID, stack);
                                stack -= mStack;
                            }
                        }
                        else
                        {
                            inventory[i].Stack = stack;
                        }
                        break;
                    }
                }
            }
        }

        ReDrawInventory();
    }

    // Надевание вещи
    public void EquipItem(int slot)
    {
        Player.MaxHealth += inventory[slot].Health;
        Player.Damage += inventory[slot].Damage;
        Player.AttackSpeed += inventory[slot].AttackSpeed;
        Player.Armor += inventory[slot].Armor;
        Player.MaxMana += inventory[slot].Mana;
        Player.ExtraExpMod += inventory[slot].ExtraExp;
        Player.ExtraMoneyMod += inventory[slot].ExtraMoney;

        //FindObjectOfType<BuffManager>().BuffsEquipAction();

        Debug.Log("Надел вещь");
    }
    // Снятие вещи
    public void EnequipItem(int slot)
    {
        Player.MaxHealth -= inventory[slot].Health;
        Player.Damage -= inventory[slot].Damage;
        Player.AttackSpeed -= inventory[slot].AttackSpeed;
        Player.Armor -= inventory[slot].Armor;
        Player.MaxMana -= inventory[slot].Mana;
        Player.ExtraExpMod -= inventory[slot].ExtraExp;
        Player.ExtraMoneyMod -= inventory[slot].ExtraMoney;

        //FindObjectOfType<BuffManager>().BuffsUnequipAction();
        
        Debug.Log("Снял вещь");
    }

    // Нажатие на слот
    public void PressSlot(int slot)
    {
        // Как должно работать?
        // Нажатие на слот -> Проверка предмета в мышке и на слоте 
        // Если мышка пустая, забираем предмет, снимаем баффы.
        // Если мышка не пустая, меняем предметы, снимаем баффы и надеваем баффы.
        // 
        //

        // Если инвентарь открыт...
        if (isOpened)
        {
            // Слоты хранения
            if (slot < 54)
            {
                // Если мышка пустая
                if (mouseSlot.Icon == null)
                {
                    // Если слот имеет предмет
                    if (inventory[slot].Icon != null)
                    {
                        // Забираем предмет в мышку
                        mouseSlot = inventory[slot];
                        // Даём новый ПУСТОЙ предмет в слот
                        inventory[slot] = new ItemProfile();
                    }
                    else // Если слот не имеет предмет
                    {
                        // Ничего не происходит
                    }
                }
                else // Если в мышке есть предмет
                {
                    // Если слот имеет предмет
                    if (inventory[slot].Icon != null)
                    {
                        // Если мы взяли предмет с экипировки, 
                        if (mouseSlot.takedSlot >= 54)
                        {
                            AddItemOnSlot(mouseSlot.ItemID, mouseSlot.Stack, mouseSlot.takedSlot);
                            EquipItem(mouseSlot.takedSlot);
                            NullMouseSlot();
                        }
                        else
                        {
                            // Если предметы одинаковые, то добавляем стак к предмету
                            if (inventory[slot].ItemID == mouseSlot.ItemID)
                            {
                                // Если добавляемый стак меньше или равен разнице, которые можно добавить, то добавляем стак
                                if (mouseSlot.Stack <= inventory[slot].MaxStack - inventory[slot].Stack)
                                {
                                    // Добавляем стак к предмету и обнуляем предмет в мышке
                                    inventory[slot].Stack += mouseSlot.Stack;
                                    NullMouseSlot();
                                }
                                else // Если стак больше допустимой разницы, то добавляем на максимум стак и вычитаем разницу
                                {
                                    mouseSlot.Stack -= inventory[slot].MaxStack - inventory[slot].Stack;
                                    inventory[slot].Stack = inventory[slot].MaxStack;
                                }
                            }
                            else // Если предметы разные, то меняем их местами
                            {
                                AddItemOnSlot(inventory[slot].ItemID, inventory[slot].Stack, mouseSlot.takedSlot);
                                // Передаём предмет между мышкой и слотом
                                inventory[slot] = mouseSlot;
                                NullMouseSlot();
                            }
                        }
                    }
                    else // Если слот не имеет предмет
                    {
                        // Даём в слот предмет с мышки
                        inventory[slot] = mouseSlot;
                        NullMouseSlot();
                    }
                }

                Debug.Log("Действие в слоте рюкзака!");
            }
            // Корзина
            else if (slot == 70)
            {
                // Если в мусорке есть предмет
                if (inventory[slot].Icon != null)
                {
                    // Если в мышке нет предмета
                    if (mouseSlot.Icon == null)
                    {
                        // Забираем предмет из мусорки
                        mouseSlot = inventory[slot];
                        inventory[slot] = new ItemProfile();
                    }
                    else // Если в мышке есть предмет, то передаём в мусорку и обнуляем мышку
                    {
                        inventory[slot] = mouseSlot;
                        NullMouseSlot();
                    }
                }
                else // Если в мусорке нет предмета
                {
                    // Помещаем предмет с мышки в мусорку, если предмет в мышке есть
                    if (mouseSlot.Icon != null)
                    {
                        inventory[slot] = mouseSlot;
                        NullMouseSlot();
                    }
                    else // Если в мышке нет предмета и в мусорке нет предмета
                    {
                        // Ничего не произойдёт
                    }
                }

                Debug.Log("Действие в корзине!");
            }
            // Слоты экипировки и руны
            else if (slot >= 54) 
            {
                // Если в мышке есть предмет
                if (mouseSlot.Icon != null)
                {
                    // Если на слоте есть предмет
                    if (inventory[slot].Icon != null)
                    {
                        // Если предмет в мышке можно положить в слот, то меняем местами
                        if (CheckEquipment(slot))
                        {
                            ItemProfile temp = mouseSlot;
                            EnequipItem(slot);
                            mouseSlot = inventory[slot];
                            inventory[slot] = temp;
                            EquipItem(slot);
                            AddItemOnSlot(mouseSlot.ItemID, mouseSlot.Stack, inventory[slot].takedSlot);
                            EquipItem(inventory[slot].takedSlot);
                            NullMouseSlot();
                        }
                        else // Предмет нельзя поместить, т.к. он отличается типом
                        {
                            // Если взяли предмет с другого слота экипировки, то надеваем его
                            if (mouseSlot.takedSlot >= 54)
                            {
                                AddItemOnSlot(mouseSlot.ItemID, mouseSlot.Stack, mouseSlot.takedSlot);
                                EquipItem(mouseSlot.takedSlot);
                                NullMouseSlot();
                            }
                            else // Если обычный предмет, то возвращаем на место
                            {
                                AddItemOnSlot(mouseSlot.ItemID, mouseSlot.Stack, mouseSlot.takedSlot);
                                NullMouseSlot();
                            }
                        }
                    }
                    else // Если на слоте нет предмета
                    {
                        // Если предмет в мышке можно поместить на слот экипировки, то помещаем
                        if (CheckEquipment(slot))
                        {
                            // Добавляем предмет в слот и экипируем
                            inventory[slot] = mouseSlot;
                            EquipItem(slot);
                            NullMouseSlot();
                        }
                        else // Разные типы предметов
                        {
                            // Мы не можем поместить слот в мышке на слот экипировки
                            // Если взяли предмет с другого слота экипировки, то надеваем его
                            if (mouseSlot.takedSlot >= 54)
                            {
                                AddItemOnSlot(mouseSlot.ItemID, mouseSlot.Stack, mouseSlot.takedSlot);
                                EquipItem(mouseSlot.takedSlot);
                                NullMouseSlot();
                            }
                            else // Если обычный предмет, то возвращаем на место
                            {
                                AddItemOnSlot(mouseSlot.ItemID, mouseSlot.Stack, mouseSlot.takedSlot);
                                NullMouseSlot();
                            }
                        }
                    }
                }
                else // Если в мышке нет предмета
                {
                    // Если на слоте есть предмет
                    if(inventory[slot].Icon != null)
                    {
                        EnequipItem(slot);
                        mouseSlot = inventory[slot];
                        inventory[slot] = new ItemProfile();
                    }
                    else // Если на слоте нет предмета
                    {
                        // Ничего не происходит, нет ни одного предмета
                    }
                }

                Debug.Log("Действие в экипировке или в рунах!");
            }
            // Ничего не произойдёт
            else
            {
                // На будущее
            }

            HighlightSlots();
            mouseSlot.takedSlot = slot;
            ReDrawInventory();

            // Если в мышке остался предмет, то показываем его описание
            if (mouseSlot.Icon != null)
                ShowDescr(mouseSlot.takedSlot);
            else
                HideDescr();
        }
        else // Инвентарь закрыт
        {
            // Если инвентарь закрыт, то слоты быстрого доступа
            if (inventory[slot].Icon != null)
                UseItem(inventory[slot].ItemID);
        }
    }

    // Проверяем снаряжение на слот экипировки
    // Если в мышке и на слоте одинаковые типы экипировки
    // То говорим об этом
    private bool CheckEquipment(int slot)
    {
        /*
         RangedWeapon = 54,
         MeleeWeapon = 55,
         MagicWeapon = 56,
         RingRegen = 57 - 61,
         RingExp = 57 - 61,
         RingGold = 57 - 61,
         Helmet = 62,
         Chestplate = 63,
         Greaves = 64,
         Shield = 65,
         Gloves = 66,
         Necklace = 67,
         Orb = 68 - 69,
         Rune = 71 - 79
         */
        if (slot == 54 && mouseSlot.Type == ItemType.RangedWeapon)
            return true;
        else if (slot == 55 && mouseSlot.Type == ItemType.MeleeWeapon)
            return true;
        else if (slot == 56 && mouseSlot.Type == ItemType.MagicWeapon)
            return true;
        else if (slot == 57 && (mouseSlot.Type == ItemType.RingRegen || mouseSlot.Type == ItemType.RingGold || mouseSlot.Type == ItemType.RingExp))
            return true;
        else if (slot == 58 && (mouseSlot.Type == ItemType.RingRegen || mouseSlot.Type == ItemType.RingGold || mouseSlot.Type == ItemType.RingExp))
            return true;
        else if (slot == 59 && (mouseSlot.Type == ItemType.RingRegen || mouseSlot.Type == ItemType.RingGold || mouseSlot.Type == ItemType.RingExp))
            return true;
        else if (slot == 60 && (mouseSlot.Type == ItemType.RingRegen || mouseSlot.Type == ItemType.RingGold || mouseSlot.Type == ItemType.RingExp))
            return true;
        else if (slot == 61 && (mouseSlot.Type == ItemType.RingRegen || mouseSlot.Type == ItemType.RingGold || mouseSlot.Type == ItemType.RingExp))
            return true;
        else if (slot == 62 && mouseSlot.Type == ItemType.Helmet)
            return true;
        else if (slot == 63 && mouseSlot.Type == ItemType.Chestplate)
            return true;
        else if (slot == 64 && mouseSlot.Type == ItemType.Greaves)
            return true;
        else if (slot == 65 && mouseSlot.Type == ItemType.Shield)
            return true;
        else if (slot == 66 && mouseSlot.Type == ItemType.Gloves)
            return true;
        else if (slot == 67 && mouseSlot.Type == ItemType.Necklace)
            return true;
        else if (slot == 68 && mouseSlot.Type == ItemType.Orb)
            return true;
        else if (slot == 69 && mouseSlot.Type == ItemType.Orb)
            return true;
        else if (slot > 70 && mouseSlot.Type == ItemType.Rune)
            return true;
        else
            return false;
    }

    // Подсветка слотов для экипировки
    public void HighlightSlots()
    {
        // Выключаем подсветку всех слотов
        for (int i = 54; i <= 69; i++)
        {
            Buttons[i].GetComponent<RawImage>().color = normalColor;
        }
        // Если в мышке есть предмет, то подсвечиваем в какой слот нужно положить
        if (mouseSlot.Icon != null)
        {
            switch (mouseSlot.Type)
            {
                case ItemType.None:
                    // Ничего не происходит :P
                    break;
                case ItemType.MagicWeapon:
                    // 56
                    Buttons[56].GetComponent<RawImage>().color = highlightedColor;
                    break;
                case ItemType.MeleeWeapon:
                    // 55
                    Buttons[55].GetComponent<RawImage>().color = highlightedColor;
                    break;
                case ItemType.RangedWeapon:
                    // 54
                    Buttons[54].GetComponent<RawImage>().color = highlightedColor;
                    break;
                case ItemType.RingExp:
                    // 57 - 61
                    Buttons[57].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[58].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[59].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[60].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[61].GetComponent<RawImage>().color = highlightedColor;
                    break;
                case ItemType.RingGold:
                    // 57 - 61
                    Buttons[57].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[58].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[59].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[60].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[61].GetComponent<RawImage>().color = highlightedColor;
                    break;
                case ItemType.RingRegen:
                    // 57 - 61
                    Buttons[57].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[58].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[59].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[60].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[61].GetComponent<RawImage>().color = highlightedColor;
                    break;
                case ItemType.Shield:
                    // 65
                    Buttons[65].GetComponent<RawImage>().color = highlightedColor;
                    break;
                case ItemType.Helmet:
                    // 62
                    Buttons[62].GetComponent<RawImage>().color = highlightedColor;
                    break;
                case ItemType.Orb:
                    // 68 - 69
                    Buttons[68].GetComponent<RawImage>().color = highlightedColor;
                    Buttons[69].GetComponent<RawImage>().color = highlightedColor;
                    break;
                case ItemType.Necklace:
                    // 67
                    Buttons[67].GetComponent<RawImage>().color = highlightedColor;
                    break;
                case ItemType.Greaves:
                    // 64
                    Buttons[64].GetComponent<RawImage>().color = highlightedColor;
                    break;
                case ItemType.Gloves:
                    // 66
                    Buttons[66].GetComponent<RawImage>().color = highlightedColor;
                    break;
                case ItemType.Chestplate:
                    // 63
                    Buttons[63].GetComponent<RawImage>().color = highlightedColor;
                    break;
            }
        }
    }

    // Подсвечивание выбранного слота
    public void TransformIcon(int slot)
    {
        mouseIcon.transform.position = Buttons[slot].transform.position;
    }

    // Перерисовка инвентаря
    public void ReDrawInventory()
    {
        if (mouseSlot.Icon != null)
        {
            if (mouseSlot.Type == ItemType.Consumable || mouseSlot.Type == ItemType.Potion)
                buttonUse.SetActive(true);
            else
                buttonUse.SetActive(false);

            mouseIcon.GetComponentInChildren<RawImage>().color = new Color(1, 1, 1, 1);
            mouseIcon.GetComponent<RawImage>().texture = mouseSlot.Icon;
            mouseIcon.GetComponentInChildren<Text>().text = mouseSlot.Stack + "/" + mouseSlot.MaxStack;
            TransformIcon(mouseSlot.takedSlot);
        }
        else
        {
            buttonUse.SetActive(false);
            mouseIcon.GetComponentInChildren<RawImage>().color = new Color(0, 0, 0, 0);
            mouseIcon.GetComponentInChildren<Text>().text = "";
        }

        for(int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].Icon != null)
            {
                Buttons[i].transform.GetChild(0).GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                Buttons[i].transform.GetChild(0).GetComponent<RawImage>().texture = inventory[i].Icon;
                Buttons[i].GetComponentInChildren<Text>().text = inventory[i].Stack + "/" + inventory[i].MaxStack;
            }
            else
            {
                if (i == 70)
                {
                    Buttons[i].transform.GetChild(0).GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                    Buttons[i].GetComponent<RawImage>().texture = trashCanIcon;
                    Buttons[i].GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    Buttons[i].transform.GetChild(0).GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                    Buttons[i].transform.GetChild(0).GetComponent<RawImage>().texture = null;
                    Buttons[i].GetComponentInChildren<Text>().text = "";
                }
            }
        }
    }

    // Открывает закрытый слот инвентаря
    public void OpenInvSlot(int slot)
    {
        Buttons[slot].GetComponent<InvButton>().isActive = true;
        Buttons[slot].GetComponent<InvButton>().UpdateState();
    }

    // Закрывает открыйтый слот инвентаря
    public void CloseInvSlot(int slot)
    {
        Buttons[slot].GetComponent<InvButton>().isActive = false;
        Buttons[slot].GetComponent<InvButton>().UpdateState();
    }

    // Открывает закрытый слот руны
    public void OpenRuneSlot(int slot)
    {
        Buttons[slot].GetComponent<InvButton>().isActive = true;
        Buttons[slot].GetComponent<InvButton>().UpdateState();
    }

    // Закрывает открытый слот руны
    public void CloseRuneSlot(int slot)
    {
        Buttons[slot].GetComponent<InvButton>().isActive = false;
        Buttons[slot].GetComponent<InvButton>().UpdateState();
    }
}