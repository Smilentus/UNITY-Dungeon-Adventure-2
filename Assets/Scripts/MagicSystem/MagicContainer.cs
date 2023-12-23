using System;
using System.Collections.Generic;
using UnityEngine;


public class MagicContainer : MonoBehaviour
{
    public event Action onMagicObjectsUpdated;


    public event Action<MagicProfile> onMagicProfileAdded;
    public event Action<MagicProfile> onMagicProfileRemoved;


    private List<MagicProfile> availableMagicProfiles = new List<MagicProfile>();
    public List<MagicProfile> AvailableMagicProfiles => availableMagicProfiles;


    public void AddMagicProfile(MagicProfile _profile)
    {
        RuntimeBattlePlayerController.Instance.AddDefaultBattleAction(_profile);

        availableMagicProfiles.Add(_profile);
        onMagicProfileAdded?.Invoke(_profile);
        onMagicObjectsUpdated?.Invoke();
    }

    public void RemoveMagicProfile(MagicProfile _profile)
    {
        RuntimeBattlePlayerController.Instance?.RemoveDefaultBattleAction(_profile);

        availableMagicProfiles.Remove(_profile);
        onMagicProfileRemoved?.Invoke(_profile);
        onMagicObjectsUpdated?.Invoke();
    }
}
