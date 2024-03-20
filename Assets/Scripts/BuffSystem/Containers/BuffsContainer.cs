using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BuffsContainer : MonoBehaviour
{
    public event Action onRuntimeBuffsChanged;


    protected List<RuntimeBuff> runtimeBuffs = new List<RuntimeBuff>();
    public List<RuntimeBuff> RuntimeBuffs => runtimeBuffs;


    public void DisableAndRemoveAllBuffs()
    {
        foreach (RuntimeBuff runtimeBuff in runtimeBuffs)
        {
            runtimeBuff.DisableBuff();
        }

        runtimeBuffs.Clear();

        onRuntimeBuffsChanged?.Invoke();
    }


    public void UpdateContainedBuffs()
    {
        foreach (RuntimeBuff runtimeBuff in runtimeBuffs)
        {
            runtimeBuff.UpdateBuffHourTick();
        }

        CheckEndedBuffs();

        onRuntimeBuffsChanged?.Invoke();
    }
    private void CheckEndedBuffs()
    {
        for (int i = runtimeBuffs.Count - 1; i >= 0; i--)
        {
            if (runtimeBuffs[i].IsBuffEnded)
            {
                RemoveBuff(runtimeBuffs[i]);
            }
        }
    }

    public void LoadBuff(BuffProfile buffProfile, int duration)
    {
        RuntimeBuff newRuntimeBuff = Instantiate(buffProfile.RuntimeBuffPrefab, this.transform);
        newRuntimeBuff.SetBuff(buffProfile);
        newRuntimeBuff.ForceSetDurationHours(duration);

        runtimeBuffs.Add(newRuntimeBuff);

        onRuntimeBuffsChanged?.Invoke();
    }

    public void AddBuff(BuffProfile buffProfile)
    {
        runtimeBuffs = runtimeBuffs.Where(x => x != null).ToList();

        RuntimeBuff founded = runtimeBuffs.Find(x => x.BuffProfile == buffProfile);

        if (founded == null)
        {
            RuntimeBuff newRuntimeBuff = Instantiate(buffProfile.RuntimeBuffPrefab, this.transform);
            newRuntimeBuff.SetBuff(buffProfile);
            newRuntimeBuff.EnableBuff();

            runtimeBuffs.Add(newRuntimeBuff);

            onRuntimeBuffsChanged?.Invoke();
        }
    }
    public void RemoveBuff(BuffProfile buffProfile)
    {
        runtimeBuffs = runtimeBuffs.Where(x => x != null).ToList();

        RuntimeBuff founded = runtimeBuffs.Find(x => x.BuffProfile == buffProfile);

        if (founded != null)
        {
            RemoveBuff(founded);
        }
    }
    public void RemoveBuff(RuntimeBuff runtimeBuff)
    {
        runtimeBuff.DisableBuff();
        runtimeBuffs.Remove(runtimeBuff);

        Destroy(runtimeBuff.gameObject);

        onRuntimeBuffsChanged?.Invoke();

    }
}
