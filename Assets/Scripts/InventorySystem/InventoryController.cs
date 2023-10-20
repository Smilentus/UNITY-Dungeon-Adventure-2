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


    [Tooltip("���������� ���������, ������� ����� ����������� ����������� ��� ������ � ���������, ������� �� ������ ����������� � ����������")]
    [SerializeField]
    private BaseInventoryContainerProfile m_dynamicTimelyContainer;


    private List<BaseInventoryContainer> inventoryContainers = new List<BaseInventoryContainer>();
    /// <summary>
    ///     ��� ��������� ��������� ������ (�����, ������� � �.�.)
    /// </summary>
    public List<BaseInventoryContainer> InventoryContainers => inventoryContainers;


    public List<BaseInventoryContainer> OpenedContainers = new List<BaseInventoryContainer>();


    private BaseMouseItemController mouseItemController;
    private DynamicTimelyInventoryContainer dynamicContainer;


    private void Awake()
    {
        dynamicContainer = new DynamicTimelyInventoryContainer(m_dynamicTimelyContainer);
    }


    public void OnAnyContainerSlotPressed(BaseInventoryContainer _container, BaseInventoryContainerSlot _slot)
    {
        // ���������� � ��:
        // 2) ���� ���� ������ � ����� ������ - �������� �� ����� � ����
        if (_slot.IsSlotEmpty && !mouseItemController.MouseSlot.IsSlotEmpty)
        {
            _slot.SetItem(mouseItemController.MouseSlot.SlotItem, mouseItemController.MouseSlot.CurrentStack);
            
            mouseItemController.MouseSlot.ClearSlot();
            return;
        }

        // 3) ���� ���� ����� � ����� ������ - ���������� ���������� � �������� � ��������� ��������
        if (!_slot.IsSlotEmpty && mouseItemController.MouseSlot.IsSlotEmpty)
        {
            mouseItemController.MouseSlot.SetItem(_slot.SlotItem, _slot.CurrentStack);

            _slot.ClearSlot();
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


    [Header("Debug")]
    public BaseInventoryContainerProfile debugProfile;

    public BaseItemProfile baseItemProfile;
    public int itemStack;

    [ContextMenu("Add Item")]
    public void DebugAddItem()
    {
        TryAddItemToAnyContainer(baseItemProfile, itemStack);
    }

    [ContextMenu("Add Container")]
    public void DebugAddContainer()
    {
        AddInventoryContainer(debugProfile);
    }
}
