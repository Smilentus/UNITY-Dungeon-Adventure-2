using UnityEngine;


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
