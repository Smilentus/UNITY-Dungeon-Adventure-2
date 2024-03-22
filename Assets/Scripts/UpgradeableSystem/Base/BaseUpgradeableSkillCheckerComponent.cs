using UnityEngine;


public class BaseUpgradeableSkillCheckerComponent : BaseUpgradeableCheckerComponent
{
    [SerializeField]
    protected int m_skillPointsCost;

    [SerializeField]
    protected int m_skillPointsCostRaiseValue;

    [Tooltip("���������� ���� ������ �� Y ������ X �������")]
    [SerializeField]
    protected int m_everyLevelRaiseSkillPoints;


    public override void LoadUpgradeableLevel(int level)
    {
        base.LoadUpgradeableLevel(level);

        for (int l = 0; l < level; l++)
        {
            CalculateSkillCost(l);
        }
    }

    public override string GetDescription()
    {
        return $"���������� {m_skillPointsCost} ��";
    }

    public override bool CanUpgrade()
    {
        return m_skillPointsCost <= RuntimePlayer.Instance.RuntimePlayerStats.SkillPoints;
    }

    public override void PostUpgrade()
    {
        base.PostUpgrade();

        SkillCore skillCore = attachedCore as SkillCore;
        CalculateSkillCost(skillCore.UpgradeableComponent.currentLevel);
    }

    protected override void ProcessUpgrade()
    {
        RuntimePlayer.Instance.RuntimePlayerStats.SkillPoints -= m_skillPointsCost;
    }

    
    protected void CalculateSkillCost(int skillLevel)
    {
        if (skillLevel % m_everyLevelRaiseSkillPoints == 0)
        {
            m_skillPointsCost += m_skillPointsCostRaiseValue;
        }
    }
}