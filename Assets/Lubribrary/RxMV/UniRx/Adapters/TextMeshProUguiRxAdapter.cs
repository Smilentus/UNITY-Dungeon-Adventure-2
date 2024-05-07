using Dimasyechka.Lubribrary.RxMV.UniRx.Adapters.Base;
using Dimasyechka.Lubribrary.RxMV.Utilities;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.Adapters
{
    public class TextMeshProUguiRxAdapter : BaseComponentRxAdapter<TextMeshProUGUI>
    {
        [Header("Single Text Settings")]
        [SerializeField]
        private UniRxReflectionField _text = new UniRxReflectionField();


        [Header("Multiple Text Settings")]
        [SerializeField]
        private bool _useMultipleText;

        [SerializeField]
        private string _multipleTextFormat;

        [SerializeField]
        private List<UniRxReflectionField> _multipleText = new List<UniRxReflectionField>();


        private List<string> _multipleTextOutput = new List<string>(4);


        protected override void OnParseReflections()
        {
            foreach (UniRxReflectionField field in _multipleText)
            {
                ParseReflectionsToReflectionField(field);
            }

            ParseReflectionsToReflectionField(_text);
        }

        protected override void OnConnectedToView()
        {
            if (_useMultipleText)
            {
                InitializeMultipleTextOutput();

                for (int i = 0; i < _multipleText.Count; i++)
                {
                    ConnectMultipleOutputField(i, _multipleText[i]);
                }
            }
            else
            {
                ConnectSingleOutputField(_text);
            }
        }

        private void ParseReflectionsToReflectionField(UniRxReflectionField reflectionField)
        {
            Dictionary<string, ReactiveProperty<string>> propertyString = _reflectionStorage.GetProperties<string>();
            Dictionary<string, ReactiveProperty<int>> propertyInt = _reflectionStorage.GetProperties<int>();
            Dictionary<string, ReactiveProperty<float>> propertyFloat = _reflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<double>> propertyDouble = _reflectionStorage.GetProperties<double>();
            Dictionary<string, ReactiveProperty<long>> propertyLong = _reflectionStorage.GetProperties<long>();
            Dictionary<string, ReactiveProperty<ulong>> propertyUlong = _reflectionStorage.GetProperties<ulong>();
            Dictionary<string, ReactiveProperty<short>> propertyShort = _reflectionStorage.GetProperties<short>();
            Dictionary<string, ReactiveProperty<ushort>> propertyUshort = _reflectionStorage.GetProperties<ushort>();
            Dictionary<string, ReactiveProperty<byte>> propertyByte = _reflectionStorage.GetProperties<byte>();
            Dictionary<string, ReactiveProperty<sbyte>> propertySbyte = _reflectionStorage.GetProperties<sbyte>();

            reflectionField.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>() {
                propertyString.Select(x => x.Key).ToArray(),
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
        }


        private void InitializeMultipleTextOutput()
        {
            _multipleTextOutput = new List<string>(4);
            for (int i = 0; i < _multipleText.Count; i++)
            {
                _multipleTextOutput.Add("");
            }
        }


        private void ConnectSingleOutputField(UniRxReflectionField reflectionField)
        {
            Dictionary<string, ReactiveProperty<string>> propertyString = _reflectionStorage.GetProperties<string>();
            Dictionary<string, ReactiveProperty<int>> propertyInt = _reflectionStorage.GetProperties<int>();
            Dictionary<string, ReactiveProperty<float>> propertyFloat = _reflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<double>> propertyDouble = _reflectionStorage.GetProperties<double>();
            Dictionary<string, ReactiveProperty<long>> propertyLong = _reflectionStorage.GetProperties<long>();
            Dictionary<string, ReactiveProperty<ulong>> propertyUlong = _reflectionStorage.GetProperties<ulong>();
            Dictionary<string, ReactiveProperty<short>> propertyShort = _reflectionStorage.GetProperties<short>();
            Dictionary<string, ReactiveProperty<ushort>> propertyUshort = _reflectionStorage.GetProperties<ushort>();
            Dictionary<string, ReactiveProperty<byte>> propertyByte = _reflectionStorage.GetProperties<byte>();
            Dictionary<string, ReactiveProperty<sbyte>> propertySbyte = _reflectionStorage.GetProperties<sbyte>();


            if (propertyString.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyString[reflectionField.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }

            if (propertyInt.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyInt[reflectionField.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }

            if (propertyFloat.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyFloat[reflectionField.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }

            if (propertyDouble.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyDouble[reflectionField.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }

            if (propertyLong.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyLong[reflectionField.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }

            if (propertyUlong.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyUlong[reflectionField.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }

            if (propertyShort.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyShort[reflectionField.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }

            if (propertyUshort.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyUshort[reflectionField.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }

            if (propertyByte.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyByte[reflectionField.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }

            if (propertySbyte.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertySbyte[reflectionField.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }
        }

        private void ConnectMultipleOutputField(int outputIndex, UniRxReflectionField reflectionField)
        {
            Dictionary<string, ReactiveProperty<string>> propertyString = _reflectionStorage.GetProperties<string>();
            Dictionary<string, ReactiveProperty<int>> propertyInt = _reflectionStorage.GetProperties<int>();
            Dictionary<string, ReactiveProperty<float>> propertyFloat = _reflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<double>> propertyDouble = _reflectionStorage.GetProperties<double>();
            Dictionary<string, ReactiveProperty<long>> propertyLong = _reflectionStorage.GetProperties<long>();
            Dictionary<string, ReactiveProperty<ulong>> propertyUlong = _reflectionStorage.GetProperties<ulong>();
            Dictionary<string, ReactiveProperty<short>> propertyShort = _reflectionStorage.GetProperties<short>();
            Dictionary<string, ReactiveProperty<ushort>> propertyUshort = _reflectionStorage.GetProperties<ushort>();
            Dictionary<string, ReactiveProperty<byte>> propertyByte = _reflectionStorage.GetProperties<byte>();
            Dictionary<string, ReactiveProperty<sbyte>> propertySbyte = _reflectionStorage.GetProperties<sbyte>();


            if (propertyString.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyString[reflectionField.SelectedName].Subscribe(x =>
                {
                    _multipleTextOutput[outputIndex] = ((x != null) ? x.ToString() : string.Empty);
                    UpdateComponentWithMultipleOutput();
                }));
            }

            if (propertyInt.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyInt[reflectionField.SelectedName].Subscribe(x =>
                {
                    _multipleTextOutput[outputIndex] = ((x != null) ? x.ToString("f0") : string.Empty);
                    UpdateComponentWithMultipleOutput();
                }));
            }

            if (propertyFloat.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyFloat[reflectionField.SelectedName].Subscribe(x =>
                {
                    _multipleTextOutput[outputIndex] = ((x != null) ? x.ToString("f0") : string.Empty);
                    UpdateComponentWithMultipleOutput();
                }));
            }

            if (propertyDouble.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyDouble[reflectionField.SelectedName].Subscribe(x =>
                {
                    _multipleTextOutput[outputIndex] = ((x != null) ? x.ToString("f0") : string.Empty);
                    UpdateComponentWithMultipleOutput();
                }));
            }

            if (propertyLong.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyLong[reflectionField.SelectedName].Subscribe(x =>
                {
                    _multipleTextOutput[outputIndex] = ((x != null) ? x.ToString("f0") : string.Empty);
                    UpdateComponentWithMultipleOutput();
                }));
            }

            if (propertyUlong.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyUlong[reflectionField.SelectedName].Subscribe(x =>
                {
                    _multipleTextOutput[outputIndex] = ((x != null) ? x.ToString("f0") : string.Empty);
                    UpdateComponentWithMultipleOutput();
                }));
            }

            if (propertyShort.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyShort[reflectionField.SelectedName].Subscribe(x =>
                {
                    _multipleTextOutput[outputIndex] = ((x != null) ? x.ToString("f0") : string.Empty);
                    UpdateComponentWithMultipleOutput();
                }));
            }

            if (propertyUshort.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyUshort[reflectionField.SelectedName].Subscribe(x =>
                {
                    _multipleTextOutput[outputIndex] = ((x != null) ? x.ToString("f0") : string.Empty);
                    UpdateComponentWithMultipleOutput();
                }));
            }

            if (propertyByte.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertyByte[reflectionField.SelectedName].Subscribe(x =>
                {
                    _multipleTextOutput[outputIndex] = ((x != null) ? x.ToString("f0") : string.Empty);
                    UpdateComponentWithMultipleOutput();
                }));
            }

            if (propertySbyte.ContainsKey(reflectionField.SelectedName) && reflectionField.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertySbyte[reflectionField.SelectedName].Subscribe(x =>
                {
                    _multipleTextOutput[outputIndex] = ((x != null) ? x.ToString("f0") : string.Empty);
                    UpdateComponentWithMultipleOutput();
                }));
            }
        }


        private void UpdateComponentWithMultipleOutput()
        {
            if (string.IsNullOrWhiteSpace(_multipleTextFormat))
            {
                BuildDefaultMultipleOutput();
            }
            else
            {
                BuildMultipleOutputWithFormat();
            }
        }


        private void BuildDefaultMultipleOutput()
        {
            string output = "";

            foreach (string line in _multipleTextOutput)
            {
                output += line;
            }

            _component.text = output;
        }

        private void BuildMultipleOutputWithFormat()
        {
            Dictionary<string, int> evaluationPoints = ParseEvaluationPoints();

            string output = _multipleTextFormat;

            foreach (KeyValuePair<string, int> kvp in evaluationPoints)
            {
                output = output.Replace(kvp.Key, _multipleTextOutput[kvp.Value]);
            }

            _component.text = output;
        }


        private Dictionary<string, int> ParseEvaluationPoints()
        {
            Dictionary<string, int> output = new Dictionary<string, int>();

            for (int i = 0; i < _multipleText.Count; i++)
            {
                if (_multipleText[i].SelectedIndex != -1)
                {
                    output.Add($"{{{i}}}", i);
                }
            }

            return output;
        }
    }
}
