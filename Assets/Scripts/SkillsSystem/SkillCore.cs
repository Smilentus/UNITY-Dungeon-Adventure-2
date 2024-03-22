using System.Collections.Generic;
using System.Linq;
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


    private List<BaseSkillComponent> skillComponents = new List<BaseSkillComponent>();

    
    // Переделай, дружище =(
    public List<string> GetSkillDeltaValues(int level)
    {
        if (skillComponents.Count == 0)
            skillComponents = GetComponentsInChildren<BaseSkillComponent>().ToList();
        
        List<string> output = new List<string>();

        foreach (BaseSkillComponent baseSkillComponent in skillComponents)
        {
            output.AddRange(baseSkillComponent.GetDeltaValues(level));
        }

        return output;
    }


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