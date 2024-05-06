using Dimasyechka.Code.BattleSystem.BattleActions.Interfaces;
using Dimasyechka.Code.BattleSystem.BattleActions.Views;
using Dimasyechka.Code.Utilities;
using UnityEngine;

namespace Dimasyechka.Code.MagicSystem.Profiles
{
    [CreateAssetMenu(menuName = "Magic System/New BaseMagicProfile", fileName = "BaseMagicProfile_")]
    public class BaseMagicProfile : ScriptableObject, IBattleActionInteraction
    {
        [SerializeField]
        private BaseBattleInteractionView m_actionProfileViewPrefab;
        public BaseBattleInteractionView ActionProfileViewPrefab => m_actionProfileViewPrefab;


        [SerializeField]
        private SerializableMonoScript<IBattleActionExecuter> m_actionExecuter;
        public SerializableMonoScript<IBattleActionExecuter> ActionExecuter => m_actionExecuter;


        [TextArea(3, 5)]
        [SerializeField]
        private string m_MagicName;
        public string InteractionTitle => m_MagicName;


        [TextArea(5, 10)]
        [SerializeField]
        private string m_MagicDescription;
        public string MagicDescription => m_MagicDescription;


        [Tooltip("Дефолтная стоимость магии")]
        [SerializeField]
        private int m_DefaultManaPointsCost;
        public int DefaultManaPointsCost => m_DefaultManaPointsCost;


        [SerializeField]
        private int m_DefaultCooldownHours;
        public int DefaultCooldownHours => m_DefaultCooldownHours;
    }
}
