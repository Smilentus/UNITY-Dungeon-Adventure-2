using Dimasyechka.Code.GameTimeFlowSystem.Profiles;
using UnityEngine;
using UnityEngine.Events;

namespace Dimasyechka.Code.GameTimeFlowSystem.GTFEvents
{
    public class UnityEventGameTimeFlowEvent : UnityEvent<GameTimeFlowEventProfile> { }

    public class GameTimeFlowEventBase : MonoBehaviour
    {
        public UnityEventGameTimeFlowEvent OnEventStarted = new UnityEventGameTimeFlowEvent();
        public UnityEventGameTimeFlowEvent OnEventFinished = new UnityEventGameTimeFlowEvent();


        [SerializeField]
        private GameTimeFlowEventProfile m_gameTimeFlowEventReference;
        public GameTimeFlowEventProfile GameTimeFlowEventReference => m_gameTimeFlowEventReference;


        private bool m_isEventStarted = false;
        public bool IsEventStarted => m_isEventStarted;


        /// <summary>
        ///     ћаска загрузки информации (просто устанавливаютс€ переменные по слепку, но ивент заново не начинаетс€ во избежание проблем и абузов) 
        /// </summary>
        public void SetMaskData()
        {
            m_isEventStarted = true;
        }


        public virtual bool CanStartEvent()
        {
            return false;
        }

        public virtual bool CanFinishEvent()
        {
            return false;
        }

        public virtual void StartEvent()
        {
            m_isEventStarted = true;
            OnEventStarted?.Invoke(m_gameTimeFlowEventReference);
        }

        public virtual void FinishEvent()
        {
            m_isEventStarted = false;
            OnEventFinished?.Invoke(m_gameTimeFlowEventReference);
        }
    }
}