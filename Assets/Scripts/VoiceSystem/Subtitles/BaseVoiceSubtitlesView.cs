using System.Collections;
using TMPro;
using UnityEngine;


public class BaseVoiceSubtitlesView : MonoBehaviour
{
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
    }


    public virtual void SetSubtitleData(SubtitleViewData viewData)
    {
        attachedViewData = viewData;

        m_subtitlesBodyTMP.text = $"<color=#{ColorUtility.ToHtmlStringRGB(viewData.subtitleAuthorColor)}>{viewData.subtitleAuthor}</color>   {viewData.subtitleBody}";

        m_fadeEffectController.onFadeOut += OnFadedOut;
        m_fadeEffectController.FadeOut();
    }


    private IEnumerator DelayedFadingProcessing(float _delaySeconds)
    {
        yield return new WaitForSeconds(_delaySeconds);

        m_fadeEffectController.onFadeIn += OnFadedIn;
        m_fadeEffectController.FadeIn();
    }

    private void OnFadedOut()
    {
        m_fadeEffectController.onFadeOut -= OnFadedOut;

        if (delayedFadingCoroutine != null)
        {
            StopCoroutine(delayedFadingCoroutine);
        }

        delayedFadingCoroutine = StartCoroutine(DelayedFadingProcessing(attachedViewData.subtitleLength));
    }

    private void OnFadedIn()
    {
        m_fadeEffectController.onFadeIn -= OnFadedIn;

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