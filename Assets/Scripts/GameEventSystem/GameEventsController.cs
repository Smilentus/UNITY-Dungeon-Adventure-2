using System.Collections.Generic;
using System.Linq;
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


    private List<IGameEventsExecuter> gameEventsExecuters = new List<IGameEventsExecuter>();


    private void Awake()
    {
        CollectGameEventExecuters();
    }


    private void CollectGameEventExecuters()
    {
        gameEventsExecuters = GetComponentsInChildren<IGameEventsExecuter>().ToList();
    }


    public void StartGameEvent(BaseGameEventProfile baseGameEventProfile)
    {
        activeGameEventProfile = baseGameEventProfile;

        foreach (IGameEventsExecuter executer in gameEventsExecuters)
        {
            executer.TryExecuteGameEvent(baseGameEventProfile);
        }
    }
}