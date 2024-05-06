using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dimasyechka.Code._LEGACY_.Buttons
{
    public class ButtonInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Color normalColor;
        public Color hoverColor;
        public Color pressedColor;


        public bool isHover;


        private void Start()
        {
            if(GetComponents<Image>().Length > 0)
                normalColor = GetComponent<Image>().color;
            else
                normalColor = GetComponent<RawImage>().color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHover = true;
            if (GetComponents<Image>().Length > 0)
                GetComponent<Image>().color = hoverColor;
            else
                GetComponent<RawImage>().color = hoverColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (GetComponents<Image>().Length > 0)
                GetComponent<Image>().color = pressedColor;
            else
                GetComponent<RawImage>().color = pressedColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHover = false;
            if (GetComponents<Image>().Length > 0)
                GetComponent<Image>().color = normalColor;
            else
                GetComponent<RawImage>().color = normalColor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isHover)
            {
                if (GetComponents<Image>().Length > 0)
                    GetComponent<Image>().color = hoverColor;
                else
                    GetComponent<RawImage>().color = hoverColor;
            }
            else
            {
                if (GetComponents<Image>().Length > 0)
                    GetComponent<Image>().color = normalColor;
                else
                    GetComponent<RawImage>().color = normalColor;
            }
        }
    }
}
