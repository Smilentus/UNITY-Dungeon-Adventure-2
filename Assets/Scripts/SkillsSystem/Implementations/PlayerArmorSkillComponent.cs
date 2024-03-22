using System.Collections.Generic;

public class PlayerArmorSkillComponent : BaseSkillComponent
{
    public override List<string> GetDeltaValues(int level)
    {
        return new List<string>() 
        {
            "Защита +1 ед."
        };
    }

    public override void OnUpgraded(int level)
    {
        double delta = 1f;

        RuntimePlayer.Instance.RuntimePlayerStats.Armor += delta;
    }
}
