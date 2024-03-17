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
                    // Всё нормально
                    Debug.Log($"Успешно удалён предмет из инвентаря для крафта");
                }
                else
                {
                    // Произошла какая-то ошибка
                    Debug.Log($"Не удалось удалить предмет из инвентаря во время крафта => '{inputCraftItem.InputItemProfile.ItemName}'  {inputCraftItem.InputItemAmount}");

                    break;
                }
            }
        }
        else
        {
            // TODO: Кастомное окошко, которое отображает список тех ресурсов, которых не хватает
            GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(InfoGlobalWindow), new InfoGlobalWindowData()
            {
                GlobalWindowTitle = "Ошибка создания предмета",
                InfoMessage = $"Не удалось создать рецепт '{craftingRecipe.RecipeName}' из-за недостатка предметов!",
                ApplyButtonText = "Подтвердить"
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