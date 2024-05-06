using System;
using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Core;
using UnityEngine;
using Zenject;

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
        private Transform _skillsParent;


        [SerializeField]
        private SkillsWarehouse _skillsWarehouse;


        private List<PlayerSkill> _playerSkills = new List<PlayerSkill>();


        private RuntimePlayer _runtimePlayer;

        [Inject]
        public void Construct(RuntimePlayer runtimePlayer)
        {
            _runtimePlayer = runtimePlayer;
        }


        public PlayerSkill[] GetPlayerSkills()
        {
            return _playerSkills.ToArray();
        }

        public PlayerSkill GetPlayerSkillByGuid(string skillGUID)
        {
            return _playerSkills.Find(x => x.SkillProfile.skillGUID == skillGUID);
        }

        public int GetPlayerSkillLevelByGuid(string skillGUID)
        {
            PlayerSkill skill = GetPlayerSkillByGuid(skillGUID);

            if (skill != null)
            {
                return skill.RuntimeSkillCore.UpgradeableComponent.currentLevel;
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
            PlayerSkill playerSkill = _playerSkills.Find(x => x.SkillProfile == skillProfile);
            return playerSkill != null;
        }

        public bool IsPlayerHaveEnoughSkillPoints(int upgradeCost)
        {
            return upgradeCost <= _runtimePlayer.RuntimePlayerStats.SkillPoints;
        }


        public void LoadSkill(string skillGUID, int obtainedLevel)
        {
            SkillProfile skillProfile = _skillsWarehouse.GetSkillProfileByGUID(skillGUID);

            if (skillProfile != null)
            {
                PlayerSkill playerSkill = AddNewSkill(skillProfile);

                if (playerSkill != null)
                {
                    playerSkill.RuntimeSkillCore.LoadLevel(obtainedLevel);
                }
            }
            else
            {
                Debug.LogError($"Не был найден навык {skillGUID} в общем хранилище");
            }
        }

        public PlayerSkill AddNewSkill(SkillProfile skillProfile)
        {
            PlayerSkill contains = _playerSkills.Find(x => x.SkillProfile == skillProfile);
            if (contains != null)
            {
                Debug.LogWarning($"Данный навык уже добавлен игроку!");
                return null;
            }


            SkillCore inGameSkillObject = Instantiate(skillProfile.skillCorePrefab, _skillsParent);

            PlayerSkill playerSkill = new PlayerSkill()
            {
                SkillProfile = skillProfile,
                RuntimeSkillCore = inGameSkillObject
            };


            _playerSkills.Add(playerSkill);
            onSkillObtained?.Invoke(playerSkill);


            TryUpgradeSkill(skillProfile);


            return playerSkill;
        }

        public bool TryUpgradeSkill(SkillProfile skillProfile)
        {
            PlayerSkill playerSkill = _playerSkills.Find(x => x.SkillProfile == skillProfile);

            if (playerSkill != null)
            {
                bool isUpgraded = playerSkill.RuntimeSkillCore.TryUpgradeSkill();

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
        public SkillProfile SkillProfile;
        public SkillCore RuntimeSkillCore;
    }
}