using UnityEngine;
using UnityEngine.UI;


public class ArtCinematicsView : MonoBehaviour
{
    [SerializeField]
    private Image m_artImage;

    [SerializeField]
    private FadeEffectController m_fadeEffectController;


    private ArtCinematicsObserver _observer;


    private void Start()
    {
        _observer = Cinematics.Instance.GetObserver<ArtCinematicsObserver>();

        if (_observer != null)
        {
            _observer.onProfileReceived += SetData;
            _observer.onCompleted += OnCompleted;
        }

        m_fadeEffectController.ForceFadeOut();
    }

    private void OnCompleted()
    {
        m_fadeEffectController.ForceFadeOut();
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
        m_fadeEffectController.ForceFadeIn();
        m_artImage.sprite = profile.ArtBody;
    }
}