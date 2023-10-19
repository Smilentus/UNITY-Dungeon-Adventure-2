[System.Serializable]
public class BaseInventoryContainerSlot
{
    protected BaseItem m_slotItem;
    public BaseItem SlotItem => m_slotItem;


    public void SetSlotItem(BaseItem _baseItemToSet)
    {
        m_slotItem = _baseItemToSet;
    }
}
