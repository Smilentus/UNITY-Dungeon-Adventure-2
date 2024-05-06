using System;
using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Core;
using UnityEngine;

namespace Dimasyechka.Code.SkillsSystem.Controllers
{
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


        public event Action<PlayerSkill> onSkillObtained;
        public event Action<PlayerSkill> onSkillUpgraded;


        [SerializeField]
        private Transform m_skillsParent;


        [SerializeField]
        private SkillsWarehouse m_skillsWarehouse;


        private List<PlayerSkill> playerSkills = new List<PlayerSkill>();


        public PlayerSkill[] GetPlayerSkills()
        {
            return playerSkills.ToArray();
        }

        public PlayerSkill GetPlayerSkillByGUID(string skillGUID)
        {
            return playerSkills.Find(x => x.skillProfile.skillGUID == skillGUID);
        }

        public int GetPlayerSkillLevelByGUID(string skillGUID)
        {
            PlayerSkill skill = GetPlayerSkillByGUID(skillGUID);

            if (skill != null)
            {
                return skill.runtimeSkillCore.UpgradeableComponent.currentLevel;
            }

            return -1;
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
                if (skillProfile.skillCorePrefab.UpgradeableComponent.CanUpgradeLevel(1))
                {
                    PlayerSkill playerSkill = AddNewSkill(skillProfile);

                    return playerSkill != null;
                }
                else
                {
                    return false;
                }
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


        public void LoadSkill(string skillGUID, int obtainedLevel)
        {
            SkillProfile skillProfile = m_skillsWarehouse.GetSkillProfileByGUID(skillGUID);

            if (skillProfile != null)
            {
                PlayerSkill playerSkill = AddNewSkill(skillProfile);

                if (playerSkill != null)
                {
                    playerSkill.runtimeSkillCore.LoadLevel(obtainedLevel);
                }
            }
            else
            {
                Debug.LogError($"Не был найден навык {skillGUID} в общем хранилище");
            }
        }

        public PlayerSkill AddNewSkill(SkillProfile skillProfile)
        {
            PlayerSkill contains = playerSkills.Find(x => x.skillProfile == skillProfile);
            if (contains != null)
            {
                Debug.LogWarning($"Данный навык уже добавлен игроку!");
                return null;
            }


            SkillCore inGameSkillObject = Instantiate(skillProfile.skillCorePrefab, m_skillsParent);

            PlayerSkill playerSkill = new PlayerSkill()
            {
                skillProfile = skillProfile,
                runtimeSkillCore = inGameSkillObject
            };


            playerSkills.Add(playerSkill);
            onSkillObtained?.Invoke(playerSkill);


            TryUpgradeSkill(skillProfile);


            return playerSkill;
        }

        public bool TryUpgradeSkill(SkillProfile skillProfile)
        {
            PlayerSkill playerSkill = playerSkills.Find(x => x.skillProfile == skillProfile);

            if (playerSkill != null)
            {
                bool isUpgraded = playerSkill.runtimeSkillCore.TryUpgradeSkill();

                if (isUpgraded)
                {
                    onSkillUpgraded?.Invoke(playerSkill);
                }

                return isUpgraded;
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
}