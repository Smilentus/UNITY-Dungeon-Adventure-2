using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using Dimasyechka.Lubribrary.RxMV.UniRx.Extensions;
using UniRx;
using Zenject;

namespace Dimasyechka.Code.SkillsSystem.Views
{
    public class SkillPointsView : MonoViewModel<RuntimePlayer>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<int> SkillPoints = new ReactiveProperty<int>();


        [Inject]
        public void Construct(RuntimePlayer runtimePlayer)
        {
            ZenjectModel(runtimePlayer);
        }


        protected override void OnSetupModel()
        {
            _disposablesStorage.AddToDisposables(Model.RuntimePlayerStats.SkillPoints.SubscribeToEachOther(SkillPoints));
        }
    }
}
