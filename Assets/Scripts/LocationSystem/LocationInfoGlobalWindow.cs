using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LocationInfoGlobalWindow : BaseGameGlobalWindow
{
    [SerializeField]
    private Image m_locationPreview;


    [SerializeField]
    private Button m_travelButton;

    [SerializeField]
    private Button m_cancelButton;


    private void Awake()
    {
        m_travelButton?.onClick.AddListener(OnTravelButton);
        m_cancelButton?.onClick.AddListener(OnCancelButton);
    }


    protected override void OnShow()
    {
        LocationInfoGlobalWindowData infoData = GetConvertedWindowData<LocationInfoGlobalWindowData>();

        if (infoData != null)
        {
            m_locationPreview.sprite = infoData.LocationProfile.LocationPreviewSprite;
        }
    }


    private void OnTravelButton()
    {
        LocationInfoGlobalWindowData infoData = GetConvertedWindowData<LocationInfoGlobalWindowData>();

        if (infoData != null)
        {
            LocationsController.Instance.TravelToLocation(infoData.LocationProfile, infoData.TravelHours);
        }

        Hide();

        GlobalWindowsController.Instance.TryHideGlobalWindow(typeof(WorldMapGlobalWindow));
    }

    private void OnCancelButton()
    {
        Hide();

        GlobalWindowsController.Instance.TryHideGlobalWindow(typeof(WorldMapGlobalWindow));
    }
}

[System.Serializable]
public class LocationInfoGlobalWindowData : BaseGameGlobalWindowData
{
    public LocationProfile LocationProfile;
    public int TravelHours;
}