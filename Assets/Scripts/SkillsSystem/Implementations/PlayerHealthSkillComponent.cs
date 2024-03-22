using System.Collections.Generic;

public class PlayerHealthSkillComponent : BaseSkillComponent
{
    public override List<string> GetDeltaValues(int level)
    {
        return new List<string>()
        {
            $"Макс. здоровье +{level * 10} ед.",
            $"Здоровье +{level * 10} ед."
        };
    }

    public override void OnUpgraded(int level)
    {
        double delta = skillCore.UpgradeableComponent.currentLevel * 10;
        RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth += delta;
        RuntimePlayer.Instance.RuntimePlayerStats.Health += delta;
    }
}