using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CraftingRecipesWarehouse", menuName = "CraftingSystem/CraftingRecipesWarehouse")]
public class CraftingRecipesWarehouse : ScriptableObject
{
    [field: SerializeField]
    public List<CraftingRecipe> Recipes { get; private set; }
}