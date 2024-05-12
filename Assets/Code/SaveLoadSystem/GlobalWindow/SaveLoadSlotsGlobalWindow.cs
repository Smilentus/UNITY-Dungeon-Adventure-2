using Dimasyechka.Code.GlobalWindows.Base;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.Views;
using Dimasyechka.Code.ZenjectFactories;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using Dimasyechka.Lubribrary.RxMV.UniRx.RxLink;
using System.IO;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Dimasyechka.Code.SaveLoadSystem.GlobalWindow
{
    public class SaveLoadSlotsGlobalWindow : BaseGameGlobalWindow, IRxLinkable
    {
        [RxAdaptableProperty]
        public ReactiveProperty<bool> IsReWriteButtonEnabled = new ReactiveProperty<bool>();


        [SerializeField]
        private LayoutGroup _contentParent;

        [SerializeField]
        private SaveLoadSlotView _saveLoadSlotViewPrefab;


        private SaveLoadSlotsController _saveLoadSlotsController;
        private SaveLoadSystemController _saveLoadSystemController;
        private SaveLoadSlotViewFactory _factory;

        [Inject]
        public void Construct(
            SaveLoadSlotsController saveLoadSlotsController,
            SaveLoadSystemController saveLoadSystemController,
            SaveLoadSlotViewFactory factory)
        {
            _saveLoadSystemController = saveLoadSystemController;
            _saveLoadSlotsController = saveLoadSlotsController;
            _factory = factory;
        }


        protected override void OnShow()
        {
            UpdateSaveSlotsData();
        }


        [RxAdaptableMethod]
        public void OnHideClicked()
        {
            this.Hide();
        }

        [RxAdaptableMethod]
        public void OnCreateSaveClicked()
        {
            _saveLoadSystemController.TrySaveGameState();
            UpdateSaveSlotsData();
        }


        private void UpdateSaveSlotsData()
        {
            if (_saveLoadSlotsController == null) return;

            _saveLoadSlotsController.LoadSaveSlots();

            ClearPanelChildren();
            DrawSaveSlots();
        }

        private void ClearPanelChildren()
        {
            for (int i = _contentParent.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_contentParent.transform.GetChild(i).gameObject);
            }
        }

        private void DrawSaveSlots()
        {
            for (int i = 0; i < _saveLoadSlotsController.SavedSlots.Count; i++)
            {
                SaveLoadSlotView saveLoadSlotView = null;

                saveLoadSlotView = _factory.InstantiateForComponent(_saveLoadSlotViewPrefab.gameObject, _contentParent.transform);

                if (Path.GetFileNameWithoutExtension(_saveLoadSlotsController.SavedSlots[i].SaveFilePath).Equals("AutoSave"))
                {
                    saveLoadSlotView.SetSaveButtonVisibility(false);
                }
                else
                {
                    saveLoadSlotView.SetSaveButtonVisibility(IsReWriteButtonEnabled.Value);
                }

                saveLoadSlotView.SetupModel(_saveLoadSlotsController.SavedSlots[i]);
                saveLoadSlotView.SetData(i);
                saveLoadSlotView.onSlotInteraction += OnSlotInteraction;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentParent.GetComponent<RectTransform>());
        }

        private void OnSlotInteraction()
        {
            UpdateSaveSlotsData();
        }
    }


    public class SaveLoadSlotViewFactory : DiContainerCreationFactory<SaveLoadSlotView> { }
}