using TMPro;
using UnityEngine;

public class BaseInventoryContainerSlotView : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text m_itemName;

    [SerializeField]
    protected TMP_Text m_itemStack;


    public void SetData(BaseInventoryContainerSlot _slot)
    {
        if (m_itemName != null)
        {
            m_itemName.text = _slot.SlotItem.BaseItemProfile.ItemName;
        }

        if (m_itemName != null)
        {

        }
    }
}
