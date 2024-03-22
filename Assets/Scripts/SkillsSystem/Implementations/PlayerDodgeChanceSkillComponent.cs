using System.Collections.Generic;

public class PlayerDodgeChanceSkillComponent : BaseSkillComponent
{
    private double delta = 1f;


    public override List<string> GetDeltaValues(int level)
    {
        return new List<string>()
        {
            $"���� ��������� +{(delta).ToString("f2")}%"
        };
    }

    public override void OnUpgraded(int level)
    {
        RuntimePlayer.Instance.RuntimePlayerStats.DodgeChance += delta;
    }
}
