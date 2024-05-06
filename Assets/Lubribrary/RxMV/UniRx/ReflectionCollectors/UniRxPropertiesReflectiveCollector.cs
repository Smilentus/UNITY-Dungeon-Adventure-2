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
    public class UniRxPropertiesReflectiveCollector : ReflectiveCollector
    {
        private Dictionary<Type, List<FieldInfo>> _reactivePropertysFieldInfos = new Dictionary<Type, List<FieldInfo>>();
        private Dictionary<Type, List<PropertyInfo>> _reactivePropertyPropertiesInfos = new Dictionary<Type, List<PropertyInfo>>();

        private Dictionary<Type, Dictionary<Type, List<FieldInfo>>> _runtimeReactivePropertysFieldInfos = new Dictionary<Type, Dictionary<Type, List<FieldInfo>>>();
        private Dictionary<Type, Dictionary<Type, List<PropertyInfo>>> _runtimeReactivePropertysPropertyInfos = new Dictionary<Type, Dictionary<Type, List<PropertyInfo>>>();


        public override void Dispose()
        {
            _reactivePropertyPropertiesInfos.Clear();
            _reactivePropertysFieldInfos.Clear();

            _runtimeReactivePropertysFieldInfos.Clear();
            _runtimeReactivePropertysPropertyInfos.Clear();


            _reactivePropertyPropertiesInfos = null;
            _reactivePropertysFieldInfos = null;

            _runtimeReactivePropertysFieldInfos = null;
            _runtimeReactivePropertysPropertyInfos = null;
        }

        public override void CollectReflections()
        {
            CollectReactivePropertyFieldAttributes();
            CollectReactivePropertyPropertyAttributes();
        }

        public Dictionary<string, ReactiveProperty<T>> ParseProperties<T>()
        {
            Dictionary<string, ReactiveProperty<T>> reactiveProperties = new Dictionary<string, ReactiveProperty<T>>();

            if (_reflectionObject != null)
            {
                foreach (var pair in _reactivePropertysFieldInfos)
                {
                    if (pair.Key == typeof(ReactiveProperty<T>) || pair.Key.IsSubclassOf(typeof(ReactiveProperty<T>)))
                    {
                        foreach (FieldInfo field in _reactivePropertysFieldInfos[pair.Key])
                        {
                            if (field.GetValue(_reflectionObject) as ReactiveProperty<T> != null)
                            {
                                reactiveProperties.Add(field.Name, field.GetValue(_reflectionObject) as ReactiveProperty<T>);
                            }
                        }
                    }
                }

                foreach (var pair in _reactivePropertyPropertiesInfos)
                {
                    if (pair.Key == typeof(ReactiveProperty<T>) || pair.Key.IsSubclassOf(typeof(ReactiveProperty<T>)))
                    {
                        foreach (PropertyInfo property in _reactivePropertyPropertiesInfos[pair.Key])
                        {
                            if (property.GetValue(_reflectionObject) as ReactiveProperty<T> != null)
                            {
                                reactiveProperties.Add(property.Name, property.GetValue(_reflectionObject) as ReactiveProperty<T>);
                            }
                        }
                    }
                }
            }

            List<string> names = reactiveProperties.Select(x => x.Key).ToList();

            foreach (string nameProperty in names)
            {
                reactiveProperties.Add($"{nameProperty} ({typeof(T).ToString().Replace($"{typeof(T).Namespace}.", "")})", reactiveProperties[nameProperty]);
                reactiveProperties.Remove(nameProperty);
            }

            return reactiveProperties;
        }

        private void CollectReactivePropertyFieldAttributes()
        {
            _reactivePropertysFieldInfos.Clear();

            if (Application.isPlaying)
            {
                if (_runtimeReactivePropertysFieldInfos.ContainsKey(_reflectionObject.GetType()))
                {
                    Dictionary<Type, List<FieldInfo>> pairs = _runtimeReactivePropertysFieldInfos[_reflectionObject.GetType()];

                    foreach (var pair in pairs)
                    {
                        if (!_reactivePropertysFieldInfos.ContainsKey(pair.Key))
                        {
                            _reactivePropertysFieldInfos.Add(pair.Key, new List<FieldInfo>());
                        }
                        _reactivePropertysFieldInfos[pair.Key].AddRange(pair.Value);
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
                    RxAdaptablePropertyAttribute adaptablePropertyAttribute = attr as RxAdaptablePropertyAttribute;
                    if (adaptablePropertyAttribute != null)
                    {
                        Type propType = field.GetValue(_reflectionObject).GetType();

                        foreach (var rpField in _reactivePropertysFieldInfos)
                        {
                            if (propType.IsSubclassOf(rpField.Key) && propType != rpField.Key)
                            {
                                propType = rpField.Key;
                            }
                        }

                        if (!_reactivePropertysFieldInfos.ContainsKey(propType))
                        {
                            _reactivePropertysFieldInfos.Add(propType, new List<FieldInfo>());
                        }

                        _reactivePropertysFieldInfos[propType].Add(field);
                    }
                }
            }

            if (Application.isPlaying)
            {
                _runtimeReactivePropertysFieldInfos.Add(_reflectionObject.GetType(), _reactivePropertysFieldInfos);
            }
        }
        private void CollectReactivePropertyPropertyAttributes()
        {
            _reactivePropertyPropertiesInfos.Clear();

            if (Application.isPlaying)
            {
                if (_runtimeReactivePropertysPropertyInfos.ContainsKey(_reflectionObject.GetType()))
                {
                    Dictionary<Type, List<PropertyInfo>> pairs = _runtimeReactivePropertysPropertyInfos[_reflectionObject.GetType()];

                    foreach (var pair in pairs)
                    {
                        if (!_reactivePropertyPropertiesInfos.ContainsKey(pair.Key))
                        {
                            _reactivePropertyPropertiesInfos.Add(pair.Key, new List<PropertyInfo>());
                        }
                        _reactivePropertyPropertiesInfos[pair.Key].AddRange(pair.Value);
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
                    RxAdaptablePropertyAttribute adaptablePropertyAttribute = attr as RxAdaptablePropertyAttribute;
                    if (adaptablePropertyAttribute != null)
                    {
                        Type propType = property.GetValue(_reflectionObject).GetType();

                        if (!_reactivePropertyPropertiesInfos.ContainsKey(propType))
                        {
                            _reactivePropertyPropertiesInfos.Add(propType, new List<PropertyInfo>());
                        }

                        _reactivePropertyPropertiesInfos[propType].Add(property);
                    }
                }
            }

            if (Application.isPlaying)
            {
                _runtimeReactivePropertysPropertyInfos.Add(_reflectionObject.GetType(), _reactivePropertyPropertiesInfos);
            }
        }
    }
}
