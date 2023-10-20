using UnityEngine;

public class BaseMouseItemController : MonoBehaviour
{
    public BaseInventoryContainerSlot MouseSlot { get; private set; }


    private void Awake()
    {
        MouseSlot = new BaseInventoryContainerSlot();
    }
}
