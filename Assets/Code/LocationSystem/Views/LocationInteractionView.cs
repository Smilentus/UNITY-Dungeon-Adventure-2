using Dimasyechka.Code.LocationSystem.Controllers;
using Dimasyechka.Code.LocationSystem.Profiles;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Dimasyechka.Code.LocationSystem.Views
{
    public class LocationInteractionView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private LocationProfile _locationProfile;

        [SerializeField]
        private int _travelHours = 1;


        private LocationsController _locationsController;

        [Inject]
        public void Construct(LocationsController locationsController)
        {
            _locationsController = locationsController;
        }

    
        public void OnPointerClick(PointerEventData eventData)
        {
            _locationsController.ShowInfoAboutLocation(_locationProfile, _travelHours);
        }
    }
}
