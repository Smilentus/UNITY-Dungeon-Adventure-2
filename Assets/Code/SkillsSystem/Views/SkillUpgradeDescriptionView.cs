using Dimasyechka.Code.UpgradeableSystem;
using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using UniRx;
using UnityEngine;

namespace Dimasyechka.Code.SkillsSystem.Views
{
    public class SkillUpgradeDescriptionView : MonoViewModel<SkillUpgradeDescriptionData>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<string> Description = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<Color> ActiveColor = new ReactiveProperty<Color>();


        [SerializeField]
        private Color _notEnoughColor;

        [SerializeField]
        private Color _accomplishedColor;


        protected override void OnSetupModel()
        {
            Description.Value = Model.Description;
            ActiveColor.Value = Model.IsAccomplished ? _accomplishedColor : _notEnoughColor;
        }
    }
}
