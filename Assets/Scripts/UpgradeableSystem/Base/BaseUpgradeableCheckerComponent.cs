using UnityEngine;


public class BaseUpgradeableCheckerComponent : CoreComponent, IUpgradeableChecker
{
    [SerializeField]
    protected int m_checkLevel = 0;
    public int CheckLevel => m_checkLevel;


    [Tooltip("True -> бесконечное увеличение уровней проверки")]
    [SerializeField]
    protected bool m_autoRaiseCheckLevel = false;


    public virtual void PostUpgrade()
    {
        ProcessUpgrade();

        if (m_autoRaiseCheckLevel)
        {
            m_checkLevel++;
        }
    }

    public virtual void LoadUpgradeableLevel(int level)
    {
        // ¬ажно добавл€ть +1 к уровню, потому что мы загружаем текущий уровень навыка
        // ј здесь нам необходимо устанавливать тот уровень, который мы провер€ем дл€ следующего улучшени€
        m_checkLevel = level + 1;
    }

    public virtual string GetDescription() 
    {
        return $"[”словие улучшени€ не описано]";
    }

    protected virtual void ProcessUpgrade() { }

    public virtual bool CanUpgrade() { return true; }
}