using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeableObserverComponent : CoreComponent
{
    [Tooltip("Компонент улучшений, который мы отслеживаем")]
    [SerializeField] private UpgradeableComponent m_observableComponent;


    [SerializeField] private int m_observableLevel;
    /// <summary>
    ///     Возвращает наблюдаемый уровень улучшений. При достижении навыка указанного уровня вызывается основное действие этого компонента.
    /// </summary>
    public int observableLevel { get => m_observableLevel; }

    /*
        Эта булка короче на будущее, она очень важна и по факту даже как античит сработает иногда + ошибки исправит
        Короче она позволяет только один раз при жизни текущего компонента вызвать данную часть (может потом ещё что-то придумаю)
    */
    private bool m_isAlreadyChanged;

    
    public UnityEvent OnObservableLvlReached = new UnityEvent();


    public override void InjectComponent(ICore core)
    {
        base.InjectComponent(core);

        if (m_observableComponent == null)
        {
            Debug.LogWarning($"Отслеживаемый компонент улучшений не был найден!", this.gameObject);
            return;
        }

        m_observableComponent.OnUpgraded.AddListener(OnObservableValueChanged);
    }

    
    private void OnObservableValueChanged(int level)
    {
        if (m_isAlreadyChanged) return;
        if (level != m_observableLevel) return;

        m_isAlreadyChanged = true;

        OnObservableLvlReached?.Invoke();
    }
}
