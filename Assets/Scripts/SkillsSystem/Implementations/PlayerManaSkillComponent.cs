public class PlayerManaSkillComponent : BaseSkillComponent
{
    public override void OnUpgraded(int level)
    {
        double delta = skillCore.UpgradeableComponent.currentLevel * 5;

        RuntimePlayer.Instance.RuntimePlayerStats.MaxMana += delta;
        RuntimePlayer.Instance.RuntimePlayerStats.Mana += delta;
    }
}