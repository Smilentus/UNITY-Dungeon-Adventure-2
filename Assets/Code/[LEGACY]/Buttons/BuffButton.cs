using Dimasyechka.Code._LEGACY_.BuffSystem;
using Dimasyechka.Code.BuffSystem.Profiles;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dimasyechka.Code._LEGACY_.Buttons
{
    public class BuffButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Тип баффа")]
        public BuffProfile.BuffType bType;

        public void OnPointerEnter(PointerEventData eventData)
        {
            FindObjectOfType<BuffManager>().ShowDescr(bType);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            FindObjectOfType<BuffManager>().HideDescr();
        }
    }
}
