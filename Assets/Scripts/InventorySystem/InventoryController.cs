using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public event Action onInventoryContainersUpdated;
    public event Action<BaseInventoryContainer> onInventoryContainerOpened;


    private List<BaseInventoryContainer> inventoryContainers = new List<BaseInventoryContainer>();
    /// <summary>
    ///     Все доступные инвентари игрока (сумки, рюкзаки и т.п.)
    /// </summary>
    public List<BaseInventoryContainer> InventoryContainers => inventoryContainers;


    public BaseInventoryContainer SelectedContainer { get; set; }


    public void AddInventoryContainer(BaseInventoryContainerProfile _baseInventoryContainerProfile)
    {
        inventoryContainers.Add(new BaseInventoryContainer(_baseInventoryContainerProfile));

        onInventoryContainersUpdated?.Invoke();
    }


    public void OpenContainer(BaseInventoryContainer _container)
    {
        SelectedContainer = _container;

        onInventoryContainerOpened?.Invoke(_container);
    }


    [Header("Debug")]
    public BaseInventoryContainerProfile debugProfile;

    [ContextMenu("AddContainer")]
    public void DebugAddContainer()
    {
        AddInventoryContainer(debugProfile);
    }
}
