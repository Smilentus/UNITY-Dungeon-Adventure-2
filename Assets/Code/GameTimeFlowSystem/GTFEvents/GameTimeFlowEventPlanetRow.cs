namespace Dimasyechka.Code.GameTimeFlowSystem.GTFEvents
{
    public class GameTimeFlowEventPlanetRow : GameTimeFlowEventBase
    {
        public override bool CanStartEvent()
        {
            return (_gameTimeFlowController.CurrentYear % 10 == 0 && _gameTimeFlowController.CurrentMonth == 1 && _gameTimeFlowController.CurrentDay < 15);
        }

        public override bool CanFinishEvent()
        {
            return !CanStartEvent();
        }

        public override void StartEvent()
        {
            GameController.Instance.ShowMessageText(GameTimeFlowEventReference.EventDescription, "[Событие]");
            //FindObjectOfType<BuffManager>().SetBuff(BuffProfile.BuffType.PlanetRowBuff);

            base.StartEvent();
        }

        public override void FinishEvent()
        {
            //FindObjectOfType<BuffManager>().DeleteBuffAction(BuffProfile.BuffType.PlanetRowBuff);

            base.FinishEvent();
        }
    }
}
