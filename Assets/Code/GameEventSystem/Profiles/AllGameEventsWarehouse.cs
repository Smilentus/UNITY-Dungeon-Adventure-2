using System.Collections.Generic;
using UnityEngine;

namespace Dimasyechka.Code.GameEventSystem.Profiles
{
    [CreateAssetMenu(menuName = "Creatable/GameEventSystem/New AllGameEventsWarehouse", fileName = "AllGameEventsWarehouse")]
    public class AllGameEventsWarehouse : ScriptableObject
    {
        [SerializeField]
        private List<BaseGameEventProfile> m_baseGameEventProfiles = new List<BaseGameEventProfile>();
        public List<BaseGameEventProfile> BaseGameEventProfiles => m_baseGameEventProfiles;


        [SerializeField]
        private List<BattleGameEventProfile> m_battleGameEventProfiles = new List<BattleGameEventProfile>();
        public List<BattleGameEventProfile> BattleGameEventProfiles => m_battleGameEventProfiles;
    }
}
