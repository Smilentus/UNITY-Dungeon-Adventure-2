namespace Dimasyechka.Code.UpgradeableSystem.Interfaces
{
    public interface IUpgradeableChecker
    {
        public int CheckLevel { get; }


        public string GetDescription();
        public void LoadUpgradeableLevel(int level);
        public bool CanUpgrade();
        public void PostUpgrade();
    }
}