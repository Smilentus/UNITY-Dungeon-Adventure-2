using Dimasyechka.Code.InventorySystem.BaseInventoryContainer;

namespace Dimasyechka.Code.InventorySystem.BaseMouse
{
    [System.Serializable]
    public class BaseMouseItemController
    {
        public BaseInventoryContainerSlot MouseSlot { get; private set; }


        public BaseMouseItemController()
        {
            MouseSlot = new BaseInventoryContainerSlot();
        }
    }
}
