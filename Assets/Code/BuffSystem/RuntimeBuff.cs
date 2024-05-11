using System;
using Dimasyechka.Code.BuffSystem.Profiles;
using UnityEngine;

namespace Dimasyechka.Code.BuffSystem
{
    [System.Serializable]
    public class RuntimeBuff : MonoBehaviour
    {
        public event Action onBuffEnded;

        public event Action<bool> onBuffEnabledStateChanged;
        public event Action<int> onBuffDurationChanged;


        protected BuffProfile _buffProfile;
        public BuffProfile BuffProfile => _buffProfile;


        protected bool _isBuffEnabled;
        public bool IsBuffEnabled => _isBuffEnabled;


        protected int _durationHours;
        public int DurationHours => _durationHours;


        protected bool _isBuffEnded;
        public bool IsBuffEnded => _isBuffEnded;


        public virtual void SetBuff(BuffProfile buffProfile)
        {
            _buffProfile = buffProfile;

            _durationHours = _buffProfile.BuffDurationHours;
        }


        public virtual RuntimeBuffSaveData GetSaveBuffData()
        {
            return new RuntimeBuffSaveData() {
                BuffUID = _buffProfile.BuffUID,
                BuffDurationHours = DurationHours
            };
        }


        public virtual void ForceSetDurationHours(int durationHours)
        {
            _durationHours = durationHours;
        }

        // Каждый тик баффа происходит каждый час - изменяется оставшееся время действия баффа
        // А также может выполниться какое-то действие баффа
        public virtual void UpdateBuffHourTick()
        {
            _durationHours--;

            onBuffDurationChanged?.Invoke(_durationHours);

            OnTickAction();

            if (_durationHours <= 0)
            {
                _isBuffEnded = true;

                OnPreBuffEnded();

                onBuffEnded?.Invoke();
            }
        }


        public virtual void OnTickAction() { }
        public virtual void OnPreBuffEnded() { }


        public virtual void EnableBuff()
        {
            if (_isBuffEnabled) return;

            _isBuffEnabled = true;
            onBuffEnabledStateChanged?.Invoke(_isBuffEnabled);
        }
        public virtual void DisableBuff()
        {
            if (!_isBuffEnabled) return;

            _isBuffEnabled = false;
            onBuffEnabledStateChanged?.Invoke(_isBuffEnabled);
        }
    }

    [System.Serializable]
    public class RuntimeBuffSaveData
    {
        public string BuffUID;
        public int BuffDurationHours;
    }
}