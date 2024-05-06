using Dimasyechka.Code.CoreComponentSystem.Core;
using Dimasyechka.Code.CoreComponentSystem.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Dimasyechka.Code.UpgradeableSystem
{
    public class UpgradeableObserverComponent : CoreComponent
    {
        [Tooltip("Компонент улучшений, который мы отслеживаем")]
        [SerializeField] private UpgradeableComponent m_observableComponent;


        [SerializeField] private int m_observableLevel;
        /// <summary>
        ///     Возвращает наблюдаемый уровень улучшений. При достижении навыка указанного уровня вызывается основное действие этого компонента.
        /// </summary>
        public int observableLevel { get => m_observableLevel; }


        public UnityEvent OnObservableLvlReached = new UnityEvent();


        public override void InjectComponent(ICore core)
        {
            base.InjectComponent(core);

            if (m_observableComponent == null)
            {
                Debug.LogWarning($"Отслеживаемый компонент улучшений не был найден!", this.gameObject);
                return;
            }

            m_observableComponent.OnUpgradedUnityEvent.AddListener(OnObservableValueChanged);
        }


        private void OnObservableValueChanged(int level)
        {
            if (m_observableLevel != -1 && level != m_observableLevel) return;

            OnObservableLvlReached?.Invoke();
        }
    }
}
