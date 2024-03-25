using UnityEngine;


[System.Serializable]
public class BaseVoiceClipData
{
    [SerializeField]
    private VoiceAuthor m_voiceAuthor;
    public VoiceAuthor VoiceAuthor => m_voiceAuthor;

    [SerializeField]
    private AudioClip m_voiceClip;
    public AudioClip VoiceClip => m_voiceClip;

    [TextArea(5, 15)]
    [SerializeField]
    private string m_subtitlesBody;
    public string SubtitlesBody => m_subtitlesBody;
}