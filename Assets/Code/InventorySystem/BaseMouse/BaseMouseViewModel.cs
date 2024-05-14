using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.InventorySystem.BaseMouse
{
    public class BaseMouseViewModel : MonoViewModel<BaseMouseItemController>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<Sprite> ItemIcon = new ReactiveProperty<Sprite>();

        [RxAdaptableProperty]
        public ReactiveProperty<int> ItemStack = new ReactiveProperty<int>();

        [RxAdaptableProperty]
        public ReactiveProperty<bool> IsMouseItemVisible = new ReactiveProperty<bool>();

        [RxAdaptableProperty]
        public ReactiveProperty<Vector2> MouseSlotPosition = new ReactiveProperty<Vector2>();


        [Inject]
        public void Construct(BaseMouseItemController controller)
        {
            ZenjectModel(controller);
        }

        // TODO: Переделать
        private void Update()
        {
            if (Model != null)
            {
                if (Model.MouseSlot.IsSlotEmpty)
                {
                    IsMouseItemVisible.Value = false;
                    return;
                }

                IsMouseItemVisible.Value = true;
                MouseSlotPosition.Value = Input.mousePosition;

                ItemIcon.Value = Model.MouseSlot.SlotItem.BaseItemProfile.ItemSprite;
                ItemStack.Value = Model.MouseSlot.CurrentStack;
            }
            else
            {
                IsMouseItemVisible.Value = false;
            }
        }
    }
}
