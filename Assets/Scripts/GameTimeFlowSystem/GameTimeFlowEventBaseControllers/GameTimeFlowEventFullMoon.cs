using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeFlowEventFullMoon : GameTimeFlowEventBaseController
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
        GameHelper._GH.ShowMessageText(GameTimeFlowEventReference.EventDescription, 0);
        FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.FullmoonBuff);

        base.StartEvent();
    }

    public override void FinishEvent()
    {
        FindObjectOfType<BuffManager>().DeleteBuffAction(Buff.BuffType.FullmoonBuff);

        base.FinishEvent();
    }
}
