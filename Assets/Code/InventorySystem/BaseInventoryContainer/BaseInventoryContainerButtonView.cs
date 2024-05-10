using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dimasyechka.Code.InventorySystem.BaseInventoryContainer
{
    public class BaseInventoryContainerButtonView : MonoViewModel<BaseInventoryContainer>, IPointerClickHandler
    {
        [RxAdaptableProperty]
        public ReactiveProperty<Color> ActiveColor = new ReactiveProperty<Color>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> ContainerTitle = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<Sprite> ContainerIcon = new ReactiveProperty<Sprite>();


        [Header("Colorizer")]
        [SerializeField]
        private Color _defaultColor;

        [SerializeField]
        private Color _selectedColor;


        private bool _isOpened;
        public bool IsOpened => _isOpened;


        private Action<BaseInventoryContainer> _pressedCallback;


        protected override void OnSetupModel()
        {
            ContainerTitle.Value = Model.InventoryContainerProfile.ContainerName;
            ContainerIcon.Value = Model.InventoryContainerProfile.ContainerSprite;
        }


        public void SetPressCallback(Action<BaseInventoryContainer> pressedCallback)
        {
            _pressedCallback = pressedCallback;
        }

        public void SetOpenStatus(bool isOpened)
        {
            _isOpened = isOpened;

            if (_isOpened)
            {
                ActiveColor.Value = _selectedColor;
            }
            else
            {
                ActiveColor.Value = _defaultColor;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _pressedCallback(Model);
        }
    }
}
