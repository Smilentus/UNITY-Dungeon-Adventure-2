using UnityEngine;


public class VoiceToSubtitleAdapter : MonoBehaviour
{
    [SerializeField]
    private SubtitlesController m_subtitlesController;

    [SerializeField]
    private VoiceController m_voiceController;


    private void Awake()
    {
        m_voiceController.onVoiceClipDataPlayed += OnVoiceClipPlayed;
    }

    private void OnDestroy()
    {
        m_voiceController.onVoiceClipDataPlayed -= OnVoiceClipPlayed;
    }
    

    private void OnVoiceClipPlayed(BaseVoiceClipData data, float subtitleLength)
    {
        m_subtitlesController.OnVoiceClipPlayed(data, subtitleLength);
    }
}
