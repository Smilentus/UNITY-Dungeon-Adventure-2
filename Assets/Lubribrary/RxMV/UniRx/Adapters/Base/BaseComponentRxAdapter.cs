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
                Debug.LogError($"����������� �������� ��������� �� '{this}'");
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
