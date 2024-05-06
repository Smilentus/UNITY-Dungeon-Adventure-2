using Dimasyechka.Code.GameEventSystem.Controllers;
using Dimasyechka.Code.GameEventSystem.Profiles;
using Dimasyechka.Code.LocationSystem.Profiles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dimasyechka.Code._LEGACY_.Buttons
{
    public class DirButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [Header("Обычный цвет")]
        public Color normalColor;
        [Header("Цвет наводки")]
        public Color hoverColor;
        [Header("Цвет нажатия")]
        public Color pressedColor;

    
        bool isHover;


        [Header("Локация")]
        [SerializeField]
        private LocationProfile m_locationProfile;

        [Header("Действие при нажатии")]
        [SerializeField]
        private BaseGameEventProfile m_gameEventProfile;


        public void OnPointerClick(PointerEventData eventData)
        {
            GetComponent<Image>().color = hoverColor;

            if (m_locationProfile != null)
            {
                GameController.Instance.PressDirectionButton(m_locationProfile);
            }
        
            if (m_gameEventProfile != null)
            {
                GameEventsController.Instance.StartGameEvent(m_gameEventProfile);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GetComponent<Image>().color = pressedColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHover = true;
            GetComponent<Image>().color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHover = false;
            GetComponent<Image>().color = normalColor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isHover)
                GetComponent<Image>().color = hoverColor;
            else
                GetComponent<Image>().color = normalColor;
        }
    }
}
