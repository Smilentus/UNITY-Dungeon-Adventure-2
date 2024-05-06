using Dimasyechka.Code.CoreComponentSystem.Core;
using Dimasyechka.Code.CoreComponentSystem.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Dimasyechka.Code.UpgradeableSystem
{
    public class UpgradeableObserverComponent : CoreComponent
    {
        [Tooltip("��������� ���������, ������� �� �����������")]
        [SerializeField] private UpgradeableComponent m_observableComponent;


        [SerializeField] private int m_observableLevel;
        /// <summary>
        ///     ���������� ����������� ������� ���������. ��� ���������� ������ ���������� ������ ���������� �������� �������� ����� ����������.
        /// </summary>
        public int observableLevel { get => m_observableLevel; }


        public UnityEvent OnObservableLvlReached = new UnityEvent();


        public override void InjectComponent(ICore core)
        {
            base.InjectComponent(core);

            if (m_observableComponent == null)
            {
                Debug.LogWarning($"������������� ��������� ��������� �� ��� ������!", this.gameObject);
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
