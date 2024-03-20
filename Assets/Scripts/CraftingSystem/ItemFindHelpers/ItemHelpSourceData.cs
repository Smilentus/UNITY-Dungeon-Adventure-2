using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemHelpSourceData", menuName = "CraftingSystem/ItemHelpSourceData")]
public class ItemHelpSourceData : ScriptableObject
{
    //[field: SerializeField]
    //public Sprite LocationDefaultIcon { get; private set; }

    //[field: SerializeField]
    //public Sprite CraftDefaultIcon { get; private set; }


    [field: SerializeField]
    public List<LocationProfile> AllLocations { get; private set; }


    [field: SerializeField]
    public List<CraftingWorkbenchProfile> AllWorkbenches { get; private set; }
}
