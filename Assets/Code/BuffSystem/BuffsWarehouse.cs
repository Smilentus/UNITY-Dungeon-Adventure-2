using System.Collections.Generic;
using Dimasyechka.Code.BuffSystem.Profiles;
using UnityEngine;

namespace Dimasyechka.Code.BuffSystem
{
    public class BuffsWarehouse : MonoBehaviour
    {
        private static BuffsWarehouse instance;
        public static BuffsWarehouse Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<BuffsWarehouse>(true);
                }
                return instance;
            }
        }


        [SerializeField]
        private List<BuffProfile> m_availableBuffProfiles = new List<BuffProfile>();
        public List<BuffProfile> AvailableBuffProfiles => m_availableBuffProfiles;


        public BuffProfile GetBuffProfileByUID(string uid)
        {
            return m_availableBuffProfiles.Find(x => x.BuffUID == uid);
        }
    }
}
