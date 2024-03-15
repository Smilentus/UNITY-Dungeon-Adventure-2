using UnityEngine;
using UnityEngine.UI;


public class SaveLoadSlotsGlobalWindow : BaseGameGlobalWindow
{
    [SerializeField]
    private Button m_createNewSaveButton;

    [SerializeField]
    private Button m_closeButton;


    [SerializeField]
    private LayoutGroup m_contentParent;

    [SerializeField]
    private SaveLoadSlotView m_saveLoadSlotViewPrefab;


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
            SaveLoadSlotView saveLoadSlotView = Instantiate(m_saveLoadSlotViewPrefab, m_contentParent.transform);

            saveLoadSlotView.SetData(i, _saveLoadSlotsController.SavedSlots[i]);
            saveLoadSlotView.onSlotInteraction += OnSlotInteraction;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(m_contentParent.GetComponent<RectTransform>());
    }

    private void OnSlotInteraction()
    {
        UpdateSaveSlotsData();
    }
}