using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;


public class ItemHelpSource : MonoBehaviour
{
    private static ItemHelpSource _instance;
    public static ItemHelpSource Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ItemHelpSource>();
            }

            return _instance;
        }
    }


    [SerializeField]
    private ItemHelpSourceData m_sourceData;


    public IEnumerable<ItemHelpData> GetItemHelpData(BaseItemProfile itemProfile)
    {
        List<ItemHelpData> itemHelpData = new List<ItemHelpData>();

        foreach (LocationProfile locationProfile in m_sourceData.AllLocations)
        {
            if (locationProfile.DroppableItems.Contains(itemProfile))
            {
                itemHelpData.Add(new ItemHelpData() 
                {
                    WhereToFindPreviewIcon = locationProfile.LocationPreviewSprite,
                    WhereToFindExplanation = $"Найти на '{locationProfile.LocationTitle.ToLower()}'"
                });
            }
        }


        foreach (CraftingWorkbenchProfile workbenchProfile in m_sourceData.AllWorkbenches)
        {
            foreach (CraftingRecipesWarehouse recipesWarehouse in workbenchProfile.CraftingRecipesWarehouse)
            {
                if (recipesWarehouse.Recipes.Where(x => x.CraftedItem.Equals(itemProfile)).ToList().Count > 0)
                {
                    itemHelpData.Add(new ItemHelpData()
                    {
                        WhereToFindPreviewIcon = workbenchProfile.WorkbenchMiniSprite,
                        WhereToFindExplanation = $"Создать на '{workbenchProfile.WorkbenchName.ToLower()}'"
                    });
                }
            }
        }

        return itemHelpData;
    }
}


[System.Serializable]
public class ItemHelpData
{
    public Sprite WhereToFindPreviewIcon;

    public string WhereToFindExplanation;
}