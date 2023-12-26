public class PlayerHealthSkillComponent : BaseSkillComponent
{
    public override void OnUpgraded(int level)
    {
        double delta = skillCore.UpgradeableComponent.currentLevel * 10;
        RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth += delta;
        RuntimePlayer.Instance.RuntimePlayerStats.Health += delta;
    }
}