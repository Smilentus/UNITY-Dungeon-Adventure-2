using System;
using System.Collections.Generic;
using System.Linq;
using Dimasyechka.Lubribrary.RxMV.UniRx.ReflectionCollectors.Base;
using UniRx;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.ReflectionCollectors.Storages
{
    public class ReflectionStorage : IDisposable
    {
        private List<ReflectiveCollector> _reflectiveCollectors = new List<ReflectiveCollector>();
        public List<ReflectiveCollector> ReflectiveCollectors => _reflectiveCollectors;


        private object _reflectionObject;


        public void Dispose()
        {
            foreach (var collector in _reflectiveCollectors)
            {
                collector.Dispose();
            }
        }


        public void SetReflectionObject(object reflectionObject)
        {
            if (_reflectionObject == reflectionObject ||
                reflectionObject == null) return;

            _reflectionObject = reflectionObject;

            foreach (var collector in _reflectiveCollectors)
            {
                collector.SetReflectionObject(_reflectionObject);
            }
        }


        public void InitializeReflectiveCollectors(object reflectionObject)
        {
            _reflectiveCollectors = new List<ReflectiveCollector>
            {
                new UniRxPropertiesReflectiveCollector(),
                new UniRxCollectionReflectiveCollector(),
                new UniRxCommandReflectiveCollector(),
                new UniRxMethodsReflectiveCollector()
            };

            if (reflectionObject == null) return;

            SetReflectionObject(reflectionObject);
            CollectReflections();
        }

        public void CollectReflections()
        {
            if (_reflectionObject == null) return;

            foreach (var reflectionCollector in _reflectiveCollectors)
            {
                reflectionCollector.CollectReflections();
            }
        }


        public string[] GetDictionaryNames(List<string[]> names)
        {
            List<string> output = new List<string>();
            for (int i = 0; i < names.Count; i++)
            {
                output = output.Concat(names[i]).ToList();
            }
            return output.ToArray();
        }

        public void InvokeDelegate<T>(string key, T value)
        {
            Dictionary<string, Action<T>> delegates = GetDelegates<T>();
            
            if (delegates.ContainsKey(key))
            {
                delegates[key].Invoke(value);
            }
        }


        protected T GetReflectiveCollector<T>() where T : ReflectiveCollector
        {
            ReflectiveCollector collector = ReflectiveCollectors.Find(x => x.GetType() == typeof(T));

            if (collector != null)
            {
                return collector as T;
            }

            return null;
        }

        public Dictionary<string, ReactiveProperty<T>> GetProperties<T>()
        {
            UniRxPropertiesReflectiveCollector reflectiveCollector =
                GetReflectiveCollector<UniRxPropertiesReflectiveCollector>();

            return reflectiveCollector != null ? reflectiveCollector.ParseProperties<T>() : new Dictionary<string, ReactiveProperty<T>>();
        }
        public Dictionary<string, ReactiveCollection<T>> GetCollections<T>()
        {
            UniRxCollectionReflectiveCollector reflectiveCollector =
                GetReflectiveCollector<UniRxCollectionReflectiveCollector>();

            return reflectiveCollector != null ? reflectiveCollector.ParseCollections<T>() : new Dictionary<string, ReactiveCollection<T>>();
        }
        public Dictionary<string, ReactiveCommand> GetCommands()
        {
            UniRxCommandReflectiveCollector reflectiveCollector =
                GetReflectiveCollector<UniRxCommandReflectiveCollector>();

            return reflectiveCollector != null ? reflectiveCollector.ParseCommands() : new Dictionary<string, ReactiveCommand>();
        }
        public Dictionary<string, Action<T>> GetDelegates<T>()
        {
            UniRxMethodsReflectiveCollector reflectiveCollector =
                GetReflectiveCollector<UniRxMethodsReflectiveCollector>();

            return reflectiveCollector != null ? reflectiveCollector.ParseDelegates<T>() : new Dictionary<string, Action<T>>();
        }
        public Dictionary<string, Action> GetDelegates()
        {
            UniRxMethodsReflectiveCollector reflectiveCollector =
                GetReflectiveCollector<UniRxMethodsReflectiveCollector>();

            return reflectiveCollector != null ? reflectiveCollector.ParseDelegates() : new Dictionary<string, Action>();
        }
    }
}
