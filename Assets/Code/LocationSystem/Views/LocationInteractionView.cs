using Dimasyechka.Code.LocationSystem.Controllers;
using Dimasyechka.Code.LocationSystem.Profiles;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dimasyechka.Code.LocationSystem.Views
{
    public class LocationInteractionView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private LocationProfile m_locationProfile;

        [SerializeField]
        private int m_travelHours = 1;
    
    
        public void OnPointerClick(PointerEventData eventData)
        {
            LocationsController.Instance.ShowInfoAboutLocation(m_locationProfile, m_travelHours);
        }
    }
}
