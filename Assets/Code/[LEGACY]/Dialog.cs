using UnityEngine;

namespace Dimasyechka.Code._LEGACY_
{
    [System.Serializable]
    public class Dialog
    {
        [Header("Идентификатор диалога")]
        public string dialogID;
        [Header("Текст повествования")]
        [TextArea(3, 5)]
        public string[] dialogText;
    }
}
