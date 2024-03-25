using System.Collections.Generic;
using UnityEngine;


public class SubtitlesPresenter : MonoBehaviour
{
    [SerializeField]
    protected BaseVoiceSubtitlesView m_subtitlesViewPrefab;

    [SerializeField]
    protected Transform m_subtitlesContentParent;


    private List<BaseVoiceSubtitlesView> _subtitleViews = new List<BaseVoiceSubtitlesView>();


    private void Awake()
    {
        SubtitlesController.Instance.onVoiceClipPlayedWithSubtitles += OnGlobalVoiceClipPlayed;
        SubtitlesController.Instance.onForceHide += OnForceHide;
    }

    private void OnDestroy()
    {
        if (SubtitlesController.Instance != null)
        {
            SubtitlesController.Instance.onVoiceClipPlayedWithSubtitles -= OnGlobalVoiceClipPlayed;
            SubtitlesController.Instance.onForceHide -= OnForceHide;
        }
    }


    private void OnGlobalVoiceClipPlayed(SubtitleViewData data)
    {
        Debug.Log($"Subtitle => {data.subtitleBody}");

        BaseVoiceSubtitlesView subtitlesView = Instantiate(m_subtitlesViewPrefab, m_subtitlesContentParent);
        subtitlesView.SetSubtitleData(data);

        subtitlesView.onDestroyed += OnViewDestroyed;

        _subtitleViews.Add(subtitlesView);
    }

    private void OnViewDestroyed(BaseVoiceSubtitlesView view)
    {
        _subtitleViews.Remove(view);

        // Проверяем на нулы на всякий случай
        for (int i = _subtitleViews.Count - 1; i >= 0; i--)
        {
            if (_subtitleViews[i] == null)
            {
                _subtitleViews.RemoveAt(i);
            }
        }
    }

    private void OnForceHide()
    {
        foreach (BaseVoiceSubtitlesView view in _subtitleViews)
        {
            Destroy(view.gameObject);
        }
    }
}
