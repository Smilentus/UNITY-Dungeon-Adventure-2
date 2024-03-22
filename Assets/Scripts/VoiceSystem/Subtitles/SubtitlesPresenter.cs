using UnityEngine;


public class SubtitlesPresenter : MonoBehaviour
{
    [SerializeField]
    protected BaseVoiceSubtitlesView m_subtitlesViewPrefab;

    [SerializeField]
    protected Transform m_subtitlesContentParent;


    private void Awake()
    {
        SubtitlesController.Instance.onVoiceClipPlayedWithSubtitles += OnGlobalVoiceClipPlayed;
    }

    private void OnDestroy()
    {
        if (SubtitlesController.Instance != null)
        {
            SubtitlesController.Instance.onVoiceClipPlayedWithSubtitles -= OnGlobalVoiceClipPlayed;
        }
    }


    private void OnGlobalVoiceClipPlayed(SubtitleViewData data)
    {
        BaseVoiceSubtitlesView subtitlesView = Instantiate(m_subtitlesViewPrefab, m_subtitlesContentParent);
        subtitlesView.SetSubtitleData(data);
    }
}
