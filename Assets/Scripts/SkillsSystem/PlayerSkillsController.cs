using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSkillsController : MonoBehaviour
{
    private static PlayerSkillsController m_instance;
    public static PlayerSkillsController instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PlayerSkillsController>();
            }

            return m_instance;
        }
    }


    public event Action onSkillObtained;


    [SerializeField]
    private Transform m_skillsParent;


    private List<PlayerSkill> playerSkills = new List<PlayerSkill>();


    public SkillProfile healthSkill;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            RuntimePlayer.Instance.RuntimePlayerStats.SkillPoints += 100;
            TryObtainPlayerSkill(healthSkill);
        }
    }


    /// <summary>
    ///     Здесь проверяем имеется ли данный навык у игрока
    ///     Если имеется - уходим глубже и пытаемся его улучшить
    ///     Если навыка нет - добавляем его рантайм игроку
    /// </summary>
    /// <param name="skillProfile"></param>
    /// <returns></returns>
    public bool TryObtainPlayerSkill(SkillProfile skillProfile)
    {
        if (IsPlayerHaveSkill(skillProfile))
        {
            return TryUpgradeSkill(skillProfile);
        }
        else
        {
            AddNewSkill(skillProfile);
            return true;
        }
    }


    public bool IsPlayerHaveSkill(SkillProfile skillProfile)
    {
        PlayerSkill playerSkill = playerSkills.Find(x => x.skillProfile == skillProfile);
        return playerSkill != null;
    }

    public bool IsPlayerHaveEnoughSkillPoints(int upgradeCost)
    {
        return upgradeCost <= RuntimePlayer.Instance.RuntimePlayerStats.SkillPoints;
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

        PlayerSkill playerSkill = new PlayerSkill()
        {
            skillProfile = skillProfile,
            runtimeSkillCore = inGameSkillObject
        };

        playerSkills.Add(playerSkill);

        onSkillObtained?.Invoke();
    }

    public bool TryUpgradeSkill(SkillProfile skillProfile)
    {
        PlayerSkill playerSkill = playerSkills.Find(x => x.skillProfile == skillProfile);

        if (playerSkill != null)
        {
            return playerSkill.runtimeSkillCore.TryUpgradeSkill();
        }
        else
        {
            return false;
        }
    }
}

public class PlayerSkill
{
    public SkillProfile skillProfile;
    public SkillCore runtimeSkillCore;
}