using System.Collections.Generic;

public class PlayerCriticalDamageComponent : BaseSkillComponent
{
    private double delta = 0.1f;


    public override List<string> GetDeltaValues(int level)
    {
        return new List<string>() {
            $"���� ������������ ����� +{(delta * 100f).ToString("f2")}%"
        };
    }

    public override void OnUpgraded(int level)
    {
        RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeDamageMultiplier += delta;
    }
}