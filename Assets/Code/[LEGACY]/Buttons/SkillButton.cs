using Dimasyechka.Code._LEGACY_.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dimasyechka.Code._LEGACY_.Buttons
{
    public class SkillButton : MonoBehaviour, IPointerClickHandler
    {
        [Header("Тип скилла для прокачки")]
        public SkillsManager.SkillType Type;

        public void OnPointerClick(PointerEventData eventData)
        {
            FindObjectOfType<SkillsManager>().ShowSkillDescr(Type);
        }
    }
}
