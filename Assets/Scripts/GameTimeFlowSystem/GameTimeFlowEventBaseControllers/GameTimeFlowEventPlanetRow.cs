using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeFlowEventPlanetRow : GameTimeFlowEventBaseController
{
    public override bool CanStartEvent()
    {
        return (GameTimeFlowController.Instance.CurrentYear % 10 == 0 && GameTimeFlowController.Instance.CurrentMonth == 1 && GameTimeFlowController.Instance.CurrentDay < 15);
    }

    public override bool CanFinishEvent()
    {
        return !CanStartEvent();
    }

    public override void StartEvent()
    {
        GameController.Instance.ShowMessageText(GameTimeFlowEventReference.EventDescription, "[Событие]");
        FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.PlanetRowBuff);

        base.StartEvent();
    }

    public override void FinishEvent()
    {
        FindObjectOfType<BuffManager>().DeleteBuffAction(Buff.BuffType.PlanetRowBuff);

        base.FinishEvent();
    }
}
