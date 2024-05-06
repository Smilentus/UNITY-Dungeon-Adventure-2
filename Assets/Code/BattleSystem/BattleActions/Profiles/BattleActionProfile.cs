using Dimasyechka.Code.BattleSystem.BattleActions.Interfaces;
using Dimasyechka.Code.BattleSystem.BattleActions.Views;
using Dimasyechka.Code.Utilities;
using UnityEngine;

namespace Dimasyechka.Code.BattleSystem.BattleActions.Profiles
{
    [CreateAssetMenu(menuName = "Creatable/BattleActionSystem/BattleActionProfile", fileName = "BattleActionProfile_")]
    public class BattleActionProfile : ScriptableObject, IBattleActionInteraction
    {
        [SerializeField]
        private BaseBattleInteractionView m_actionProfileViewPrefab;
        public BaseBattleInteractionView ActionProfileViewPrefab => m_actionProfileViewPrefab;
         

        [SerializeField]
        private SerializableMonoScript<IBattleActionExecuter> m_actionExecuter;
        public SerializableMonoScript<IBattleActionExecuter> ActionExecuter => m_actionExecuter;


        [TextArea(3, 5)]
        [SerializeField]
        private string m_actionTitle;
        public string InteractionTitle => m_actionTitle;


        [TextArea(5, 10)]
        [SerializeField]
        private string m_actionDescription;
        public string ActionDescription => m_actionDescription;


        [Tooltip("Очки действий, которые необходимо затратить для того, чтобы применить это действие")]
        [SerializeField]
        private int m_spendableActions;
        /// <summary>
        ///     Очки действий, которые необходимо затратить для того, чтобы применить это действие
        /// </summary>
        public int SpendableActions => m_spendableActions;
    }
}
