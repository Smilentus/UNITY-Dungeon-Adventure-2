using Dimasyechka.Code.BattleSystem.Controllers;
using Dimasyechka.Code.GameEventSystem.Interfaces;
using Dimasyechka.Code.GameEventSystem.Profiles;
using UnityEngine;

namespace Dimasyechka.Code.GameEventSystem.GameEventsExecuters
{
    public class BattleGameEventsExecuter : MonoBehaviour, IGameEventsExecuter
    {
        public System.Type ProfileType => typeof(BattleGameEventProfile);


        public void TryExecuteGameEvent(BaseGameEventProfile _profile)
        {
            if (_profile.GetType().Equals(ProfileType))
            {
                BattleGameEventProfile battleGameEventProfile = _profile as BattleGameEventProfile;
                BattleController.Instance.TryStartBattle(battleGameEventProfile.Characters);
            }
        }
    }
}
