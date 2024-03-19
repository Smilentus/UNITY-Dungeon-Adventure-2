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

        m_itemImage.sprite = itemProfile.ItemSprite;
        m_itemNameTMP.text = itemProfile.ItemName;
        m_itemDescriptionTMP.text = itemProfile.ItemDescription;
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