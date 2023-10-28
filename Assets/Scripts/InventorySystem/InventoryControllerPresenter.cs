using System.Collections.Generic;
using UnityEngine;

public class InventoryControllerPresenter : MonoBehaviour
{
    [Header("Inventory Container References")]
    [SerializeField]
    private BaseInventoryContainerButtonView m_buttonViewPrefab;

    [SerializeField]
    private Transform m_containerButtonsParent;


    [SerializeField]
    private BaseInventoryContainerView m_containerViewPrefab;

    [SerializeField]
    private Transform m_containerViewsParent;


    [SerializeField]
    private BaseInventoryContainerView m_quickSlotsContainer;


    private InventoryController controller;


    private List<BaseInventoryContainerButtonView> containerButtonViews = new List<BaseInventoryContainerButtonView>();
    private List<BaseInventoryContainerView> openedContainers = new List<BaseInventoryContainerView>();


    private void Start()
    {
        InitializeController();
    }

    private void OnDestroy()
    {
        if (controller != null)
        {
            controller.onInventoryContainersUpdated -= OnInventoryContainersUpdated;
            controller.onInventoryContainerOpened -= OnInventoryContainerOpened;
            controller.onInventoryContainerClosed -= OnInventoryContainerClosed;
        }
    }


    private void InitializeController()
    {
        controller = FindObjectOfType<InventoryController>();

        if (controller == null) return;

        controller.onInventoryContainersUpdated += OnInventoryContainersUpdated;
        controller.onInventoryContainerOpened += OnInventoryContainerOpened;
        controller.onInventoryContainerClosed += OnInventoryContainerClosed;

        
        m_quickSlotsContainer.OpenContainer(controller.QuickSlotsContainer);


        containerButtonViews = new List<BaseInventoryContainerButtonView>();
    }

    private void OnInventoryContainerClosed(BaseInventoryContainer _container)
    {
        HighlightSelectedContainerButton(_container, false);

        DestroyContainerView(_container);
    }

    private void OnInventoryContainerOpened(BaseInventoryContainer _container)
    {
        HighlightSelectedContainerButton(_container, true);

        InstantiateContainerView(_container);
    }


    private void InstantiateContainerView(BaseInventoryContainer _container)
    {
        BaseInventoryContainerView containerView = Instantiate(m_containerViewPrefab, m_containerViewsParent);
        containerView.OpenContainer(_container);

        openedContainers.Add(containerView);
    }

    private void DestroyContainerView(BaseInventoryContainer _container)
    {
        BaseInventoryContainerView containerView = openedContainers.Find(x => x.InventoryContainer == _container);
        if (containerView != null)
        {
            containerView.HideContainer();
            Destroy(containerView.gameObject);

            openedContainers.Remove(containerView); 
        }
    }


    private void HighlightSelectedContainerButton(BaseInventoryContainer _container, bool _toggle)
    {
        BaseInventoryContainerButtonView buttonView = containerButtonViews.Find(x => x.Container == _container);
        if (buttonView != null)
        {
            buttonView.SetOpenStatus(_toggle);
        }
    }


    private void OnInventoryContainersUpdated()
    {
        // Удаляем старые кнопки с контейнерами
        for (int i = containerButtonViews.Count - 1; i >= 0; i--)
        {
            Destroy(containerButtonViews[i].gameObject);
        }
        containerButtonViews.Clear();


        // Заполняем новые кнопки с контейнерами
        foreach (BaseInventoryContainer baseInventoryContainer in controller.InventoryContainers)
        {
            BaseInventoryContainerButtonView containerButtonView = Instantiate(m_buttonViewPrefab, m_containerButtonsParent);
            containerButtonView.SetData(baseInventoryContainer, OnBaseInventoryContainerButtonViewPressed);
            containerButtonView.SetOpenStatus(controller.OpenedContainers.Contains(baseInventoryContainer));

            containerButtonViews.Add(containerButtonView);
        }
    }

    private void OnBaseInventoryContainerButtonViewPressed(BaseInventoryContainer _container)
    {
        controller.ToggleContainer(_container);
    }
}
