using Dimasyechka.Code.CraftingSystem.GlobalWindow;
using Dimasyechka.Code.GlobalWindows.Controllers;
using Dimasyechka.Code.InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dimasyechka.Code.CraftingSystem.Recipes.Views
{
    public class CraftInputItemView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image m_itemImage;

        [SerializeField]
        private TMP_Text m_itemCount;


        [SerializeField]
        private Color m_notEnoughColor;

        [SerializeField]
        private Color m_correctAmountColor;


        private InputCraftItem _inputItem;


        public void SetData(InputCraftItem inputCraftItem)
        {
            _inputItem = inputCraftItem;

            m_itemImage.sprite = _inputItem.InputItemProfile.ItemSprite;

            GenerateAmountString(
                InventoryController.Instance.GetItemAmountInAnyContainer(_inputItem.InputItemProfile),
                inputCraftItem.InputItemAmount
            );
        }


        private void GenerateAmountString(int current, int expected)
        {
            if (current >= expected)
            {
                m_itemCount.color = m_correctAmountColor;
            }
            else
            {
                m_itemCount.color = m_notEnoughColor;
            }

            m_itemCount.text = $"{current}/{expected}";
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