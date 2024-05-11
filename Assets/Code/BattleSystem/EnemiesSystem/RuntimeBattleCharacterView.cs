using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using UniRx;
using UnityEngine;

namespace Dimasyechka.Code.BattleSystem.EnemiesSystem
{
    public class RuntimeBattleCharacterView : MonoViewModel<CharacterDrawerData>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<Texture> CharacterTexture = new ReactiveProperty<Texture>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> CharacterName = new ReactiveProperty<string>();


        [RxAdaptableProperty]
        public ReactiveProperty<double> CharacterHealth = new ReactiveProperty<double>();

        [RxAdaptableProperty]
        public ReactiveProperty<double> CharacterMaxHealth = new ReactiveProperty<double>();


        [RxAdaptableProperty]
        public ReactiveProperty<float> CharacterHealthRatio = new ReactiveProperty<float>();


        protected override void OnSetupModel()
        {
            CharacterTexture.Value = Model.RuntimeBattleCharacter.CharacterProfile.CharacterImage;
            CharacterName.Value = Model.RuntimeBattleCharacter.CharacterProfile.Name;
            CharacterHealthRatio.Value = (float)Model.RuntimeBattleCharacter.Health / (float)Model.RuntimeBattleCharacter.MaxHealth;

            CharacterHealth.Value = Model.RuntimeBattleCharacter.Health;
            CharacterMaxHealth.Value = Model.RuntimeBattleCharacter.MaxHealth;
        }
    }

    public class CharacterDrawerData
    {
        public RuntimeBattleCharacter RuntimeBattleCharacter;
    }
}