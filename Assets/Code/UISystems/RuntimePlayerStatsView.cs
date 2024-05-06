using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.LocationSystem.Controllers;
using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using UniRx;
using Zenject;

namespace Dimasyechka.Code.UISystems
{
    // TODO: Весь класс переделать, какой-то кринж накопленный за 2 года ...
    public class RuntimePlayerStatsView : MonoViewModel<RuntimePlayer>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<string> CurrentDateTime = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerHealth = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerMana = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerLevel = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerHealthRegen = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerManaRegen = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerDamage = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerAttackSpeed = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerArmor = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerElemental = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerGold = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerExperience = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerExtraExperience = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerExtraMoney = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> CurrentLocation = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> DodgeChance = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerLuck = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerCriticalChance = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerCriticalDamage = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> PlayerSkillPoints = new ReactiveProperty<string>();


        private GameTimeFlowController _gameTimeFlowController;
        private LocationsController _locationsController;


        [Inject]
        public void Construct(GameTimeFlowController gameTimeFlowController, LocationsController locationsController)
        {
            _gameTimeFlowController = gameTimeFlowController;
            _locationsController = locationsController;
        }


        protected override void OnSetupModel()
        {
            // ...    
        }


        // TODO: Переделать модель на реактивки + добавить туда сериализацию с реактивок
        private void Update()
        {
            UpdateTexts();
            UpdateSliders();
        }



        private void UpdateTexts()
        {
            PlayerSkillPoints.Value = Model.RuntimePlayerStats.SkillPoints.ToString();
            CurrentDateTime.Value = $"{_gameTimeFlowController.DateNow()}\n{_gameTimeFlowController.DayStatusNow()}";

            PlayerHealth.Value = $"{Model.RuntimePlayerStats.Health.ToString("f2")}/{Model.RuntimePlayerStats.MaxHealth.ToString("f2")} ОЗ";
            PlayerHealthRegen.Value = $"Реген. {Model.RuntimePlayerStats.HealthRegen.ToString("f2")} ОЗ";

            PlayerLevel.Value = $"Урв. {Model.RuntimePlayerStats.Lvl}";

            PlayerMana.Value = $"{Model.RuntimePlayerStats.Mana.ToString("f2")}/{Model.RuntimePlayerStats.MaxMana.ToString("f2")} ОМ";
            PlayerManaRegen.Value = $"Реген. {Model.RuntimePlayerStats.ManaRegen.ToString("f2")} ОМ";

            PlayerDamage.Value = $"Урон {Model.RuntimePlayerStats.Damage.ToString("f2")} ед.";
            PlayerAttackSpeed.Value = $"Скорость {Model.RuntimePlayerStats.AttackSpeed.ToString("f2")} ед.";
            PlayerArmor.Value = $"Защита {Model.RuntimePlayerStats.Armor.ToString("f2")} ед.";

            PlayerElemental.Value = $"{Model.RuntimePlayerStats.elementStr}";

            PlayerGold.Value = $"Золото {Model.RuntimePlayerStats.Money.ToString("f0")} ед.";
            PlayerExtraExperience.Value = $"Доп. опыт {(Model.RuntimePlayerStats.ExtraExpMultiplier * 100).ToString("f2")}%";
            PlayerExtraMoney.Value = $"Доп. золото {(Model.RuntimePlayerStats.ExtraMoneyMultiplier * 100).ToString("f2")}%";

            CurrentLocation.Value = $"Локация {(_locationsController.CurrentLocation == null ? "Неизвестно" : _locationsController.CurrentLocation.LocationTitle)}";

            DodgeChance.Value = $"Уклонение {Model.RuntimePlayerStats.DodgeChance.ToString("f2")}%";
            PlayerLuck.Value = $"Удача {Model.RuntimePlayerStats.Luck.ToString("f2")}%";

            PlayerCriticalChance.Value = $"Крит. шанс {Model.RuntimePlayerStats.CriticalStrikeChance.ToString("f2")}%";
            PlayerCriticalDamage.Value = $"Крит. урон {(Model.RuntimePlayerStats.CriticalStrikeDamageMultiplier * 100).ToString("f2")}%";
        }

        private void UpdateSliders()
        {
            //m_playerHealthSlider.maxValue = (float)Model.RuntimePlayerStats.MaxHealth;
            //m_playerHealthSlider.minValue = 0;
            //m_playerHealthSlider.value = (float)Model.RuntimePlayerStats.Health;

            //m_playerLevelSlider.maxValue = (float)Model.RuntimePlayerStats.MaxExp;
            //m_playerLevelSlider.minValue = 0;
            //m_playerLevelSlider.value = (float)Model.RuntimePlayerStats.Exp;

            //m_playerManaSlider.maxValue = (float)Model.RuntimePlayerStats.MaxMana;
            //m_playerManaSlider.minValue = 0;
            //m_playerManaSlider.value = (float)Model.RuntimePlayerStats.Mana;
        }
    }
}
