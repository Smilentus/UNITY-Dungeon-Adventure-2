using System;
using System.Collections.Generic;
using System.Linq;
using Dimasyechka.Code.CoreComponentSystem.Core;
using Dimasyechka.Code.CoreComponentSystem.Interfaces;
using Dimasyechka.Code.UpgradeableSystem.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Dimasyechka.Code.UpgradeableSystem
{
    [System.Serializable] public class UnityEventInt : UnityEvent<int> { }

    public class UpgradeableComponent : CoreComponent
    {
        [Tooltip("Максимально допустимый уровень улучшений (-1 бесконечно)")]
        [SerializeField] private int m_maxUpgradesLevel;
        /// <summary>
        ///     Возвращает максимально допустимый уровень улучшений
        /// </summary>
        public int maxUpgradesLevel { get => m_maxUpgradesLevel; }


        private bool m_reachedMaxUpgrades = false;
        /// <summary>
        ///     Возвращает статус достижения максимального уровня апгрейдов.
        /// </summary>
        public bool reachedMaxUpgrades { get => m_reachedMaxUpgrades; }

    
        private int m_currentLevel;
        /// <summary>
        ///     Возвращает текущий уровень
        /// </summary>
        public int currentLevel { get => m_currentLevel; }

    
        /// <summary>
        ///     События вызываемые при улучшении на 1 уровень
        /// </summary>
        public UnityEventInt OnUpgradedUnityEvent = new UnityEventInt();


        /// <summary>
        ///     Событие вызываемое при достижении максимального уровня
        /// </summary>
        public UnityEvent OnMaxUpgradesReachedUnityEvent = new UnityEvent();


        public event Action<int> OnUpgraded;
        public event Action OnMaxUpgradesReached;


        private List<IUpgradeableChecker> upgradeableCheckers = new List<IUpgradeableChecker>();


        public override void InjectComponent(ICore core)
        {
            base.InjectComponent(core);

            GetCheckers();
        }


        private void GetCheckers()
        {
            if (upgradeableCheckers.Count == 0)
            {
                upgradeableCheckers = GetComponentsInChildren<IUpgradeableChecker>(true).ToList();
            }
        }

        /// <summary>
        ///     Добавляет 1 уровень
        /// </summary>
        public bool TryAddLevel()
        {
            if (m_reachedMaxUpgrades) return false;

            if (!CanUpgradeLevel(m_currentLevel + 1)) return false;

            m_currentLevel++;

            PostUpgradeCheckers();

            OnUpgradedUnityEvent?.Invoke(m_currentLevel);
            OnUpgraded?.Invoke(m_currentLevel);

            CheckMaxUpgrades();

            return true;
        }

        public List<SkillUpgradeDescriptionData> GetUpgradeDescriptions()
        {
            GetCheckers();

            List<SkillUpgradeDescriptionData> output = new List<SkillUpgradeDescriptionData>();

            foreach (IUpgradeableChecker upgradeableChecker in upgradeableCheckers)
            {
                SkillUpgradeDescriptionData description = new SkillUpgradeDescriptionData()
                {
                    IsAccomplished = upgradeableChecker.CanUpgrade(),
                    Description = upgradeableChecker.GetDescription()
                };
                output.Add(description);
            }

            return output;
        }

        private void CheckMaxUpgrades(bool invokeEvents = true)
        {
            if (maxUpgradesLevel != -1 && m_currentLevel >= m_maxUpgradesLevel)
            {
                m_reachedMaxUpgrades = true;
                m_currentLevel = m_maxUpgradesLevel;

                if (invokeEvents)
                {
                    OnMaxUpgradesReachedUnityEvent?.Invoke();
                    OnMaxUpgradesReached?.Invoke();
                }
            }
        }

        public void LoadLevel(int loadableLevel)
        {
            GetCheckers();

            m_currentLevel = loadableLevel;

            foreach (IUpgradeableChecker upgradeableChecker in upgradeableCheckers)
            {
                upgradeableChecker.LoadUpgradeableLevel(m_currentLevel);
            }

            CheckMaxUpgrades(false);
        }

        public bool CanUpgradeLevel(int upgradeableLevel)
        {
            GetCheckers();

            IUpgradeableChecker[] checkers = upgradeableCheckers.Where(x => x.CheckLevel == upgradeableLevel).ToArray();

            foreach (IUpgradeableChecker checker in checkers)
            {
                if (!checker.CanUpgrade()) return false;
            }

            return true;
        }

        private void PostUpgradeCheckers()
        {
            GetCheckers();

            foreach (IUpgradeableChecker upgradeableChecker in upgradeableCheckers)
            {
                upgradeableChecker.PostUpgrade();
            }
        }
    }


    public struct SkillUpgradeDescriptionData
    {
        public bool IsAccomplished;
        public string Description;
    }
}