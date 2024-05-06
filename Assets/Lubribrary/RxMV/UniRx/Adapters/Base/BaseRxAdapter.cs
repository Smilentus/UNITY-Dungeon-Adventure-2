using Dimasyechka.Lubribrary.RxMV.UniRx.ReflectionCollectors.Storages;
using Dimasyechka.Lubribrary.RxMV.UniRx.RxLink;
using Dimasyechka.Lubribrary.RxMV.Utilities;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.Adapters.Base
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(-999)]
    public abstract class BaseRxAdapter : MonoBehaviour
    {
        [SerializeField]
        private ParentComponentsExtractor<IRxLinkable> _parentComponentsExtractor = null;

        protected IRxLinkable RxLinkable;

        protected DisposablesStorage _disposablesStorage = new DisposablesStorage();

        protected ReflectionStorage _reflectionStorage = new ReflectionStorage();


        protected virtual void Awake()
        {
            CreateReflectiveStorage();
            CreateComponentExtractor();
            CollectAdaptable();
            ParseReflectionsFromAdaptable();

            if (Application.isPlaying)
            {
                ConnectAdapterToView();
            }
        }

        protected virtual void Update()
        {
            if (!Application.isPlaying)
            {
                CreateReflectiveStorage();
                CreateComponentExtractor();
                CollectAdaptable();
                ParseReflectionsFromAdaptable();
            }
        }

        protected virtual void OnDestroy()
        {
            Dispose();
        }

        protected virtual void Dispose()
        {
            _disposablesStorage.Dispose();

            _disposablesStorage = null;
            _reflectionStorage = null;

            RxLinkable = null;
        }


        private void CreateReflectiveStorage()
        {
            _reflectionStorage = new ReflectionStorage();
            _reflectionStorage.InitializeReflectiveCollectors(RxLinkable);

            SetAdaptableReflectionObject();
        }

        private void SetAdaptableReflectionObject()
        {
            if (RxLinkable != null)
            {
                _reflectionStorage.SetReflectionObject(RxLinkable);
                _reflectionStorage.CollectReflections();
            }
        }

        private void OnRxAdaptableFound()
        {
            SetAdaptableReflectionObject();

            ParseReflectionsFromAdaptable();
        }


        private void ParseReflectionsFromAdaptable()
        {
            OnParseReflections();
        }

        protected abstract void OnParseReflections();


        private void ConnectAdapterToView()
        {
            if (RxLinkable == null) return;

            OnConnectedToView();
        }

        protected abstract void OnConnectedToView();


        private void CreateComponentExtractor()
        {
            if (_parentComponentsExtractor != null)
            {
                if (!_parentComponentsExtractor.IsMonoBehaviourAssigned)
                {
                    _parentComponentsExtractor.SetMonoBehaviour(this);
                }

                return;
            }

            _parentComponentsExtractor = new ParentComponentsExtractor<IRxLinkable>(this, false);
            _parentComponentsExtractor.CollectComponent();
        }

        private void CollectAdaptable()
        {
            _parentComponentsExtractor.CollectComponent();

            if (_parentComponentsExtractor.ExtractedObject != null)
            {
                if (!Equals(RxLinkable, _parentComponentsExtractor.ExtractedObject))
                {
                    RxLinkable = _parentComponentsExtractor.ExtractedObject;

                    if (RxLinkable != null)
                    {
                        OnRxAdaptableFound();
                    }
                }
            }
        }
    }
}