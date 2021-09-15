using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeScript : MonoBehaviour
{
    // Тип события
    public enum TimeEvent
    {
        None,
        Sunstay,
        Fullmoon,
        PlanetRow,
    }

    // Текущий ивент
    public TimeEvent currentTimeEvent;

    [Header("Текущие день, месяц, год, час")]
    public static int CurrentDay, CurrentMonth, CurrentYear, CurrentHour;

    // Строка для возврата полной даты
    public static string DateNow()
    {
        return CurrentHour + "ч. - " + CurrentDay + "д " + CurrentMonth + "м " + CurrentYear + "г";
    }

    // Строка для возврата состояния суток
    public static string DayStatusNow()
    {
        if (isDay())
            return "День";
        else
            return "Ночь";
    }

    // Сейчас день или нет?
    public static bool isDay()
    {
        if (CurrentHour >= 5 && CurrentHour <= 20)
            return true;
        else
            return false;
    }
    // Сейчас ночь или нет?
    public static bool isNight()
    {
        if (CurrentHour < 5 && CurrentHour > 20)
        {
            return true;
        }
        else
            return false;
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
        SetEvent();
    }

    // Случайный ивент связанный с временем
    public void SetEvent()
    {
        if (currentTimeEvent == TimeEvent.None)
        {
            if (CurrentMonth == 10 && CurrentDay < 15)
            {
                currentTimeEvent = TimeEvent.Fullmoon;
                GameHelper._GH.ShowMessageText("В этом месяце наблюдается полное лунное затмение, будьте аккуратны, возможны некоторые сюрпризы!", 0);
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.FullmoonBuff);
            }
            else if (CurrentYear % 10 == 0 && CurrentMonth == 1 && CurrentDay <  15)
            {
                currentTimeEvent = TimeEvent.PlanetRow;
                GameHelper._GH.ShowMessageText("Планетная колонна. В этом году весь месяц планеты будут выстроены в ряд. Наблюдается усиление некоторых способностей!", 0);
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.PlanetRowBuff);
            }
            else
            {
                currentTimeEvent = TimeEvent.None;
            }
        }
    }
}
