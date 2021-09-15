using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    [Header("Панель крафта")]
    public GameObject craftPanel;

    [Header("Основные ссылки")]
    public Inventory inv;
    public GameHelper GH;

    [Space(20)]
    [Header("Все рецепты в игре")]
    public RecipeSlot[] AllRecipes;

    [Space(20)]
    [Header("Кнопки панелей")]
    public GameObject[] PanelButtons;
    [Header("Панели крафта")]
    public GameObject[] Panels;

    [Header("Цвета кнопок")]
    public Color pressedColor;
    public Color normalColor;

    [Space(20)]
    [Header("Префаб слота рецепта")]
    public GameObject recipeSlot;
    [Header("Родитель для установки")]
    public Transform recipeSlotsPotions;
    [Header("Родитель для материалов")]
    public Transform recipeSlotsMaterials;
    [Header("Родитель для оружия")]
    public Transform recipeSlotsWeapons;
    [Header("Родитель для брони")]
    public Transform recipeSlotsArmors;
    [Header("Родитель для рун")]
    public Transform recipeSlotsRunes;
    [Header("Родитель для слитков")]
    public Transform recipeSlotsIngots;

    [Space(20)]
    [Header("Панель действия")]
    public GameObject actionsPanel;
    [Header("Панель описания")]
    public Text descrText;
    [Header("Префаб нужных ресурсов")]
    public GameObject neededItemPrefab;
    [Header("Родитель для нужных ресурсов")]
    public Transform neededItemsParent;

    [Space(20)]
    [Header("Лист вещей для крафта")]
    public List<GameObject> neededItemsSaving = new List<GameObject>();

    // Текущий рецепт
    int currentRecipeNum = -1;
    int currentSlotNum = -1;

    private void Awake()
    {
        SetRecipes();
    }

    // Установка рецептов при запуске
    public void SetRecipes()
    {
        Transform parent = recipeSlotsMaterials;

        // Загружаем панели и в каждую панель добавляем слоты рецептов
        for (int i = 0; i < AllRecipes.Length; i++)
        {
            // Устанавливаем родителя по типу рецепта
            switch (AllRecipes[i].recipes[0].Type)
            {
                case Recipe.RecipeType.Armor:
                    parent = recipeSlotsArmors;
                    break;
                case Recipe.RecipeType.Material:
                    parent = recipeSlotsMaterials;
                    break;
                case Recipe.RecipeType.Potion:
                    parent = recipeSlotsPotions;
                    break;
                case Recipe.RecipeType.Weapon:
                    parent = recipeSlotsWeapons;
                    break;
                case Recipe.RecipeType.Ingot:
                    parent = recipeSlotsIngots;
                    break;
                case Recipe.RecipeType.Rune:
                    parent = recipeSlotsRunes;
                    break;
            }

            // Добавляем слоты рецептов
            for(int s = 0; s < AllRecipes[i].recipes.Length; s++)
            {
                GameObject newSlot = Instantiate(recipeSlot, parent);

                newSlot.transform.GetChild(0).GetComponent<RawImage>().texture = AllRecipes[i].recipes[s].resultItem.Icon;

                newSlot.GetComponent<RecipeButton>().slotNum = s;
                newSlot.GetComponent<RecipeButton>().recipeNum = i;
            }
        }
    }

    // Закрытие панелей кроме 
    public void ClosePanels(int except)
    {
        for(int i = 0; i < Panels.Length; i++)
        {
            if (i == except)
                continue;
            else
            {
                PanelButtons[i].GetComponent<RawImage>().color = normalColor;
                Panels[i].SetActive(false);
            }
        }
    }

    // Открытие определённой панели крафта
    public void OpenPanel(int num)
    {
        // Закрываем панели
        ClosePanels(num);
        HideActions();

        // Изменяем цвет нажатой кнопки
        PanelButtons[num].GetComponent<RawImage>().color = pressedColor;

        // Открываем панель
        Panels[num].SetActive(true);
    }

    // Открытие и закрытие панели крафта
    public void ShowHideCraftPanel()
    {
        ClosePanels(0);

        FindObjectOfType<PanelsManager>().CloseAllPlayerPanels(3);
        FindObjectOfType<PanelsManager>().OpenHidePlayerPanel(3, !FindObjectOfType<PanelsManager>().PlayerPanels[3].activeSelf);
        HideActions();
    }
    
    // Открытие панели действий
    public void ShowActions(int recipeNum, int slotNum)
    {
        currentRecipeNum = recipeNum;
        currentSlotNum = slotNum;

        string descr = AllRecipes[recipeNum].recipes[slotNum].resultItem.Descr;

        if (AllRecipes[recipeNum].recipes[slotNum].resultItem.Health != 0)
            descr += "\nБонус ОЗ: " + AllRecipes[recipeNum].recipes[slotNum].resultItem.Health;
        if (AllRecipes[recipeNum].recipes[slotNum].resultItem.Mana != 0)
            descr += "\nБонус ОМ: " + AllRecipes[recipeNum].recipes[slotNum].resultItem.Mana;
        if (AllRecipes[recipeNum].recipes[slotNum].resultItem.Armor != 0)
            descr += "\nБонус защиты: " + AllRecipes[recipeNum].recipes[slotNum].resultItem.Armor;
        if (AllRecipes[recipeNum].recipes[slotNum].resultItem.Damage != 0)
            descr += "\nБонус урона: " + AllRecipes[recipeNum].recipes[slotNum].resultItem.Damage;
        if (AllRecipes[recipeNum].recipes[slotNum].resultItem.AttackSpeed != 0)
            descr += "\nБонус скорости атаки: " + AllRecipes[recipeNum].recipes[slotNum].resultItem.AttackSpeed;

        descrText.text = descr;



        ReDrawRecipes(recipeNum, slotNum);

        actionsPanel.SetActive(true);
    }
    // Закрытие панели действий
    public void HideActions()
    {
        actionsPanel.SetActive(false);
    }

    // Перерисовка параметров в панели действий
    public void ReDrawRecipes(int recipeNum, int slotNum)
    {
        // Очистка старых иконок
        for(int c = 0; c < neededItemsSaving.Count; c++)
        {
            Destroy(neededItemsSaving[c]);
        }
        neededItemsSaving.Clear();

        actionsPanel.transform.GetChild(1).GetChild(0).GetComponent<RawImage>().texture = AllRecipes[recipeNum].recipes[slotNum].resultItem.Icon;
        actionsPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = AllRecipes[recipeNum].recipes[slotNum].resultItem.Name;

        // Установка новых иконок
        for (int i = 0; i < AllRecipes[recipeNum].recipes[slotNum].neededItems.Length; i++)
        {
            // Инициализация нового слота
            GameObject newItem = Instantiate(neededItemPrefab, neededItemsParent);

            // Установка иконки предмета
            newItem.GetComponent<RawImage>().texture = AllRecipes[recipeNum].recipes[slotNum].neededItems[i].neededItem.Icon;

            // Установка вещи
            newItem.GetComponent<neededItemSlot>().neededItem = AllRecipes[recipeNum].recipes[slotNum].neededItems[i].neededItem;

            // Доп. переменные для облегчения
            int findableStack = inv.GetItemStack(AllRecipes[recipeNum].recipes[slotNum].neededItems[i].neededItem.ItemID);
            int itemStack = AllRecipes[recipeNum].recipes[slotNum].neededItems[i].neededStack;

            // Обновляем текст о кол-ве ресурсов
            newItem.GetComponentInChildren<Text>().text = findableStack + "/" + itemStack;

            // Меняем цвет текста в зависимости от кол-ва ресурсов
            if (findableStack >= itemStack)
                newItem.GetComponentInChildren<Text>().color = Color.green;
            else
                newItem.GetComponentInChildren<Text>().color = Color.red;

            neededItemsSaving.Add(newItem);
        }
    }

    // Крафт предмета
    public void CraftItem()
    {
        // Проверяем открыт ли сейчас рецепт
        if (currentRecipeNum == -1 || currentSlotNum == -1)
        {
            GH.ShowMessageText("Сначала выберите рецепт!", 1);
            return;
        }

        // Доп. переменная для проверка
        bool isContain = false;

        // Проверяем наличие всех предметов в инвентаре
        for (int i = 0; i < AllRecipes[currentRecipeNum].recipes[currentSlotNum].neededItems.Length; i++)
        {
            // Проверяем есть ли в инвентаре эта вещь в указанном количестве = true
            if (inv.IsContainItem(AllRecipes[currentRecipeNum].recipes[currentSlotNum].neededItems[i].neededItem.ItemID, AllRecipes[currentRecipeNum].recipes[currentSlotNum].neededItems[i].neededStack))
                isContain = true;
            else // Если нет, то ставим false
            {
                isContain = false;
                break;
            }
        }

        // Если найдены все вещи, то крафтим
        if (isContain)
        {
            // Удаляем вещи для крафта
            for (int i = 0; i < AllRecipes[currentRecipeNum].recipes[currentSlotNum].neededItems.Length; i++)
            {
                if (Random.Range(0, 101) > Player.ChanceNotToDelete + Player.Luck)
                {
                    inv.DeleteItemAmount(AllRecipes[currentRecipeNum].recipes[currentSlotNum].neededItems[i].neededItem.ItemID, AllRecipes[currentRecipeNum].recipes[currentSlotNum].neededItems[i].neededStack);
                    // Информируем игрока об удалении предмета
                    GH.AddEventText("Удалён предмет: " + AllRecipes[currentRecipeNum].recipes[currentSlotNum].neededItems[i].neededItem.Name + " в кол-ве " + AllRecipes[currentRecipeNum].recipes[currentSlotNum].neededItems[i].neededStack + " шт.");
                }
            }

            // Если выпал шанс на крафт вещи дважды, даём доп. вещь
            if (Random.Range(0, 101) <= Player.ChanceToCraftTwice + Player.Luck)
            {
                // Добавляем скрафченную вещь в инвентарь и +1 копию
                inv.AddItem(AllRecipes[currentRecipeNum].recipes[currentSlotNum].resultItem.ItemID, AllRecipes[currentRecipeNum].recipes[currentSlotNum].resultStack + 1);
                // Информируем игрока об этом
                GH.AddEventText("Добавлен предмет: " + AllRecipes[currentRecipeNum].recipes[currentSlotNum].resultItem.Name + " в кол-ве " + (AllRecipes[currentRecipeNum].recipes[currentSlotNum].resultStack + 1) + " шт.");

            }
            else
            {
                // Просто добавляем скрафченную вещь
                inv.AddItem(AllRecipes[currentRecipeNum].recipes[currentSlotNum].resultItem.ItemID, AllRecipes[currentRecipeNum].recipes[currentSlotNum].resultStack);
                // Информируем игрока об этом
                GH.AddEventText("Добавлен предмет: " + AllRecipes[currentRecipeNum].recipes[currentSlotNum].resultItem.Name + " в кол-ве " + AllRecipes[currentRecipeNum].recipes[currentSlotNum].resultStack + " шт.");

            }

            ReDrawRecipes(currentRecipeNum, currentSlotNum);
        }
        else
            GH.ShowMessageText("Недостаточно ресурсов для создания предмета!", 1);
    }
}
