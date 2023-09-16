using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class LocationManager : MonoBehaviour
{
    // ДОБАВЛЯЙ ВНИЗ, А ТО СЛОМАЕШЬ ВСЁ :3!
    public enum Location
    {
        None,
        Hell,
        EmptyDesert,
        DragonMountain,
        OceanHole,
        WhiteForest,
        BlueForest,
        GreenForest,
        RedForest,
        YellowForest,
        Dungeon,
        Castle,
        GF_Village,
        YF_Village,
        RF_Village,
        BF_Village,
        DungeonMines,
        RF_House,
        RF_HouseInside,
        GF_Bar,
        Player_Village,
        UM_Mine,
        GF_ForestCutter,
    }

    public enum Action
    {
        None,
        TalkToBarmen,
        GF_Fight,
        RF_Fight,
        BF_Fight,
        YF_Fight,
        WF_Fight,
        Dungeon_Fight,
        HellDiablo_Fight,
        HellCastle_Fight,
        HellPath_Fight,
        Ocean_Fight,
        LowMountain_Fight,
        HighMountain_Fight,
        RH_WatchRoom,
        UM_MineResources,
        GF_CutResources,
    }        

    [Header("Текущая локация")]
    public static Location CurrentLocation;

    [Header("Панели всех локаций")]
    public GameObject[] AllLocations;

    [Header("Окно информации о локации")]
    public GameObject locationInfoPanel;

    [Header("Список открытых дополнительных локаций игроком")]
    public List<Location> foundedLocations = new List<Location>();

    [Header("Панель дополнительных локаций")]
    public GameObject foundedLocationsPanel;
    [Header("Информация о локации")]
    public Text locationInfoText;
    [Header("Скролл-панель локаций")]
    public Transform foundedLocationsParent;
    [Header("Префаб слота локации")]
    public GameObject extraLocationPrefab;
    [Header("Вспомогательные кнопки")]
    public GameObject buttonAdventure;
    public GameObject buttonFoundedLocations;
    public Texture[] foundedLocationsImages;

    // Текущая локация, про которую открыта информация
    private Location currentInfoLoc;

    // Проверка локаций
    public void CheckLocations()
    {
        if (foundedLocations.Count > 0)
            buttonAdventure.SetActive(true);
        else
            buttonAdventure.SetActive(false);
    }
    // Проверка открыта ли локация уже
    private bool CheckExtraLoc(Location locToCheck)
    {
        bool isFounded = false;

        for(int i = 0; i < foundedLocations.Count; i++)
        {
            if(foundedLocations[i] == locToCheck)
            {
                isFounded = true;
                break;
            }
        }

        return isFounded;
    }
    // Поиск локации в общем списке
    private int LocPos(Location locToFind)
    {
        for(int i = 0; i < AllLocations.Length; i++)
        {
            if (AllLocations[i].GetComponent<DropOnLocation>().panelLocation == locToFind)
                return i;
        }
        return 0; 
    }

    // Текстовое отображение локации
    public static string LocationToText(Location locToText)
    {
        string text = "Локация: ";

        switch(locToText)
        {
            case Location.GreenForest:
                text += "Зелёный лес";
                break;
            case Location.BlueForest:
                text += "Синий лес";
                break;
            case Location.RedForest:
                text += "Красный лес";
                break;
            case Location.YellowForest:
                text += "Жёлтый лес";
                break;
            case Location.WhiteForest:
                text += "Белый лес";
                break;
            case Location.Hell:
                text += "Ад";
                break;
            case Location.DragonMountain:
                text += "Драконий Утёс";
                break;
            case Location.Dungeon:
                text += "Подземелье";
                break;
            case Location.OceanHole:
                text += "Водоворот";
                break;
            case Location.GF_Village:
                text += "Деревня в зелёном лесу";
                break;
            case Location.YF_Village:
                text += "Деревня в жёлтом лесу";
                break;
            case Location.BF_Village:
                text += "Деревня в синем лесу";
                break;
            case Location.RF_Village:
                text += "Деревня в красном лесу";
                break;
            case Location.DungeonMines:
                text += "Подземные шахты";
                break;
            case Location.RF_House:
                text += "Красный дом";
                break;
            case Location.RF_HouseInside:
                text += "Красный дом: Внутри";
                break;
            case Location.Player_Village:
                text += "Деревня игрока";
                break;
            case Location.GF_Bar:
                text += "Бар в зелёном лесу";
                break;
            case Location.UM_Mine:
                text += "Шахта";
                break;
            case Location.GF_ForestCutter:
                text += "Лесопилка";
                break;
            default:
                text = "Неизвестную!";
                break;
        }

        return text;
    }

    // Добавление новой дополнительной локации игроку
    public void AddNewExtraLocation(Location extraLocation)
    {
        if (CheckExtraLoc(extraLocation) == false)
        {
            foundedLocations.Add(extraLocation);
            GameController.Instance.ShowMessageText("Вы нашли локацию " + LocationToText(extraLocation));
            buttonFoundedLocations.SetActive(true);
        }
        ReDrawFoundedLocations();
    }

    // Открытие списка дополнительных локаций
    public void ShowExtraLocations()
    {
        ReDrawFoundedLocations();
        foundedLocationsPanel.SetActive(true);
    }
    // Отрисовка найденных локаций
    public void ReDrawFoundedLocations()
    {
        // Очищаем панель
        for(int i = 0; i < foundedLocationsParent.childCount; i++)
        {
            Destroy(foundedLocationsParent.GetChild(i).gameObject);
        }

        // Добавляем новые слоты
        for(int i = 0; i < foundedLocations.Count; i++)
        {
            GameObject newSlot = Instantiate(extraLocationPrefab, foundedLocationsParent);
            newSlot.GetComponent<ExtraLocationSlot>().thisLocation = foundedLocations[i];
            // Текст локации
            newSlot.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = LocationToText(foundedLocations[i]);
            // Картинка локации
            newSlot.transform.GetChild(0).GetComponent<RawImage>().texture = Resources.Load<Texture>(("LocationsImages/"+ foundedLocations[i].ToString()));
        }
    }
    // Отображение информации о локации и кнопки перемещения
    public void ShowInfoAbout(Location locToInfo)
    {
        locationInfoText.text = "Здесь будет информация о локации, куда вы направляетесь!";
        currentInfoLoc = locToInfo;
        buttonAdventure.SetActive(true);
    }

    // Путешествие на эту локацию
    public void AdventureOnLocation()
    {
        ChangeLocation(true, 3);
        HideExtraLocations();
    }

    // Закрытие списка дополнительных локаций
    public void HideExtraLocations()
    {
        buttonAdventure.SetActive(false);
        foundedLocationsPanel.SetActive(false);
    }

    // Открыть панель информации о локации
    public void ShowLocationInfo(Location loc, string locName, string itemsToDrop, Texture partTexture)
    {
        locationInfoPanel.SetActive(true);

        currentInfoLoc = loc;

        // Превью локации
        locationInfoPanel.transform.GetChild(1).GetChild(0).GetComponent<RawImage>().texture = partTexture;

        // Описание локации
        locationInfoPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "Информация о локации: " +
            "\n Название: " + locName +
            "\n Время перемещения: 3 часа";
    }
    // Закрыть панель информации о локации
    public void HideLocationInfo()
    {
        locationInfoPanel.SetActive(false);
    }

    public void DirectChangeLocation(Location locToGo, int timeToAdd)
    {
        currentInfoLoc = locToGo;

        ChangeLocation(false, timeToAdd);
    }
    // Смена локации
    public void ChangeLocation(bool value, int timeToAdd)
    {
        try
        {
            if (CurrentLocation != currentInfoLoc)
            {
                FindObjectOfType<GameTimeFlowController>().AddTime(timeToAdd);

                int pos = LocPos(currentInfoLoc);

                CurrentLocation = currentInfoLoc;

                for (int i = 0; i < AllLocations.Length; i++)
                {
                    if (AllLocations[i].activeSelf)
                        AllLocations[i].SetActive(false);
                }
                
                FindObjectOfType<GameController>().AddEventText("Вы переместились на локацию.");
                AllLocations[pos].SetActive(true);
                Debug.Log(AllLocations[pos].name);

                if (value)
                    FindObjectOfType<PanelsManager>().OpenHideMap();
            }
            else
            {
                FindObjectOfType<GameController>().ShowMessageText("Вы уже находитесь на этой локации!");
            }
        }
        catch (System.Exception)
        {
            GameController.Instance.ShowMessageText("Такой локации не существует! :С");
        }

        HideLocationInfo();
    }
    // Смена локации после загрузки и прочего, без уведомления игрока
    public void ChangeLocationAfterLoading(Location locToChange)
    {
        try
        {
            int pos = LocPos(locToChange);

            CurrentLocation = locToChange;

            for (int i = 0; i < AllLocations.Length; i++)
            {
                if (AllLocations[i].activeSelf)
                    AllLocations[i].SetActive(false);
            }

            AllLocations[pos].SetActive(true);
        }
        catch (System.Exception)
        {
            Debug.Log("Ашыпка при перемещении!");
        }
    }
}
