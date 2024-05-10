using Dimasyechka.Code.InventorySystem.BaseInventoryContainer;
using Dimasyechka.Code.InventorySystem.BaseItem;
using Dimasyechka.Code.InventorySystem.BaseMouse;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dimasyechka.Code.InventorySystem
{
    public class InventoryController : MonoBehaviour
    {
        public event Action onInventoryContainersUpdated;
        public event Action<BaseInventoryContainer.BaseInventoryContainer> onInventoryContainerOpened;
        public event Action<BaseInventoryContainer.BaseInventoryContainer> onInventoryContainerClosed;


        [SerializeField]
        private BaseInventoryContainerProfile _quickSlotsContainerProfile;

        [Tooltip("���������� ���������, ������� ����� ����������� ����������� ��� ������ � ���������, ������� �� ������ ����������� � ����������")]
        [SerializeField]
        private BaseInventoryContainerProfile _dynamicTimelyContainerProfile;


        private List<BaseInventoryContainer.BaseInventoryContainer> _inventoryContainers = new List<BaseInventoryContainer.BaseInventoryContainer>();
        /// <summary>
        ///     ��� ��������� ��������� ������ (�����, ������� � �.�.)
        /// </summary>
        public List<BaseInventoryContainer.BaseInventoryContainer> InventoryContainers => _inventoryContainers;


        [HideInInspector]
        public List<BaseInventoryContainer.BaseInventoryContainer> OpenedContainers = new List<BaseInventoryContainer.BaseInventoryContainer>();


        private BaseInventoryContainer.BaseInventoryContainer _quickSlotsContainer;
        public BaseInventoryContainer.BaseInventoryContainer QuickSlotsContainer => _quickSlotsContainer;


        private BaseInventoryContainer.BaseInventoryContainer _dynamicTimelyContainer;


        private BaseMouseItemController _mouseItemController;
        public BaseMouseItemController MouseItemController => _mouseItemController;


        private void Awake()
        {
            _mouseItemController = new BaseMouseItemController();

            _quickSlotsContainer = new BaseInventoryContainer.BaseInventoryContainer(_quickSlotsContainerProfile);
            _dynamicTimelyContainer = new BaseInventoryContainer.BaseInventoryContainer(_dynamicTimelyContainerProfile);
        }


        public void OnAnyContainerSlotPressed(BaseInventoryContainer.BaseInventoryContainer _container, BaseInventoryContainerSlot _slot)
        {
            // ���������� � ��:
            // 2) ���� ���� ������ � ����� ������ - �������� �� ����� � ����
            if (_slot.IsSlotEmpty && !_mouseItemController.MouseSlot.IsSlotEmpty)
            {
                _slot.SetItem(_mouseItemController.MouseSlot.SlotItem, _mouseItemController.MouseSlot.CurrentStack);

                _mouseItemController.MouseSlot.ClearSlot();
                _container.ForceUpdateStoredItems();
                return;
            }

            // 3) ���� ���� ����� � ����� ������ - ���������� ���������� � �������� � ��������� ��������
            if (!_slot.IsSlotEmpty && _mouseItemController.MouseSlot.IsSlotEmpty)
            {
                _mouseItemController.MouseSlot.SetItem(_slot.SlotItem, _slot.CurrentStack);

                _slot.ClearSlot();
                _container.ForceUpdateStoredItems();
                return;
            }

            // 4) ���� ���� ����� � ����� ������ - ������� �������� � ����� � �� ���� � �������� �� ��� ���� � �����
            // * �� ����� ���� ��� ���� ��������� ����� ������� �� ��������, ���� �������� ���������� - ��������� ����� ���� ��������, ������� � ����������
            // * �� ��� �� ������ - ������� � �����
            // * ���� �������� ������ - ������� ��
            if (!_slot.IsSlotEmpty && !_mouseItemController.MouseSlot.IsSlotEmpty)
            {
                if (_slot.SlotItem.BaseItemProfile == _mouseItemController.MouseSlot.SlotItem.BaseItemProfile)
                {
                    int unplacedStack = _slot.AddItemStack(_mouseItemController.MouseSlot.CurrentStack);
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
                    BaseItem.BaseItem tempItem = _slot.SlotItem;
                    int tempStack = _slot.CurrentStack;

                    _slot.SetItem(_mouseItemController.MouseSlot.SlotItem, _mouseItemController.MouseSlot.CurrentStack);
                    _mouseItemController.MouseSlot.SetItem(tempItem, tempStack);
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
            if (_inventoryContainers.Count == 0) { return; }

            BaseInventoryAdditionData additionData = new BaseInventoryAdditionData(new BaseItem.BaseItem(_baseItemProfile), _stack);

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
                // �� ������ ���������� ������� �� � ���� ��������� - ������ �������� ��� � ������������ ��������� � ������� (��� �� �����, � ���� �� �����)
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
