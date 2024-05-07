using Dimasyechka.Code.GameTimeFlowSystem.GTFEvents;
using Dimasyechka.Code.GameTimeFlowSystem.Profiles;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.GameTimeFlowSystem.Controllers
{
    public class GameTimeFlowController : MonoBehaviour
    {
        public event Action<int> onTimeHoursPassed;
        public event Action<int> onTimeDaysPassed;
        public event Action<int> onTimeMonthsPassed;
        public event Action<int> onTimeYearsPassed;


        [SerializeField]
        private List<GameTimeFlowEventBase> _gameTimeFlowEventBaseControllers = new List<GameTimeFlowEventBase>();


        [SerializeField]
        private List<GameTimeFlowEventProfile> _availableGameTimeFlowEvents = new List<GameTimeFlowEventProfile>();
        public List<GameTimeFlowEventProfile> AvailableGameTimeFlowEvents => _availableGameTimeFlowEvents;


        private List<GameTimeFlowEventProfile> _currentGameTimeEvents = new List<GameTimeFlowEventProfile>();
        public List<GameTimeFlowEventProfile> CurrentGameTimeFlowEvents => _currentGameTimeEvents;


        public int CurrentDay, CurrentMonth, CurrentYear, CurrentHour;


        private RuntimePlayer _runtimePlayer;

        [Inject]
        public void Construct(RuntimePlayer runtimePlayer)
        {
            _runtimePlayer = runtimePlayer;
        }


        public GameTimeFlowEventSaveMaskData GetSaveMaskData()
        {
            GameTimeFlowEventSaveMaskData maskData = new GameTimeFlowEventSaveMaskData();

            maskData.CurrentDay = CurrentDay;
            maskData.CurrentMonth = CurrentMonth;
            maskData.CurrentYear = CurrentYear;
            maskData.CurrentHour = CurrentHour;

            maskData.ActiveGameTimeFlowEventGUIDs = new List<string>();
            foreach (GameTimeFlowEventBase controller in _gameTimeFlowEventBaseControllers)
            {
                if (controller.IsEventStarted)
                {
                    maskData.ActiveGameTimeFlowEventGUIDs.Add(controller.GameTimeFlowEventReference.EventUID);
                }
            }

            return maskData;
        }
        public void SetSaveMaskData(GameTimeFlowEventSaveMaskData maskData)
        {
            CurrentDay = maskData.CurrentDay;
            CurrentMonth = maskData.CurrentMonth;
            CurrentYear = maskData.CurrentYear;
            CurrentHour = maskData.CurrentHour;

            foreach (string eventGUID in maskData.ActiveGameTimeFlowEventGUIDs)
            {
                GameTimeFlowEventBase controller = _gameTimeFlowEventBaseControllers.Find(x => x.GameTimeFlowEventReference.EventUID == eventGUID);

                if (controller != null)
                {
                    controller.SetMaskData();
                }
            }
        }


        // Строка для возврата полной даты
        public string DateNow()
        {
            return CurrentHour + "ч. - " + CurrentDay + "д " + CurrentMonth + "м " + CurrentYear + "г";
        }

        // Строка для возврата состояния суток
        public string DayStatusNow()
        {
            if (IsItDay())
            {
                return "День";
            }
            else
            {
                return "Ночь";
            }
        }

        // Сейчас день или нет?
        public bool IsItDay()
        {
            if (CurrentHour >= 5 && CurrentHour <= 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Сейчас ночь или нет?
        public bool IsItNight()
        {
            if (CurrentHour < 5 && CurrentHour > 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Прибавление игрового времени
        public void AddTime(int hour)
        {
            CurrentHour += hour;

            // Почасовые экшоны
            for (int i = 0; i < hour; i++)
            {
                _runtimePlayer.PerformHealthRegeneration();
                _runtimePlayer.PerformManaRegeneration();
            }

            onTimeHoursPassed?.Invoke(hour);

            // Добавляем время
            while (CurrentHour > 30)
            {
                if (CurrentHour >= 30)
                {
                    CurrentHour -= 30;

                    CurrentDay++;

                    onTimeDaysPassed?.Invoke(1);

                    //FindObjectOfType<PlayerVillageActivity>().GetProducedItems();
                    if (CurrentDay > 45)
                    {
                        CurrentDay = 1;
                        CurrentMonth++;

                        onTimeMonthsPassed?.Invoke(1);

                        if (CurrentMonth > 20)
                        {
                            CurrentMonth = 1;

                            CurrentYear++;

                            onTimeYearsPassed?.Invoke(1);
                        }
                    }
                }
            }

            // Проверяем какой сейчас ивент!
            CheckTimeFlowEvents();
        }

        public void ForceFinishGameTimeFlowEvent(string eventUID)
        {
            GameTimeFlowEventBase controller = _gameTimeFlowEventBaseControllers.Find(x => x.GameTimeFlowEventReference.EventUID == eventUID);
            if (controller != null)
            {
                controller.FinishEvent();

                _currentGameTimeEvents.Remove(controller.GameTimeFlowEventReference);
            }
        }

        public void ForceFinishAllGameTimeFlowEvents()
        {
            foreach (GameTimeFlowEventBase controller in _gameTimeFlowEventBaseControllers)
            {
                if (controller.IsEventStarted)
                {
                    controller.FinishEvent();

                    _currentGameTimeEvents.Remove(controller.GameTimeFlowEventReference);
                }
            }
        }

        // Случайный ивент связанный с временем
        public void CheckTimeFlowEvents()
        {
            foreach (GameTimeFlowEventBase controller in _gameTimeFlowEventBaseControllers)
            {
                if (controller.IsEventStarted && controller.CanFinishEvent())
                {
                    _currentGameTimeEvents.Remove(controller.GameTimeFlowEventReference);
                    controller.FinishEvent();
                }

                if (!controller.IsEventStarted && controller.CanStartEvent())
                {
                    _currentGameTimeEvents.Add(controller.GameTimeFlowEventReference);
                    controller.StartEvent();
                }
            }
        }

        public void SetTimeFlowEvents(List<GameTimeFlowEventProfile> gameTimeFlowEvents)
        {
            _availableGameTimeFlowEvents = gameTimeFlowEvents;
        }
    }

    [System.Serializable]
    public class GameTimeFlowEventSaveMaskData
    {
        public int CurrentDay;
        public int CurrentHour;
        public int CurrentMonth;
        public int CurrentYear;

        public List<string> ActiveGameTimeFlowEventGUIDs = new List<string>();
    }
}