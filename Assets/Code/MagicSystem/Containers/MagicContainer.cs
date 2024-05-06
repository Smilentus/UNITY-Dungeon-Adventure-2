using System;
using System.Collections.Generic;
using Dimasyechka.Code.BattleSystem.PlayerSystem;
using Dimasyechka.Code.MagicSystem.Profiles;
using UnityEngine;

namespace Dimasyechka.Code.MagicSystem.Containers
{
    public class MagicContainer : MonoBehaviour
    {
        public event Action onMagicObjectsUpdated;


        public event Action<BaseMagicProfile> onMagicProfileAdded;
        public event Action<BaseMagicProfile> onMagicProfileRemoved;


        private List<BaseMagicProfile> availableMagicProfiles = new List<BaseMagicProfile>();
        public List<BaseMagicProfile> AvailableMagicProfiles => availableMagicProfiles;


        public void AddMagicProfile(BaseMagicProfile _profile)
        {
            RuntimeBattlePlayerController.Instance.AddDefaultBattleAction(_profile);

            availableMagicProfiles.Add(_profile);
            onMagicProfileAdded?.Invoke(_profile);
            onMagicObjectsUpdated?.Invoke();
        }

        public void RemoveMagicProfile(BaseMagicProfile _profile)
        {
            RuntimeBattlePlayerController.Instance?.RemoveDefaultBattleAction(_profile);

            availableMagicProfiles.Remove(_profile);
            onMagicProfileRemoved?.Invoke(_profile);
            onMagicObjectsUpdated?.Invoke();
        }
    }
}
