using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SaveLoadSlotView : MonoBehaviour
{
    public event Action onSlotInteraction;


    [SerializeField]
    private TMP_Text m_slotName;

    [SerializeField]
    private TMP_Text m_saveDateTime;

    [SerializeField]
    private TMP_Text m_gameSaveDateTime;

    [SerializeField]
    private TMP_Text m_gameVersion;


    [SerializeField]
    private Button m_loadButton;

    [SerializeField]
    private Button m_reWriteButton;

    [SerializeField]
    private Button m_deleteButton;


    private RuntimeSaveLoadSlotData _slotData;
    private int _slotIndex;


    private void OnEnable()
    {
        m_loadButton?.onClick.AddListener(LoadThisSave);
        m_reWriteButton?.onClick.AddListener(ReWriteThisSave);
        m_deleteButton?.onClick.AddListener(DeleteThisSave);
    }

    private void OnDisable()
    {
        m_loadButton?.onClick.RemoveListener(LoadThisSave);
        m_reWriteButton?.onClick.RemoveListener(ReWriteThisSave);
        m_deleteButton?.onClick.RemoveListener(DeleteThisSave);
    }


    public void SetData(int index, RuntimeSaveLoadSlotData runtimeSaveLoadSlotData, bool isReWriteButtonEnabled = true)
    {
        _slotIndex = index;
        _slotData = runtimeSaveLoadSlotData;

        DateTime converted = new DateTime(_slotData.SlotData.SaveDateTimeStamp);

        m_slotName.text = $"{_slotData.SlotData.VisibleSaveFileName}";
        m_saveDateTime.text = $"{converted.ToString("g")}";
        m_gameSaveDateTime.text = $"{_slotData.SlotData.GameSaveDateTimeStamp}";
        m_gameVersion.text = $"{_slotData.SlotData.GameVersion}";

        SetSaveButtonVisibility(isReWriteButtonEnabled);
    }

    public void SetSaveButtonVisibility(bool value)
    {
        m_reWriteButton?.gameObject.SetActive(value);
    }

    private void LoadThisSave()
    {
        SaveLoadSlotsController.Instance.LoadSaveSlotData(_slotIndex);

        onSlotInteraction?.Invoke();
    }

    private void ReWriteThisSave()
    {
        SaveLoadSystemController.Instance.TrySaveGameState(_slotData.SaveFilePath);

        onSlotInteraction?.Invoke();
    }

    private void DeleteThisSave()
    {
        SaveLoadSystemController.Instance.TryDeleteSaveFile(_slotData.SaveFilePath);

        onSlotInteraction?.Invoke();
    }
}
