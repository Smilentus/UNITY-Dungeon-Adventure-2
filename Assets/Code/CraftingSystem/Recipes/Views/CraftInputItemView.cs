using Dimasyechka.Code.CraftingSystem.GlobalWindow;
using Dimasyechka.Code.GlobalWindows.Controllers;
using Dimasyechka.Code.InventorySystem;
using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Dimasyechka.Code.CraftingSystem.Recipes.Views
{
    public class CraftInputItemView : MonoViewModel<InputCraftItem>, IPointerClickHandler
    {
        [RxAdaptableProperty]
        public ReactiveProperty<Sprite> ItemIcon = new ReactiveProperty<Sprite>();

        [RxAdaptableProperty]
        public ReactiveProperty<int> CurrentItemAmount = new ReactiveProperty<int>();

        [RxAdaptableProperty]
        public ReactiveProperty<int> ExpectedItemAmount = new ReactiveProperty<int>();

        [RxAdaptableProperty]
        public ReactiveProperty<Color> AmountColor = new ReactiveProperty<Color>();


        [SerializeField]
        private Color _notEnoughColor;

        [SerializeField]
        private Color _correctAmountColor;


        private InventoryController _inventoryController;

        [Inject]
        public void Construct(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }


        protected override void OnSetupModel()
        {
            ItemIcon.Value = Model.InputItemProfile.ItemSprite;

            GenerateAmountString(
                _inventoryController.GetItemAmountInAnyContainer(Model.InputItemProfile),
                Model.InputItemAmount
            );
        }


        private void GenerateAmountString(int current, int expected)
        {
            if (current >= expected)
            {
                AmountColor.Value = _correctAmountColor;
            }
            else
            {
                AmountColor.Value = _notEnoughColor;
            }

            CurrentItemAmount.Value = current;
            ExpectedItemAmount.Value = expected;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            GlobalWindowsController.Instance.TryShowGlobalWindow(
                typeof(ItemDescriptionGlobalWindow),
                new ItemDescriptionGlobalWindowData()
                {
                    ItemProfile = Model.InputItemProfile
                }
            );
        }
    }
}