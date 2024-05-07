using Dimasyechka.Code.LocationSystem.Controllers;
using Dimasyechka.Code.LocationSystem.Profiles;
using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using UniRx;
using Zenject;

namespace Dimasyechka.Code.UISystems
{
    public class LocationView : MonoViewModel<LocationsController>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<string> CurrentLocation = new ReactiveProperty<string>();

        [Inject]
        public void Construct(LocationsController locationsController)
        {
            ZenjectModel(locationsController);
        }


        protected override void OnSetupModel()
        {
            Model.onTravelToLocation += OnTravelToLocation;

            CurrentLocation.Value = Model.CurrentLocationTitle;
        }

        protected override void OnRemoveModel()
        {
            Model.onTravelToLocation -= OnTravelToLocation;
        }


        private void OnTravelToLocation(LocationProfile locationProfile)
        {
            CurrentLocation.Value = locationProfile.LocationTitle;
        }
    }
}
