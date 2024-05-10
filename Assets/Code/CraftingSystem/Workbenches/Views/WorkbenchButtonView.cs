using Dimasyechka.Code.CraftingSystem.Workbenches.Profiles;
using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dimasyechka.Code.CraftingSystem.Workbenches.Views
{
    public class WorkbenchButtonView : MonoViewModel<CraftingWorkbenchProfile>, IPointerClickHandler
    {
        [RxAdaptableProperty]
        public ReactiveProperty<Sprite> ContainerIcon = new ReactiveProperty<Sprite>();

        [RxAdaptableProperty]
        public ReactiveProperty<Color> ActiveColor = new ReactiveProperty<Color>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> ContainerTitle = new ReactiveProperty<string>();


        private Action<CraftingWorkbenchProfile> _pressCallback;


        [SerializeField]
        private Color _highlightedColor;

        [SerializeField]
        private Color _defaultColor;


        private bool _isOpened;
        public bool IsOpened => _isOpened;


        protected override void OnSetupModel()
        {
            ContainerIcon.Value = Model.WorkbenchMiniSprite;
            ContainerTitle.Value = Model.WorkbenchName;
        }


        public void SetPressCallback(Action<CraftingWorkbenchProfile> callback)
        {
            _pressCallback = callback;
        }

        public void SetOpenStatus(bool isOpened)
        {
            _isOpened = isOpened;

            if (_isOpened)
            {
                ActiveColor.Value = _highlightedColor;
            }
            else
            {
                ActiveColor.Value = _defaultColor;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _pressCallback?.Invoke(Model);
        }
    }
}
