using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemDescriptionGlobalWindow : BaseGameGlobalWindow
{
    [SerializeField]
    private Button m_closeButton;


    [SerializeField]
    private TMP_Text m_itemNameTMP;

    [SerializeField]
    private TMP_Text m_itemDescriptionTMP;

    [SerializeField]
    private Image m_itemImage;


    [SerializeField]
    private ItemFindHelperView m_itemHelperPrefab;

    [SerializeField]
    private Transform m_locationsContentParent;


    private BaseItemProfile _itemProfile;


    private void OnEnable()
    {
        m_closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    private void OnDisable()
    {
        m_closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }


    protected override void OnShow()
    {
        BaseItemProfile itemProfile = GetConvertedWindowData<ItemDescriptionGlobalWindowData>().ItemProfile;

        _itemProfile = itemProfile;

        m_itemImage.sprite = itemProfile.ItemSprite;
        m_itemNameTMP.text = itemProfile.ItemName;
        m_itemDescriptionTMP.text = itemProfile.ItemDescription;

        DrawItemHelpers();
    }

    
    private void DrawItemHelpers()
    {
        for (int i = m_locationsContentParent.childCount - 1; i >= 0; i--) 
        {
            Destroy(m_locationsContentParent.GetChild(i).gameObject);
        }

        IEnumerable<ItemHelpData> helpData = ItemHelpSource.Instance.GetItemHelpData(_itemProfile);

        foreach(ItemHelpData itemHelpData in helpData)
        { 
            ItemFindHelperView helperView = Instantiate(m_itemHelperPrefab, m_locationsContentParent);

            helperView.SetData(itemHelpData);
        }
    }


    private void OnCloseButtonClicked()
    {
        GlobalWindowsController.Instance.TryHideGlobalWindow(typeof(ItemDescriptionGlobalWindow));
    }
}

[System.Serializable]
public class ItemDescriptionGlobalWindowData : BaseGameGlobalWindowData
{
    public BaseItemProfile ItemProfile;
}