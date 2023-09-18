using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class neededResourceSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Ресурс")]
    public ItemProfile item;
    public int producingStack;

    public bool isProducing;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isProducing)
        {
            // Тут какое-то действие при нажатии
            // И просто добавляем предмет
            FindObjectOfType<PlayerVillageActivity>().AddProducingResource(item, producingStack);
            isProducing = true;
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            // Удаляем предмет
            FindObjectOfType<PlayerVillageActivity>().RemoveProducingResource(item.ItemID, producingStack);
            isProducing = false;
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
