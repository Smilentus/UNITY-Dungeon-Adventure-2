using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class UnityEventInt : UnityEvent<int> { }

public class UpgradeableComponent : CoreComponent
{
    [Tooltip("Максимально допустимый уровень улучшений")]
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
    public UnityEventInt OnUpgraded = new UnityEventInt();


    /// <summary>
    ///     Событие вызываемое при достижении максимального уровня
    /// </summary>
    public UnityEvent OnMaxUpgradesReached = new UnityEvent();

    
    /// <summary>
    ///     Добавляет 1 уровень 
    /// </summary>
    public void AddLevel()
    {
        if (m_reachedMaxUpgrades) return;

        m_currentLevel++;

        OnUpgraded?.Invoke(m_currentLevel);

        if (m_currentLevel >= m_maxUpgradesLevel)
        {
            m_reachedMaxUpgrades = true;
            m_currentLevel = m_maxUpgradesLevel;
            OnMaxUpgradesReached?.Invoke();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            AddLevel();
            Debug.Log($"Upgrade Skill -> {m_currentLevel}");
        }
    }
}
