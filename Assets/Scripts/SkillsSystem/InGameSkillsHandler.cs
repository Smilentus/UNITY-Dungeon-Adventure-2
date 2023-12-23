using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InGameSkillsHandler : MonoBehaviour
{
    private static InGameSkillsHandler m_instance;
    public static InGameSkillsHandler instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<InGameSkillsHandler>();
            }

            return m_instance;
        }
    }

    [SerializeField] private Transform m_skillsParent;

    [SerializeField] public UnityEvent OnLevelGained = new UnityEvent();
    [SerializeField] public UnityEvent OnExperienceChanged = new UnityEvent();


    private List<PlayerSkill> playerSkills = new List<PlayerSkill>();


    private int m_currentLevel;
    public int currentLevel { get => m_currentLevel; }


    private int m_experience;
    public int experience { get => m_experience; }
    public float experienceNormalized => Mathf.Clamp01((float)m_experience / (float)m_experienceToUpgrade);


    private int m_experienceToUpgrade;
    public int experienceToUpgrade { get => m_experienceToUpgrade; }


    private void Start()
    {
        m_experience = 0;
        m_currentLevel = 0;
        m_experienceToUpgrade = 5;
    }


    public void AddExperience(int experience)
    {
        m_experience += experience;

        OnExperienceChanged?.Invoke();

        CheckNextLevel();
    }

    public void CheckNextLevel()
    {
        if (m_experience >= m_experienceToUpgrade)
        {
            m_experience -= m_experienceToUpgrade;

            if (m_experience < 0)
            {
                m_experience = 0;
            }

            OnExperienceChanged?.Invoke();

            m_currentLevel++;

            m_experienceToUpgrade += 5;
            
            OnLevelGained?.Invoke();
        }
    }


    public void AddNewSkill(SkillProfile skillProfile)
    {
        PlayerSkill contains = playerSkills.Find(x => x.skillProfile == skillProfile);
        if (contains != null)
        {
            Debug.LogWarning($"Данный навык уже добавлен игроку!");
            return;
        }

        SkillCore inGameSkillObject = Instantiate(skillProfile.skillCorePrefab, m_skillsParent);

        PlayerSkill playerSkill = new PlayerSkill() {
            skillProfile = skillProfile,
            inGameCoreObject = inGameSkillObject
        };
        
        playerSkills.Add(playerSkill);
    }
    public void RemoveSkill(PlayerSkill playerSkill)
    {
        playerSkills.Remove(playerSkill);
    }
}

public class PlayerSkill
{
    public SkillProfile skillProfile;
    public SkillCore inGameCoreObject;
}