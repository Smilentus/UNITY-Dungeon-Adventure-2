public class RuntimeMagicObject_SmallFireball : BattleMagicExecuter_BaseActionExecuter
{
    private float damage = 5f;


    public override void ExecuteAction()
    {
        base.ExecuteAction();

        BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Health -= damage;
        GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - Вы нанесли урон магией '" + magicProfile.InteractionTitle + "' " + damage + " ед.");
    }
}
