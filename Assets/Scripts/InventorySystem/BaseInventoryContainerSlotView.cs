using System;
using TMPro;
using UnityEngine;

public class BaseInventoryContainerSlotView : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text m_itemName;

    [SerializeField]
    protected TMP_Text m_itemStack;


    private Action<BaseInventoryContainerSlot> pressedCallback;

    private BaseInventoryContainerSlot containerSlot;


    public void SetData(BaseInventoryContainerSlot _slot, Action<BaseInventoryContainerSlot> _callback)
    {
        pressedCallback = _callback;

        if (m_itemName != null)
        {
            m_itemName.text = _slot.SlotItem.BaseItemProfile.ItemName;
        }

        if (m_itemName != null)
        {

        }
    }

    public void OnSlotPressed()
    {
        pressedCallback(containerSlot);
    }
}
