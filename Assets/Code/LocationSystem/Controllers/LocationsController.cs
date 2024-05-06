using Dimasyechka.Code.GameEventSystem.Controllers;
using Dimasyechka.Code.GameEventSystem.Profiles;
using Dimasyechka.Code.GlobalWindows.Controllers;
using Dimasyechka.Code.LocationSystem.GlobalWindow;
using Dimasyechka.Code.LocationSystem.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.LocationSystem.Controllers
{
    [System.Serializable]
    public class LocationsController : MonoBehaviour
    {
        public event Action<LocationProfile> onTravelToLocation;
        public event Action<LocationProfile> onLocationExplored;


        [Tooltip("Сюда добавляем все доступные локации в игре")]
        [SerializeField]
        private List<LocationProfile> _allLocations = new List<LocationProfile>();


        private List<RuntimeLocationObject> _locationObjects = new List<RuntimeLocationObject>();


        private List<LocationProfile> _exploredLocations = new List<LocationProfile>();
        /// <summary>
        ///     Список всех изученных локаций игроком
        /// </summary>
        public List<LocationProfile> ExploredLocations => _exploredLocations;


        private LocationProfile _currentLocation;
        /// <summary>
        ///     Текущая локация на которой мы находимся
        /// </summary>
        public LocationProfile CurrentLocation => _currentLocation;


        private RuntimeLocationObject _currentRuntimeLocationObject;


        private GameTimeFlowController _gameTimeFlowController;

        [Inject]
        public void Construct(GameTimeFlowController gameTimeFlowController)
        {
            _gameTimeFlowController = gameTimeFlowController;
        }


        private void Awake()
        {
            GetAllRuntimeLocations();
        }


        public void SetLocationAfterLoading(LocationProfile loadingProfile)
        {
            RuntimeLocationObject tempLocationObject = _locationObjects.Find(x => x.LocationProfileReference.Equals(loadingProfile));

            if (tempLocationObject != null)
            {
                _currentLocation = loadingProfile;

                _currentRuntimeLocationObject = tempLocationObject;

                HideAllLocationObjects();

                _currentRuntimeLocationObject.gameObject.SetActive(true);
            }
        }


        public void TravelToLocation(LocationProfile locationToChange, int travelTimeHours = 0)
        {
            RuntimeLocationObject tempLocationObject = _locationObjects
                .Where(x => x.LocationProfileReference != null)
                .ToList()
                .Find(x => x.LocationProfileReference == locationToChange);

            if (tempLocationObject != null)
            {
                if (_currentLocation != locationToChange)
                {
                    _currentLocation = locationToChange;

                    _currentRuntimeLocationObject = tempLocationObject;

                    HideAllLocationObjects();

                    _currentRuntimeLocationObject.gameObject.SetActive(true);

                    _gameTimeFlowController.AddTime(travelTimeHours);

                    onTravelToLocation?.Invoke(_currentLocation);
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
            foreach (RuntimeLocationObject runtimeLocationObject in _locationObjects)
            {
                runtimeLocationObject.gameObject.SetActive(false);
            }
        }

        private void GetAllRuntimeLocations()
        {
            _locationObjects = FindObjectsOfType<RuntimeLocationObject>(true).ToList();

            //Debug.Log($"LocationsController => Было найдено {_locationObjects.Count} готовых локаций на сцене!");
        }


        public void ExploreLocation(LocationProfile exploredLocation)
        {
            if (!_exploredLocations.Contains(exploredLocation))
            {
                _exploredLocations.Add(exploredLocation);
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
            if (_currentLocation != null)
            {
                BaseGameEventProfile randomEvent = _currentLocation.LocationEvents[UnityEngine.Random.Range(0, _currentLocation.LocationEvents.Count)];

                if (randomEvent != null)
                {
                    GameEventsController.Instance.StartGameEvent(randomEvent);
                }
                else
                {
                    Debug.LogError($"Локация {_currentLocation.LocationTitle} не имеет привязанных событий.");
                }
            }
        }
    }
}