using Dimasyechka.Code.CinematicSystem.Observers;
using Dimasyechka.Code.CinematicSystem.Profiles;
using Dimasyechka.Code.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Dimasyechka.Code.CinematicSystem.Views
{
    public class ArtCinematicsView : MonoBehaviour
    {
        [SerializeField]
        private Image _artImage;

        [SerializeField]
        private FadeEffectController _fadeEffectController;

        private ArtCinematicsObserver _observer;


        private Cinematics _cinematics;

        [Inject]
        public void Construct(Cinematics cinematics)
        {
            _cinematics = cinematics;
        }


        private void Start()
        {
            _observer = _cinematics.GetObserver<ArtCinematicsObserver>();

            if (_observer != null)
            {
                _observer.onProfileReceived += SetData;
                _observer.onCompleted += OnCompleted;
            }

            _fadeEffectController.ForceFadeOut();
        }

        private void OnCompleted()
        {
            _fadeEffectController.ForceFadeOut();
        }

        private void OnDestroy()
        {
            if (_observer != null)
            {
                _observer.onProfileReceived -= SetData;
            }
        }


        public void SetData(ArtCinematicProfile profile)
        {
            _fadeEffectController.ForceFadeIn();
            _artImage.sprite = profile.ArtBody;
        }
    }
}