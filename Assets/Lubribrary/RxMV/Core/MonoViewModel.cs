using Dimasyechka.Lubribrary.RxMV.Utilities;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.Core
{
    [DefaultExecutionOrder(-1001)]
    public class MonoViewModel<T> : MonoBehaviour, IViewModel<T>
    {
        public T Model { get; private set; }


        private T _zenjectedModel;


        protected readonly IDisposablesStorage _disposablesStorage = new DisposablesStorage();
        public IDisposablesStorage DisposableStorage => _disposablesStorage;


        protected virtual void Start()
        {
            if (_zenjectedModel != null && Model == null)
            {
                SetupModel(_zenjectedModel);
            }
        }


        protected virtual void OnDestroy()
        {
            RemoveModel();
        }

        public void Dispose()
        {
            _disposablesStorage.Dispose();
        }

        public void ZenjectModel(T model)
        {
            _zenjectedModel = model;
        }

        public void SetupModel(T model)
        {
            RemoveModel();

            if (model == null)
            {
                Debug.LogError($"Alerted empty model setup '{GetType()}' at object '{gameObject.name}'");
                return;
            }

            Model = model;
            OnSetupModel();
        }
        protected virtual void OnSetupModel() { }


        public void RemoveModel()
        {
            if (Model == null) return;

            _disposablesStorage.ClearDisposables();

            OnRemoveModel();

            Model = default(T);
        }
        protected virtual void OnRemoveModel() { }
    }
}
