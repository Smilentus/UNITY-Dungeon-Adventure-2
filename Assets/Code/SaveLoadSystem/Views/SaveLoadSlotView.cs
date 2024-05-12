using Dimasyechka.Code.SaveLoadSystem.BetweenScenes;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using System;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;

namespace Dimasyechka.Code.SaveLoadSystem.Views
{
    public class SaveLoadSlotView : MonoViewModel<RuntimeSaveLoadSlotData>
    {
        public event Action onSlotInteraction;


        [RxAdaptableProperty]
        public ReactiveProperty<string> SlotName = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> SaveDateTime = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> GameDateTime = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> GameVersion = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<bool> IsReWriteButtonEnabled = new ReactiveProperty<bool>();


        private int _slotIndex;


        private SaveLoadSystemController _saveLoadSystemController;

        [Inject]
        public void Construct(SaveLoadSystemController saveLoadSystemController)
        {
            _saveLoadSystemController = saveLoadSystemController;
        }

        public void SetData(int index)
        {
            _slotIndex = index;

            DateTime converted = new DateTime(Model.SlotData.SaveDateTimeStamp);

            SlotName.Value = $"{Model.SlotData.VisibleSaveFileName}";
            SaveDateTime.Value = $"{converted.ToString("g")}";
            GameDateTime.Value = $"{Model.SlotData.GameSaveDateTimeStamp}";
            GameVersion.Value = $"{Model.SlotData.GameVersion}";
        }

        public void SetSaveButtonVisibility(bool value)
        {
            IsReWriteButtonEnabled.Value = value;
        }

        [RxAdaptableMethod]
        public void LoadThisSave()
        {
            BetweenScenesLoaderAdapter.Instance.SetLoadablePath(Model.SaveFilePath);

            SceneManager.LoadScene("GameScene");
        }

        [RxAdaptableMethod]
        public void ReWriteThisSave()
        {
            _saveLoadSystemController.TrySaveGameState(Model.SaveFilePath);

            onSlotInteraction?.Invoke();
        }

        [RxAdaptableMethod]
        public void DeleteThisSave()
        {
            _saveLoadSystemController.TryDeleteSaveFile(Model.SaveFilePath);

            onSlotInteraction?.Invoke();
        }
    }
}
