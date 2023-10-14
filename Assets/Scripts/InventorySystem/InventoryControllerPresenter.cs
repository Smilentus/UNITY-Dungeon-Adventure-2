using System.Collections.Generic;
using UnityEngine;

public class InventoryControllerPresenter : MonoBehaviour
{
    [Header("Inventory Container References")]
    [SerializeField]
    private BaseInventoryContainerButtonView m_buttonViewPrefab;

    [SerializeField]
    private Transform m_containerButtonsParent;


    [Header("Selected Inventory Container")]
    [SerializeField]
    private BaseInventoryContainerView m_selectedContainerView;


    private InventoryController controller;


    private List<BaseInventoryContainerButtonView> containerButtonViews = new List<BaseInventoryContainerButtonView>();


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
        }
    }


    private void InitializeController()
    {
        controller = FindObjectOfType<InventoryController>();

        if (controller == null) return;

        controller.onInventoryContainersUpdated += OnInventoryContainersUpdated;
        controller.onInventoryContainerOpened += OnInventoryContainerOpened;

        containerButtonViews = new List<BaseInventoryContainerButtonView>();
    }

    private void OnInventoryContainerOpened(BaseInventoryContainer _container)
    {
        HighlightSelectedContainerButton(_container);
        m_selectedContainerView.OpenContainer(_container);
    }

    private void HighlightSelectedContainerButton(BaseInventoryContainer _container)
    {
        foreach (BaseInventoryContainerButtonView containerButtonView in containerButtonViews)
        {
            containerButtonView.SetOpenStatus(false);
        }

        BaseInventoryContainerButtonView buttonView = containerButtonViews.Find(x => x.Container == _container);
        if (buttonView != null)
        {
            buttonView.SetOpenStatus(true);
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
            containerButtonView.SetOpenStatus(false);

            containerButtonViews.Add(containerButtonView);
        }


        // Ищем последний открытый контейнер и на всякий случай снова его открываем
        BaseInventoryContainerButtonView foundedView = containerButtonViews.Find(x => x.Container == controller.SelectedContainer);
        if (foundedView != null)
        {
            OnInventoryContainerOpened(foundedView.Container);
        }
        else
        {
            if (containerButtonViews.Count > 0)
            {
                OnInventoryContainerOpened(containerButtonViews[0].Container);
            }
            else
            {
                // Нет сумок, какая-то заглушка наверно открывается, да?
            }
        }
    }

    private void OnBaseInventoryContainerButtonViewPressed(BaseInventoryContainer _container)
    {
        controller.OpenContainer(_container);
    }
}
