using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code.InventorySystem.BaseMouse
{
    public class BaseMouseControllerPresenter : MonoBehaviour
    {
        [SerializeField]
        private Transform movableMouseItem;

        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private TMP_Text itemStack;


        [SerializeField]
        private Vector2 movableIconOffset;
    

        private InventoryController playerInventory;


        private void Start()
        {
            playerInventory = FindObjectOfType<InventoryController>(true);
        }


        // Переделать
        private void Update()
        {
            if (playerInventory.MouseItemController != null)
            {
                if (playerInventory.MouseItemController.MouseSlot.IsSlotEmpty)
                {
                    movableMouseItem.gameObject.SetActive(false);
                    return; 
                }

                movableMouseItem.gameObject.SetActive(true);
                movableMouseItem.position = Input.mousePosition;

                itemImage.sprite = playerInventory.MouseItemController.MouseSlot.SlotItem.BaseItemProfile.ItemSprite;
                itemStack.text = playerInventory.MouseItemController.MouseSlot.CurrentStack.ToString();
            }
            else
            {
                movableMouseItem.gameObject.SetActive(false);
            }
        }
    }
}
