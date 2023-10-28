[System.Serializable]
public class BaseMouseItemController
{
    public BaseInventoryContainerSlot MouseSlot { get; private set; }


    public BaseMouseItemController()
    {
        MouseSlot = new BaseInventoryContainerSlot();
    }
}
