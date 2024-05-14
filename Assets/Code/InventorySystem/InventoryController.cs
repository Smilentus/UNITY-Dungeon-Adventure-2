using Dimasyechka.Code.InventorySystem.BaseInventoryContainer;
using Dimasyechka.Code.InventorySystem.BaseItem;
using Dimasyechka.Code.InventorySystem.BaseMouse;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.InventorySystem
{
    public class InventoryController : MonoBehaviour
    {
        public event Action onInventoryContainersUpdated;
        public event Action<BaseInventoryContainer.BaseInventoryContainer> onInventoryContainerOpened;
        public event Action<BaseInventoryContainer.BaseInventoryContainer> onInventoryContainerClosed;


        [SerializeField]
        private BaseInventoryContainerProfile _quickSlotsContainerProfile;

        [Tooltip("Уникальный контейнер, который будет динамически расширяться для мусора и предметов, которые не смогли поместиться в контейнере")]
        [SerializeField]
        private BaseInventoryContainerProfile _dynamicTimelyContainerProfile;


        private List<BaseInventoryContainer.BaseInventoryContainer> _inventoryContainers = new List<BaseInventoryContainer.BaseInventoryContainer>();
        /// <summary>
        ///     Все доступные инвентари игрока (сумки, рюкзаки и т.п.)
        /// </summary>
        public List<BaseInventoryContainer.BaseInventoryContainer> InventoryContainers => _inventoryContainers;


        [HideInInspector]
        public List<BaseInventoryContainer.BaseInventoryContainer> OpenedContainers = new List<BaseInventoryContainer.BaseInventoryContainer>();


        private BaseInventoryContainer.BaseInventoryContainer _quickSlotsContainer;
        public BaseInventoryContainer.BaseInventoryContainer QuickSlotsContainer => _quickSlotsContainer;


        private BaseInventoryContainer.BaseInventoryContainer _dynamicTimelyContainer;


        private BaseMouseItemController _mouseItemController;
        public BaseMouseItemController MouseItemController => _mouseItemController;


        [Inject]
        public void Construct(BaseMouseItemController mouseItemController)
        {
            _mouseItemController = mouseItemController;
        }


        private void Awake()
        {
            _quickSlotsContainer = new BaseInventoryContainer.BaseInventoryContainer(_quickSlotsContainerProfile);
            _dynamicTimelyContainer = new BaseInventoryContainer.BaseInventoryContainer(_dynamicTimelyContainerProfile);
        }


        public void OnAnyContainerSlotPressed(BaseInventoryContainer.BaseInventoryContainer container, BaseInventoryContainerSlot slot)
        {
            // Управление с ПК:
            // 2) Если слот ПУСТОЙ и мышка ЗАНЯТА - помещаем из мышки в слот
            if (slot.IsSlotEmpty && !_mouseItemController.MouseSlot.IsSlotEmpty)
            {
                slot.SetItem(_mouseItemController.MouseSlot.SlotItem, _mouseItemController.MouseSlot.CurrentStack);

                _mouseItemController.MouseSlot.ClearSlot();
                container.ForceUpdateStoredItems();
                return;
            }

            // 3) Если слот ЗАНЯТ и мышка ПУСТАЯ - показываем информацию о предмете и возможные действия
            if (!slot.IsSlotEmpty && _mouseItemController.MouseSlot.IsSlotEmpty)
            {
                _mouseItemController.MouseSlot.SetItem(slot.SlotItem, slot.CurrentStack);

                slot.ClearSlot();
                container.ForceUpdateStoredItems();
                return;
            }

            // 4) Если слот ЗАНЯТ и мышка ЗАНЯТА - свапаем предметы с мышки и на слот и забираем то что было в мышку
            // * на самом деле тут надо проверять какой предмет мы помещаем, если предметы одинаковые - дополняем стаки того предмета, который в контейнере
            // * всё что не влезло - остаётся в мышке
            // * если предметы разные - свапаем их
            if (!slot.IsSlotEmpty && !_mouseItemController.MouseSlot.IsSlotEmpty)
            {
                if (slot.SlotItem.BaseItemProfile == _mouseItemController.MouseSlot.SlotItem.BaseItemProfile)
                {
                    int unplacedStack = slot.AddItemStack(_mouseItemController.MouseSlot.CurrentStack);
                    if (unplacedStack > 0)
                    {
                        _mouseItemController.MouseSlot.CurrentStack = unplacedStack;
                    }
                    else
                    {
                        _mouseItemController.MouseSlot.ClearSlot();
                    }
                }
                else
                {
                    BaseItem.BaseItem tempItem = slot.SlotItem;
                    int tempStack = slot.CurrentStack;

                    slot.SetItem(_mouseItemController.MouseSlot.SlotItem, _mouseItemController.MouseSlot.CurrentStack);
                    _mouseItemController.MouseSlot.SetItem(tempItem, tempStack);
                    tempItem = null;
                }
                container.ForceUpdateStoredItems();
                return;
            }

            // По факту тут надо обрабатывать любой слот в любом контейнере, что я и делаю
            // Тут должны быть обработки, а пустой ли этот слот, а пустой ли слот в мышке
            // А могу ли я взять этот предмет и т.п.
            // А что если все контейнеры заполнятся, пока я буду перетаскивать предмет?
            // (тут лайфхак сделаю, появится контейнер, который будет по типу земли, якобы ты вещь уронил просто из рук и убрать её некуда было, и таймер запустится ещё, чтобы не абузили в 1 минуту на каждый предмет)

            // В идеале здесь вынести ещё всё это в контроллер, который будет проверять взаимодействие со слотом
            // Типо нажали, зажали или удержали (но я думаю это на сам слот надо выносить)
            // Каждый слот обрабатывает сам себя короче

            // Фактические проверки: (должны отличаться в зависимости от устройств по факту же...)
            // НА ПК:
            // НАЖАТИЕ НА СЛОТ
            // 1) Если слот ПУСТОЙ и мышка ПУСТАЯ - ничего не происходит
            // 2) Если слот ПУСТОЙ и мышка ЗАНЯТА - помещаем из мышки в слот
            // 3) Если слот ЗАНЯТ и мышка ПУСТАЯ - показываем информацию о предмете и возможные действия
            // 4) Если слот ЗАНЯТ и мышка ЗАНЯТА - свапаем предметы с мышки и на слот и забираем то что было в мышку
            //
            // НА МОБИЛКЕ:
            // НАЖАТИЕ НА СЛОТ
            // 1) Если слот ЗАНЯТ и палец ПУСТОЙ - показываем информацию о предмете и возможные действия
            // 2) Если слот ПУСТОЙ и палец ПУСТОЙ - ничего не происходит
            // 3) Если слот ЗАНЯТ и палец ЗАНЯТ - ничего не происходит
            // 4) Если слот ПУСТОЙ и палец ЗАНЯТ - ничего не происходит
            // 
            // УДЕРЖИВАЕМ ПАЛЕЦ НАД СЛОТОМ
            // 1) Если слот ПУСТОЙ и палец ПУСТОЙ - ничего не происходит
            // 2) Если слот ПУСТОЙ и палец ЗАНЯТ - ничего не происходит
            // 3) Если слот ЗАНЯТ и палец ЗАНЯТ - ничего не происходит
            // 4) Если слот ЗАНЯТ и палец ПУСТОЙ - пытаемся забрать предмет в палец, для перемещения
            //
            // ОТПУСКАЕМ ПАЛЕЦ НАД СЛОТОМ 
            // 1) Если слот ПУСТОЙ и палец ЗАНЯТ - помещаем из пальца в слот
            // 2) Если слот ПУСТОЙ и палец ПУСТОЙ - ничего не происходит
            // 3) Если слот ЗАНЯТ и палец ПУСТОЙ - ничего не происходит
            // 4) Если слот ЗАНЯТ и палец ЗАНЯТ - пытаемся вернуть предмет обратно в любой из доступных контейнеров
            // 
            // На любой платформе, в любом случае:
            // Если мы пытались переместить предмет и у нас не получилось
            // То предмет пытается ВЕРНУТЬСЯ в ЛЮБОЙ ДОСТУПНЫЙ контейнер
            // Если же НИ ОДИН КОНТЕЙНЕР НЕ ДОСТУПЕН, то предмет ПОМЕЩАЕТСЯ на ЗЕМЛЮ (динамичный воздушный контейнер)
            // Каждая вещь, помещённая в подобный контейнер имеет собственный таймер (чтобы не обузили, надо лимиты делать)
            // Когда предмет забираешь, таймер не сбрасывается, пока ты его не поместишь обратно в контейнер
            // Если очень часто пытаться поместить предмет с земли в контейнер, он в конечном счёте пропадает (его съедает природное окружение)
            // По истечению таймера предмет навсегда пропадает

        }


        public void TryAddItemToAnyContainer(BaseItemProfile baseItemProfile, int stack)
        {
            if (_inventoryContainers.Count == 0) { return; }

            BaseInventoryAdditionData additionData = new BaseInventoryAdditionData(new BaseItem.BaseItem(baseItemProfile), stack);

            for (int i = 0; i < _inventoryContainers.Count; i++)
            {
                additionData = _inventoryContainers[i].TryAddItem(additionData.BaseItem, additionData.ItemStack);

                if (additionData.ItemStack == 0)
                {
                    break;
                }
            }

            if (additionData.ItemStack > 0)
            {
                // Не смогли разместить предмет ни в один контейнер - значит помещаем его в динамический контейнер с мусором (или на землю, я пока не решил)
            }
        }


        public void AddInventoryContainer(BaseInventoryContainerProfile baseInventoryContainerProfile)
        {
            _inventoryContainers.Add(new BaseInventoryContainer.BaseInventoryContainer(baseInventoryContainerProfile));

            onInventoryContainersUpdated?.Invoke();
        }


        public void ToggleContainer(BaseInventoryContainer.BaseInventoryContainer container)
        {
            if (OpenedContainers.Contains(container))
            {
                CloseContainer(container);
            }
            else
            {
                OpenContainer(container);
            }
        }


        public void OpenContainer(BaseInventoryContainer.BaseInventoryContainer container)
        {
            if (!OpenedContainers.Contains(container))
            {
                OpenedContainers.Add(container);

                onInventoryContainerOpened?.Invoke(container);
            }
        }

        public void CloseContainer(BaseInventoryContainer.BaseInventoryContainer container)
        {
            if (OpenedContainers.Contains(container))
            {
                OpenedContainers.Remove(container);

                onInventoryContainerClosed?.Invoke(container);
            }
        }


        public bool IsItemContainsInAnyContainer(BaseItemProfile profile, int amount = 1)
        {
            return GetItemAmountInAnyContainer(profile) >= amount;
        }

        public int GetItemAmountInAnyContainer(BaseItemProfile profile)
        {
            int foundedAmount = 0;

            foreach (BaseInventoryContainer.BaseInventoryContainer baseInventoryContainer in InventoryContainers)
            {
                foundedAmount += baseInventoryContainer.GetItemAmount(profile);
            }

            return foundedAmount;
        }


        public bool TryRemoveItemFromAnyInventory(BaseItemProfile profile, int amount = 1)
        {
            if (GetItemAmountInAnyContainer(profile) < amount) return false;

            int amountLeft = amount;

            foreach (BaseInventoryContainer.BaseInventoryContainer inventoryContainer in InventoryContainers)
            {
                if (amount <= 0) break;

                amountLeft = inventoryContainer.TryDeleteItemAmount(profile, amount);
            }

            return true;
        }
    }
}
