using Dimasyechka.Code.GameEventSystem.Interfaces;
using Dimasyechka.Code.GameEventSystem.Profiles;
using Dimasyechka.Code.GlobalWindows;
using Dimasyechka.Code.GlobalWindows.Controllers;
using UnityEngine;

namespace Dimasyechka.Code.GameEventSystem.GameEventsExecuters
{
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
}
