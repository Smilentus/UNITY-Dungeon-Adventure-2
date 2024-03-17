using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "CraftingSystem/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("Info")]
    [TextArea(3, 5)]
    [SerializeField]
    private string m_recipeName;
    public string RecipeName => m_recipeName;

    [TextArea(5, 10)]
    [SerializeField]
    private string m_recipeDescription;
    public string RecipeDescription => m_recipeDescription;


    [Header("Output")]
    [SerializeField]
    private BaseItemProfile m_craftedItem;
    public BaseItemProfile CraftedItem => m_craftedItem;

    [SerializeField]
    private int m_craftedItemAmount;
    public int CraftedItemAmount => m_craftedItemAmount;


    [Header("Input")]
    [SerializeField]
    private List<InputCraftItem> m_inputCraftItems = new List<InputCraftItem>();
    public List<InputCraftItem> InputCraftItems => m_inputCraftItems;
}


[System.Serializable]
public class InputCraftItem
{
    [field: SerializeField]
    public BaseItemProfile InputItemProfile { get; private set; }

    [field: SerializeField]
    public int InputItemAmount { get; private set; }
}