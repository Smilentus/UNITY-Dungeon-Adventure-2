using System;
using Dimasyechka.Code.GameEventSystem.Interfaces;
using Dimasyechka.Code.GameEventSystem.Profiles;
using Dimasyechka.Code.LocationSystem.Controllers;
using UnityEngine;

namespace Dimasyechka.Code.GameEventSystem.GameEventsExecuters
{
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
}
