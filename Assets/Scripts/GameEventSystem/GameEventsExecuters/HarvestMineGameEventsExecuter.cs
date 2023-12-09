using System;
using UnityEngine;


public class HarvestMineGameEventsExecuter : MonoBehaviour, IGameEventsExecuter
{
    public Type ProfileType => typeof(HarvestMineGameEventProfile);


    public void TryExecuteGameEvent(BaseGameEventProfile _profile)
    {
        if (_profile.GetType().Equals(ProfileType))
        {
            HarvestMineGameEventProfile profile = _profile as HarvestMineGameEventProfile;

            foreach (BaseItemProfile harvestableProfile in profile.BaseHarvestables)
            {
                int randomAmount = UnityEngine.Random.Range(profile.HarvestableAmountMin, profile.HarvestableAmountMax);

                InventoryController.Instance.TryAddItemToAnyContainer(harvestableProfile, randomAmount);
            }

            GameController.Instance.AddEventText($"{profile.EventTitle}");
        }
    }
}
