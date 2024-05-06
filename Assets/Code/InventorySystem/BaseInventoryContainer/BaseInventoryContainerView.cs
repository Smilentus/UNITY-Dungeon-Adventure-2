using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.InventorySystem.BaseInventoryContainer
{
    /// <summary>
    ///     Отрисовывает основной интерфейс контейнера (слоты и т.п. в конкретном контейнере)
    /// </summary>
    public class BaseInventoryContainerView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _viewGameObject;

        [SerializeField]
        private TMP_Text _containerNameTMP;


        [SerializeField]
        private BaseInventoryContainerSlotView _baseInventoryContainerSlotViewPrefab;

        [SerializeField]
        private Transform _contentParent;



        private BaseInventoryContainer _inventoryContainer;
        public BaseInventoryContainer InventoryContainer => _inventoryContainer;


        private List<BaseInventoryContainerSlotView> _slotViews = new List<BaseInventoryContainerSlotView>();


        private bool _isSlotsInstantiated;


        private InventoryController _inventoryController;

        [Inject]
        public void Construct(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
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

            if (_containerNameTMP != null)
            {
                _containerNameTMP.text = $"'{inventoryContainer.InventoryContainerProfile.ContainerName}' ({inventoryContainer.InventoryContainerProfile.ContainerCapacity} яч.)"; // (Вместительность {inventoryContainer.InventoryContainerProfile.ContainerCapacity} ячеек)
            }

            this._inventoryContainer = inventoryContainer;
            CreateNewSlots();

            this._inventoryContainer.onInventorySlotsUpdated += OnInventorySlotsUpdated;
            OnInventorySlotsUpdated();

            _viewGameObject.SetActive(true);
        }

        public void HideContainer()
        {
            _viewGameObject.SetActive(false);
        }

        private void OnInventorySlotsUpdated()
        {
            // Пока что так, подумать про кэширование и прочие штуки
            _isSlotsInstantiated = false;
            CreateNewSlots();
        }

        private void CreateNewSlots()
        {
            if (_isSlotsInstantiated) return;

            _isSlotsInstantiated = true;

            // Очищаем старые слоты
            for (int i = _slotViews.Count - 1; i >= 0; i--)
            {
                Destroy(_slotViews[i].gameObject);
            }
            _slotViews.Clear();


            // Добавляем слоты заново
            int slotIndex = 0;
            foreach (BaseInventoryContainerSlot inventorySlot in _inventoryContainer.InventorySlots)
            {
                BaseInventoryContainerSlotView slotView = Instantiate(_baseInventoryContainerSlotViewPrefab, _contentParent);
                slotView.SetData(inventorySlot, OnSlotPressedCallback);
            
                _slotViews.Add(slotView);

                slotIndex++;
            }
        }

        private void OnSlotPressedCallback(BaseInventoryContainerSlot _containerSlot)
        {
            _inventoryController.OnAnyContainerSlotPressed(_inventoryContainer, _containerSlot);
        }

        private void ClearLastContainerData()
        {
            if (_inventoryContainer != null)
            {
                _inventoryContainer.onInventorySlotsUpdated -= OnInventorySlotsUpdated;
            }
        }
    }
}
