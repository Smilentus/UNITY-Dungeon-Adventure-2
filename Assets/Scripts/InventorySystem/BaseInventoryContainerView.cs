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



    private BaseInventoryContainer lastOpenedContainer;


    private List<BaseInventoryContainerSlotView> slotViews = new List<BaseInventoryContainerSlotView>();


    private void OnDestroy()
    {
        ClearLastContainerData();
    }


    public void OpenContainer(BaseInventoryContainer inventoryContainer)
    {
        if (inventoryContainer == null)
        {
            HideContainer();
            return;
        }

        ClearLastContainerData();

        if (m_containerNameTMP != null)
        {
            m_containerNameTMP.text = $"'{inventoryContainer.InventoryContainerProfile.ContainerName}' (Вместительность {inventoryContainer.InventoryContainerProfile.ContainerCapacity} ячеек)";
        }

        lastOpenedContainer = inventoryContainer;
        lastOpenedContainer.onInventorySlotsUpdated += OnInventorySlotsUpdated;
        OnInventorySlotsUpdated();

        m_viewGameObject.SetActive(true);
    }

    public void HideContainer()
    {
        m_viewGameObject.SetActive(false);
    }

    private void OnInventorySlotsUpdated()
    {
        // Очищаем старые слоты
        for (int i = slotViews.Count - 1; i >= 0; i--)
        {
            Destroy(slotViews[i].gameObject);
        }
        slotViews.Clear();


        // Добавляем слоты заново
        foreach (BaseInventoryContainerSlot inventorySlot in lastOpenedContainer.InventorySlots)
        {
            BaseInventoryContainerSlotView slotView = Instantiate(m_baseInventoryContainerSlotViewPrefab, m_contentParent);
            slotView.SetData(inventorySlot);

            slotViews.Add(slotView);
        }
    }

    private void ClearLastContainerData()
    {
        if (lastOpenedContainer != null)
        {
            lastOpenedContainer.onInventorySlotsUpdated -= OnInventorySlotsUpdated;
        }
    }
}
