using System.Collections.Generic;

public class PlayerArmorSkillComponent : BaseSkillComponent
{
    public override List<string> GetDeltaValues(int level)
    {
        return new List<string>() 
        {
            "������ +1 ��."
        };
    }

    public override void OnUpgraded(int level)
    {
        double delta = 1f;

        RuntimePlayer.Instance.RuntimePlayerStats.Armor += delta;
    }
}
