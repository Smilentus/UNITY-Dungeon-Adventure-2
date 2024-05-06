using Dimasyechka.Code.BattleSystem.Controllers;
using Dimasyechka.Code.GameEventSystem.Interfaces;
using Dimasyechka.Code.GameEventSystem.Profiles;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.GameEventSystem.GameEventsExecuters
{
    public class BattleGameEventsExecuter : MonoBehaviour, IGameEventsExecuter
    {
        public System.Type ProfileType => typeof(BattleGameEventProfile);


        private BattleController _battleController;

        [Inject]
        public void Construct(BattleController battleController)
        {
            _battleController = battleController;
        }



        public void TryExecuteGameEvent(BaseGameEventProfile gameEventProfile)
        {
            if (gameEventProfile.GetType().Equals(ProfileType))
            {
                BattleGameEventProfile battleGameEventProfile = gameEventProfile as BattleGameEventProfile;
                _battleController.TryStartBattle(battleGameEventProfile.Characters);
            }
        }
    }
}
