using Dimasyechka.Code.InventorySystem;
using Dimasyechka.Code.InventorySystem.BaseInventoryContainer;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class InventorySaveLoadConverter : SaveLoadBaseConverter<InventorySaveData>
    {
        [SerializeField]
        private BaseInventoryContainerProfile[] _defaultPlayerContainers;


        private InventoryController _inventoryController;

        [Inject]
        public void Construct(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        public override InventorySaveData GetConverterData(string saveFileName)
        {
            InventorySaveData inventorySaveData = new InventorySaveData();

            // Сохраняем инвентарь

            return inventorySaveData;
        }

        public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
        {

        }

        public override void SetDefaultData()
        {
            foreach (BaseInventoryContainerProfile container in _defaultPlayerContainers)
            {
                _inventoryController.AddInventoryContainer(container);
            }
        }
    }

    [System.Serializable]
    public class InventorySaveData
    {

    }
}