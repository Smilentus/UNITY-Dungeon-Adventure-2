using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
///     ������������ �������� ��������� ���������� (����� � �.�. � ���������� ����������)
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
            m_containerNameTMP.text = $"'{inventoryContainer.InventoryContainerProfile.ContainerName}' (��������������� {inventoryContainer.InventoryContainerProfile.ContainerCapacity} �����)";
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
        // ������� ������ �����
        for (int i = slotViews.Count - 1; i >= 0; i--)
        {
            Destroy(slotViews[i].gameObject);
        }
        slotViews.Clear();


        // ��������� ����� ������
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
