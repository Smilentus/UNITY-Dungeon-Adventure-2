using Dimasyechka.Code._LEGACY_.BuffSystem;
using Dimasyechka.Code.BuffSystem.Profiles;
using Dimasyechka.Code.GameTimeFlowSystem.Controllers;

namespace Dimasyechka.Code.GameTimeFlowSystem.GTFEvents
{
    public class GameTimeFlowEventFullMoon : GameTimeFlowEventBase
    {
        public override bool CanStartEvent()
        {
            return GameTimeFlowController.Instance.CurrentMonth == 10 && GameTimeFlowController.Instance.CurrentDay < 15;
        }

        public override bool CanFinishEvent()
        {
            return !CanStartEvent();
        }

        public override void StartEvent()
        {
            GameController.Instance.ShowMessageText(GameTimeFlowEventReference.EventDescription, "[Событие]");
            FindObjectOfType<BuffManager>().SetBuff(BuffProfile.BuffType.FullmoonBuff);

            base.StartEvent();
        }

        public override void FinishEvent()
        {
            FindObjectOfType<BuffManager>().DeleteBuffAction(BuffProfile.BuffType.FullmoonBuff);

            base.FinishEvent();
        }
    }
}
