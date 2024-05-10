using Dimasyechka.Code.ZenjectFactories;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using System.Collections.Generic;
using Dimasyechka.Lubribrary.RxMV.UniRx.RxLink;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.InventorySystem.BaseInventoryContainer
{
    /// <summary>
    ///     ќтрисовывает основной интерфейс контейнера (слоты и т.п. в конкретном контейнере)
    /// </summary>
    public class BaseInventoryContainerView : MonoBehaviour, IRxLinkable
    {
        [RxAdaptableProperty]
        public ReactiveProperty<bool> IsContainerActive = new ReactiveProperty<bool>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> ContainerTitle = new ReactiveProperty<string>();



        [SerializeField]
        private BaseInventoryContainerSlotView _baseInventoryContainerSlotViewPrefab;

        [SerializeField]
        private Transform _contentParent;



        private BaseInventoryContainer _inventoryContainer;
        public BaseInventoryContainer InventoryContainer => _inventoryContainer;


        private List<BaseInventoryContainerSlotView> _slotViews = new List<BaseInventoryContainerSlotView>();


        private bool _isSlotsInstantiated;


        private InventoryController _inventoryController;
        private BaseInventoryContainerSlotViewFactory _factory;

        [Inject]
        public void Construct(InventoryController inventoryController, BaseInventoryContainerSlotViewFactory factory)
        {
            _inventoryController = inventoryController;
            _factory = factory;
        }

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

            ContainerTitle.Value =
                $"'{inventoryContainer.InventoryContainerProfile.ContainerName}' ({inventoryContainer.InventoryContainerProfile.ContainerCapacity} €ч.)"; // (¬местительность {inventoryContainer.InventoryContainerProfile.ContainerCapacity} €чеек

            this._inventoryContainer = inventoryContainer;
            CreateNewSlots();

            this._inventoryContainer.onInventorySlotsUpdated += OnInventorySlotsUpdated;
            OnInventorySlotsUpdated();

            IsContainerActive.Value = true;
        }

        public void HideContainer()
        {
            IsContainerActive.Value = false;
        }

        private void OnInventorySlotsUpdated()
        {
            // ѕока что так, подумать про кэширование и прочие штуки
            _isSlotsInstantiated = false;
            CreateNewSlots();
        }

        private void CreateNewSlots()
        {
            if (_isSlotsInstantiated) return;

            _isSlotsInstantiated = true;

            for (int i = _slotViews.Count - 1; i >= 0; i--)
            {
                Destroy(_slotViews[i].gameObject);
            }
            _slotViews.Clear();


            int slotIndex = 0;
            foreach (BaseInventoryContainerSlot inventorySlot in _inventoryContainer.InventorySlots)
            {
                BaseInventoryContainerSlotView slotView = _factory.InstantiateForComponent(_baseInventoryContainerSlotViewPrefab.gameObject, _contentParent);
                slotView.SetupModel(inventorySlot);
                slotView.SetPressCallback(OnSlotPressedCallback);

                _slotViews.Add(slotView);

                slotIndex++;
            }
        }

        private void OnSlotPressedCallback(BaseInventoryContainerSlot containerSlot)
        {
            _inventoryController.OnAnyContainerSlotPressed(_inventoryContainer, containerSlot);
        }

        private void ClearLastContainerData()
        {
            if (_inventoryContainer != null)
            {
                _inventoryContainer.onInventorySlotsUpdated -= OnInventorySlotsUpdated;
            }
        }
    }


    public class BaseInventoryContainerSlotViewFactory : DiContainerCreationFactory<BaseInventoryContainerSlotView> { }
}
