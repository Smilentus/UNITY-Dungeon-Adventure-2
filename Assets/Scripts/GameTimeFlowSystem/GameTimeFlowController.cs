using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeFlowController : MonoBehaviour
{
    private static GameTimeFlowController instance;
    public static GameTimeFlowController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameTimeFlowController>(true);
            }

            return instance;
        }
    }


    [SerializeField]
    private List<GameTimeFlowEventBaseController> m_gameTimeFlowEventBaseControllers = new List<GameTimeFlowEventBaseController>();


    [SerializeField]
    private List<GameTimeFlowEvent> m_availableGameTimeFlowEvents = new List<GameTimeFlowEvent>();
    public List<GameTimeFlowEvent> AvailableGameTimeFlowEvents => m_availableGameTimeFlowEvents;


    private List<GameTimeFlowEvent> currentGameTimeEvents = new List<GameTimeFlowEvent>();
    public List<GameTimeFlowEvent> CurrentGameTimeFlowEvents => currentGameTimeEvents;


    public int CurrentDay, CurrentMonth, CurrentYear, CurrentHour;


    public GameTimeFlowEventSaveMaskData GetSaveMaskData()
    {
        GameTimeFlowEventSaveMaskData maskData = new GameTimeFlowEventSaveMaskData();

        maskData.CurrentDay = CurrentDay;
        maskData.CurrentMonth = CurrentMonth;
        maskData.CurrentYear = CurrentYear;
        maskData.CurrentHour = CurrentHour;

        maskData.ActiveGameTimeFlowEventGUIDs = new List<string>();
        foreach (GameTimeFlowEventBaseController controller in m_gameTimeFlowEventBaseControllers)
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
            GameTimeFlowEventBaseController controller = m_gameTimeFlowEventBaseControllers.Find(x => x.GameTimeFlowEventReference.EventUID == eventGUID);

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
        if (isDay())
        {
            return "День";
        }
        else
        {
            return "Ночь";
        }
    }

    // Сейчас день или нет?
    public bool isDay()
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
    public bool isNight()
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

        // Добавляем время
        while (CurrentHour > 30)
        {
            if (CurrentHour >= 30)
            {
                CurrentHour -= 30;
                CurrentDay++;
                FindObjectOfType<PlayerVillageActivity>().GetProducedItems();
                if(CurrentDay > 45)
                {
                    CurrentDay = 1;
                    CurrentMonth++;
                    if(CurrentMonth > 20)
                    {
                        CurrentMonth = 1;
                        CurrentYear++;
                    }
                }
            }
        }

        // Проверяем какой сейчас ивент!
        CheckTimeFlowEvents();
    }

    public void ForceFinishGameTimeFlowEvent(string eventUID)
    {
        GameTimeFlowEventBaseController controller = m_gameTimeFlowEventBaseControllers.Find(x => x.GameTimeFlowEventReference.EventUID == eventUID);
        if (controller != null)
        {
            controller.FinishEvent();

            currentGameTimeEvents.Remove(controller.GameTimeFlowEventReference);
        }
    }

    public void ForceFinishAllGameTimeFlowEvents()
    {
        foreach (GameTimeFlowEventBaseController controller in m_gameTimeFlowEventBaseControllers)
        {
            if (controller.IsEventStarted)
            {
                controller.FinishEvent();

                currentGameTimeEvents.Remove(controller.GameTimeFlowEventReference);
            }
        }
    }

    // Случайный ивент связанный с временем
    public void CheckTimeFlowEvents()
    {
        foreach (GameTimeFlowEventBaseController controller in m_gameTimeFlowEventBaseControllers)
        {
            if (controller.IsEventStarted && controller.CanFinishEvent())
            {
                currentGameTimeEvents.Remove(controller.GameTimeFlowEventReference);
                controller.FinishEvent();
            }

            if (!controller.IsEventStarted && controller.CanStartEvent())
            {
                currentGameTimeEvents.Add(controller.GameTimeFlowEventReference);
                controller.StartEvent();
            }
        }
    }

    public void SetTimeFlowEvents(List<GameTimeFlowEvent> gameTimeFlowEvents)
    {
        m_availableGameTimeFlowEvents = gameTimeFlowEvents;
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