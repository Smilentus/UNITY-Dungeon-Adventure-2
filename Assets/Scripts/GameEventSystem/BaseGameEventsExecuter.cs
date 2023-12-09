using UnityEngine;


public class BaseGameEventsExecuter : MonoBehaviour, IGameEventsExecuter
{
    public System.Type ProfileType => typeof(BaseGameEventProfile);


    public void TryExecuteGameEvent(BaseGameEventProfile _profile)
    {
        if (_profile.GetType().Equals(ProfileType))
        {
            GlobalWindowsController.Instance.TryShowGlobalWindow(
                typeof(InfoGlobalWindow),
                new InfoGlobalWindowData()
                {
                    ApplyButtonText = "Принять",
                    GlobalWindowTitle = _profile.EventTitle,
                    InfoMessage = _profile.EventDescription
                }
            );
        }
    }
}
