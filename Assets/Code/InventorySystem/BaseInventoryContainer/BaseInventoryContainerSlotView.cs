using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dimasyechka.Code.InventorySystem.BaseInventoryContainer
{
    public class BaseInventoryContainerSlotView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        protected Image m_itemImage;

        [SerializeField]
        protected TMP_Text m_itemName;

        [SerializeField]
        protected TMP_Text m_itemStack;


        private Action<BaseInventoryContainerSlot> pressedCallback;

        private BaseInventoryContainerSlot containerSlot;


        public void SetData(BaseInventoryContainerSlot _slot, Action<BaseInventoryContainerSlot> _callback)
        {
            containerSlot = _slot;
            pressedCallback = _callback;

            if (_slot == null || _slot.IsSlotEmpty)
            {
                if (m_itemImage != null)
                {
                    m_itemImage.gameObject.SetActive(false);
                    m_itemImage.sprite = null;
                }

                if (m_itemName != null)
                {
                    m_itemName.text = "";
                }

                if (m_itemStack != null)
                {
                    m_itemStack.text = "";
                }
            }
            else
            {
                if (m_itemImage != null)
                {
                    m_itemImage.gameObject.SetActive(true);
                    m_itemImage.sprite = _slot.SlotItem.BaseItemProfile.ItemSprite;
                }

                if (m_itemName != null)
                {
                    m_itemName.text = _slot.SlotItem.BaseItemProfile.ItemName;
                }

                if (m_itemStack != null)
                {
                    m_itemStack.text = _slot.CurrentStack.ToString();
                }
            }
        }

        public void OnSlotPressed()
        {
            pressedCallback(containerSlot);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnSlotPressed();
        }
    }
}
