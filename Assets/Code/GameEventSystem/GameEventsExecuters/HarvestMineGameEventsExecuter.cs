using System;
using Dimasyechka.Code.GameEventSystem.Interfaces;
using Dimasyechka.Code.GameEventSystem.Profiles;
using Dimasyechka.Code.InventorySystem;
using Dimasyechka.Code.InventorySystem.BaseItem;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.GameEventSystem.GameEventsExecuters
{
    public class HarvestMineGameEventsExecuter : MonoBehaviour, IGameEventsExecuter
    {
        public Type ProfileType => typeof(HarvestMineGameEventProfile);


        private InventoryController _inventoryController;

        [Inject]
        public void Construct(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }


        public void TryExecuteGameEvent(BaseGameEventProfile gameEventProfile)
        {
            if (gameEventProfile.GetType().Equals(ProfileType))
            {
                HarvestMineGameEventProfile profile = gameEventProfile as HarvestMineGameEventProfile;

                foreach (BaseItemProfile harvestableProfile in profile.BaseHarvestables)
                {
                    int randomAmount = UnityEngine.Random.Range(profile.HarvestableAmountMin, profile.HarvestableAmountMax);

                    _inventoryController.TryAddItemToAnyContainer(harvestableProfile, randomAmount);
                }

                //GameController.Instance.AddEventText($"{profile.EventTitle}");
            }
        }
    }
}
