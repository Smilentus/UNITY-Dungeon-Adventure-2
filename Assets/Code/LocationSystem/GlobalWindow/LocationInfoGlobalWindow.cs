using Dimasyechka.Code.GlobalWindows;
using Dimasyechka.Code.GlobalWindows.Base;
using Dimasyechka.Code.GlobalWindows.Controllers;
using Dimasyechka.Code.LocationSystem.Controllers;
using Dimasyechka.Code.LocationSystem.Profiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Dimasyechka.Code.LocationSystem.GlobalWindow
{
    public class LocationInfoGlobalWindow : BaseGameGlobalWindow
    {
        [SerializeField]
        private Image _locationPreview;

        [SerializeField]
        private TMP_Text _locationTitle;

        [SerializeField]
        private TMP_Text _locationDescription;

        [SerializeField]
        private TMP_Text _travelButtonText;


        [SerializeField]
        private Button _travelButton;

        [SerializeField]
        private Button _cancelButton;


        private LocationsController _locationsController;

        [Inject]
        public void Construct(LocationsController locationsController)
        {
            _locationsController = locationsController;
        }


        private void Awake()
        {
            _travelButton.onClick.AddListener(OnTravelButton);
            _cancelButton.onClick.AddListener(OnCancelButton);
        }


        protected override void OnShow()
        {
            LocationInfoGlobalWindowData infoData = GetConvertedWindowData<LocationInfoGlobalWindowData>();

            if (infoData != null)
            {
                _locationPreview.sprite = infoData.LocationProfile.LocationPreviewSprite;
                _locationTitle.text = infoData.LocationProfile.LocationTitle;
                _locationDescription.text = infoData.LocationProfile.LocationDescription;
            }
        }


        private void OnTravelButton()
        {
            LocationInfoGlobalWindowData infoData = GetConvertedWindowData<LocationInfoGlobalWindowData>();

            if (infoData != null)
            {
                _locationsController.TravelToLocation(infoData.LocationProfile, infoData.TravelHours);
            }

            Hide();

            GlobalWindowsController.Instance.TryHideGlobalWindow(typeof(WorldMapGlobalWindow));
        }

        private void OnCancelButton()
        {
            Hide();
        }
    }

    [System.Serializable]
    public class LocationInfoGlobalWindowData : BaseGameGlobalWindowData
    {
        public LocationProfile LocationProfile;
        public int TravelHours;
    }
}