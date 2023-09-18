using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "New Recipe", menuName = "Creatable/Create Recipe")]
public class Recipe : ScriptableObject
{
    public enum RecipeType
    {
        Potion,
        Weapon,
        Armor,
        Material,
        Ingot,
        Rune
    }

    private void Start()
    {
        recipeName = resultItem.Name;
    }

    [Header("Тип рецепта")]
    public RecipeType Type;
    [HideInInspector()]
    [Header("Название рецепта")]
    public string recipeName;
    [Space]
    [Header("Получаемый предмет")]
    public ItemProfile resultItem;
    [Header("Получаемое кол-во предмета")]
    public int resultStack;
    [Header("Необходимые вещи")]
    public NeededItem[] neededItems;
}

[System.Serializable]
public class RecipeSlot
{
    public string recipesName;

    public Recipe[] recipes = new Recipe[10];
}