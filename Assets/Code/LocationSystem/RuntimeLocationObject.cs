using Dimasyechka.Code.LocationSystem.Profiles;
using UnityEngine;

namespace Dimasyechka.Code.LocationSystem
{
    public class RuntimeLocationObject : MonoBehaviour
    {
        [SerializeField]
        private LocationProfile m_locationProfile;
        /// <summary>
        ///     ������ �� ����������� ������� � �������
        /// </summary>
        public LocationProfile LocationProfileReference => m_locationProfile;
    }
}