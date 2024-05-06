using Dimasyechka.Code._LEGACY_.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dimasyechka.Code._LEGACY_.Buttons
{
    public class RecipeButton : MonoBehaviour, IPointerClickHandler
    {
        [Header("Номер рецепта")]
        public int slotNum;
        public int recipeNum;

        public void OnPointerClick(PointerEventData eventData)
        {
            FindObjectOfType<CraftingManager>().ShowActions(recipeNum, slotNum);
        }
    }
}
