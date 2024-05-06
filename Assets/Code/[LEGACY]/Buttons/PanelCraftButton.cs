using Dimasyechka.Code._LEGACY_.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dimasyechka.Code._LEGACY_.Buttons
{
    public class PanelCraftButton : MonoBehaviour, IPointerClickHandler
    {
        [Header("Номер кнопки 0 - Зелья. 1 - Материалы. 2 - Броня. 3 - Оружие")]
        public int buttonNum = 0;

        public void OnPointerClick(PointerEventData eventData)
        {
            FindObjectOfType<CraftingManager>().OpenPanel(buttonNum);
        }
    }
}
