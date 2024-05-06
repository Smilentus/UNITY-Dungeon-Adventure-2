using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.GameTimeFlowSystem.Profiles;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Dimasyechka.Code.GameTimeFlowSystem.GTFEvents
{
    public class UnityEventGameTimeFlowEvent : UnityEvent<GameTimeFlowEventProfile> { }

    public class GameTimeFlowEventBase : MonoBehaviour
    {
        public UnityEventGameTimeFlowEvent OnEventStarted = new UnityEventGameTimeFlowEvent();
        public UnityEventGameTimeFlowEvent OnEventFinished = new UnityEventGameTimeFlowEvent();


        [SerializeField]
        private GameTimeFlowEventProfile _gameTimeFlowEventReference;
        public GameTimeFlowEventProfile GameTimeFlowEventReference => _gameTimeFlowEventReference;


        private bool _isEventStarted = false;
        public bool IsEventStarted => _isEventStarted;


        protected GameTimeFlowController _gameTimeFlowController;


        [Inject]
        public void Construct(GameTimeFlowController gameTimeFlowController)
        {
            _gameTimeFlowController = gameTimeFlowController;
        }


        /// <summary>
        ///     ћаска загрузки информации (просто устанавливаютс€ переменные по слепку, но ивент заново не начинаетс€ во избежание проблем и абузов) 
        /// </summary>
        public void SetMaskData()
        {
            _isEventStarted = true;
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
            _isEventStarted = true;
            OnEventStarted?.Invoke(_gameTimeFlowEventReference);
        }

        public virtual void FinishEvent()
        {
            _isEventStarted = false;
            OnEventFinished?.Invoke(_gameTimeFlowEventReference);
        }
    }
}