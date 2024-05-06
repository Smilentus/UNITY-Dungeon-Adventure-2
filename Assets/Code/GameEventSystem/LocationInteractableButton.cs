using Dimasyechka.Code.GameEventSystem.Controllers;
using Dimasyechka.Code.GameEventSystem.Profiles;
using Dimasyechka.Code.LocationSystem.Profiles;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dimasyechka.Code.GameEventSystem
{
    public class LocationInteractableButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField]
        private LocationProfile _locationProfile;


        public void Press()
        {
            if (_locationProfile == null)
            {
                Debug.LogError($"Ћокаци€ не назначени€ на действие!", this.gameObject);
                return;
            }

            if (_locationProfile.LocationEvents.Count == 0)
            {
                Debug.LogError($"Ќа локации не назначены действий!", this.gameObject);
                return;
            }

            BaseGameEventProfile baseGameEventProfile = _locationProfile.GetRandomLocationEvent;

            if (baseGameEventProfile == null)
            {
                Debug.LogError($"Ќе смогли получить случайное событие на локации!", this.gameObject);
                return;
            }

            GameEventsController.Instance.StartGameEvent(baseGameEventProfile);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            Press();
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }
    }
}
