public interface IUpgradeableChecker
{
    public int CheckLevel { get; }


    public void LoadUpgradeableLevel(int level);
    public bool CanUpgrade();
    public void PostUpgrade();
}
