using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LocationsController : MonoBehaviour
{
    // ===========
    private static LocationsController instance;
    public static LocationsController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LocationsController>(true);
            }

            return instance;
        }
    }


    public event Action<LocationProfile> onTravelToLocation;
    public event Action<LocationProfile> onLocationExplored;


    [Tooltip("Сюда добавляем все доступные локации в игре")]
    [SerializeField]
    private List<LocationProfile> m_allLocations = new List<LocationProfile>();


    private List<RuntimeLocationObject> m_locationObjects = new List<RuntimeLocationObject>();


    private List<LocationProfile> m_exploredLocations = new List<LocationProfile>();
    /// <summary>
    ///     Список всех изученных локаций игроком
    /// </summary>
    public List<LocationProfile> ExploredLocations => m_exploredLocations;


    private LocationProfile currentLocation;
    /// <summary>
    ///     Текущая локация на которой мы находимся
    /// </summary>
    public LocationProfile CurrentLocation => currentLocation;


    private RuntimeLocationObject currentRuntimeLocationObject;


    private void Awake()
    {
        GetAllRuntimeLocations();
    }


    public void SetLocationAfterLoading(LocationProfile loadingProfile)
    {
        RuntimeLocationObject tempLocationObject = m_locationObjects.Find(x => x.LocationProfileReference.Equals(loadingProfile));

        if (tempLocationObject != null)
        {
            currentLocation = loadingProfile;

            currentRuntimeLocationObject = tempLocationObject;

            HideAllLocationObjects();

            currentRuntimeLocationObject.gameObject.SetActive(true);
        }
    }


    public void TravelToLocation(LocationProfile locationToChange, int travelTimeHours = 0)
    {
        RuntimeLocationObject tempLocationObject = m_locationObjects
                .Where(x => x.LocationProfileReference != null)
                .ToList()
                .Find(x => x.LocationProfileReference == locationToChange);

        if (tempLocationObject != null)
        {
            if (currentLocation != locationToChange)
            {
                currentLocation = locationToChange;

                currentRuntimeLocationObject = tempLocationObject;

                HideAllLocationObjects();

                currentRuntimeLocationObject.gameObject.SetActive(true);

                GameTimeFlowController.Instance.AddTime(travelTimeHours);

                onTravelToLocation?.Invoke(currentLocation);
            }
            else
            {
                Debug.Log($"LocationsController => Мы пытаемся переместиться на ту же локацию, на которой находимся!");
            }
        }
        else
        {
            Debug.Log($"LocationsController => Попытались переместиться на локацию, которой не существует на сцене!");
        }
    }

    private void HideAllLocationObjects()
    {
        foreach (RuntimeLocationObject runtimeLocationObject in m_locationObjects)
        {
            runtimeLocationObject.gameObject.SetActive(false);
        }
    }

    private void GetAllRuntimeLocations()
    {
        m_locationObjects = FindObjectsOfType<RuntimeLocationObject>(true).ToList();

        //Debug.Log($"LocationsController => Было найдено {m_locationObjects.Count} готовых локаций на сцене!");
    }


    public void ExploreLocation(LocationProfile exploredLocation)
    {
        if (!m_exploredLocations.Contains(exploredLocation))
        {
            m_exploredLocations.Add(exploredLocation);
            onLocationExplored?.Invoke(exploredLocation);
        }
        else
        {
            Debug.Log($"Вы уже исследовали локацию {exploredLocation.LocationTitle}");
        }
    }

    public void ShowInfoAboutLocation(LocationProfile locationInfo, int travelHours)
    {
        GlobalWindowsController.Instance.TryShowGlobalWindow(
            typeof(LocationInfoGlobalWindow), 
            new LocationInfoGlobalWindowData() 
            { 
                LocationProfile = locationInfo,
                TravelHours = travelHours
            }
            );
    }


    /// <summary>
    ///     Основное действие направления по локации
    /// </summary>
    public void ExecuteLocationEventRandomly()
    {
        if (currentLocation != null)
        {
            BaseGameEventProfile randomEvent = currentLocation.LocationEvents[UnityEngine.Random.Range(0, currentLocation.LocationEvents.Count)];

            if (randomEvent != null)
            {
                GameEventsController.Instance.StartGameEvent(randomEvent);
            }
            else
            {
                Debug.LogError($"Локация {currentLocation.LocationTitle} не имеет привязанных событий.");
            }
        }
    }
}