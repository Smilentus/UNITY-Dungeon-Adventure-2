using Dimasyechka.Code._LEGACY_.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dimasyechka.Code._LEGACY_
{
    public class neededItemSlot : MonoBehaviour, IPointerClickHandler
    {
        [Header("Нужный предмет")]
        public ItemProfile neededItem;

        public void OnPointerClick(PointerEventData eventData)
        {
            GameController.Instance.ShowMessageText(("Как получить предмет: \""+neededItem.Name+"\"\n\n" + neededItem.WhereToFind));
        }
    }
}
