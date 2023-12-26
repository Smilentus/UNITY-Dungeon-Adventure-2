public interface IUpgradeableChecker
{
    public int CheckLevel { get; }


    public bool CanUpgrade();
    public void PostUpgrade();
}
