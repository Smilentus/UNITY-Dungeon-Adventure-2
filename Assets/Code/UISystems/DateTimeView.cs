using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using UniRx;
using UnityEngine.PlayerLoop;
using Zenject;

namespace Dimasyechka.Code.UISystems
{
    public class DateTimeView : MonoViewModel<GameTimeFlowController>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<string> CurrentDateTime = new ReactiveProperty<string>();

        [Inject]
        public void Construct(GameTimeFlowController gameTimeFlowController)
        {
            ZenjectModel(gameTimeFlowController);
        }

        protected override void OnSetupModel()
        {
            SubscribeEvents();
            UpdateView();
        }

        protected override void OnRemoveModel()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            Model.onTimeHoursPassed += OnHoursPassed;
            Model.onTimeDaysPassed += OnDaysPassed;
            Model.onTimeMonthsPassed += OnMonthsPassed;
            Model.onTimeYearsPassed += OnYearsPassed;
        }

        private void UnsubscribeEvents()
        {
            Model.onTimeHoursPassed -= OnHoursPassed;
            Model.onTimeDaysPassed -= OnDaysPassed;
            Model.onTimeMonthsPassed -= OnMonthsPassed;
            Model.onTimeYearsPassed -= OnYearsPassed;
        }

        private void OnYearsPassed(int passedYears)
        {
            UpdateView();
        }

        private void OnMonthsPassed(int passedMonths)
        {
            UpdateView();
        }

        private void OnDaysPassed(int passedDays)
        {
            UpdateView();
        }

        private void OnHoursPassed(int passedHours)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            CurrentDateTime.Value = $"{Model.DayStatusNow()}\n{Model.DateNow()}";
        }
    }
}
