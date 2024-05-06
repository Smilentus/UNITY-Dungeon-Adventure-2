namespace Dimasyechka.Code.InventorySystem.BaseItem
{
    [System.Serializable]
    public class BaseItem
    {
        protected BaseItemProfile baseItemProfile;
        public BaseItemProfile BaseItemProfile => baseItemProfile;


        public BaseItem(BaseItemProfile _baseItemProfile)
        {
            baseItemProfile = _baseItemProfile;
        }
    }
}
