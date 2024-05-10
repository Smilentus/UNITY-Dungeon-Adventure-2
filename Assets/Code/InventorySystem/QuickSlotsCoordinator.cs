using Dimasyechka.Code.InventorySystem.BaseInventoryContainer;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.InventorySystem
{
    public class QuickSlotsCoordinator : MonoBehaviour
    {
        [SerializeField]
        private BaseInventoryContainerView _quickSlotsView;


        private InventoryController _inventoryController;

        [Inject]
        public void Construct(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }


        private void Start()
        {
            _quickSlotsView.OpenContainer(_inventoryController.QuickSlotsContainer);
        }
    }
}
