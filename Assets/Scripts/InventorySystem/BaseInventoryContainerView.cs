using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
///     Отрисовывает основной интерфейс контейнера (слоты и т.п. в конкретном контейнере)
/// </summary>
public class BaseInventoryContainerView : MonoBehaviour
{
    [SerializeField]
    private GameObject m_viewGameObject;

    [SerializeField]
    private TMP_Text m_containerNameTMP;


    [SerializeField]
    private BaseInventoryContainerSlotView m_baseInventoryContainerSlotViewPrefab;

    [SerializeField]
    private Transform m_contentParent;



    private BaseInventoryContainer inventoryContainer;
    public BaseInventoryContainer InventoryContainer => inventoryContainer;


    private List<BaseInventoryContainerSlotView> slotViews = new List<BaseInventoryContainerSlotView>();


    private bool isSlotsInstantiated;


    private void OnDestroy()
    {
        ClearLastContainerData();
    }


    public void OpenContainer(BaseInventoryContainer _inventoryContainer)
    {
        if (_inventoryContainer == null)
        {
            HideContainer();
            return;
        }

        ClearLastContainerData();

        if (m_containerNameTMP != null)
        {
            m_containerNameTMP.text = $"'{_inventoryContainer.InventoryContainerProfile.ContainerName}' ({_inventoryContainer.InventoryContainerProfile.ContainerCapacity} яч.)"; // (Вместительность {_inventoryContainer.InventoryContainerProfile.ContainerCapacity} ячеек)
        }

        this.inventoryContainer = _inventoryContainer;
        CreateNewSlots();

        this.inventoryContainer.onInventorySlotsUpdated += OnInventorySlotsUpdated;
        OnInventorySlotsUpdated();

        m_viewGameObject.SetActive(true);
    }

    public void HideContainer()
    {
        m_viewGameObject.SetActive(false);
    }

    private void OnInventorySlotsUpdated()
    {
        // Пока что так, подумать про кэширование и прочие штуки
        isSlotsInstantiated = false;
        CreateNewSlots();
    }

    private void CreateNewSlots()
    {
        if (isSlotsInstantiated) return;

        isSlotsInstantiated = true;

        // Очищаем старые слоты
        for (int i = slotViews.Count - 1; i >= 0; i--)
        {
            Destroy(slotViews[i].gameObject);
        }
        slotViews.Clear();


        // Добавляем слоты заново
        int slotIndex = 0;
        foreach (BaseInventoryContainerSlot inventorySlot in inventoryContainer.InventorySlots)
        {
            BaseInventoryContainerSlotView slotView = Instantiate(m_baseInventoryContainerSlotViewPrefab, m_contentParent);
            slotView.SetData(inventorySlot, OnSlotPressedCallback);
            
            slotViews.Add(slotView);

            slotIndex++;
        }
    }

    private void OnSlotPressedCallback(BaseInventoryContainerSlot _containerSlot)
    {
        InventoryController.Instance.OnAnyContainerSlotPressed(inventoryContainer, _containerSlot);
    }

    private void ClearLastContainerData()
    {
        if (inventoryContainer != null)
        {
            inventoryContainer.onInventorySlotsUpdated -= OnInventorySlotsUpdated;
        }
    }
}
