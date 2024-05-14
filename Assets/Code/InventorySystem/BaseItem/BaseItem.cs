namespace Dimasyechka.Code.InventorySystem.BaseItem
{
    [System.Serializable]
    public class BaseItem
    {
        protected BaseItemProfile _baseItemProfile;
        public BaseItemProfile BaseItemProfile => _baseItemProfile;


        public BaseItem(BaseItemProfile baseItemProfile)
        {
            _baseItemProfile = baseItemProfile;
        }
    }
}
