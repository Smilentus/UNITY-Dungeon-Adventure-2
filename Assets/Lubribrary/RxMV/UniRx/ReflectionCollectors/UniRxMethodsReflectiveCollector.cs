using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using Dimasyechka.Lubribrary.RxMV.UniRx.ReflectionCollectors.Base;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.ReflectionCollectors
{
    public class UniRxMethodsReflectiveCollector : ReflectiveCollector
    {
        private Dictionary<Type, List<MethodInfo>> _methodInfos = new Dictionary<Type, List<MethodInfo>>();

        private Dictionary<Type, Dictionary<Type, List<MethodInfo>>> _runtimeMethodInfos = new Dictionary<Type, Dictionary<Type, List<MethodInfo>>>();


        public override void Dispose()
        {
            _methodInfos.Clear();
            _runtimeMethodInfos.Clear();

            _methodInfos = null;
            _runtimeMethodInfos = null;
        }

        public override void CollectReflections()
        {
            CollectMethodAttributes();
        }

        public Dictionary<string, Action<T>> ParseDelegates<T>()
        {
            Dictionary<string, Action<T>> delegates = new Dictionary<string, Action<T>>();

            if (_reflectionObject != null)
            {
                foreach (var pair in _methodInfos)
                {
                    if (pair.Key == typeof(T) || pair.Key.IsSubclassOf(typeof(T)))
                    {
                        foreach (MethodInfo method in _methodInfos[pair.Key])
                        {
                            ParameterInfo[] parameters = method.GetParameters();
                            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(T))
                            {
                                delegates.Add(method.Name, method.CreateDelegate(typeof(Action<T>), _reflectionObject) as Action<T>);
                            }
                        }
                    }
                }
            }

            string[] names = delegates.Select(x => x.Key).ToArray();

            foreach (string nameDelegete in names)
            {
                delegates.Add($"{nameDelegete} ({typeof(T).ToString().Replace($"{typeof(T).Namespace}.", "")})", delegates[nameDelegete]);
                delegates.Remove(nameDelegete);
            }

            return delegates;
        }
        public Dictionary<string, Action> ParseDelegates()
        {
            Dictionary<string, Action> delegates = new Dictionary<string, Action>();

            if (_reflectionObject != null)
            {
                foreach (var pair in _methodInfos)
                {
                    if (pair.Key == typeof(void))
                    {
                        foreach (MethodInfo method in _methodInfos[pair.Key])
                        {
                            ParameterInfo[] parameters = method.GetParameters();
                            if (parameters.Length == 0)
                            {
                                delegates.Add(method.Name, method.CreateDelegate(typeof(Action), _reflectionObject) as Action);
                            }
                        }
                    }
                }
            }

            string[] names = delegates.Select(x => x.Key).ToArray();

            foreach (string nameDelegete in names)
            {
                delegates.Add($"{nameDelegete} (void)", delegates[nameDelegete]);
                delegates.Remove(nameDelegete);
            }

            return delegates;
        }

        private void CollectMethodAttributes()
        {
            _methodInfos.Clear();

            if (Application.isPlaying)
            {
                if (_runtimeMethodInfos.ContainsKey(_reflectionObject.GetType()))
                {
                    Dictionary<Type, List<MethodInfo>> pairs = _runtimeMethodInfos[_reflectionObject.GetType()];

                    foreach (var pair in pairs)
                    {
                        if (!_methodInfos.ContainsKey(pair.Key))
                        {
                            _methodInfos.Add(pair.Key, new List<MethodInfo>());
                        }
                        _methodInfos[pair.Key].AddRange(pair.Value);
                    }
                    return;
                }
            }

            MethodInfo[] actions = _reflectionObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (MethodInfo action in actions)
            {
                object[] attrs = action.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    RxAdaptableMethodAttribute uniRxAdaptableMethodAttribute = attr as RxAdaptableMethodAttribute;
                    if (uniRxAdaptableMethodAttribute != null)
                    {
                        ParameterInfo[] parameters = action.GetParameters();
                        Type parameterType = typeof(void);

                        if (parameters.Length == 1)
                        {
                            parameterType = parameters[0].ParameterType;
                        }

                        if (parameters.Length > 1)
                        {
                            continue;
                        }

                        if (!_methodInfos.ContainsKey(parameterType))
                        {
                            _methodInfos.Add(parameterType, new List<MethodInfo>());
                        }

                        _methodInfos[parameterType].Add(action);
                    }
                }
            }

            if (Application.isPlaying)
            {
                _runtimeMethodInfos.Add(_reflectionObject.GetType(), _methodInfos);
            }
        }
    }
}
