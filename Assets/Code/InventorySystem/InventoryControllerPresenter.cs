using System.Collections.Generic;
using Dimasyechka.Code.InventorySystem.BaseInventoryContainer;
using Dimasyechka.Code.ZenjectFactories;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.InventorySystem
{
    public class InventoryControllerPresenter : MonoBehaviour
    {
        [SerializeField]
        private BaseInventoryContainerButtonView _buttonViewPrefab;

        [SerializeField]
        private Transform _containerButtonsParent;


        [SerializeField]
        private BaseInventoryContainerView _containerViewPrefab;

        [SerializeField]
        private Transform _containerViewsParent;


        private List<BaseInventoryContainerButtonView> _containerButtonViews = new List<BaseInventoryContainerButtonView>();
        private List<BaseInventoryContainerView> _openedContainers = new List<BaseInventoryContainerView>();


        private InventoryController _inventoryController;
        private BaseInventoryContainerButtonViewFactory _buttonFactory;
        private BaseInventoryContainerViewFactory _containerFactory;

        [Inject]
        public void Construct(
            InventoryController inventoryController, 
            BaseInventoryContainerButtonViewFactory buttonFactory,
            BaseInventoryContainerViewFactory containerFactory)
        {
            _inventoryController = inventoryController;
            _buttonFactory = buttonFactory;
            _containerFactory = containerFactory;
        }


        private void Start()
        {
            InitializeController();
        }

        private void OnDestroy()
        {
            if (_inventoryController != null)
            {
                _inventoryController.onInventoryContainersUpdated -= OnInventoryContainersUpdated;
                _inventoryController.onInventoryContainerOpened -= OnInventoryContainerOpened;
                _inventoryController.onInventoryContainerClosed -= OnInventoryContainerClosed;
            }
        }


        private void InitializeController()
        {
            _inventoryController.onInventoryContainersUpdated += OnInventoryContainersUpdated;
            _inventoryController.onInventoryContainerOpened += OnInventoryContainerOpened;
            _inventoryController.onInventoryContainerClosed += OnInventoryContainerClosed;

            _containerButtonViews = new List<BaseInventoryContainerButtonView>();

            OnInventoryContainersUpdated();
        }

        private void OnInventoryContainerClosed(BaseInventoryContainer.BaseInventoryContainer container)
        {
            HighlightSelectedContainerButton(container, false);

            DestroyContainerView(container);
        }

        private void OnInventoryContainerOpened(BaseInventoryContainer.BaseInventoryContainer container)
        {
            HighlightSelectedContainerButton(container, true);

            InstantiateContainerView(container);
        }


        private void InstantiateContainerView(BaseInventoryContainer.BaseInventoryContainer container)
        {
            BaseInventoryContainerView containerView = _containerFactory.InstantiateForComponent(_containerViewPrefab.gameObject, _containerViewsParent);
            containerView.OpenContainer(container);

            _openedContainers.Add(containerView);
        }

        private void DestroyContainerView(BaseInventoryContainer.BaseInventoryContainer container)
        {
            BaseInventoryContainerView containerView = _openedContainers.Find(x => x.InventoryContainer == container);
            if (containerView != null)
            {
                containerView.HideContainer();
                Destroy(containerView.gameObject);

                _openedContainers.Remove(containerView); 
            }
        }


        private void HighlightSelectedContainerButton(BaseInventoryContainer.BaseInventoryContainer container, bool toggle)
        {
            BaseInventoryContainerButtonView buttonView = _containerButtonViews.Find(x => x.Model == container);
            if (buttonView != null)
            {
                buttonView.SetOpenStatus(toggle);
            }
        }


        private void OnInventoryContainersUpdated()
        {
            for (int i = _containerButtonViews.Count - 1; i >= 0; i--)
            {
                Destroy(_containerButtonViews[i].gameObject);
            }
            _containerButtonViews.Clear();


            foreach (BaseInventoryContainer.BaseInventoryContainer baseInventoryContainer in _inventoryController.InventoryContainers)
            {
                BaseInventoryContainerButtonView containerButtonView = _buttonFactory.InstantiateForComponent(_buttonViewPrefab.gameObject, _containerButtonsParent);
                containerButtonView.SetupModel(baseInventoryContainer);
                containerButtonView.SetPressCallback(OnBaseInventoryContainerButtonViewPressed);
                containerButtonView.SetOpenStatus(_inventoryController.OpenedContainers.Contains(baseInventoryContainer));

                _containerButtonViews.Add(containerButtonView);
            }
        }

        private void OnBaseInventoryContainerButtonViewPressed(BaseInventoryContainer.BaseInventoryContainer container)
        {
            _inventoryController.ToggleContainer(container);
        }
    }


    public class BaseInventoryContainerButtonViewFactory : DiContainerCreationFactory<BaseInventoryContainerButtonView> { }

    public class BaseInventoryContainerViewFactory : DiContainerCreationFactory<BaseInventoryContainerView> { }
}