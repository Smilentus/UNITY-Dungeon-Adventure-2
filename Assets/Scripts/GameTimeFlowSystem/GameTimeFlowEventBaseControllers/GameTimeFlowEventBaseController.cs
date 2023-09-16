using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventGameTimeFlowEvent : UnityEvent<GameTimeFlowEvent> { }

public class GameTimeFlowEventBaseController : MonoBehaviour
{
    public UnityEventGameTimeFlowEvent OnEventStarted = new UnityEventGameTimeFlowEvent();
    public UnityEventGameTimeFlowEvent OnEventFinished = new UnityEventGameTimeFlowEvent();


    [SerializeField]
    private GameTimeFlowEvent m_gameTimeFlowEventReference;
    public GameTimeFlowEvent GameTimeFlowEventReference => m_gameTimeFlowEventReference;


    private bool m_isEventStarted = false;
    public bool IsEventStarted => m_isEventStarted;


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
