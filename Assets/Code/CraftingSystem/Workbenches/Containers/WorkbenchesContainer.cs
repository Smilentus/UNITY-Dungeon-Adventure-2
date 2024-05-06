using System;
using System.Collections.Generic;
using Dimasyechka.Code.CraftingSystem.Workbenches.Profiles;
using UnityEngine;

namespace Dimasyechka.Code.CraftingSystem.Workbenches.Containers
{
    public class WorkbenchesContainer : MonoBehaviour
    {
        public event Action onWorkbenchesChanged;


        private List<CraftingWorkbenchProfile> _availableCraftingWorkbenches = new List<CraftingWorkbenchProfile>();
        public List<CraftingWorkbenchProfile> AvailableCraftingWorkbenches => _availableCraftingWorkbenches;


        public CraftingWorkbenchProfile addable;


        [ContextMenu("Add")]
        public void ADd()
        {
            AddCraftingWorkbench(addable);
        }


        public void AddCraftingWorkbench(CraftingWorkbenchProfile craftingWorkbench)
        {
            if (craftingWorkbench == null) return;
            if (_availableCraftingWorkbenches.Contains(craftingWorkbench)) return;

            _availableCraftingWorkbenches.Add(craftingWorkbench);

            onWorkbenchesChanged?.Invoke();
        }

        public void RemoveCraftingWorkbench(CraftingWorkbenchProfile craftingWorkbench)
        {
            if (craftingWorkbench == null) return;
            if (!_availableCraftingWorkbenches.Contains(craftingWorkbench)) return;

            _availableCraftingWorkbenches.Remove(craftingWorkbench);

            onWorkbenchesChanged?.Invoke();
        }
    }
}