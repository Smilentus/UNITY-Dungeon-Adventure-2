using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionPointsBattleInteractionView : BaseBattleInteractionView
{
    [SerializeField]
    protected TMP_Text m_actionPointsCostTMP;


    public override void SetData(AvailableBattleActionData availableBattleActionData, int index)
    {
        base.SetData(availableBattleActionData, index);

        BattleActionProfile profile = availableBattleActionData.interaction as BattleActionProfile;

        m_actionPointsCostTMP.text = $"{profile.SpendableActions} нд";
    }
}