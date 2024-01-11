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
        // Важно добавлять +1 к уровню, потому что мы загружаем текущий уровень навыка
        // А здесь нам необходимо устанавливать тот уровень, который мы проверяем для следующего улучшения
        m_checkLevel = level + 1;
    }

    protected virtual void ProcessUpgrade() { }

    public virtual bool CanUpgrade() { return true; }
}