using UnityEngine;


public class BaseUpgradeableCheckerComponent : CoreComponent, IUpgradeableChecker
{
    [SerializeField]
    protected int m_checkLevel;
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

    protected virtual void ProcessUpgrade() { }

    public virtual bool CanUpgrade() { return true; }
}