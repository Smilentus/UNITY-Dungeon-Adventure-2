using System;
using System.Collections.Generic;


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


    protected BaseInventoryContainerSlot[] m_inventorySlots;
    public BaseInventoryContainerSlot[] InventorySlots => m_inventorySlots;


    protected BaseInventoryContainerProfile inventoryContainerProfile;
    public BaseInventoryContainerProfile InventoryContainerProfile => inventoryContainerProfile;


    public BaseInventoryContainer(BaseInventoryContainerProfile _profile)
    {
        inventoryContainerProfile = _profile;

        m_inventorySlots = new BaseInventoryContainerSlot[_profile.ContainerCapacity];
    }


    /// <summary>
    ///     Общая функция добавления предмета в контейнер.
    ///     Предмет пытается добавиться в контейнер до тех пор, пока всё количество не получится уместить.
    ///     Если же места в контейнере недостаточно или он заполнился, функция возвращает предмет, который она не смогла поместить и его остаточное количество.
    /// </summary>
    /// <param name="_baseItemToAdd">
    ///     Добавляемый предмет
    /// </param>
    /// <param name="_stack">
    ///     Добавляемое количество предмета
    /// </param>
    /// <returns>
    ///     Добавляемый предмет и то количество, которое мы не смогли разместить в текущем контейнере
    /// </returns>
    public virtual BaseInventoryAdditionData TryAddItem(BaseItem _baseItemToAdd, int _stack)
    {
        int unplacedStacks = _stack;

        int emptySlotIndex = -1;
        int replenishItemIndex = -1;

        bool isContainerAvailable = true;

        while (isContainerAvailable)
        {
            replenishItemIndex = GetNearestSlotWithItemStackDelta(_baseItemToAdd);
            if (replenishItemIndex == -1)
            {
                emptySlotIndex = GetNearestEmptySlotIndex();
                if (emptySlotIndex == -1)
                {
                    isContainerAvailable = false;
                }
                else
                {
                    m_inventorySlots[emptySlotIndex].SetItem(_baseItemToAdd);
                    unplacedStacks = m_inventorySlots[emptySlotIndex].AddItemStack(unplacedStacks);

                    if (unplacedStacks == 0)
                    {
                        isContainerAvailable = false;
                    }
                }
            }
            else
            {
                unplacedStacks = m_inventorySlots[replenishItemIndex].AddItemStack(unplacedStacks);

                if (unplacedStacks == 0)
                {
                    isContainerAvailable = false;
                }
            }
        }

        return new BaseInventoryAdditionData(_baseItemToAdd, unplacedStacks);
    }

    protected virtual int GetNearestEmptySlotIndex()
    {
        for (int i = 0; i < m_inventorySlots.Length; i++)
        {
            if (m_inventorySlots[i].IsSlotEmpty) return i;
        }

        return -1;
    }

    protected virtual int GetNearestSlotWithItemStackDelta(BaseItem _baseItem)
    {
        for (int i = 0; i < m_inventorySlots.Length; i++)
        {
            if (m_inventorySlots[i].IsSlotEmpty || m_inventorySlots[i].DeltaStack == 0) continue;

            if (m_inventorySlots[i].SlotItem.BaseItemProfile == _baseItem.BaseItemProfile)
            {
                if (m_inventorySlots[i].DeltaStack > 0)
                    return i;
            }
        }

        return -1;
    }


    public virtual BaseItem GetItemAtSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < m_inventorySlots.Length)
        {
            return m_inventorySlots[slotIndex].SlotItem;
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

    public virtual void DestroyItem(BaseItem baseItem)
    {
        onInventorySlotsUpdated?.Invoke();
    }


    public virtual void ClearSlots()
    {
        m_inventorySlots = new BaseInventoryContainerSlot[InventoryContainerProfile.ContainerCapacity];

        onInventorySlotsUpdated?.Invoke();
    }
}


[System.Serializable]
public class BaseInventoryAdditionData
{
    public BaseItem BaseItem { get; set; }
    public int ItemStack { get; set; }


    public BaseInventoryAdditionData(BaseItem _baseItem, int _stack)
    {
        this.BaseItem = _baseItem;
        this.ItemStack = _stack;
    }
}