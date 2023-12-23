using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSkillsLibrary : MonoBehaviour
{
    private static AllSkillsLibrary m_instance;
    public static AllSkillsLibrary instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<AllSkillsLibrary>();
            }

            return m_instance;
        }
    }


    [SerializeField] private List<SkillProfile> m_allInGameSkills;
    /// <summary>
    ///     ¬озвращает список всех доступных навыков в игре
    /// </summary>
    public List<SkillProfile> allInGameSkills { get => m_allInGameSkills; }

    
    
}
