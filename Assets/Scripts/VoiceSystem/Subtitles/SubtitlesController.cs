using System;
using UnityEngine;


public class SubtitlesController : MonoBehaviour
{
    private static SubtitlesController instance;
    public static SubtitlesController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SubtitlesController>();
            }

            return instance;
        }
    }


    public event Action<SubtitleViewData> onVoiceClipPlayedWithSubtitles;
    public event Action onForceHide;


    public void ForceHide()
    {
        onForceHide?.Invoke();
    }

    public void OnVoiceClipPlayed(BaseVoiceClipData voiceClipData, float subtitleLength)
    {
        SubtitleViewData subtitleData = new SubtitleViewData()
        {
            subtitleAuthor = voiceClipData.VoiceAuthor.AuthorName,
            subtitleAuthorColor = voiceClipData.VoiceAuthor.SubtitleAuthorColor,
            subtitleBody = voiceClipData.SubtitlesBody,
            subtitleLength = subtitleLength
        };

        onVoiceClipPlayedWithSubtitles?.Invoke(subtitleData);
    }
}
