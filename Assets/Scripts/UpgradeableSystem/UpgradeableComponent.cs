using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class UnityEventInt : UnityEvent<int> { }

public class UpgradeableComponent : CoreComponent
{
    [Tooltip("����������� ���������� ������� ��������� (-1 ����������)")]
    [SerializeField] private int m_maxUpgradesLevel;
    /// <summary>
    ///     ���������� ����������� ���������� ������� ���������
    /// </summary>
    public int maxUpgradesLevel { get => m_maxUpgradesLevel; }


    private bool m_reachedMaxUpgrades = false;
    /// <summary>
    ///     ���������� ������ ���������� ������������� ������ ���������.
    /// </summary>
    public bool reachedMaxUpgrades { get => m_reachedMaxUpgrades; }

    
    private int m_currentLevel;
    /// <summary>
    ///     ���������� ������� �������
    /// </summary>
    public int currentLevel { get => m_currentLevel; }

    
    /// <summary>
    ///     ������� ���������� ��� ��������� �� 1 �������
    /// </summary>
    public UnityEventInt OnUpgradedUnityEvent = new UnityEventInt();


    /// <summary>
    ///     ������� ���������� ��� ���������� ������������� ������
    /// </summary>
    public UnityEvent OnMaxUpgradesReachedUnityEvent = new UnityEvent();


    public event Action<int> OnUpgraded;
    public event Action OnMaxUpgradesReached;


    private List<IUpgradeableChecker> upgradeableCheckers = new List<IUpgradeableChecker>();


    public override void InjectComponent(ICore core)
    {
        base.InjectComponent(core);

        upgradeableCheckers = GetComponentsInChildren<IUpgradeableChecker>(true).ToList();
    }

    /// <summary>
    ///     ��������� 1 �������
    /// </summary>
    public bool TryAddLevel()
    {
        if (m_reachedMaxUpgrades) return false;

        if (!CanUpgradeLevel(m_currentLevel + 1)) return false;

        m_currentLevel++;

        PostUpgradeCheckers();

        OnUpgradedUnityEvent?.Invoke(m_currentLevel);
        OnUpgraded?.Invoke(m_currentLevel);

        if (maxUpgradesLevel != -1 && m_currentLevel >= m_maxUpgradesLevel)
        {
            m_reachedMaxUpgrades = true;
            m_currentLevel = m_maxUpgradesLevel;

            OnMaxUpgradesReachedUnityEvent?.Invoke();
            OnMaxUpgradesReached?.Invoke();
        }

        return true;
    }


    private bool CanUpgradeLevel(int upgradeableLevel)
    {
        IUpgradeableChecker[] checkers = upgradeableCheckers.Where(x => x.CheckLevel == upgradeableLevel).ToArray();

        foreach (IUpgradeableChecker checker in checkers)
        {
            if (!checker.CanUpgrade()) return false;
        }

        return true;
    }

    private void PostUpgradeCheckers()
    {
        foreach (IUpgradeableChecker upgradeableChecker in upgradeableCheckers)
        {
            upgradeableChecker.PostUpgrade();
        }
    }
}
