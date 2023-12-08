using System;
using System.Collections.Generic;
using UnityEngine;


public class MagicContainer : MonoBehaviour
{
    public event Action onMagicObjectsUpdated;

    public event Action<RuntimeMagicObject> onMagicProfileAdded;
    public event Action<RuntimeMagicObject> onMagicProfileRemoved;


    private List<RuntimeMagicObject> availableMagicObjects = new List<RuntimeMagicObject>();
    public List<RuntimeMagicObject> AvailableMagicObjects => availableMagicObjects;


    public void ResetAllCooldowns()
    {
        foreach (RuntimeMagicObject runtimeMagicObject in availableMagicObjects)
        {
            runtimeMagicObject.SetSkillCooldown(0);
        }    
    }


    public void UpdateMagicCooldowns()
    {
        foreach (RuntimeMagicObject runtimeMagicObject in availableMagicObjects)
        {
            runtimeMagicObject.UpdateCooldown();
        }

        onMagicObjectsUpdated?.Invoke();
    }


    public bool TryUseMagic(MagicProfile _profile)
    {
        RuntimeMagicObject runtimeMagicObject = availableMagicObjects.Find(x => x.MagicProfile == _profile);

        if (runtimeMagicObject != null)
        {
            bool output = runtimeMagicObject.TryUseMagic();

            UpdateMagicCooldowns();

            return output;
        }
        else
        {
            return false;
        }
    }


    public void AddMagicProfile(MagicProfile _profile)
    {
        RuntimeMagicObject runtimeMagicObject = availableMagicObjects.Find(x => x.MagicProfile == _profile);

        if (runtimeMagicObject == null)
        {
            RuntimeMagicObject instantiatedMagicObject = Instantiate(_profile.MagicObject, this.transform);

            instantiatedMagicObject.SetupMagicObject(_profile);

            availableMagicObjects.Add(instantiatedMagicObject);

            onMagicProfileAdded?.Invoke(instantiatedMagicObject);
            onMagicObjectsUpdated?.Invoke();
        }
    }

    public void RemoveMagicProfile(MagicProfile _profile)
    {
        RuntimeMagicObject runtimeMagicObject = availableMagicObjects.Find(x => x.MagicProfile == _profile);

        if (runtimeMagicObject != null)
        {
            availableMagicObjects.Remove(runtimeMagicObject);

            onMagicProfileRemoved?.Invoke(runtimeMagicObject);
            onMagicObjectsUpdated?.Invoke();

            Destroy(runtimeMagicObject.gameObject);
        }
    }
}
