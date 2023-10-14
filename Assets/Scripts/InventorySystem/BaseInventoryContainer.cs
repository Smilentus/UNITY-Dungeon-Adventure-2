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


    public virtual void AddItem(BaseItem baseItemToAdd)
    {


        onInventorySlotsUpdated?.Invoke();
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
    }
}
