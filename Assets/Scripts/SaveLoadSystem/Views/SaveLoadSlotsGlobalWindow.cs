using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class SaveLoadSlotsGlobalWindow : BaseGameGlobalWindow
{
    private const string AutoSaveName = "AutoSave"; // Не нравится по названию определять, надо булку добавить наверно внутрь файла :c


    [SerializeField]
    private bool m_isReWriteButtonEnabled = true;


    [SerializeField]
    private Button m_createNewSaveButton;

    [SerializeField]
    private Button m_closeButton;


    [SerializeField]
    private LayoutGroup m_contentParent;

    [SerializeField]
    private SaveLoadSlotView m_saveLoadSlotViewPrefab;

    [SerializeField]
    private SaveLoadSlotView m_autoSaveSlotViewPrefab;


    private SaveLoadSlotsController _saveLoadSlotsController;


    private void Awake()
    {
        _saveLoadSlotsController = SaveLoadSlotsController.Instance;

        m_closeButton?.onClick.AddListener(Hide);
        m_createNewSaveButton?.onClick.AddListener(() => 
        {
            _saveLoadSlotsController.CreateNewSaveSlot();
            UpdateSaveSlotsData();
        });
    }

    private void OnDestroy()
    {
        m_closeButton?.onClick.RemoveAllListeners();
        m_createNewSaveButton?.onClick.RemoveAllListeners();
    }


    protected override void OnShow()
    {
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
        for (int i = m_contentParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(m_contentParent.transform.GetChild(i).gameObject);
        }
    }

    private void DrawSaveSlots()
    {
        for (int i = 0; i < _saveLoadSlotsController.SavedSlots.Count; i++)
        {
            SaveLoadSlotView saveLoadSlotView = null;

            if (Path.GetFileNameWithoutExtension(_saveLoadSlotsController.SavedSlots[i].SaveFilePath).Equals("AutoSave"))
            {
                saveLoadSlotView = Instantiate(m_autoSaveSlotViewPrefab, m_contentParent.transform);
            }
            else
            {
                saveLoadSlotView = Instantiate(m_saveLoadSlotViewPrefab, m_contentParent.transform);
            }

            saveLoadSlotView.SetData(i, _saveLoadSlotsController.SavedSlots[i], m_isReWriteButtonEnabled);
            saveLoadSlotView.onSlotInteraction += OnSlotInteraction;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(m_contentParent.GetComponent<RectTransform>());
    }

    private void OnSlotInteraction()
    {
        UpdateSaveSlotsData();
    }
}