using Dimasyechka.Code.VoiceSystem.Subtitles;
using Dimasyechka.Code.VoiceSystem.Voice;
using UnityEngine;

namespace Dimasyechka.Code.VoiceSystem.Adapters
{
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
}
