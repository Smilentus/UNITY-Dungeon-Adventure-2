using UnityEngine;


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
    private FadeEffectController m_fadeEffectController;
    public FadeEffectController FadeEffectController => m_fadeEffectController;


    public void ForceFadeIn()
    {
        m_fadeEffectController.ForceFadeIn();
    }

    public void ForceFadeOut()
    {
        m_fadeEffectController.ForceFadeOut();
    }


    public void FadeInScreen()
    {
        m_fadeEffectController.FadeIn();
    }

    public void FadeOutScreen()
    {
        m_fadeEffectController.FadeOut();
    }
}