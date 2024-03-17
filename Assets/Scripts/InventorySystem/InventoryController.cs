using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private static InventoryController instance;
    public static InventoryController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryController>(); 
            }

            return instance;
        }
    }


    public event Action onInventoryContainersUpdated;
    public event Action<BaseInventoryContainer> onInventoryContainerOpened;
    public event Action<BaseInventoryContainer> onInventoryContainerClosed;


    [SerializeField]
    private BaseInventoryContainerProfile m_quickSlotsContainer;
   
    [Tooltip("���������� ���������, ������� ����� ����������� ����������� ��� ������ � ���������, ������� �� ������ ����������� � ����������")]
    [SerializeField]
    private BaseInventoryContainerProfile m_dynamicTimelyContainerProfile;


    private List<BaseInventoryContainer> inventoryContainers = new List<BaseInventoryContainer>();
    /// <summary>
    ///     ��� ��������� ��������� ������ (�����, ������� � �.�.)
    /// </summary>
    public List<BaseInventoryContainer> InventoryContainers => inventoryContainers;


    public List<BaseInventoryContainer> OpenedContainers = new List<BaseInventoryContainer>();


    private BaseInventoryContainer quickSlotsContainer;
    public BaseInventoryContainer QuickSlotsContainer => quickSlotsContainer;


    private BaseInventoryContainer dynamicTimelyContainer;


    private BaseMouseItemController mouseItemController;
    public BaseMouseItemController MouseItemController => mouseItemController;


    private void Awake()
    {
        mouseItemController = new BaseMouseItemController();

        quickSlotsContainer = new BaseInventoryContainer(m_quickSlotsContainer);
        dynamicTimelyContainer = new BaseInventoryContainer(m_dynamicTimelyContainerProfile);
    }


    public void OnAnyContainerSlotPressed(BaseInventoryContainer _container, BaseInventoryContainerSlot _slot)
    {
        // ���������� � ��:
        // 2) ���� ���� ������ � ����� ������ - �������� �� ����� � ����
        if (_slot.IsSlotEmpty && !mouseItemController.MouseSlot.IsSlotEmpty)
        {
            _slot.SetItem(mouseItemController.MouseSlot.SlotItem, mouseItemController.MouseSlot.CurrentStack);
            
            mouseItemController.MouseSlot.ClearSlot();
            _container.ForceUpdateStoredItems();
            return;
        }

        // 3) ���� ���� ����� � ����� ������ - ���������� ���������� � �������� � ��������� ��������
        if (!_slot.IsSlotEmpty && mouseItemController.MouseSlot.IsSlotEmpty)
        {
            mouseItemController.MouseSlot.SetItem(_slot.SlotItem, _slot.CurrentStack);

            _slot.ClearSlot();
            _container.ForceUpdateStoredItems();
            return;
        }

        // 4) ���� ���� ����� � ����� ������ - ������� �������� � ����� � �� ���� � �������� �� ��� ���� � �����
        // * �� ����� ���� ��� ���� ��������� ����� ������� �� ��������, ���� �������� ���������� - ��������� ����� ���� ��������, ������� � ����������
        // * �� ��� �� ������ - ������� � �����
        // * ���� �������� ������ - ������� ��
        if (!_slot.IsSlotEmpty && !mouseItemController.MouseSlot.IsSlotEmpty)
        {
            if (_slot.SlotItem.BaseItemProfile == mouseItemController.MouseSlot.SlotItem.BaseItemProfile)
            {
                int unplacedStack = _slot.AddItemStack(mouseItemController.MouseSlot.CurrentStack);
                if (unplacedStack > 0)
                {
                    mouseItemController.MouseSlot.CurrentStack = unplacedStack;
                }
                else
                {
                    mouseItemController.MouseSlot.ClearSlot();
                }
            }
            else
            {
                BaseItem tempItem = _slot.SlotItem;
                int tempStack = _slot.CurrentStack;

                _slot.SetItem(mouseItemController.MouseSlot.SlotItem, mouseItemController.MouseSlot.CurrentStack);
                mouseItemController.MouseSlot.SetItem(tempItem, tempStack);
                tempItem = null;
            }
            _container.ForceUpdateStoredItems();
            return;
        }

        // �� ����� ��� ���� ������������ ����� ���� � ����� ����������, ��� � � �����
        // ��� ������ ���� ���������, � ������ �� ���� ����, � ������ �� ���� � �����
        // � ���� �� � ����� ���� ������� � �.�.
        // � ��� ���� ��� ���������� ����������, ���� � ���� ������������� �������?
        // (��� ������� ������, �������� ���������, ������� ����� �� ���� �����, ����� �� ���� ������ ������ �� ��� � ������ � ������ ����, � ������ ���������� ���, ����� �� ������� � 1 ������ �� ������ �������)

        // � ������ ����� ������� ��� �� ��� � ����������, ������� ����� ��������� �������������� �� ������
        // ���� ������, ������ ��� �������� (�� � ����� ��� �� ��� ���� ���� ��������)
        // ������ ���� ������������ ��� ���� ������

        // ����������� ��������: (������ ���������� � ����������� �� ��������� �� ����� ��...)
        // �� ��:
        // ������� �� ����
        // 1) ���� ���� ������ � ����� ������ - ������ �� ����������
        // 2) ���� ���� ������ � ����� ������ - �������� �� ����� � ����
        // 3) ���� ���� ����� � ����� ������ - ���������� ���������� � �������� � ��������� ��������
        // 4) ���� ���� ����� � ����� ������ - ������� �������� � ����� � �� ���� � �������� �� ��� ���� � �����
        //
        // �� �������:
        // ������� �� ����
        // 1) ���� ���� ����� � ����� ������ - ���������� ���������� � �������� � ��������� ��������
        // 2) ���� ���� ������ � ����� ������ - ������ �� ����������
        // 3) ���� ���� ����� � ����� ����� - ������ �� ����������
        // 4) ���� ���� ������ � ����� ����� - ������ �� ����������
        // 
        // ���������� ����� ��� ������
        // 1) ���� ���� ������ � ����� ������ - ������ �� ����������
        // 2) ���� ���� ������ � ����� ����� - ������ �� ����������
        // 3) ���� ���� ����� � ����� ����� - ������ �� ����������
        // 4) ���� ���� ����� � ����� ������ - �������� ������� ������� � �����, ��� �����������
        //
        // ��������� ����� ��� ������ 
        // 1) ���� ���� ������ � ����� ����� - �������� �� ������ � ����
        // 2) ���� ���� ������ � ����� ������ - ������ �� ����������
        // 3) ���� ���� ����� � ����� ������ - ������ �� ����������
        // 4) ���� ���� ����� � ����� ����� - �������� ������� ������� ������� � ����� �� ��������� �����������
        // 
        // �� ����� ���������, � ����� ������:
        // ���� �� �������� ����������� ������� � � ��� �� ����������
        // �� ������� �������� ��������� � ����� ��������� ���������
        // ���� �� �� ���� ��������� �� ��������, �� ������� ���������� �� ����� (���������� ��������� ���������)
        // ������ ����, ���������� � �������� ��������� ����� ����������� ������ (����� �� �������, ���� ������ ������)
        // ����� ������� ���������, ������ �� ������������, ���� �� ��� �� ��������� ������� � ���������
        // ���� ����� ����� �������� ��������� ������� � ����� � ���������, �� � �������� ����� ��������� (��� ������� ��������� ���������)
        // �� ��������� ������� ������� �������� ���������

    }


    public void TryAddItemToAnyContainer(BaseItemProfile _baseItemProfile, int _stack)
    {
        if (inventoryContainers.Count == 0) { return; }

        BaseInventoryAdditionData additionData = new BaseInventoryAdditionData(new BaseItem(_baseItemProfile), _stack);

        for (int i = 0; i < inventoryContainers.Count; i++)
        {
            additionData = inventoryContainers[i].TryAddItem(additionData.BaseItem, additionData.ItemStack);

            if (additionData.ItemStack == 0)
            {
                break;
            }
        }

        if (additionData.ItemStack > 0)
        {
            // �� ������ ���������� ������� �� � ���� ��������� - ������ �������� ��� � ������������ ��������� � ������� (��� �� �����, � ���� �� �����)
        }
    }


    public void AddInventoryContainer(BaseInventoryContainerProfile _baseInventoryContainerProfile)
    {
        inventoryContainers.Add(new BaseInventoryContainer(_baseInventoryContainerProfile));

        onInventoryContainersUpdated?.Invoke();
    }


    public void ToggleContainer(BaseInventoryContainer _container)
    {
        if (OpenedContainers.Contains(_container))
        {
            CloseContainer(_container);
        }
        else
        {
            OpenContainer(_container);
        }
    }


    public void OpenContainer(BaseInventoryContainer _container)
    {
        if (!OpenedContainers.Contains(_container))
        {
            OpenedContainers.Add(_container);

            onInventoryContainerOpened?.Invoke(_container);
        }
    }

    public void CloseContainer(BaseInventoryContainer _container)
    {
        if (OpenedContainers.Contains(_container))
        {
            OpenedContainers.Remove(_container);

            onInventoryContainerClosed?.Invoke(_container);
        }
    }


    public bool IsItemContainsInAnyContainer(BaseItemProfile profile, int amount = 1)
    {
        return GetItemAmountInAnyContainer(profile) >= amount;
    }

    public int GetItemAmountInAnyContainer(BaseItemProfile profile)
    {
        int foundedAmount = 0;

        foreach (BaseInventoryContainer baseInventoryContainer in InventoryContainers)
        {
            foundedAmount += baseInventoryContainer.GetItemAmount(profile);
        }

        return foundedAmount;
    }


    public bool TryRemoveItemFromAnyInventory(BaseItemProfile profile, int amount = 1)
    {
        if (GetItemAmountInAnyContainer(profile) < amount) return false;

        int amountLeft = amount;

        foreach (BaseInventoryContainer inventoryContainer in InventoryContainers)
        {
            if (amount <= 0) break;

            amountLeft = inventoryContainer.TryDeleteItemAmount(profile, amount);
        }

        return true;
    }
}
