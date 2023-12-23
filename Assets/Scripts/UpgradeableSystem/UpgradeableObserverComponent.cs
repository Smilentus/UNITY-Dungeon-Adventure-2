using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeableObserverComponent : CoreComponent
{
    [Tooltip("��������� ���������, ������� �� �����������")]
    [SerializeField] private UpgradeableComponent m_observableComponent;


    [SerializeField] private int m_observableLevel;
    /// <summary>
    ///     ���������� ����������� ������� ���������. ��� ���������� ������ ���������� ������ ���������� �������� �������� ����� ����������.
    /// </summary>
    public int observableLevel { get => m_observableLevel; }

    /*
        ��� ����� ������ �� �������, ��� ����� ����� � �� ����� ���� ��� ������� ��������� ������ + ������ ��������
        ������ ��� ��������� ������ ���� ��� ��� ����� �������� ���������� ������� ������ ����� (����� ����� ��� ���-�� ��������)
    */
    private bool m_isAlreadyChanged;

    
    public UnityEvent OnObservableLvlReached = new UnityEvent();


    public override void InjectComponent(ICore core)
    {
        base.InjectComponent(core);

        if (m_observableComponent == null)
        {
            Debug.LogWarning($"������������� ��������� ��������� �� ��� ������!", this.gameObject);
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
