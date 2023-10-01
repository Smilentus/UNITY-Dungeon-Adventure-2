using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsController : MonoBehaviour
{
    private static GameEventsController instance;
    public static GameEventsController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameEventsController>();    
            }

            return instance;
        }
    }


    private BaseGameEventProfile activeGameEventProfile;
    public BaseGameEventProfile ActiveGameEventProfile => activeGameEventProfile;


    public void StartGameEvent(BaseGameEventProfile baseGameEventProfile)
    {
        activeGameEventProfile = baseGameEventProfile;

        if (baseGameEventProfile.GetType().Equals(typeof(BaseGameEventProfile)))
        {
            GlobalWindowsController.Instance.TryShowGlobalWindow(
                typeof(InfoGlobalWindow),
                new InfoGlobalWindowData() 
                {
                    ApplyButtonText = "Принять",
                    GlobalWindowTitle = activeGameEventProfile.EventTitle,
                    InfoMessage = activeGameEventProfile.EventDescription
                }    
            );
        }
        else if (baseGameEventProfile.GetType().Equals(typeof(BattleGameEventProfile)))
        {
            BattleGameEventProfile battleGameEventProfile = baseGameEventProfile as BattleGameEventProfile;

            BattleController.Instance.TryStartBattle(battleGameEventProfile.Characters);
        }
    }
}