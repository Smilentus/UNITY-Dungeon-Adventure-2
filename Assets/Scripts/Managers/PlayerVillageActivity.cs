using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVillageActivity : MonoBehaviour
{
    [HideInInspector()]
    public Inventory INV;
    [HideInInspector()]
    public GameController GH;

    public enum villagePart
    {
        Church,
        Houses,
        MainHall,
        Port,
        Farm,
        Sawmill,
    }
    public enum villagersStatus
    {
        LowPlaces = -1000,
        Agressy = -50,
        Happiness = 85,
        Starving = -115,
        Dying = -175,
    }

    [Header("Характеристика деревни")]
    public string villageName;
    // Популярность деревни
    public double villagePopularity
    {
        get
        {
            double popul = 0;
            for(int i = 0; i < VillageParts.Length; i++)
            {
                popul += VillageParts[i].currentLvl * 25;
            }
            return popul;
        }
        set { }
    }
    [Header("Статус жителей")]
    public villagersStatus villageStatus;
    public string _villageStatus
    {
        get
        {
            string status = "";

            // Выдаём статус деревни
            switch(villageStatus)
            {
                case villagersStatus.Agressy:
                    status = "Население агрессивно в Вашу сторону!";
                    break;
                case villagersStatus.Dying:
                    status = "Население умирает!";
                    break;
                case villagersStatus.Happiness:
                    status = "Население счастливо!";
                    break;
                case villagersStatus.LowPlaces:
                    status = "Недостаточно мест для жительства!";
                    break;
                case villagersStatus.Starving:
                    status = "Население голодает!";
                    break;
            }

            return status;
        }
    }
    [Header("Вместимость жителей в домах")]
    public int villageCapacity;
    public int villageMaxCapacity;
    // Общий доход деревни
    public double villageIncome
    {
        get
        {
            double income = 0;
            for(int i = 0; i < VillageParts.Length; i++)
            {
                income += VillageParts[i].currentIncome;
            }
            return income;
        }
    }
    [Space(20)]
    [Header("Панели информации")]
    public GameObject[] InfoPanels;
    public VillagePart[] VillageParts;
    [Space(20)]
    public GameObject BackgroundPanel;
    public GameObject Blocker;

    [Header("Панель инпута нового названия")]
    public GameObject changeVillageName;

    // Различные префабы
    public GameObject resourceSlotPrefab;
    public GameObject neededItemSlotPrefab;

    // Список добываемых ресурсов
    public List<producingItem> currentProducingItems;

    private void Start()
    {
        INV = FindObjectOfType<Inventory>();
        GH = FindObjectOfType<GameController>();
    }

    // Поиск части по типу в списке
    public int partPos(villagePart fPart)
    {
        for(int i = 0; i < VillageParts.Length; i++)
        {
            if (VillageParts[i].vilPart == fPart)
                return i;
        }
        return 0;
    }

    // Дефолтные настройки деревни
    public void SetDefault()
    {
        currentProducingItems = new List<producingItem>();

        // Основные переменные
        villageName = "Название деревни";
        villagePopularity = 1;
        villageCapacity = 24;
        villageMaxCapacity = 24;
        villageStatus = villagersStatus.Happiness;

        // Инициализация дефолтных параметров
        for(int i = 0; i < VillageParts.Length; i++)
        {
            VillageParts[i].maxLvl = VillageParts[i].defaultMaxLvl;
            VillageParts[i].currentLvl = 0;
            VillageParts[i].currentPopularity = VillageParts[i].defaultCurrentPopularity;
            VillageParts[i].currentWorkers = 0;
            VillageParts[i].maxWorkers = VillageParts[i].defaultMaxWorkers;
            VillageParts[i].upgradeCost = VillageParts[i].defaultUpgradeCost;
            VillageParts[i].upgradeMulty = VillageParts[i].defaultUpgradeMulty;
            VillageParts[i].currentIncome = VillageParts[i].defaultCurrentIncome;
        }
    }

    private void ClearUpgradeResourcesAt(int part)
    {
        Debug.Log("Очищаю ресурсы");
        for (int i = 0; i < InfoPanels[part].transform.GetChild(InfoPanels[part].transform.childCount - 1).GetChild(3).childCount; i++)
        {
            Destroy(InfoPanels[part].transform.GetChild(InfoPanels[part].transform.childCount - 1).GetChild(3).GetChild(i).gameObject);
        }
    }
    private void ClearProductResourcesAt(int part)
    {
        // ПЕРЕДЕЛАТЬ ПАНЕЛИ, КОГДА СДЕЛАЮ
        Debug.Log("Очищаю продукцию");
        for(int i = 0; i < InfoPanels[part].transform.GetChild(InfoPanels[part].transform.childCount - 2).childCount; i++)
        {
            Destroy(InfoPanels[part].transform.GetChild(InfoPanels[part].transform.childCount - 2).GetChild(i).gameObject);
        }
    }

    // Перерисовываем предметы для улучшения
    public void ReDrawNeededResourcesAt(int part)
    {
        ClearUpgradeResourcesAt(part);

        Debug.Log("Перерисовываю ресурсы в " + VillageParts[part].name);

        int i = VillageParts[part].currentLvl;
        if (i < VillageParts[part].maxLvl)
        {
            i = VillageParts[part].currentLvl;

            for (int j = 0; j < VillageParts[part].neededItemsToUpgrade[i].neededItems.Length; j++)
            {
                GameObject slot = Instantiate(neededItemSlotPrefab, InfoPanels[part].transform.GetChild(InfoPanels[part].transform.childCount - 1).GetChild(3));
                slot.GetComponent<neededItemSlot>().neededItem = VillageParts[part].neededItemsToUpgrade[i].neededItems[j].neededItem;

                // Текущее количество этого предмета в инвентаре игрока
                int findableStack = INV.GetItemStack(VillageParts[part].neededItemsToUpgrade[i].neededItems[j].neededItem.ItemID);

                // Обновляем картинку предмета
                slot.GetComponent<RawImage>().texture = VillageParts[part].neededItemsToUpgrade[i].neededItems[j].neededItem.Icon;
                // Обновляем текст о количестве предмета
                slot.transform.GetChild(0).GetComponent<Text>().text = findableStack + "/" + VillageParts[part].neededItemsToUpgrade[i].neededItems[j].neededStack;

                // Меняем цвет текста в зависимости от кол-ва ресурсов
                if (findableStack >= VillageParts[part].neededItemsToUpgrade[i].neededItems[j].neededStack)
                    slot.GetComponentInChildren<Text>().color = Color.green;
                else
                    slot.GetComponentInChildren<Text>().color = Color.red;
            }
        }
        else
        {
            // Здесь появляется надпись в здании, что оно достигло максимального уровня
            Debug.Log("Здание прокачено на максимум!");
        }
    }

    private int CheckProducingItem(string itemID, int stack)
    {
        for(int i = 0; i < currentProducingItems.Count; i++)
        {
            if (currentProducingItems[i].item.ItemID == itemID && currentProducingItems[i].producingStack == stack)
                return i;
        }

        return -1;
    }

    // Перерисовываем предметы для производства
    public void ReDrawProductResourcesAt(int part)
    {
        ClearProductResourcesAt(part);

        Debug.Log("Перерисовываю продукцию в " + VillageParts[part].name);

        for (int i = 0; i < VillageParts[part].productResources.Length; i++)
        {
            if (VillageParts[part].productResources[i].neededLvl <= VillageParts[part].currentLvl)
            {
                GameObject slot = Instantiate(resourceSlotPrefab, InfoPanels[part].transform.GetChild(InfoPanels[part].transform.childCount - 2));

                // Обновляем картинку предмета
                slot.GetComponent<RawImage>().texture = VillageParts[part].productResources[i].resource.neededItem.Icon;
                // Обновляем текст о количестве предмета
                slot.transform.GetChild(0).GetComponent<Text>().text = VillageParts[part].productResources[i].resource.neededStack.ToString();

                // Инициализация
                slot.GetComponent<neededResourceSlot>().item = VillageParts[part].productResources[i].resource.neededItem;
                slot.GetComponent<neededResourceSlot>().producingStack = VillageParts[part].productResources[i].resource.neededStack;

                if (CheckProducingItem(slot.GetComponent<neededResourceSlot>().item.ItemID, slot.GetComponent<neededResourceSlot>().producingStack) != -1)
                {
                    slot.GetComponent<neededResourceSlot>().isProducing = true;
                    slot.transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    slot.GetComponent<neededResourceSlot>().isProducing = false;
                    slot.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }

    // Показать информацию о выбранной части
    public void ShowInfoAbout(int panel)
    {
        // Закрываем другие
        CloseInfoPanels(panel);

        // Открываем панель описания
        UpdateDescrInfo(panel);

        // Открываем нашу
        Blocker.SetActive(true);
        BackgroundPanel.SetActive(true);
        InfoPanels[panel].SetActive(true);
    }

    // Закрыть информацию о выбранной части
    public void HideInfoAbout()
    {
        Blocker.SetActive(false);
        BackgroundPanel.SetActive(false);
        CloseInfoPanels(-1);
    }

    // Отобразить информацию 
    public void UpdateDescrInfo(int about)
    {
        Debug.Log("Перерисовываю тексты в " + InfoPanels[about].name);
        if (about == 4)
        {
            InfoPanels[about].transform.GetChild(0).GetComponent<Text>().text = villageName;
            InfoPanels[about].transform.GetChild(4).GetComponent<Text>().text = "Популярность деревни: " + villagePopularity;
            InfoPanels[about].transform.GetChild(5).GetComponent<Text>().text = "Статус: " + _villageStatus;
            InfoPanels[about].transform.GetChild(6).GetComponent<Text>().text = "Общая прибыль золота: " + villageIncome;
            InfoPanels[about].transform.GetChild(7).GetComponent<Text>().text = "Население: " + villageCapacity + "/" + villageMaxCapacity;
            InfoPanels[about].transform.GetChild(InfoPanels[about].transform.childCount - 1).GetChild(0).GetComponent<Text>().text = "Урв: " + VillageParts[about].currentLvl + "/" + VillageParts[about].maxLvl;
            InfoPanels[about].transform.GetChild(InfoPanels[about].transform.childCount - 1).GetChild(1).GetComponent<Text>().text = "Цена: " + VillageParts[about].upgradeCost;
        }
        else
        { 
            InfoPanels[about].transform.GetChild(1).GetComponent<Text>().text = "Прибыль золота: " + VillageParts[about].currentIncome;
            InfoPanels[about].transform.GetChild(2).GetComponent<Text>().text = "Престиж: " + VillageParts[about].currentPopularity;
            InfoPanels[about].transform.GetChild(InfoPanels[about].transform.childCount - 1).GetChild(0).GetComponent<Text>().text = "Урв: " + VillageParts[about].currentLvl + "/" + VillageParts[about].maxLvl;
            InfoPanels[about].transform.GetChild(InfoPanels[about].transform.childCount - 1).GetChild(1).GetComponent<Text>().text = "Цена: " + VillageParts[about].upgradeCost;
        }

        ReDrawNeededResourcesAt(about);
        ReDrawProductResourcesAt(about);
    }

    // Закрытие всех панелей кроме одной
    public void CloseInfoPanels(int except)
    {
        for (int i = 0; i < InfoPanels.Length; i++)
        {
            if (i == except)
                continue;
            else
                InfoPanels[i].SetActive(false);
        }
    }

    // Проверяем есть ли у игрока все предметы для улучшения постройки
    private bool isAllNeededItemsExist(int part)
    {
        bool isExist = true;

        for(int i = 0; i < VillageParts[part].neededItemsToUpgrade[VillageParts[part].currentLvl].neededItems.Length; i++)
        {
            if(INV.GetItemStack(VillageParts[part].neededItemsToUpgrade[VillageParts[part].currentLvl].neededItems[i].neededItem.ItemID) < VillageParts[part].neededItemsToUpgrade[VillageParts[part].currentLvl].neededItems[i].neededStack)
            {
                isExist = false;
                break;
            }
        }

        return isExist;
    }

    // Ищем текущий ресурс в общем списке
    public int ResourcePos(string itemID, int stack)
    {
        for(int i = 0; i < currentProducingItems.Count; i++)
        {
            if (currentProducingItems[i].item.ItemID == itemID && currentProducingItems[i].producingStack == stack)
                return i;
        }

        return -1;
    }

    // Получить продукции с производства
    public void GetProducedItems()
    {
        GH.GiveMoney(villageIncome);
        GH.AddEventText("Доход с деревни: ");
        GH.GiveExp(villageIncome * 0.1);
        GH.AddEventText("Опыт с деревни: ");

        for (int i = 0; i < currentProducingItems.Count; i++)
        {
            INV.AddItem(currentProducingItems[i].item.ItemID, currentProducingItems[i].producingStack);
        }
        if (currentProducingItems.Count > 0)
            GH.AddEventText("Предметы с деревни: ");
        else
            GH.AddEventText("Предметов с деревни не получено.");
    }

    // Добавляем в общий список продукцию
    public void AddProducingResource(ItemProfile item, int stack)
    {
        currentProducingItems.Add(new producingItem() { item = item, producingStack = stack });
    }

    // Удаляем из общего списка продукцию
    public void RemoveProducingResource(string itemID, int stack)
    {
        int res = ResourcePos(itemID, stack);
        // Удаляем ресурс из переработки
        if (res != -1)
            currentProducingItems.RemoveAt(res);
    }

    // Улучшение части
    public void UpgradePart(int partNum)
    {
        if (VillageParts[partNum].currentLvl < VillageParts[partNum].maxLvl)
        {
            if (Player.Money >= VillageParts[partNum].upgradeCost)
            {
                if (isAllNeededItemsExist(partNum))
                {
                    // Вычитаем деньги
                    Player.Money -= VillageParts[partNum].upgradeCost;
                    // Увеличиваем цену
                    VillageParts[partNum].upgradeCost += VillageParts[partNum].upgradeMulty;
                    // Добавляем уровень
                    VillageParts[partNum].currentLvl++;
                    // Добавляем рабочих
                    VillageParts[partNum].maxWorkers += VillageParts[partNum].defaultWorkersUpgrade;
                    // Добавляем популярность
                    VillageParts[partNum].currentPopularity += VillageParts[partNum].defaultPopularityUpgrade;
                    // Добавляем инкам
                    VillageParts[partNum].currentIncome += VillageParts[partNum].defaultIncomeUpgrade;

                    UpdateDescrInfo(partNum);
                }
                else
                    GH.ShowMessageText("Недостаточно ресурсов для улучшения!");
            }
            else
                GH.ShowMessageText("Недостаточно золотых монет для улучшения!");
        }
        else
            GH.ShowMessageText("Здание улучшено на максимальный уровень!");
    }

    // Изменение имени деревни
    public void ChangeVillageName()
    {
        changeVillageName.SetActive(true);
    }

    // Подтверждение изменения
    public void AcceptVillageName()
    {
        // Устанавливаем значение
        villageName = changeVillageName.GetComponent<InputField>().text;
        // Помещаем в текст 
        InfoPanels[4].transform.GetChild(0).GetComponent<Text>().text = villageName;
        // Закрываем панель ввода
        changeVillageName.SetActive(false);
    }
}
