using Dimasyechka.Code._LEGACY_.Inventory;
using UnityEngine;

namespace Dimasyechka.Code._LEGACY_
{
    [System.Serializable]
    public class producingItem : MonoBehaviour
    {
        [Header("Предмет")]
        public ItemProfile item;
        [Header("Производственное количество")]
        public int producingStack;
    }
}
