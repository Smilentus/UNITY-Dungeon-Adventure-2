using System;
using System.Collections.Generic;
using Dimasyechka.Code.InventorySystem.BaseItem;

namespace Dimasyechka.Code.InventorySystem.BaseInventoryContainer
{
    /*
По факту должен быть контроллер доступных хранилищ, короче сумки и т.п.
Затем должна быть возможность создавать, удалять и управлять этими хранилищами с предметами
По типу игрок получил рюкзак на 15 слотов, должна быть возможность хранить там вещи, переносить и т.п.
Значит каждое хранилище должно иметь список активных предметов и количество слотов, доступных для хранения
А также то, что хранится в этих слотах и базовые методы для этого хранилища
Во время игры у каждого контейнера будет свой ГУИД, который генериться во время игры, просто для того, чтобы понимать какой контейнер каким является
Я думаю это можно сделать чисто рантаймовское, без сохранения гуида

TODO и т.п.:
> Рантайм гуиды для контейнеров и т.п.
> Тултипы и всплывающие подсказки при наведении или при выборе предмета
> Правильное перемещение предметов и т.п.
> Кэширование при изменении предметов в инвентарях
> Иконка информирования о том, что был добавлен новый предмет
> Динамическое открытие и добавление сумок в общий пул инвентаря (прикольная идея я думаю)
*/

    [System.Serializable]
    public class BaseInventoryContainer
    {
        public event Action onInventorySlotsUpdated;


        protected List<BaseInventoryContainerSlot> _inventorySlots;
        public List<BaseInventoryContainerSlot> InventorySlots => _inventorySlots;


        private int _inventoryCapacity;
        public int InventoryCapacity => _inventoryCapacity;


        protected BaseInventoryContainerProfile _inventoryContainerProfile;
        public BaseInventoryContainerProfile InventoryContainerProfile => _inventoryContainerProfile;


        public BaseInventoryContainer(BaseInventoryContainerProfile profile)
        {
            _inventoryContainerProfile = profile;
            _inventoryCapacity = profile.ContainerCapacity;

            CreateNewInventorySlots(); // Ухади отсюдава (TODO)
        }


        protected virtual void CreateNewInventorySlots()
        {
            if (_inventoryCapacity == -1)
            {
                _inventoryCapacity = 0;
            }

            _inventorySlots = new List<BaseInventoryContainerSlot>(_inventoryCapacity);
            for (int i = 0; i < _inventoryCapacity; i++)
            {
                _inventorySlots.Add(new BaseInventoryContainerSlot());
            }
        }


        /// <summary>
        ///     Общая функция добавления предмета в контейнер.
        ///     Предмет пытается добавиться в контейнер до тех пор, пока всё количество не получится уместить.
        ///     Если же места в контейнере недостаточно или он заполнился, функция возвращает предмет, который она не смогла поместить и его остаточное количество.
        /// </summary>
        /// <param name="baseItemToAdd">
        ///     Добавляемый предмет
        /// </param>
        /// <param name="stack">
        ///     Добавляемое количество предмета
        /// </param>
        /// <returns>
        ///     Добавляемый предмет и то количество, которое мы не смогли разместить в текущем контейнере
        /// </returns>
        public virtual BaseInventoryAdditionData TryAddItem(BaseItem.BaseItem baseItemToAdd, int stack)
        {
            int unplacedStacks = stack;

            int emptySlotIndex = -1;
            int replenishItemIndex = -1;

            bool isContainerAvailable = true;

            while (isContainerAvailable)
            {
                replenishItemIndex = GetNearestSlotWithItemStackDelta(baseItemToAdd);
                if (replenishItemIndex == -1)
                {
                    emptySlotIndex = GetNearestEmptySlotIndex();
                    if (emptySlotIndex == -1)
                    {
                        isContainerAvailable = false;
                    }
                    else
                    {
                        _inventorySlots[emptySlotIndex].SetItem(baseItemToAdd);
                        unplacedStacks = _inventorySlots[emptySlotIndex].AddItemStack(unplacedStacks);

                        if (unplacedStacks == 0)
                        {
                            isContainerAvailable = false;
                        }
                    }
                }
                else
                {
                    unplacedStacks = _inventorySlots[replenishItemIndex].AddItemStack(unplacedStacks);

                    if (unplacedStacks == 0)
                    {
                        isContainerAvailable = false;
                    }
                }
            }

            onInventorySlotsUpdated?.Invoke();

            return new BaseInventoryAdditionData(baseItemToAdd, unplacedStacks);
        }

        public bool IsContainsItem(BaseItemProfile profile, int amount = 1)
        {
            return GetItemAmount(profile) >= amount;
        }

        public int GetItemAmount(BaseItemProfile profile)
        {
            int itemsAmount = 0;

            foreach (BaseInventoryContainerSlot itemSlot in _inventorySlots)
            {
                if (itemSlot.IsSlotEmpty) continue;

                if (itemSlot.SlotItem.BaseItemProfile.Equals(profile))
                {
                    itemsAmount += itemSlot.CurrentStack;
                }
            }

            return itemsAmount;
        }


        public int TryDeleteItemAmount(BaseItemProfile profile, int amount)
        {
            int leftAmount = amount;

            foreach (BaseInventoryContainerSlot slot in _inventorySlots)
            {
                if (leftAmount <= 0) break;
                if (slot.IsSlotEmpty) continue;

                if (slot.CurrentStack > leftAmount)
                {
                    slot.CurrentStack -= leftAmount;
                    leftAmount = 0;
                }
                else
                {
                    leftAmount -= slot.CurrentStack;
                    DestroyItem(slot.SlotItem);
                }
            }

            return leftAmount;
        }


        protected int GetNearestEmptySlotIndex()
        {
            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                if (_inventorySlots[i].IsSlotEmpty) return i;
            }

            return -1;
        }

        protected int GetNearestSlotWithItemStackDelta(BaseItem.BaseItem baseItem)
        {
            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                if (_inventorySlots[i].IsSlotEmpty || _inventorySlots[i].DeltaStack == 0) continue;

                if (_inventorySlots[i].SlotItem.BaseItemProfile == baseItem.BaseItemProfile)
                {
                    if (_inventorySlots[i].DeltaStack > 0)
                        return i;
                }
            }

            return -1;
        }


        public virtual BaseItem.BaseItem GetItemAtSlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < _inventorySlots.Count)
            {
                return _inventorySlots[slotIndex].SlotItem;
            }
            else
            {
                return null;
            }
        }


        public virtual void DestroyItem(int slotIndex)
        {
            onInventorySlotsUpdated?.Invoke();
        }

        public virtual void DestroyItem(BaseItem.BaseItem baseItem)
        {
            onInventorySlotsUpdated?.Invoke();
        }


        public virtual void ForceUpdateStoredItems()
        {
            onInventorySlotsUpdated?.Invoke();
        }

        public virtual void ClearSlots()
        {
            CreateNewInventorySlots();

            onInventorySlotsUpdated?.Invoke();
        }
    }


    [System.Serializable]
    public class BaseInventoryAdditionData
    {
        public BaseItem.BaseItem BaseItem { get; set; }
        public int ItemStack { get; set; }


        public BaseInventoryAdditionData(BaseItem.BaseItem baseItem, int stack)
        {
            this.BaseItem = baseItem;
            this.ItemStack = stack;
        }
    }
}