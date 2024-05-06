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


        protected BuffProfile buffProfile;
        public BuffProfile BuffProfile => buffProfile;


        protected bool isBuffEnabled;
        public bool IsBuffEnabled => isBuffEnabled;


        protected int durationHours;
        public int DurationHours => durationHours;


        protected bool isBuffEnded;
        public bool IsBuffEnded => isBuffEnded;


        public virtual void SetBuff(BuffProfile _buffProfile)
        {
            buffProfile = _buffProfile;

            durationHours = buffProfile.BuffDurationHours;
        }


        public virtual RuntimeBuffSaveData GetSaveBuffData()
        {
            return new RuntimeBuffSaveData() {
                BuffUID = buffProfile.BuffUID,
                BuffDurationHours = DurationHours
            };
        }

        /// <summary>
        ///     Метод в основном используемый для загрузки
        /// </summary>
        /// <param name="durationHours">
        ///     Оставшееся время действия баффа в часах
        /// </param>
        public virtual void ForceSetDurationHours(int _durationHours)
        {
            durationHours = _durationHours;
        }

        // Каждый тик баффа происходит каждый час - изменяется оставшееся время действия баффа
        // А также может выполниться какое-то действие баффа
        public virtual void UpdateBuffHourTick()
        {
            durationHours--;

            onBuffDurationChanged?.Invoke(durationHours);

            OnTickAction();

            if (durationHours <= 0)
            {
                isBuffEnded = true;

                OnPreBuffEnded();

                onBuffEnded?.Invoke();
            }
        }


        public virtual void OnTickAction() { }
        public virtual void OnPreBuffEnded() { }


        public virtual void EnableBuff()
        {
            if (isBuffEnabled) return;

            isBuffEnabled = true;
            onBuffEnabledStateChanged?.Invoke(isBuffEnabled);
        }
        public virtual void DisableBuff()
        {
            if (!isBuffEnabled) return;

            isBuffEnabled = false;
            onBuffEnabledStateChanged?.Invoke(isBuffEnabled);
        }
    }

    [System.Serializable]
    public class RuntimeBuffSaveData
    {
        public string BuffUID;
        public int BuffDurationHours;
    }
}