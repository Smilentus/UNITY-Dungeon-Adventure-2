using Dimasyechka.Code.CraftingSystem.GlobalWindow;
using Dimasyechka.Code.GlobalWindows.Controllers;
using Dimasyechka.Code.InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Dimasyechka.Code.CraftingSystem.Recipes.Views
{
    public class CraftInputItemView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image _itemImage;

        [SerializeField]
        private TMP_Text _itemCount;


        [SerializeField]
        private Color _notEnoughColor;

        [SerializeField]
        private Color _correctAmountColor;


        private InputCraftItem _inputItem;


        private InventoryController _inventoryController;

        [Inject]
        public void Construct(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }


        public void SetData(InputCraftItem inputCraftItem)
        {
            _inputItem = inputCraftItem;

            _itemImage.sprite = _inputItem.InputItemProfile.ItemSprite;

            GenerateAmountString(
                _inventoryController.GetItemAmountInAnyContainer(_inputItem.InputItemProfile),
                inputCraftItem.InputItemAmount
            );
        }


        private void GenerateAmountString(int current, int expected)
        {
            if (current >= expected)
            {
                _itemCount.color = _correctAmountColor;
            }
            else
            {
                _itemCount.color = _notEnoughColor;
            }

            _itemCount.text = $"{current}/{expected}";
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked");
            GlobalWindowsController.Instance.TryShowGlobalWindow(
                typeof(ItemDescriptionGlobalWindow),
                new ItemDescriptionGlobalWindowData()
                {
                    ItemProfile = _inputItem.InputItemProfile
                }
            );
        }
    }
}