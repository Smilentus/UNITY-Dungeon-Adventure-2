using System;
using System.Collections.Generic;


/*
�� ����� ������ ���� ���������� ��������� ��������, ������ ����� � �.�.
����� ������ ���� ����������� ���������, ������� � ��������� ����� ����������� � ����������
�� ���� ����� ������� ������ �� 15 ������, ������ ���� ����������� ������� ��� ����, ���������� � �.�.
������ ������ ��������� ������ ����� ������ �������� ��������� � ���������� ������, ��������� ��� ��������
� ����� ��, ��� �������� � ���� ������ � ������� ������ ��� ����� ���������
�� ����� ���� � ������� ���������� ����� ���� ����, ������� ���������� �� ����� ����, ������ ��� ����, ����� �������� ����� ��������� ����� ��������
� ����� ��� ����� ������� ����� �������������, ��� ���������� �����

TODO � �.�.:
> ������� ����� ��� ����������� � �.�.
> ������� � ����������� ��������� ��� ��������� ��� ��� ������ ��������
> ���������� ����������� ��������� � �.�.
> ����������� ��� ��������� ��������� � ����������
> ������ �������������� � ���, ��� ��� �������� ����� �������
> ������������ �������� � ���������� ����� � ����� ��� ��������� (���������� ���� � �����)
*/

[System.Serializable]
public class BaseInventoryContainer
{
    public event Action onInventorySlotsUpdated;


    protected List<BaseInventoryContainerSlot> m_inventorySlots;
    public List<BaseInventoryContainerSlot> InventorySlots => m_inventorySlots;


    private int inventoryCapacity;
    public int InventoryCapacity => inventoryCapacity;


    protected BaseInventoryContainerProfile inventoryContainerProfile;
    public BaseInventoryContainerProfile InventoryContainerProfile => inventoryContainerProfile;


    public BaseInventoryContainer(BaseInventoryContainerProfile _profile)
    {
        inventoryContainerProfile = _profile;
        inventoryCapacity = _profile.ContainerCapacity;

        CreateNewInventorySlots(); // ����� �������� (TODO)
    }


    protected virtual void CreateNewInventorySlots()
    {
        if (inventoryCapacity == -1)
        {
            inventoryCapacity = 0;
        }

        m_inventorySlots = new List<BaseInventoryContainerSlot>(inventoryCapacity);
        for (int i = 0; i < inventoryCapacity; i++)
        {
            m_inventorySlots.Add(new BaseInventoryContainerSlot());
        }
    }


    /// <summary>
    ///     ����� ������� ���������� �������� � ���������.
    ///     ������� �������� ���������� � ��������� �� ��� ���, ���� �� ���������� �� ��������� ��������.
    ///     ���� �� ����� � ���������� ������������ ��� �� ����������, ������� ���������� �������, ������� ��� �� ������ ��������� � ��� ���������� ����������.
    /// </summary>
    /// <param name="_baseItemToAdd">
    ///     ����������� �������
    /// </param>
    /// <param name="_stack">
    ///     ����������� ���������� ��������
    /// </param>
    /// <returns>
    ///     ����������� ������� � �� ����������, ������� �� �� ������ ���������� � ������� ����������
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

        onInventorySlotsUpdated?.Invoke();

        return new BaseInventoryAdditionData(_baseItemToAdd, unplacedStacks);
    }

    public bool IsContainsItem(BaseItemProfile profile, int amount = 1)
    {
        return GetItemAmount(profile) >= amount;
    }

    public int GetItemAmount(BaseItemProfile profile)
    {
        int itemsAmount = 0;

        foreach (BaseInventoryContainerSlot itemSlot in m_inventorySlots)
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

        foreach (BaseInventoryContainerSlot slot in m_inventorySlots)
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
        for (int i = 0; i < m_inventorySlots.Count; i++)
        {
            if (m_inventorySlots[i].IsSlotEmpty) return i;
        }

        return -1;
    }

    protected int GetNearestSlotWithItemStackDelta(BaseItem _baseItem)
    {
        for (int i = 0; i < m_inventorySlots.Count; i++)
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
        if (slotIndex >= 0 && slotIndex < m_inventorySlots.Count)
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
    public BaseItem BaseItem { get; set; }
    public int ItemStack { get; set; }


    public BaseInventoryAdditionData(BaseItem _baseItem, int _stack)
    {
        this.BaseItem = _baseItem;
        this.ItemStack = _stack;
    }
}