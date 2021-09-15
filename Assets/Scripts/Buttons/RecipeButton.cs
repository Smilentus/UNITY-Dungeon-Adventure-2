using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
