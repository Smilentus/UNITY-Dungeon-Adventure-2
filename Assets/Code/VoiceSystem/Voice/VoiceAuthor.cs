using UnityEngine;

namespace Dimasyechka.Code.VoiceSystem.Voice
{
    [CreateAssetMenu(menuName = "VoiceSystem/VoiceAuthor", fileName = "VoiceAuthor")]
    public class VoiceAuthor : ScriptableObject
    {
        [field: SerializeField] 
        public string AuthorName { get; protected set; }
    

        [field: SerializeField] 
        public Color SubtitleAuthorColor { get; protected set; }
    }
}