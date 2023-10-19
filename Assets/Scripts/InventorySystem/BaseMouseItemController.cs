using UnityEngine;

public class BaseMouseItemController : MonoBehaviour
{
    public BaseInventoryContainerSlot MouseSlot { get; private set; }


    private void Awake()
    {
        MouseSlot = new BaseInventoryContainerSlot();
    }


    public bool IsMouseSlotEmpty()
    {
        return MouseSlot.SlotItem == null;
    }


    public void SetItemToMouseSlot(BaseItem _itemToAdd)
    {
        MouseSlot.SetSlotItem(_itemToAdd);
    }
}
