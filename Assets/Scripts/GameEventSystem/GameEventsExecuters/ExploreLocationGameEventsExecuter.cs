using System;
using UnityEngine;


public class ExploreLocationGameEventsExecuter : MonoBehaviour, IGameEventsExecuter
{
    public Type ProfileType => typeof(ExploreLocationGameEventProfile);


    public void TryExecuteGameEvent(BaseGameEventProfile _profile)
    {
        if (_profile.GetType().Equals(ProfileType))
        {
            ExploreLocationGameEventProfile profile = _profile as ExploreLocationGameEventProfile;

            LocationsController.Instance.ExploreLocation(profile.ExplorableLocationProfile);

            GameController.Instance.AddEventText($"{profile.EventTitle}");
        }
    }
}
