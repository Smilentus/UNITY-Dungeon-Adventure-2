using System;
using System.Collections.Generic;
using System.Linq;
using Dimasyechka.Code.BuffSystem.Profiles;
using UnityEngine;

namespace Dimasyechka.Code.BuffSystem.Containers
{
    public class BuffsContainer : MonoBehaviour
    {
        public event Action onRuntimeBuffsChanged;


        protected List<RuntimeBuff> _runtimeBuffs = new List<RuntimeBuff>();
        public List<RuntimeBuff> RuntimeBuffs => _runtimeBuffs;


        public void DisableAndRemoveAllBuffs()
        {
            foreach (RuntimeBuff runtimeBuff in _runtimeBuffs)
            {
                runtimeBuff.DisableBuff();
            }

            _runtimeBuffs.Clear();

            onRuntimeBuffsChanged?.Invoke();
        }


        public void UpdateContainedBuffs()
        {
            foreach (RuntimeBuff runtimeBuff in _runtimeBuffs)
            {
                runtimeBuff.UpdateBuffHourTick();
            }

            CheckEndedBuffs();

            onRuntimeBuffsChanged?.Invoke();
        }
        private void CheckEndedBuffs()
        {
            for (int i = _runtimeBuffs.Count - 1; i >= 0; i--)
            {
                if (_runtimeBuffs[i].IsBuffEnded)
                {
                    RemoveBuff(_runtimeBuffs[i]);
                }
            }
        }

        public void LoadBuff(BuffProfile buffProfile, int duration)
        {
            RuntimeBuff newRuntimeBuff = Instantiate(buffProfile.RuntimeBuffPrefab, this.transform);
            newRuntimeBuff.SetBuff(buffProfile);
            newRuntimeBuff.ForceSetDurationHours(duration);

            _runtimeBuffs.Add(newRuntimeBuff);

            onRuntimeBuffsChanged?.Invoke();
        }

        public void AddBuff(BuffProfile buffProfile)
        {
            _runtimeBuffs = _runtimeBuffs.Where(x => x != null).ToList();

            RuntimeBuff founded = _runtimeBuffs.Find(x => x.BuffProfile == buffProfile);

            if (founded == null)
            {
                RuntimeBuff newRuntimeBuff = Instantiate(buffProfile.RuntimeBuffPrefab, this.transform);
                newRuntimeBuff.SetBuff(buffProfile);
                newRuntimeBuff.EnableBuff();

                _runtimeBuffs.Add(newRuntimeBuff);

                onRuntimeBuffsChanged?.Invoke();
            }
        }
        public void RemoveBuff(BuffProfile buffProfile)
        {
            _runtimeBuffs = _runtimeBuffs.Where(x => x != null).ToList();

            RuntimeBuff founded = _runtimeBuffs.Find(x => x.BuffProfile == buffProfile);

            if (founded != null)
            {
                RemoveBuff(founded);
            }
        }
        public void RemoveBuff(RuntimeBuff runtimeBuff)
        {
            runtimeBuff.DisableBuff();
            _runtimeBuffs.Remove(runtimeBuff);

            Destroy(runtimeBuff.gameObject);

            onRuntimeBuffsChanged?.Invoke();

        }
    }
}
