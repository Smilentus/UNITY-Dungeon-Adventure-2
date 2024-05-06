using System.Collections.Generic;
using Dimasyechka.Code.GlobalWindows;
using Dimasyechka.Code.GlobalWindows.Controllers;
using Dimasyechka.Code.InventorySystem;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.CraftingSystem.Recipes
{
    public class CraftSystem : MonoBehaviour
    {
        private InventoryController _inventoryController;

        [Inject]
        public void Construct(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }


        public void TryCraftRecipe(CraftingRecipe craftingRecipe)
        {
            if (IsInventoryContainsAllInputItems(craftingRecipe.InputCraftItems))
            {
                foreach (InputCraftItem inputCraftItem in craftingRecipe.InputCraftItems)
                {
                    if (_inventoryController.TryRemoveItemFromAnyInventory(inputCraftItem.InputItemProfile, inputCraftItem.InputItemAmount))
                    {
                        // �� ���������
                        Debug.Log($"������� ����� ������� �� ��������� ��� ������");
                    }
                    else
                    {
                        // ��������� �����-�� ������
                        Debug.Log($"�� ������� ������� ������� �� ��������� �� ����� ������ => '{inputCraftItem.InputItemProfile.ItemName}'  {inputCraftItem.InputItemAmount}");

                        break;
                    }
                }
            }
            else
            {
                // TODO: ��������� ������, ������� ���������� ������ ��� ��������, ������� �� �������
                GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(InfoGlobalWindow), new InfoGlobalWindowData()
                {
                    GlobalWindowTitle = "������ �������� ��������",
                    InfoMessage = $"�� ������� ������� ������ '{craftingRecipe.RecipeName}' ��-�� ���������� ���������!",
                    ApplyButtonText = "�����������"
                });
            }
        }


        private bool IsInventoryContainsAllInputItems(List<InputCraftItem> inputItems)
        {
            bool isInventoryContainsAllItems = true;

            foreach (InputCraftItem inputCraftItem in inputItems)
            {
                if (!IsInventoryContainsItem(inputCraftItem))
                {
                    isInventoryContainsAllItems = false;
                }
            }

            return isInventoryContainsAllItems;
        }

        private bool IsInventoryContainsItem(InputCraftItem inputCraftItem)
        {
            return _inventoryController.IsItemContainsInAnyContainer(inputCraftItem.InputItemProfile, inputCraftItem.InputItemAmount);
        }
    }
}