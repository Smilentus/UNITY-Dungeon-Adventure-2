using System.Collections.Generic;
using Dimasyechka.Code.CraftingSystem.Workbenches.Profiles;
using Dimasyechka.Code.LocationSystem.Profiles;
using UnityEngine;

namespace Dimasyechka.Code.CraftingSystem.ItemFindHelpers
{
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
}
