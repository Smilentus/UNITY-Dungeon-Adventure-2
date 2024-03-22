using UnityEngine;

/// <summary>
///     Базовый класс для построения текущего оружия из разных скриптов/модулей и т.п.
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
