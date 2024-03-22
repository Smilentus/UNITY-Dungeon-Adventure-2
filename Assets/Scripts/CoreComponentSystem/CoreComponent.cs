using UnityEngine;

/// <summary>
///     ������� ����� ��� ���������� �������� ������ �� ������ ��������/������� � �.�.
/// </summary>
public class CoreComponent : MonoBehaviour, IComponent
{
    public ICore attachedCore { get; set; }

    public virtual void InjectComponent(ICore core)
    {
        attachedCore = core;
    }

    public virtual void OnDestroyHandler() { }

    private void OnDestroy()
    {
        OnDestroyHandler();
    }
}
