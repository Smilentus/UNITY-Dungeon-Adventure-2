using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dimasyechka.Code.InventorySystem.BaseInventoryContainer
{
    public class BaseInventoryContainerSlotView : MonoViewModel<BaseInventoryContainerSlot>, IPointerClickHandler
    {
        [RxAdaptableProperty]
        public ReactiveProperty<string> ItemName = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<Sprite> ItemIcon = new ReactiveProperty<Sprite>();

        [RxAdaptableProperty]
        public ReactiveProperty<bool> IsIconAvailable = new ReactiveProperty<bool>();

        [RxAdaptableProperty]
        public ReactiveProperty<int> ItemStack = new ReactiveProperty<int>();


        private Action<BaseInventoryContainerSlot> _pressedCallback;


        protected override void OnSetupModel()
        {
            if (Model == null || Model.IsSlotEmpty)
            {
                IsIconAvailable.Value = false;
                ItemIcon.Value = null;
                ItemName.Value = "";
                ItemStack.Value = 0;
            }
            else
            {
                IsIconAvailable.Value = true;
                ItemIcon.Value = Model.SlotItem.BaseItemProfile.ItemSprite;
                ItemName.Value = Model.SlotItem.BaseItemProfile.ItemName;
                ItemStack.Value = Model.CurrentStack;
            }
        }


        public void SetPressCallback(Action<BaseInventoryContainerSlot> callback)
        {
            _pressedCallback = callback;
        }

        public void OnSlotPressed()
        {
            _pressedCallback(Model);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnSlotPressed();
        }
    }
}
