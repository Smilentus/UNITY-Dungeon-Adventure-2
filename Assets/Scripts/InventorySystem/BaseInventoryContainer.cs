using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
            // ��������� �������� - ��� ���� ���-�� ������
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
