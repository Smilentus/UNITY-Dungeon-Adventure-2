using Dimasyechka.Code.SkillsSystem.Core;
using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Dimasyechka.Code.SkillsSystem.Views
{
    public class ObtainSkillDataView : MonoViewModel<ObtainSkillData>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<Sprite> SkillIcon = new ReactiveProperty<Sprite>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> SkillTitle = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<int> SkillLevel = new ReactiveProperty<int>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> SkillDescription = new ReactiveProperty<string>();


        [RxAdaptableProperty]
        public ReactiveProperty<bool> IsSkillEnabled = new ReactiveProperty<bool>();



        protected override void OnSetupModel()
        {
            SetData();
        }


        public void SetData()
        {
            if (Model.SkillLevelData == null)
            {
                IsSkillEnabled.Value = false;
            }
            else
            {
                IsSkillEnabled.Value = true;

                SkillLevel.Value = Model.SkillLevel;
                SkillIcon.Value = Model.SkillLevelData.skillLevelIcon;
                SkillTitle.Value = Model.SkillLevelData.skillLevelTitle;

                string skillDescription = Model.SkillLevelData.skillLevelDescription;

                if (Model.DeltaValues != null)
                {
                    skillDescription += "\n\n";

                    foreach (string deltaValue in Model.DeltaValues)
                    {
                        skillDescription += $"{deltaValue}\n";
                    }
                }

                SkillDescription.Value = skillDescription;
            }
        }
    }

    [System.Serializable]
    public class ObtainSkillData
    {
        public SkillLevelData SkillLevelData;
        public int SkillLevel;
        public List<string> DeltaValues = null;
    }
}