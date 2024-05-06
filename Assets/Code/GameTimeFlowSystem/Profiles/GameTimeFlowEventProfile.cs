using UnityEngine;

namespace Dimasyechka.Code.GameTimeFlowSystem.Profiles
{
    [CreateAssetMenu(fileName = "GameTimeFlowEventProfile", menuName = "GameTimeFlowSystem/New GameTimeFlowEventProfile")]
    public partial class GameTimeFlowEventProfile : ScriptableObject
    {
        public string EventUID;

        [TextArea(3, 5)]
        public string EventTitle;

        [TextArea(5, 15)]
        public string EventDescription;
    }
}