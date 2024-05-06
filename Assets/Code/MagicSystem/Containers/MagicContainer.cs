using Dimasyechka.Code.BattleSystem.PlayerSystem;
using Dimasyechka.Code.MagicSystem.Profiles;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.MagicSystem.Containers
{
    public class MagicContainer : MonoBehaviour
    {
        public event Action onMagicObjectsUpdated;


        public event Action<BaseMagicProfile> onMagicProfileAdded;
        public event Action<BaseMagicProfile> onMagicProfileRemoved;


        private List<BaseMagicProfile> _availableMagicProfiles = new List<BaseMagicProfile>();
        public List<BaseMagicProfile> AvailableMagicProfiles => _availableMagicProfiles;


        private RuntimeBattlePlayerController _runtimeBattlePlayerController;

        [Inject]
        public void Construct(RuntimeBattlePlayerController runtimeBattlePlayerController)
        {
            _runtimeBattlePlayerController = runtimeBattlePlayerController;
        }


        public void AddMagicProfile(BaseMagicProfile _profile)
        {
            _runtimeBattlePlayerController.AddDefaultBattleAction(_profile);

            _availableMagicProfiles.Add(_profile);
            onMagicProfileAdded?.Invoke(_profile);
            onMagicObjectsUpdated?.Invoke();
        }

        public void RemoveMagicProfile(BaseMagicProfile _profile)
        {
            _runtimeBattlePlayerController.RemoveDefaultBattleAction(_profile);

            _availableMagicProfiles.Remove(_profile);
            onMagicProfileRemoved?.Invoke(_profile);
            onMagicObjectsUpdated?.Invoke();
        }
    }
}
