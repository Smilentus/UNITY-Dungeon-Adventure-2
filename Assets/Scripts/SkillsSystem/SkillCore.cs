using UnityEngine;


public class SkillCore : Core
{
    [SerializeField]
    protected SkillProfile m_skillProfile;
    public SkillProfile SkillProfile => m_skillProfile;


    [SerializeField] 
    protected UpgradeableComponent m_upgradeableComponent;
    public UpgradeableComponent UpgradeableComponent => m_upgradeableComponent;


    public bool isSkillReachedMax => m_upgradeableComponent.reachedMaxUpgrades;


    public override void BuildWithComponents()
    {
        base.BuildWithComponents();
    }


    public void LoadLevel(int level)
    {
        m_upgradeableComponent.LoadLevel(level);
    }


    public bool TryUpgradeSkill()
    {
        // ��� ��������� �������� �� �� ����� �� �� ������ ������� ������ � �.�.

        return m_upgradeableComponent.TryAddLevel();
    }
}