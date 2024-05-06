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
    public class UniRxCommandReflectiveCollector : ReflectiveCollector
    {
        private Dictionary<Type, List<FieldInfo>> _reactiveCommandsFieldInfos = new Dictionary<Type, List<FieldInfo>>();
        private Dictionary<Type, List<PropertyInfo>> _reactiveCommandsPropertiesInfos = new Dictionary<Type, List<PropertyInfo>>();


        private Dictionary<Type, Dictionary<Type, List<FieldInfo>>> _runtimeReactiveCommandsFieldInfos = new Dictionary<Type, Dictionary<Type, List<FieldInfo>>>();
        private Dictionary<Type, Dictionary<Type, List<PropertyInfo>>> _runtimeReactiveCommandsPropertyInfos = new Dictionary<Type, Dictionary<Type, List<PropertyInfo>>>();


        public override void Dispose()
        {
            _reactiveCommandsFieldInfos.Clear();
            _reactiveCommandsPropertiesInfos.Clear();

            _runtimeReactiveCommandsFieldInfos.Clear();
            _runtimeReactiveCommandsPropertyInfos.Clear();


            _reactiveCommandsFieldInfos = null;
            _reactiveCommandsPropertiesInfos = null;

            _runtimeReactiveCommandsFieldInfos = null;
            _runtimeReactiveCommandsPropertyInfos = null;
        }

        public override void CollectReflections()
        {
            CollectReactiveCommandFieldAttributes();
            CollectReactiveCommandPropertyAttributes();
        }

        public Dictionary<string, ReactiveCommand> ParseCommands()
        {
            Dictionary<string, ReactiveCommand> reactiveCommands = new Dictionary<string, ReactiveCommand>();

            if (_reflectionObject != null)
            {
                foreach (var pair in _reactiveCommandsFieldInfos)
                {
                    if (pair.Key == typeof(ReactiveCommand) || pair.Key.IsSubclassOf(typeof(ReactiveCommand)))
                    {
                        foreach (FieldInfo field in _reactiveCommandsFieldInfos[pair.Key])
                        {
                            if (field.GetValue(_reflectionObject) as ReactiveCommand != null)
                            {
                                reactiveCommands.Add(field.Name, field.GetValue(_reflectionObject) as ReactiveCommand);
                            }
                        }
                    }
                }

                foreach (var pair in _reactiveCommandsPropertiesInfos)
                {
                    if (pair.Key == typeof(ReactiveCommand) || pair.Key.IsSubclassOf(typeof(ReactiveCommand)))
                    {
                        foreach (PropertyInfo property in _reactiveCommandsPropertiesInfos[pair.Key])
                        {
                            if (property.GetValue(_reflectionObject) as ReactiveCommand != null)
                            {
                                reactiveCommands.Add(property.Name, property.GetValue(_reflectionObject) as ReactiveCommand);
                            }
                        }
                    }
                }
            }

            List<string> names = reactiveCommands.Select(x => x.Key).ToList();

            foreach (string nameProperty in names)
            {
                reactiveCommands.Add($"{nameProperty} (CMD)", reactiveCommands[nameProperty]);
                reactiveCommands.Remove(nameProperty);
            }

            return reactiveCommands;
        }

        private void CollectReactiveCommandFieldAttributes()
        {
            _reactiveCommandsFieldInfos.Clear();

            if (Application.isPlaying)
            {
                if (_runtimeReactiveCommandsFieldInfos.ContainsKey(_reflectionObject.GetType()))
                {
                    Dictionary<Type, List<FieldInfo>> pairs = _runtimeReactiveCommandsFieldInfos[_reflectionObject.GetType()];

                    foreach (var pair in pairs)
                    {
                        if (!_reactiveCommandsFieldInfos.ContainsKey(pair.Key))
                        {
                            _reactiveCommandsFieldInfos.Add(pair.Key, new List<FieldInfo>());
                        }
                        _reactiveCommandsFieldInfos[pair.Key].AddRange(pair.Value);
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
                    RxAdaptableCommandAttribute propertyAttribute = attr as RxAdaptableCommandAttribute;
                    if (propertyAttribute != null)
                    {
                        Type propType = field.GetValue(_reflectionObject).GetType();

                        foreach (var rpField in _reactiveCommandsFieldInfos)
                        {
                            if (propType.IsSubclassOf(rpField.Key) && propType != rpField.Key)
                            {
                                propType = rpField.Key;
                            }
                        }

                        if (!_reactiveCommandsFieldInfos.ContainsKey(propType))
                        {
                            _reactiveCommandsFieldInfos.Add(propType, new List<FieldInfo>());
                        }

                        _reactiveCommandsFieldInfos[propType].Add(field);
                    }
                }
            }

            if (Application.isPlaying)
            {
                _runtimeReactiveCommandsFieldInfos.Add(_reflectionObject.GetType(), _reactiveCommandsFieldInfos);
            }
        }
        private void CollectReactiveCommandPropertyAttributes()
        {
            _reactiveCommandsPropertiesInfos.Clear();

            if (Application.isPlaying)
            {
                if (_runtimeReactiveCommandsPropertyInfos.ContainsKey(_reflectionObject.GetType()))
                {
                    Dictionary<Type, List<PropertyInfo>> pairs = _runtimeReactiveCommandsPropertyInfos[_reflectionObject.GetType()];

                    foreach (var pair in pairs)
                    {
                        if (!_reactiveCommandsPropertiesInfos.ContainsKey(pair.Key))
                        {
                            _reactiveCommandsPropertiesInfos.Add(pair.Key, new List<PropertyInfo>());
                        }
                        _reactiveCommandsPropertiesInfos[pair.Key].AddRange(pair.Value);
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
                    RxAdaptableCommandAttribute propertyAttribute = attr as RxAdaptableCommandAttribute;
                    if (propertyAttribute != null)
                    {
                        Type propType = property.GetValue(_reflectionObject).GetType();

                        if (!_reactiveCommandsPropertiesInfos.ContainsKey(propType))
                        {
                            _reactiveCommandsPropertiesInfos.Add(propType, new List<PropertyInfo>());
                        }

                        _reactiveCommandsPropertiesInfos[propType].Add(property);
                    }
                }
            }

            if (Application.isPlaying)
            {
                _runtimeReactiveCommandsPropertyInfos.Add(_reflectionObject.GetType(), _reactiveCommandsPropertiesInfos);
            }
        }
    }
}
