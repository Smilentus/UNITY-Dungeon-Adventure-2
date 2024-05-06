using System.Collections.Generic;
using System.Linq;
using Dimasyechka.Lubribrary.RxMV.UniRx.ReflectionCollectors.Storages;
using Dimasyechka.Lubribrary.RxMV.UniRx.RxLink;
using Dimasyechka.Lubribrary.RxMV.Utilities;
using UniRx;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.Core
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(-1000)]
    public abstract class BaseRxView : MonoBehaviour, IRxLinkable
    {
        [SerializeField]
        private ParentComponentsExtractor<IViewModel> _parentComponentsExtractor = null;

        [SerializeField]
        protected List<UniRxViewConnectorField> _viewConnectorFields = new List<UniRxViewConnectorField>();


        protected IViewModel _usedPresenter = null;


        protected DisposablesStorage _disposablesStorage = new DisposablesStorage();

        protected ReflectionStorage _presenterReflectionStorage = null;

        protected ReflectionStorage _viewReflectionStorage = null;


        public IViewModel UsedPresenter => _usedPresenter;
        public ReflectionStorage PresenterReflectionStorage => _presenterReflectionStorage;
        public ReflectionStorage ViewReflectionStorage => _viewReflectionStorage;


        protected virtual void Awake()
        {
            CreateReflectiveStorage();
            CreateComponentExtractor();
            CollectPresenter();
            ParseReflections();

            if (Application.isPlaying)
            {
                ConnectViewToPresenter();
            }
        }

        protected virtual void OnDestroy()
        {
            Dispose();
        }

        protected virtual void Update()
        {
            if (!Application.isPlaying)
            {
                CreateReflectiveStorage();
                CreateComponentExtractor();
                CollectPresenter();
                ParseReflections();
            }
        }

        protected virtual void Dispose()
        {
            _disposablesStorage?.Dispose();

            _presenterReflectionStorage?.Dispose();
            _viewReflectionStorage?.Dispose();

            _disposablesStorage = null;

            _presenterReflectionStorage = null;
            _viewReflectionStorage = null;

            _usedPresenter = null;
        }


        private void CreateReflectiveStorage()
        {
            if (_presenterReflectionStorage == null)
            {
                _presenterReflectionStorage = new ReflectionStorage();
                _presenterReflectionStorage.InitializeReflectiveCollectors(_usedPresenter);

                Debug.Log($"CreateReflectiveStorage From Presenter => {_presenterReflectionStorage.ReflectiveCollectors.Count}");
            }

            if (_viewReflectionStorage == null)
            {
                _viewReflectionStorage = new ReflectionStorage();
                _viewReflectionStorage.InitializeReflectiveCollectors(_usedPresenter);

                Debug.Log($"CreateReflectiveStorage From View => {_viewReflectionStorage.ReflectiveCollectors.Count}");
            }
        }
        private void CreateComponentExtractor()
        {
            if (_parentComponentsExtractor != null)
            {
                if (!_parentComponentsExtractor.IsMonoBehaviourAssigned)
                {
                    _parentComponentsExtractor.SetMonoBehaviour(this);
                }
            }
            else
            {
                _parentComponentsExtractor = new ParentComponentsExtractor<IViewModel>(this, false);
                _parentComponentsExtractor.CollectComponent();
            }
        }


        private void CollectPresenter()
        {
            _parentComponentsExtractor.CollectComponent();

            if (_parentComponentsExtractor.ExtractedObject != null)
            {
                if (!Equals(_usedPresenter, _parentComponentsExtractor.ExtractedObject))
                {
                    _usedPresenter = _parentComponentsExtractor.ExtractedObject;

                    if (_usedPresenter != null)
                    {
                        OnPresenterFound();
                    }
                }
            }
        }
        private void OnPresenterFound()
        {
            SetPresenterAsReflectionObject();

            SetViewAsReflectionObject();

            ParseReflections();
        }


        private void SetPresenterAsReflectionObject()
        {
            if (_usedPresenter != null)
            {
                _presenterReflectionStorage.SetReflectionObject(_usedPresenter);
                _presenterReflectionStorage.CollectReflections();
            }
        }

        private void SetViewAsReflectionObject()
        {
            if (_usedPresenter != null)
            {
                _viewReflectionStorage.SetReflectionObject(_usedPresenter);
                _viewReflectionStorage.CollectReflections();
            }
        }


        // TODO: ѕродумать автоматическое св€зывание по типам и т.п.
        private void ConnectViewToPresenter()
        {
            if (_usedPresenter == null) return;

            /*
            Dictionary<string, ReactiveProperty<int>> presenterPropertiesInt = PresenterReflectionStorage.GetProperties<int>();
            Dictionary<string, ReactiveProperty<float>> presenterPropertiesFloat = PresenterReflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<double>> presenterPropertiesDouble = PresenterReflectionStorage.GetProperties<double>();
            Dictionary<string, ReactiveProperty<string>> presenterPropertiesString = PresenterReflectionStorage.GetProperties<string>();


            Dictionary<string, ReactiveProperty<int>> viewPropertiesInt = ViewReflectionStorage.GetProperties<int>();
            Dictionary<string, ReactiveProperty<float>> viewPropertiesFloat = ViewReflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<double>> viewPropertiesDouble = ViewReflectionStorage.GetProperties<double>();
            Dictionary<string, ReactiveProperty<string>> viewPropertiesString = ViewReflectionStorage.GetProperties<string>();

            // ќй сколько всего тут накручиваетс€ (надо думать)
            foreach (UniRxViewConnectorField connectorField in _viewConnectorFields)
            {
                if (presenterPropertiesInt.ContainsKey(connectorField.PresenterReactiveField.SelectedName) &&
                    connectorField.PresenterReactiveField.SelectedIndex != -1)
                {
                    if (viewPropertiesInt.ContainsKey(connectorField.ViewReactiveField.SelectedName) &&
                        connectorField.ViewReactiveField.SelectedIndex != -1)
                    {
                        _disposablesStorage.AddToDisposables(presenterPropertiesInt[connectorField.PresenterReactiveField.SelectedName]
                            .Subscribe(x =>
                            {
                                
                            }));
                    }
                }
            }
            */

            OnConnectedToPresenter();
        }

        protected virtual void OnConnectedToPresenter() { }


        private void ParseReflections()
        {
            ParsePresenterReflections();

            ParseViewReflections();

            OnParseReflections();
        }

        private void ParsePresenterReflections()
        {
            if (_usedPresenter == null) return;

            Dictionary<string, ReactiveProperty<int>> presenterPropertiesInt = PresenterReflectionStorage.GetProperties<int>();
            Dictionary<string, ReactiveProperty<float>> presenterPropertiesFloat = PresenterReflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<double>> presenterPropertiesDouble = PresenterReflectionStorage.GetProperties<double>();
            Dictionary<string, ReactiveProperty<string>> presenterPropertiesString = PresenterReflectionStorage.GetProperties<string>();

            string[] reflectionNames = PresenterReflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                presenterPropertiesInt.Select(x => x.Key).ToArray(),
                presenterPropertiesFloat.Select(x => x.Key).ToArray(),
                presenterPropertiesDouble.Select(x => x.Key).ToArray(),
                presenterPropertiesString.Select(x => x.Key).ToArray()
            });

            foreach (UniRxViewConnectorField connectorField in _viewConnectorFields)
            {
                connectorField.PresenterReactiveField.ReflectiveNames = reflectionNames;
            }
        }

        private void ParseViewReflections()
        {
            Dictionary<string, ReactiveProperty<int>> viewPropertiesInt = ViewReflectionStorage.GetProperties<int>();
            Dictionary<string, ReactiveProperty<float>> viewPropertiesFloat = ViewReflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<double>> viewPropertiesDouble = ViewReflectionStorage.GetProperties<double>();
            Dictionary<string, ReactiveProperty<string>> viewPropertiesString = ViewReflectionStorage.GetProperties<string>();

            string[] reflectionNames = ViewReflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                viewPropertiesInt.Select(x => x.Key).ToArray(),
                viewPropertiesFloat.Select(x => x.Key).ToArray(),
                viewPropertiesDouble.Select(x => x.Key).ToArray(),
                viewPropertiesString.Select(x => x.Key).ToArray()
            });

            foreach (UniRxViewConnectorField connectorField in _viewConnectorFields)
            {
                connectorField.ViewReactiveField.ReflectiveNames = reflectionNames;
            }
        }

        protected virtual void OnParseReflections() { }

    }
}