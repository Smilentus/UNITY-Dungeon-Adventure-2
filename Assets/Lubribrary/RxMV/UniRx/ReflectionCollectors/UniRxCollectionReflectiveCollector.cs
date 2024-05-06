using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using Dimasyechka.Lubribrary.RxMV.UniRx.ReflectionCollectors.Base;
using UniRx;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.ReflectionCollectors
{
    public class UniRxCollectionReflectiveCollector : ReflectiveCollector
    {
        private Dictionary<Type, List<FieldInfo>> _reactiveCollectionFieldInfos = new Dictionary<Type, List<FieldInfo>>();
        private Dictionary<Type, List<PropertyInfo>> _reactiveCollectionPropertiesInfos = new Dictionary<Type, List<PropertyInfo>>();


        private Dictionary<Type, Dictionary<Type, List<FieldInfo>>> _runtimeReactiveCollectionFieldInfos = new Dictionary<Type, Dictionary<Type, List<FieldInfo>>>();
        private Dictionary<Type, Dictionary<Type, List<PropertyInfo>>> _runtimeReactiveCollectionPropertyInfos = new Dictionary<Type, Dictionary<Type, List<PropertyInfo>>>();


        public override void Dispose()
        {
            _reactiveCollectionFieldInfos.Clear();
            _reactiveCollectionPropertiesInfos.Clear();

            _runtimeReactiveCollectionFieldInfos.Clear();
            _runtimeReactiveCollectionPropertyInfos.Clear();


            _reactiveCollectionFieldInfos = null;
            _reactiveCollectionPropertiesInfos = null;

            _runtimeReactiveCollectionFieldInfos = null;
            _runtimeReactiveCollectionPropertyInfos = null;
        }

        public override void CollectReflections()
        {
            CollectReactiveCollectionFieldAttributes();
            CollectReactiveCollectionPropertyAttributes();
        }

        public Dictionary<string, ReactiveCollection<T>> ParseCollections<T>()
        {
            Dictionary<string, ReactiveCollection<T>> reactiveCollections = new Dictionary<string, ReactiveCollection<T>>();

            if (_reflectionObject != null)
            {
                foreach (var pair in _reactiveCollectionFieldInfos)
                {
                    if (pair.Key == typeof(ReactiveCollection<T>) || pair.Key.IsSubclassOf(typeof(ReactiveCollection<T>)))
                    {
                        foreach (FieldInfo field in _reactiveCollectionFieldInfos[pair.Key])
                        {
                            if (field.GetValue(_reflectionObject) as ReactiveCollection<T> != null)
                            {
                                reactiveCollections.Add(field.Name, field.GetValue(_reflectionObject) as ReactiveCollection<T>);
                            }
                        }
                    }
                }

                foreach (var pair in _reactiveCollectionPropertiesInfos)
                {
                    if (pair.Key == typeof(ReactiveCollection<T>) ||
                        pair.Key.IsSubclassOf(typeof(ReactiveCollection<T>)))
                    {
                        foreach (PropertyInfo property in _reactiveCollectionPropertiesInfos[pair.Key])
                        {
                            if (property.GetValue(_reflectionObject) as ReactiveCollection<T> != null)
                            {
                                reactiveCollections.Add(property.Name, property.GetValue(_reflectionObject) as ReactiveCollection<T>);
                            }
                        }
                    }
                }
            }

            string[] names = reactiveCollections.Select(x => x.Key).ToArray();

            foreach (string nameProperty in names)
            {
                reactiveCollections.Add($"{nameProperty} ({typeof(T).ToString().Replace($"{typeof(T).Namespace}.", "")})", reactiveCollections[nameProperty]);
                reactiveCollections.Remove(nameProperty);
            }

            return reactiveCollections;
        }

        private void CollectReactiveCollectionFieldAttributes()
        {
            _reactiveCollectionFieldInfos.Clear();

            if (Application.isPlaying)
            {
                if (_runtimeReactiveCollectionFieldInfos.ContainsKey(_reflectionObject.GetType()))
                {
                    Dictionary<Type, List<FieldInfo>> pairs = _runtimeReactiveCollectionFieldInfos[_reflectionObject.GetType()];

                    foreach (var pair in pairs)
                    {
                        if (!_reactiveCollectionFieldInfos.ContainsKey(pair.Key))
                        {
                            _reactiveCollectionFieldInfos.Add(pair.Key, new List<FieldInfo>());
                        }
                        _reactiveCollectionFieldInfos[pair.Key].AddRange(pair.Value);
                    }
                    return;
                }
            }

            FieldInfo[] fields = _reflectionObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                object[] attrs = field.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    RxAdaptableCollectionAttribute propertyAttribute = attr as RxAdaptableCollectionAttribute;
                    if (propertyAttribute != null)
                    {
                        Type propType = field.GetValue(_reflectionObject).GetType();

                        if (!_reactiveCollectionFieldInfos.ContainsKey(propType))
                        {
                            _reactiveCollectionFieldInfos.Add(propType, new List<FieldInfo>());
                        }

                        _reactiveCollectionFieldInfos[propType].Add(field);
                    }
                }
            }

            if (Application.isPlaying)
            {
                _runtimeReactiveCollectionFieldInfos.Add(_reflectionObject.GetType(), _reactiveCollectionFieldInfos);
            }
        }
        private void CollectReactiveCollectionPropertyAttributes()
        {
            _reactiveCollectionPropertiesInfos.Clear();

            if (Application.isPlaying)
            {
                if (_runtimeReactiveCollectionPropertyInfos.ContainsKey(_reflectionObject.GetType()))
                {
                    Dictionary<Type, List<PropertyInfo>> pairs = _runtimeReactiveCollectionPropertyInfos[_reflectionObject.GetType()];

                    foreach (var pair in pairs)
                    {
                        if (!_reactiveCollectionPropertiesInfos.ContainsKey(pair.Key))
                        {
                            _reactiveCollectionPropertiesInfos.Add(pair.Key, new List<PropertyInfo>());
                        }
                        _reactiveCollectionPropertiesInfos[pair.Key].AddRange(pair.Value);
                    }
                    return;
                }
            }

            PropertyInfo[] properties = _reflectionObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo property in properties)
            {
                object[] attrs = property.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    RxAdaptableCollectionAttribute propertyAttribute = attr as RxAdaptableCollectionAttribute;
                    if (propertyAttribute != null)
                    {
                        Type propType = property.GetValue(_reflectionObject).GetType();

                        if (!_reactiveCollectionPropertiesInfos.ContainsKey(propType))
                        {
                            _reactiveCollectionPropertiesInfos.Add(propType, new List<PropertyInfo>());
                        }

                        _reactiveCollectionPropertiesInfos[propType].Add(property);
                    }
                }
            }

            if (Application.isPlaying)
            {
                _runtimeReactiveCollectionPropertyInfos.Add(_reflectionObject.GetType(), _reactiveCollectionPropertiesInfos);
            }
        }
    }
}
