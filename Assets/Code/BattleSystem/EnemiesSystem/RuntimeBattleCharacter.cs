using System;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.BattleSystem.EnemiesSystem
{
    /// <summary>
    ///     Основной класс, который содержит информацию и прочее о игровом персонаже противника, с которым мы сражаемся
    /// </summary>
    public class RuntimeBattleCharacter : MonoBehaviour
    {
        public event Action onCharacterDeath;
        public event Action onCharacterActionsCompleted;


        [SerializeField]
        protected RuntimeBattleCharacterView _characterView;


        [HideInInspector()]
        public CharacterProfile CharacterProfile;


        // Основные переменные персонажа
        [HideInInspector()]
        public double MaxHealth, Health, MaxMana, Mana, HealthRegen, ManaRegen;

        [HideInInspector()]
        public double Armor, AttackSpeed, Damage, DodgeChance;

        public int ActionPoints;


        protected RuntimePlayer _runtimePlayer;

        [Inject]
        public void Construct(RuntimePlayer runtimePlayer)
        {
            _runtimePlayer = runtimePlayer;
        }


        public virtual void CreateBattleCharacter(CharacterProfile profile)
        {
            CharacterProfile = profile;

            MaxHealth = Health = profile.MaxHealth;
            MaxMana = Mana = profile.MaxMana;
            HealthRegen = profile.HealthRegen;
            ManaRegen = profile.ManaRegen;

            Armor = profile.Armor;
            AttackSpeed = profile.AttackSpeed;
            Damage = profile.Damage;
            DodgeChance = profile.DodgeChance;

            ActionPoints = profile.DefaultActionPoints;

            _characterView.SetupModel(new CharacterDrawerData()
            {
                RuntimeBattleCharacter = this
            });

            UpdateCharacterView();
        }

        public virtual void ProcessCharacterActions()
        {
            onCharacterActionsCompleted?.Invoke();

            UpdateCharacterView();
        }

        public virtual void UpdateCharacterView()
        {
            // TODO: На реактивках будет, это уберём
            _characterView.SetupModel(new CharacterDrawerData()
            {
                RuntimeBattleCharacter = this
            });
        }
    }
}
