using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        
        m_inventorySlots = new BaseInventoryContainerSlot[inventoryContainerProfile.ContainerCapacity];
    }


    public virtual bool TryAddItem(BaseItem _baseItemToAdd)
    {
        int nearestSlot = GetNearestEmptySlotIndex();

        if (nearestSlot == -1)
        {
            // Контейнер заполнен - тут надо что-то делать
            return false;
        }

        return TryPlaceItemToEmptySlot(_baseItemToAdd, nearestSlot);
    }

    public virtual bool TryAddItem(BaseItem _baseItemToAdd, int _slotIndex)
    {
        return TryPlaceItemToEmptySlot(_baseItemToAdd, _slotIndex);
    }

    
    protected virtual bool TryPlaceItemToEmptySlot(BaseItem _baseItemToAdd, int _slotIndex)
    {
        if (m_inventorySlots[_slotIndex].SlotItem == null)
        {
            m_inventorySlots[_slotIndex].SetSlotItem(_baseItemToAdd);

            return true;
        }
        else
        {
            return false;
        }
    }


    protected virtual int GetNearestEmptySlotIndex()
    {
        for (int i = 0; i < m_inventorySlots.Length; i++)
        {
            if (m_inventorySlots[i].SlotItem == null) return i;
        }

        return -1;
    }


    public virtual BaseItem GetItem(int slotIndex)
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
        m_inventorySlots = new BaseInventoryContainerSlot[inventoryContainerProfile.ContainerCapacity];

        onInventorySlotsUpdated?.Invoke();
    }
}
