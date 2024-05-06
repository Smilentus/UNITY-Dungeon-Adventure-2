using System;
using System.Collections.Generic;
using Dimasyechka.Code.InventorySystem.BaseInventoryContainer;
using Dimasyechka.Code.InventorySystem.BaseItem;
using Dimasyechka.Code.InventorySystem.BaseMouse;
using UnityEngine;

namespace Dimasyechka.Code.InventorySystem
{
    public class InventoryController : MonoBehaviour
    {
        public event Action onInventoryContainersUpdated;
        public event Action<BaseInventoryContainer.BaseInventoryContainer> onInventoryContainerOpened;
        public event Action<BaseInventoryContainer.BaseInventoryContainer> onInventoryContainerClosed;


        [SerializeField]
        private BaseInventoryContainerProfile _quickSlotsContainer;
   
        [Tooltip("���������� ���������, ������� ����� ����������� ����������� ��� ������ � ���������, ������� �� ������ ����������� � ����������")]
        [SerializeField]
        private BaseInventoryContainerProfile _dynamicTimelyContainerProfile;


        private List<BaseInventoryContainer.BaseInventoryContainer> _inventoryContainers = new List<BaseInventoryContainer.BaseInventoryContainer>();
        /// <summary>
        ///     ��� ��������� ��������� ������ (�����, ������� � �.�.)
        /// </summary>
        public List<BaseInventoryContainer.BaseInventoryContainer> InventoryContainers => _inventoryContainers;


        public List<BaseInventoryContainer.BaseInventoryContainer> OpenedContainers = new List<BaseInventoryContainer.BaseInventoryContainer>();


        private BaseInventoryContainer.BaseInventoryContainer quickSlotsContainer;
        public BaseInventoryContainer.BaseInventoryContainer QuickSlotsContainer => quickSlotsContainer;


        private BaseInventoryContainer.BaseInventoryContainer dynamicTimelyContainer;


        private BaseMouseItemController mouseItemController;
        public BaseMouseItemController MouseItemController => mouseItemController;


        private void Awake()
        {
            mouseItemController = new BaseMouseItemController();

            quickSlotsContainer = new BaseInventoryContainer.BaseInventoryContainer(_quickSlotsContainer);
            dynamicTimelyContainer = new BaseInventoryContainer.BaseInventoryContainer(_dynamicTimelyContainerProfile);
        }


        public void OnAnyContainerSlotPressed(BaseInventoryContainer.BaseInventoryContainer _container, BaseInventoryContainerSlot _slot)
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
                    BaseItem.BaseItem tempItem = _slot.SlotItem;
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


        public void AddInventoryContainer(BaseInventoryContainerProfile _baseInventoryContainerProfile)
        {
            _inventoryContainers.Add(new BaseInventoryContainer.BaseInventoryContainer(_baseInventoryContainerProfile));

            onInventoryContainersUpdated?.Invoke();
        }


        public void ToggleContainer(BaseInventoryContainer.BaseInventoryContainer _container)
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


        public void OpenContainer(BaseInventoryContainer.BaseInventoryContainer _container)
        {
            if (!OpenedContainers.Contains(_container))
            {
                OpenedContainers.Add(_container);

                onInventoryContainerOpened?.Invoke(_container);
            }
        }

        public void CloseContainer(BaseInventoryContainer.BaseInventoryContainer _container)
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
