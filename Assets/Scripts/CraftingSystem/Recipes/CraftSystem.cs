using System.Collections.Generic;
using UnityEngine;


public class CraftSystem : MonoBehaviour
{
    private static CraftSystem _instance;
    public static CraftSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CraftSystem>();
            }

            return _instance;
        }
    }


    public void TryCraftRecipe(CraftingRecipe craftingRecipe)
    {
        if (IsInventoryContainsAllInputItems(craftingRecipe.InputCraftItems))
        {
            foreach (InputCraftItem inputCraftItem in craftingRecipe.InputCraftItems)
            {
                if (InventoryController.Instance.TryRemoveItemFromAnyInventory(inputCraftItem.InputItemProfile, inputCraftItem.InputItemAmount))
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
        return InventoryController.Instance.IsItemContainsInAnyContainer(inputCraftItem.InputItemProfile, inputCraftItem.InputItemAmount);
    }
}