using Dimasyechka.Code.SkillsSystem.Core;
using Dimasyechka.Code.ZenjectFactories;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.SkillsSystem.Controllers
{
    public class PlayerSkillsController : MonoBehaviour
    {
        public event Action<PlayerSkill> onSkillObtained;
        public event Action<PlayerSkill> onSkillUpgraded;


        [SerializeField]
        private Transform _skillsParent;


        [SerializeField]
        private SkillsWarehouse _skillsWarehouse;


        private List<PlayerSkill> _runtimePlayerSkills = new List<PlayerSkill>();

        private SkillCore _temporalUpgradeable;


        private RuntimePlayer _runtimePlayer;
        private SkillCoreFactory _skillCoreFactory;

        [Inject]
        public void Construct(RuntimePlayer runtimePlayer, SkillCoreFactory skillCoreFactory)
        {
            _runtimePlayer = runtimePlayer;
            _skillCoreFactory = skillCoreFactory;
        }


        public PlayerSkill[] GetPlayerSkills()
        {
            return _runtimePlayerSkills.ToArray();
        }

        public PlayerSkill GetPlayerSkillByGuid(string skillGUID)
        {
            return _runtimePlayerSkills.Find(x => x.SkillProfile.skillGUID == skillGUID);
        }


        public SkillCore GetUpgradeableSkillCore(SkillProfile skillProfile)
        {
            if (IsPlayerHaveSkill(skillProfile))
            {
                PlayerSkill runtimeObtainedSkill = GetPlayerSkillByGuid(skillProfile.skillGUID);
                if (runtimeObtainedSkill != null)
                {
                    return runtimeObtainedSkill.RuntimeSkillCore;
                }
                else
                {
                    return CreateTemporalUpgradeable(skillProfile);
                }
            }
            else
            {
                return CreateTemporalUpgradeable(skillProfile);
            }
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
                PlayerSkill playerSkill = AddNewSkill(skillProfile);

                return playerSkill != null;
            }
        }


        // Костыль, хочется систему переделать
        public SkillCore CreateTemporalUpgradeable(SkillProfile profile)
        {
            SkillCore skillCore = _skillCoreFactory.InstantiateForComponent(profile.skillCorePrefab.gameObject, null);

            skillCore.name += "_TemporalUpgradeable";
            _temporalUpgradeable = skillCore;

            return skillCore;
        }

        // Второй костыль, ноги же две у нас
        public void DestroyTemporalUpgradeable()
        {
            Destroy(_temporalUpgradeable.gameObject);
        }


        public bool IsPlayerHaveSkill(SkillProfile skillProfile)
        {
            PlayerSkill playerSkill = _runtimePlayerSkills.Find(x => x.SkillProfile == skillProfile);
            return playerSkill != null;
        }

        public bool IsPlayerHaveEnoughSkillPoints(int upgradeCost)
        {
            return upgradeCost <= _runtimePlayer.RuntimePlayerStats.SkillPoints.Value;
        }


        public void LoadSkill(string skillGuid, int obtainedLevel)
        {
            SkillProfile skillProfile = _skillsWarehouse.GetSkillProfileByGuid(skillGuid);

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
                Debug.LogError($"Не был найден навык {skillGuid} в общем хранилище");
            }
        }

        public PlayerSkill AddNewSkill(SkillProfile skillProfile)
        {
            PlayerSkill contains = _runtimePlayerSkills.Find(x => x.SkillProfile == skillProfile);
            if (contains != null)
            {
                Debug.LogWarning($"Данный навык уже добавлен игроку!");
                return null;
            }


            SkillCore inGameSkillObject = _skillCoreFactory.InstantiateForComponent(
                skillProfile.skillCorePrefab.gameObject,
                _skillsParent);

            PlayerSkill playerSkill = new PlayerSkill()
            {
                SkillProfile = skillProfile,
                RuntimeSkillCore = inGameSkillObject
            };

            _runtimePlayerSkills.Add(playerSkill);
            onSkillObtained?.Invoke(playerSkill);

            TryUpgradeSkill(skillProfile);

            return playerSkill;
        }

        public bool TryUpgradeSkill(SkillProfile skillProfile)
        {
            PlayerSkill playerSkill = _runtimePlayerSkills.Find(x => x.SkillProfile == skillProfile);

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

    public class SkillCoreFactory : DiContainerCreationFactory<SkillCore> { }

    public class PlayerSkill
    {
        public SkillProfile SkillProfile;
        public SkillCore RuntimeSkillCore;
    }
}