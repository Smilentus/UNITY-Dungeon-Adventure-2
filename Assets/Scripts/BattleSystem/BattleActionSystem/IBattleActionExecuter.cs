using UnityEngine;

public interface IBattleActionExecuter
{
    public void Initialize();

    public void EveryTurnCheck(BattleController.TurnStatus turnStatus);

    public void ExecuteAction();
}
