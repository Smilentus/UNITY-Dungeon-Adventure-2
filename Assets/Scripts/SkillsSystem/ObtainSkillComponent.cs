using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainSkillComponent : CoreComponent
{
    [SerializeField] private SkillProfile m_obtainableSkill;
    public SkillProfile obtainableSkill { get => m_obtainableSkill; set => m_obtainableSkill = value; }

    private bool isObtained = false;

    public void AddNewSkillToPlayer()
    {
        if (isObtained) return;

        isObtained = true;
        InGameSkillsHandler.instance.AddNewSkill(m_obtainableSkill);
    }
}
