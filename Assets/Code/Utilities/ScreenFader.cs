using UnityEngine;

namespace Dimasyechka.Code.Utilities
{
    public class ScreenFader : MonoBehaviour
    {
        private static ScreenFader _instance;
        public static ScreenFader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ScreenFader>();    
                }

                return _instance;
            }
        }


        [SerializeField]
        private FadeEffectController _fadeEffectController;
        public FadeEffectController FadeEffectController => _fadeEffectController;


        public void ForceFadeIn()
        {
            _fadeEffectController.ForceFadeIn();
        }

        public void ForceFadeOut()
        {
            _fadeEffectController.ForceFadeOut();
        }


        public void FadeInScreen()
        {
            _fadeEffectController.FadeIn();
        }

        public void FadeOutScreen()
        {
            _fadeEffectController.FadeOut();
        }
    }
}