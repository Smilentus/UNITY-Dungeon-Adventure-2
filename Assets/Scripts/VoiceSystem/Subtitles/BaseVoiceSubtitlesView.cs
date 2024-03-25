using System.Collections;
using TMPro;
using UnityEngine;


public class BaseVoiceSubtitlesView : MonoBehaviour
{
    public event System.Action<BaseVoiceSubtitlesView> onDestroyed;


    [SerializeField]
    protected TMP_Text m_subtitlesBodyTMP;

    [SerializeField]
    protected FadeEffectController m_fadeEffectController;


    private Coroutine delayedFadingCoroutine;
    private SubtitleViewData attachedViewData;


    private void OnDestroy()
    {
        m_fadeEffectController.onFadeIn -= OnFadedIn;
        m_fadeEffectController.onFadeOut -= OnFadedOut;

        if (delayedFadingCoroutine != null)
        {
            StopCoroutine(delayedFadingCoroutine);
        }

        onDestroyed?.Invoke(this);
    }


    public virtual void SetSubtitleData(SubtitleViewData viewData)
    {
        attachedViewData = viewData;

        m_subtitlesBodyTMP.text = $"<color=#{ColorUtility.ToHtmlStringRGB(viewData.subtitleAuthorColor)}>{viewData.subtitleAuthor}</color>   {viewData.subtitleBody}";

        m_fadeEffectController.onFadeIn += OnFadedIn;
        m_fadeEffectController.FadeIn();
    }


    private IEnumerator DelayedFadingProcessing(float _delaySeconds)
    {
        yield return new WaitForSeconds(_delaySeconds);

        m_fadeEffectController.onFadeOut += OnFadedOut;
        m_fadeEffectController.FadeOut();
    }

    private void OnFadedIn()
    {
        m_fadeEffectController.onFadeIn -= OnFadedIn;

        if (delayedFadingCoroutine != null)
        {
            StopCoroutine(delayedFadingCoroutine);
        }

        delayedFadingCoroutine = StartCoroutine(DelayedFadingProcessing(attachedViewData.subtitleLength));
    }

    private void OnFadedOut()
    {
        m_fadeEffectController.onFadeOut -= OnFadedOut;

        Destroy(this.gameObject);
    }
}

[System.Serializable]
public class SubtitleViewData
{
    public string subtitleAuthor { get; set; }
    public Color subtitleAuthorColor { get; set; }
    public string subtitleBody { get; set; }
    public float subtitleLength { get; set; }
}