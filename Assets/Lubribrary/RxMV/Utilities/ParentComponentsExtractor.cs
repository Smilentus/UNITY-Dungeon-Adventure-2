using System.Collections.Generic;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.Utilities
{
    [System.Serializable]
    public sealed class ParentComponentsExtractor<T> where T : class
    {
        [Header("Extracted Data")]
        [SerializeField]
        private bool _useAutoCollectionMode = true;

        [SerializeField]
        private Component _extractedComponent;


        private MonoBehaviour _monoBehaviour = null;
        private T _extractedObject = null;

        private bool _includeMyself = false;


        public ParentComponentsExtractor()  { }
        public ParentComponentsExtractor(MonoBehaviour monoBehaviour, bool includeMyself)
        {
            _monoBehaviour = monoBehaviour;
            _includeMyself = includeMyself;
        }


        public T ExtractedObject => _extractedObject;
        public bool IsMonoBehaviourAssigned => _monoBehaviour != null;


        public void SetMonoBehaviour(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour; 
        }


        public void CollectComponent()
        {
            if (_monoBehaviour == null)
            {
                Debug.LogError($"MonoBehaviour is not assigned!");
                return;
            }

            if (_useAutoCollectionMode)
            {
                AutomaticExtractPresenter();
            }
            else
            {
                ManualExtractPresenter();
            }
        }

        private void AutomaticExtractPresenter()
        {
            List<Component> components = new List<Component>();
            _monoBehaviour.GetComponentsInParent(true, components);

            if (_includeMyself)
            {
                List<Component> thisComponents = new List<Component>();
                _monoBehaviour.GetComponents(typeof(Component), thisComponents);
                components.AddRange(thisComponents);
            }

            GetFirstEntryFromComponents(components);
        }

        private void ManualExtractPresenter()
        {
            if (_extractedComponent == null)
            {
                _extractedObject = null;
                return;
            }

            List<Component> components = new List<Component>();
            _extractedComponent.GetComponents(components);

            _extractedComponent = null;
            _extractedObject = null;

            GetFirstEntryFromComponents(components);
        }


        private void GetFirstEntryFromComponents(List<Component> components)
        {
            if (components == null) return;

            Component searchingComponent = components.Find(x => x is T);

            if (searchingComponent == null) return;

            _extractedComponent = searchingComponent;
            _extractedObject = _extractedComponent as T;
        }
    }
}
