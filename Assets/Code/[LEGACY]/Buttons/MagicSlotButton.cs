using Dimasyechka.Code._LEGACY_.Managers;
using UnityEngine;

namespace Dimasyechka.Code._LEGACY_.Buttons
{
    public class MagicSlotButton : MonoBehaviour
    {
        public MagicManager.MagicType currentType;

        public void Press()
        {
            FindObjectOfType<MagicManager>().UseMagic(currentType);
        }
    }
}
