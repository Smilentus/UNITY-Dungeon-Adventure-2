using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BaseInventoryContainerSlot
{
    protected BaseItem m_slotItem;
    public BaseItem SlotItem => m_slotItem;
}
