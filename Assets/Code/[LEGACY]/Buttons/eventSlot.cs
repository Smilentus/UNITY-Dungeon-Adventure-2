using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code._LEGACY_.Buttons
{
    public class eventSlot : MonoBehaviour
    {
        // UI элемент
        Text text;

        [Header("Текст описания")]
        [TextArea(3,5)]
        public string aboutText;

        private void Start()
        {
            text = transform.GetChild(0).GetComponent<Text>();
            text.text = aboutText;
        }
    }
}
