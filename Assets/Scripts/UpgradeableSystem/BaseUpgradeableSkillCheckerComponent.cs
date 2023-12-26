using UnityEngine;


public class BaseUpgradeableSkillCheckerComponent : BaseUpgradeableCheckerComponent
{
    [SerializeField]
    protected int m_skillPointsCost;

    [SerializeField]
    protected int m_skillPointsCostRaiseValue;

    [Tooltip("Увеличение цены навыка на Y каждый X уровень")]
    [SerializeField]
    protected int m_everyLevelRaiseSkillPoints;


    public override bool CanUpgrade()
    {
        return m_skillPointsCost <= RuntimePlayer.Instance.RuntimePlayerStats.SkillPoints;
    }

    public override void PostUpgrade()
    {
        base.PostUpgrade();

        SkillCore skillCore = attachedCore as SkillCore;

        if (skillCore.UpgradeableComponent.currentLevel % m_everyLevelRaiseSkillPoints == 0)
        {
            m_skillPointsCost += m_skillPointsCostRaiseValue;
        }
    }

    protected override void ProcessUpgrade()
    {
        RuntimePlayer.Instance.RuntimePlayerStats.SkillPoints -= m_skillPointsCost;
    }
}