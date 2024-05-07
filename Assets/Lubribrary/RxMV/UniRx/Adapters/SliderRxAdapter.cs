using Dimasyechka.Lubribrary.RxMV.UniRx.Adapters.Base;
using Dimasyechka.Lubribrary.RxMV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.Adapters
{
    public class SliderRxAdapter : BaseComponentRxAdapter<Slider>
    {
        [SerializeField]
        private UniRxReflectionField _interactable = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _value = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _minValue = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _maxValue = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _useWholeNumbers = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _onValueChangedCallback = new UniRxReflectionField();


        protected override void OnParseReflections()
        {
            Dictionary<string, ReactiveProperty<int>> propertyInt = _reflectionStorage.GetProperties<int>();
            Dictionary<string, ReactiveProperty<float>> propertyFloat = _reflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<double>> propertyDouble = _reflectionStorage.GetProperties<double>();
            Dictionary<string, ReactiveProperty<long>> propertyLong = _reflectionStorage.GetProperties<long>();
            Dictionary<string, ReactiveProperty<ulong>> propertyUlong = _reflectionStorage.GetProperties<ulong>();
            Dictionary<string, ReactiveProperty<short>> propertyShort = _reflectionStorage.GetProperties<short>();
            Dictionary<string, ReactiveProperty<ushort>> propertyUshort = _reflectionStorage.GetProperties<ushort>();
            Dictionary<string, ReactiveProperty<byte>> propertyByte = _reflectionStorage.GetProperties<byte>();
            Dictionary<string, ReactiveProperty<sbyte>> propertySbyte = _reflectionStorage.GetProperties<sbyte>();

            Dictionary<string, ReactiveProperty<bool>> propertyBool = _reflectionStorage.GetProperties<bool>();

            Dictionary<string, Action<int>> delegateInt = _reflectionStorage.GetDelegates<int>();
            Dictionary<string, Action<float>> delegateFloat = _reflectionStorage.GetDelegates<float>();
            Dictionary<string, Action<double>> delegateDouble = _reflectionStorage.GetDelegates<double>();
            Dictionary<string, Action<long>> delegateLong = _reflectionStorage.GetDelegates<long>();
            Dictionary<string, Action<ulong>> delegateUlong = _reflectionStorage.GetDelegates<ulong>();
            Dictionary<string, Action<short>> delegateShort = _reflectionStorage.GetDelegates<short>();
            Dictionary<string, Action<ushort>> delegateUshort = _reflectionStorage.GetDelegates<ushort>();
            Dictionary<string, Action<byte>> delegateByte = _reflectionStorage.GetDelegates<byte>();
            Dictionary<string, Action<sbyte>> delegateSbyte = _reflectionStorage.GetDelegates<sbyte>();

            string[] propertyNames = _reflectionStorage.GetDictionaryNames(new List<string[]>() {
                propertyInt.Select(x => x.Key).ToArray(),
                propertyFloat.Select(x => x.Key).ToArray(),
                propertyDouble.Select(x => x.Key).ToArray(),
                propertyLong.Select(x => x.Key).ToArray(),
                propertyUlong.Select(x => x.Key).ToArray(),
                propertyShort.Select(x => x.Key).ToArray(),
                propertyUshort.Select(x => x.Key).ToArray(),
                propertyByte.Select(x => x.Key).ToArray(),
                propertySbyte.Select(x => x.Key).ToArray()
            });

            string[] delegateNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                delegateInt.Select(x => x.Key).ToArray(),
                delegateFloat.Select(x => x.Key).ToArray(),
                delegateDouble.Select(x => x.Key).ToArray(),
                delegateLong.Select(x => x.Key).ToArray(),
                delegateUlong.Select(x => x.Key).ToArray(),
                delegateShort.Select(x => x.Key).ToArray(),
                delegateUshort.Select(x => x.Key).ToArray(),
                delegateByte.Select(x => x.Key).ToArray(),
                delegateSbyte.Select(x => x.Key).ToArray(),
            });

            _value.ReflectiveNames = propertyNames;
            _minValue.ReflectiveNames = propertyNames;
            _maxValue.ReflectiveNames = propertyNames;

            _onValueChangedCallback.ReflectiveNames = delegateNames;

            _useWholeNumbers.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>() {
                propertyBool.Select(x => x.Key).ToArray()
            });

            _interactable.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                propertyBool.Select(x => x.Key).ToArray()
            });
        }

        protected override void OnConnectedToView()
        {
            Dictionary<string, ReactiveProperty<int>> propertyInt = _reflectionStorage.GetProperties<int>();
            Dictionary<string, ReactiveProperty<float>> propertyFloat = _reflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<double>> propertyDouble = _reflectionStorage.GetProperties<double>();
            Dictionary<string, ReactiveProperty<long>> propertyLong = _reflectionStorage.GetProperties<long>();
            Dictionary<string, ReactiveProperty<ulong>> propertyUlong = _reflectionStorage.GetProperties<ulong>();
            Dictionary<string, ReactiveProperty<short>> propertyShort = _reflectionStorage.GetProperties<short>();
            Dictionary<string, ReactiveProperty<ushort>> propertyUshort = _reflectionStorage.GetProperties<ushort>();
            Dictionary<string, ReactiveProperty<byte>> propertyByte = _reflectionStorage.GetProperties<byte>();
            Dictionary<string, ReactiveProperty<sbyte>> propertySbyte = _reflectionStorage.GetProperties<sbyte>();

            Dictionary<string, ReactiveProperty<bool>> propertyBool = _reflectionStorage.GetProperties<bool>();

            Dictionary<string, Action<int>> delegateInt = _reflectionStorage.GetDelegates<int>();
            Dictionary<string, Action<float>> delegateFloat = _reflectionStorage.GetDelegates<float>();
            Dictionary<string, Action<double>> delegateDouble = _reflectionStorage.GetDelegates<double>();
            Dictionary<string, Action<long>> delegateLong = _reflectionStorage.GetDelegates<long>();
            Dictionary<string, Action<ulong>> delegateUlong = _reflectionStorage.GetDelegates<ulong>();
            Dictionary<string, Action<short>> delegateShort = _reflectionStorage.GetDelegates<short>();
            Dictionary<string, Action<ushort>> delegateUshort = _reflectionStorage.GetDelegates<ushort>();
            Dictionary<string, Action<byte>> delegateByte = _reflectionStorage.GetDelegates<byte>();
            Dictionary<string, Action<sbyte>> delegateSbyte = _reflectionStorage.GetDelegates<sbyte>();


            if (propertyBool.ContainsKey(_useWholeNumbers.SelectedName) && _useWholeNumbers.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyBool[_useWholeNumbers.SelectedName].Subscribe(x =>
                {
                    _component.wholeNumbers = x;
                }));
            }

            if (propertyBool.ContainsKey(_interactable.SelectedName) && _interactable.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyBool[_interactable.SelectedName].Subscribe(x =>
                {
                    _component.interactable = x;
                }));
            }


            if (propertyInt.ContainsKey(_minValue.SelectedName) && _minValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyInt[_minValue.SelectedName].Subscribe(x =>
                {
                    _component.minValue = (float)x;
                }));
            }

            if (propertyFloat.ContainsKey(_minValue.SelectedName) && _minValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyFloat[_minValue.SelectedName].Subscribe(x =>
                {
                    _component.minValue = x;
                }));
            }

            if (propertyDouble.ContainsKey(_minValue.SelectedName) && _minValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyDouble[_minValue.SelectedName].Subscribe(x =>
                {
                    _component.minValue = (float)x;
                }));
            }

            if (propertyLong.ContainsKey(_minValue.SelectedName) && _minValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyLong[_minValue.SelectedName].Subscribe(x =>
                {
                    _component.minValue = (float)x;
                }));
            }

            if (propertyUlong.ContainsKey(_minValue.SelectedName) && _minValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyUlong[_minValue.SelectedName].Subscribe(x =>
                {
                    _component.minValue = (float)x;
                }));
            }

            if (propertyShort.ContainsKey(_minValue.SelectedName) && _minValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyShort[_minValue.SelectedName].Subscribe(x =>
                {
                    _component.minValue = (float)x;
                }));
            }

            if (propertyUshort.ContainsKey(_minValue.SelectedName) && _minValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyUshort[_minValue.SelectedName].Subscribe(x =>
                {
                    _component.minValue = (float)x;
                }));
            }

            if (propertyByte.ContainsKey(_minValue.SelectedName) && _minValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyByte[_minValue.SelectedName].Subscribe(x =>
                {
                    _component.minValue = (float)x;
                }));
            }

            if (propertySbyte.ContainsKey(_minValue.SelectedName) && _minValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertySbyte[_minValue.SelectedName].Subscribe(x =>
                {
                    _component.minValue = (float)x;
                }));
            }


            if (propertyInt.ContainsKey(_maxValue.SelectedName) && _maxValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyInt[_maxValue.SelectedName].Subscribe(x =>
                {
                    _component.maxValue = (float)x;
                }));
            }

            if (propertyFloat.ContainsKey(_maxValue.SelectedName) && _maxValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyFloat[_maxValue.SelectedName].Subscribe(x =>
                {
                    _component.maxValue = x;
                }));
            }

            if (propertyDouble.ContainsKey(_maxValue.SelectedName) && _maxValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyDouble[_maxValue.SelectedName].Subscribe(x =>
                {
                    _component.maxValue = (float)x;
                }));
            }

            if (propertyLong.ContainsKey(_maxValue.SelectedName) && _maxValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyLong[_minValue.SelectedName].Subscribe(x =>
                {
                    _component.maxValue = (float)x;
                }));
            }

            if (propertyUlong.ContainsKey(_maxValue.SelectedName) && _maxValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyUlong[_maxValue.SelectedName].Subscribe(x =>
                {
                    _component.maxValue = (float)x;
                }));
            }

            if (propertyShort.ContainsKey(_maxValue.SelectedName) && _maxValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyShort[_maxValue.SelectedName].Subscribe(x =>
                {
                    _component.maxValue = (float)x;
                }));
            }

            if (propertyUshort.ContainsKey(_maxValue.SelectedName) && _maxValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyUshort[_maxValue.SelectedName].Subscribe(x =>
                {
                    _component.maxValue = (float)x;
                }));
            }

            if (propertyByte.ContainsKey(_maxValue.SelectedName) && _maxValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyByte[_maxValue.SelectedName].Subscribe(x =>
                {
                    _component.maxValue = (float)x;
                }));
            }

            if (propertySbyte.ContainsKey(_maxValue.SelectedName) && _maxValue.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertySbyte[_maxValue.SelectedName].Subscribe(x =>
                {
                    _component.maxValue = (float)x;
                }));
            }


            if (propertyInt.ContainsKey(_value.SelectedName) && _value.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyInt[_value.SelectedName].Subscribe(x =>
                {
                    _component.value = (float)x;
                }));
            }

            if (propertyFloat.ContainsKey(_value.SelectedName) && _value.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyFloat[_value.SelectedName].Subscribe(x =>
                {
                    _component.value = x;
                }));
            }

            if (propertyDouble.ContainsKey(_value.SelectedName) && _value.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyDouble[_value.SelectedName].Subscribe(x =>
                {
                    _component.value = (float)x;
                }));
            }

            if (propertyLong.ContainsKey(_value.SelectedName) && _value.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyLong[_minValue.SelectedName].Subscribe(x =>
                {
                    _component.value = (float)x;
                }));
            }

            if (propertyUlong.ContainsKey(_value.SelectedName) && _value.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyUlong[_value.SelectedName].Subscribe(x =>
                {
                    _component.value = (float)x;
                }));
            }

            if (propertyShort.ContainsKey(_value.SelectedName) && _value.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyShort[_value.SelectedName].Subscribe(x =>
                {
                    _component.value = (float)x;
                }));
            }

            if (propertyUshort.ContainsKey(_value.SelectedName) && _value.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyUshort[_value.SelectedName].Subscribe(x =>
                {
                    _component.value = (float)x;
                }));
            }

            if (propertyByte.ContainsKey(_value.SelectedName) && _value.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyByte[_value.SelectedName].Subscribe(x =>
                {
                    _component.value = (float)x;
                }));
            }

            if (propertySbyte.ContainsKey(_value.SelectedName) && _value.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertySbyte[_value.SelectedName].Subscribe(x =>
                {
                    _component.value = (float)x;
                }));
            }


            if (delegateInt.ContainsKey(_onValueChangedCallback.SelectedName) && _onValueChangedCallback.SelectedIndex != -1)
            {
                _component.onValueChanged.AddListener((x) =>
                {
                    _reflectionStorage.InvokeDelegate(_onValueChangedCallback.SelectedName, (int)x);
                });
            }

            if (delegateFloat.ContainsKey(_onValueChangedCallback.SelectedName) && _onValueChangedCallback.SelectedIndex != -1)
            {
                _component.onValueChanged.AddListener((x) =>
                {
                    _reflectionStorage.InvokeDelegate(_onValueChangedCallback.SelectedName, x);
                });
            }

            if (delegateDouble.ContainsKey(_onValueChangedCallback.SelectedName) && _onValueChangedCallback.SelectedIndex != -1)
            {
                _component.onValueChanged.AddListener((x) =>
                {
                    _reflectionStorage.InvokeDelegate(_onValueChangedCallback.SelectedName, (double)x);
                });
            }

            if (delegateLong.ContainsKey(_onValueChangedCallback.SelectedName) && _onValueChangedCallback.SelectedIndex != -1)
            {
                _component.onValueChanged.AddListener((x) =>
                {
                    _reflectionStorage.InvokeDelegate(_onValueChangedCallback.SelectedName, (long)x);
                });
            }

            if (delegateUlong.ContainsKey(_onValueChangedCallback.SelectedName) && _onValueChangedCallback.SelectedIndex != -1)
            {
                _component.onValueChanged.AddListener((x) =>
                {
                    _reflectionStorage.InvokeDelegate(_onValueChangedCallback.SelectedName, (ulong)x);
                });
            }

            if (delegateShort.ContainsKey(_onValueChangedCallback.SelectedName) && _onValueChangedCallback.SelectedIndex != -1)
            {
                _component.onValueChanged.AddListener((x) =>
                {
                    _reflectionStorage.InvokeDelegate(_onValueChangedCallback.SelectedName, (short)x);
                });
            }

            if (delegateUshort.ContainsKey(_onValueChangedCallback.SelectedName) && _onValueChangedCallback.SelectedIndex != -1)
            {
                _component.onValueChanged.AddListener((x) =>
                {
                    _reflectionStorage.InvokeDelegate(_onValueChangedCallback.SelectedName, (ushort)x);
                });
            }

            if (delegateByte.ContainsKey(_onValueChangedCallback.SelectedName) && _onValueChangedCallback.SelectedIndex != -1)
            {
                _component.onValueChanged.AddListener((x) =>
                {
                    _reflectionStorage.InvokeDelegate(_onValueChangedCallback.SelectedName, (byte)x);
                });
            }

            if (delegateSbyte.ContainsKey(_onValueChangedCallback.SelectedName) && _onValueChangedCallback.SelectedIndex != -1)
            {
                _component.onValueChanged.AddListener((x) =>
                {
                    _reflectionStorage.InvokeDelegate(_onValueChangedCallback.SelectedName, (sbyte)x);
                });
            }
        }
    }
}
