using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using UniRx;
using UnityEngine;

namespace Dimasyechka.Code.BuffSystem.Views
{
    public class RuntimeBuffView : MonoViewModel<RuntimeBuff>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<string> BuffTitle = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<Sprite> BuffIcon = new ReactiveProperty<Sprite>();

        [RxAdaptableProperty]
        public ReactiveProperty<int> BuffDuration = new ReactiveProperty<int>();

        [RxAdaptableProperty]
        public ReactiveProperty<int> MaxBuffDuration = new ReactiveProperty<int>();


        protected override void OnSetupModel()
        {
            BuffTitle.Value = Model.BuffProfile.BuffName;
            BuffIcon.Value = Model.BuffProfile.BuffIcon;
            MaxBuffDuration.Value = Model.BuffProfile.MaxDuration;
            
            BuffDuration.Value = Model.DurationHours;
        }
    }
}
