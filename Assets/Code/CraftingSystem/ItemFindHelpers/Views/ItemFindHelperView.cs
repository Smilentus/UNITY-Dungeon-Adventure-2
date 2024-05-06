using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code.CraftingSystem.ItemFindHelpers.Views
{
    public class ItemFindHelperView : MonoBehaviour
    {
        [SerializeField]
        private Image m_typeImage;

        [SerializeField]
        private TMP_Text m_whereToFindTMP;


        public void SetData(ItemHelpData helpData)
        {
            m_typeImage.sprite = helpData.WhereToFindPreviewIcon;
            m_whereToFindTMP.text = helpData.WhereToFindExplanation;
        }
    }
}