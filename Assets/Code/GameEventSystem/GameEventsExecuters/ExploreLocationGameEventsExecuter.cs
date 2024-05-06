using System;
using Dimasyechka.Code.GameEventSystem.Interfaces;
using Dimasyechka.Code.GameEventSystem.Profiles;
using Dimasyechka.Code.LocationSystem.Controllers;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.GameEventSystem.GameEventsExecuters
{
    public class ExploreLocationGameEventsExecuter : MonoBehaviour, IGameEventsExecuter
    {
        public Type ProfileType => typeof(ExploreLocationGameEventProfile);


        private LocationsController _locationsController;

        [Inject]
        public void Construct(LocationsController locationsController)
        {
            _locationsController = locationsController;
        }


        public void TryExecuteGameEvent(BaseGameEventProfile gameEventProfile)
        {
            if (gameEventProfile.GetType().Equals(ProfileType))
            {
                ExploreLocationGameEventProfile profile = gameEventProfile as ExploreLocationGameEventProfile;

                _locationsController.ExploreLocation(profile.ExplorableLocationProfile);

                GameController.Instance.AddEventText($"{profile.EventTitle}");
            }
        }
    }
}
