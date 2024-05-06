using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.Adapters.Base
{
    public abstract class BaseComponentRxAdapter<T> : BaseRxAdapter where T : Component
    {
        [SerializeField]
        protected T _component;


        protected override void Awake()
        {
            if (IsNullComponent())
            {
                _component = GetComponent<T>();
            }
            if (IsNullComponent())
            {
                Debug.LogError($"Отсутствует основной компонент на '{this}'");
                return;
            }

            base.Awake();
        }


        protected bool IsNullComponent()
        {
            return (_component == null || _component.Equals(null));
        }
    }
}
