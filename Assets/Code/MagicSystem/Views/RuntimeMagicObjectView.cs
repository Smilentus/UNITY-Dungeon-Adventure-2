using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code.MagicSystem.Views
{
    public class RuntimeMagicObjectView : MonoBehaviour
    {
        [SerializeField]
        private Image m_FillableImage;

        [SerializeField]
        private TMP_Text m_CooldownTMP;


        public void SetFillAmountRatio(int _cooldown, float _ratio)
        {
            m_CooldownTMP.text = _cooldown.ToString("f0");
            m_FillableImage.fillAmount = Mathf.Clamp01(_ratio);
        }
    }
}
