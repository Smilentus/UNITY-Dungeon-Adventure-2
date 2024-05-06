using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dimasyechka.Code.InventorySystem.BaseInventoryContainer
{
    public class BaseInventoryContainerButtonView : MonoBehaviour, IPointerClickHandler
    {
        [Header("Colorizer")]
        [SerializeField]
        private Image m_containerHighlighter;

        [SerializeField]
        private Color m_defaultColor;

        [SerializeField]
        private Color m_selectedColor;


        [Header("Inner References")]
        [SerializeField]
        protected Image m_containerIcon;

        [SerializeField]
        protected TMP_Text m_containerTitle;


        protected BaseInventoryContainer container;
        public BaseInventoryContainer Container => container;


        private bool isOpened;
        public bool IsOpened => isOpened;


        private Action<BaseInventoryContainer> pressedCallback;


        public void SetData(BaseInventoryContainer _container, Action<BaseInventoryContainer> _pressedCallback)
        {
            if (_container == null) return;

            pressedCallback = _pressedCallback;

            container = _container;

            m_containerIcon.sprite = _container.InventoryContainerProfile.ContainerSprite;
            m_containerTitle.text = _container.InventoryContainerProfile.ContainerName;
        }

        public void SetOpenStatus(bool _isOpened)
        {
            isOpened = _isOpened;

            if (isOpened)
            {
                m_containerHighlighter.color = m_selectedColor;
            }
            else
            {
                m_containerHighlighter.color = m_defaultColor;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            pressedCallback(container);
        }
    }
}
