using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CraftingWorkbenchProfile", menuName = "CraftingSystem/CraftingWorkbenchProfile")]
public class CraftingWorkbenchProfile : ScriptableObject
{
    [TextArea(3, 5)]
    [SerializeField]
    private string m_workbenchName;
    public string WorkbenchName => m_workbenchName;

    [TextArea(5, 10)]
    [SerializeField]
    private string m_workbenchDescription;
    public string WorkbenchDescription => m_workbenchDescription;

    [field: SerializeField]
    public Sprite WorkbenchMiniSprite { get; private set; }


    [field: SerializeField]
    public List<CraftingRecipesWarehouse> CraftingRecipesWarehouse { get; private set; } = new List<CraftingRecipesWarehouse>();


    public List<CraftingRecipe> GetAllCraftingRecipes()
    {
        List<CraftingRecipe> recipes = new List<CraftingRecipe>();

        foreach (CraftingRecipesWarehouse warehouse in CraftingRecipesWarehouse)
        {
            foreach (CraftingRecipe recipe in warehouse.Recipes)
            {
                recipes.Add(recipe);
            }
        }

        return recipes;
    }
}