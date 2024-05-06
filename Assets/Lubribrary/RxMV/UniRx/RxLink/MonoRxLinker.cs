using System;
using Dimasyechka.Lubribrary.RxMV.UniRx.ReflectionCollectors.Storages;
using Dimasyechka.Lubribrary.RxMV.Utilities;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.RxLink
{
    // TODO: ѕодумот как вынести все св€зки в класс 
    public class MonoRxLinker : MonoBehaviour, IDisposable
    {
        [SerializeField]
        private ParentComponentsExtractor<IRxLinkable> _parentComponentsExtractor = null;

        protected IRxLinkable RxLinkable;

        protected ReflectionStorage _reflectionStorage = new ReflectionStorage();


        public void Initialize()
        {        
            CreateReflectiveStorage();
            CreateComponentExtractor();
            CollectAdaptable();
            OnParseReflectionsFromAdaptable();
        }

        public virtual void Dispose()
        {
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

        private void OnRxAdaptableFound()
        {
            SetAdaptableReflectionObject();

            OnParseReflectionsFromAdaptable();
        }


        public virtual void OnParseReflectionsFromAdaptable() { }
    }
}
