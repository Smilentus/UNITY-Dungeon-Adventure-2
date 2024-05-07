using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.LocationSystem.Controllers;
using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using Dimasyechka.Lubribrary.RxMV.UniRx.Extensions;
using UniRx;
using Zenject;

namespace Dimasyechka.Code.UISystems
{
    // TODO: Весь класс переделать, какой-то кринж накопленный за 2 года ...
    public class RuntimePlayerStatsView : MonoViewModel<RuntimePlayer>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<string> CurrentLocation = new ReactiveProperty<string>();


        [RxAdaptableProperty]
        public ReactiveProperty<float> PlayerHealthRatio = new ReactiveProperty<float>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<float> PlayerManaRatio = new ReactiveProperty<float>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<float> PlayerExpRatio = new ReactiveProperty<float>(-1);


        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerHealth = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerMaxHealth = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerHealthRegen = new ReactiveProperty<double>(-1);


        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerMana = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerMaxMana = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerManaRegen = new ReactiveProperty<double>(-1);


        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerLevel = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerExp = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerMaxExp = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerExtraExp = new ReactiveProperty<double>(-1);


        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerDamage = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerAttackSpeed = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerArmor = new ReactiveProperty<double>(-1);


        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerMoney = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerExtraMoney = new ReactiveProperty<double>(-1);


        [RxAdaptableProperty]
        public ReactiveProperty<double> DodgeChance = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerLuck = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerCriticalChance = new ReactiveProperty<double>(-1);

        [RxAdaptableProperty]
        public ReactiveProperty<double> PlayerCriticalDamage = new ReactiveProperty<double>(-1);


        [Inject]
        public void Construct(RuntimePlayer runtimePlayer)
        {
            ZenjectModel(runtimePlayer);
        }


        protected override void OnSetupModel()
        {
            SubscribeHealthValues();
            SubscribeManaValues();
            SubscribeExpValues();

            SubscribeOffensiveValues();

            SubscribeMiscValues();

            SubscribeChancesValues();
        }

        private void SubscribeChancesValues()
        {
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.DodgeChance.SubscribeToEachOther(DodgeChance));
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.CriticalStrikeChance.SubscribeToEachOther(PlayerCriticalChance));
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.CriticalStrikeDamageMultiplier.SubscribeToEachOther(PlayerCriticalDamage));
        }

        private void SubscribeMiscValues()
        {
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.Money.SubscribeToEachOther(PlayerMoney));
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.ExtraMoneyMultiplier.SubscribeToEachOther(PlayerExtraMoney));
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.Luck.SubscribeToEachOther(PlayerLuck));
        }

        private void SubscribeOffensiveValues()
        {
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.Damage.SubscribeToEachOther(PlayerDamage));
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.Armor.SubscribeToEachOther(PlayerArmor));
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.AttackSpeed.SubscribeToEachOther(PlayerAttackSpeed));
        }

        private void SubscribeExpValues()
        {
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.MaxExp.SubscribeToEachOther(PlayerExp));
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.Exp.Subscribe(x =>
            {
                PlayerExp.Value = x;
                PlayerExpRatio.Value = (float)Model.RuntimePlayerStats.ExpRatio;
            }));
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.ExtraExpMultiplier.SubscribeToEachOther(PlayerExtraExp));
        }

        private void SubscribeManaValues()
        {
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.MaxMana.SubscribeToEachOther(PlayerMaxMana));
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.ManaRegen.SubscribeToEachOther(PlayerManaRegen));
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.Mana.Subscribe(x =>
            {
                PlayerMana.Value = x;
                PlayerManaRatio.Value = (float)Model.RuntimePlayerStats.ManaRatio;
            }));
        }

        private void SubscribeHealthValues()
        {
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.MaxHealth.SubscribeToEachOther(PlayerMaxHealth));
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.HealthRegen.SubscribeToEachOther(PlayerHealthRegen));
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.Health.Subscribe(x =>
            {
                PlayerHealth.Value = x;
                PlayerHealthRatio.Value = (float)Model.RuntimePlayerStats.HealthRatio;
            }));
        }
    }
}
