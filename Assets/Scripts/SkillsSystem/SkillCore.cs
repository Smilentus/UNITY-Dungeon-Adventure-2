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
        // Тут добавляем проверки на то можем ли мы апнуть уровень навыка и т.п.

        return m_upgradeableComponent.TryAddLevel();
    }
}