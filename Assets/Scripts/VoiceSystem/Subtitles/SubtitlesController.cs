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


    public void OnVoiceClipPlayed(BaseVoiceClipData voiceClipData)
    {
        SubtitleViewData subtitleData = new SubtitleViewData()
        {
            subtitleAuthor = voiceClipData.VoiceAuthor.AuthorName,
            subtitleAuthorColor = voiceClipData.VoiceAuthor.SubtitleAuthorColor,
            subtitleBody = voiceClipData.SubtitlesBody,
            subtitleLength = CalculateSubtitlesShownSeconds(voiceClipData)
        };

        onVoiceClipPlayedWithSubtitles?.Invoke(subtitleData);
    }


    private float CalculateSubtitlesShownSeconds(BaseVoiceClipData data)
    {
        if (data != null)
        {
            if (data.VoiceClip != null)
            {
                return data.VoiceClip.length + 0.25f;
            }
            else
            {
                return (data.SubtitlesBody.Trim().Replace(" ", "").Length * 0.05f) + 0.25f;
            }
        }

        return 1f;
    }
}
