using System.Collections.Generic;
using System.Linq;
using Dimasyechka.Lubribrary.RxMV.UniRx.Adapters.Base;
using Dimasyechka.Lubribrary.RxMV.Utilities;
using TMPro;
using UniRx;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.Adapters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextMeshProUguiRxAdapter : BaseComponentRxAdapter<TextMeshProUGUI>
    {
        [SerializeField]
        private UniRxReflectionField _text = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _fontSize = new UniRxReflectionField();


        protected override void OnConnectedToView()
        {
            Dictionary<string, ReactiveProperty<int>> propertiesInt = _reflectionStorage.GetProperties<int>();
            Dictionary<string, ReactiveProperty<float>> propertiesFloat = _reflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<double>> propertiesDouble = _reflectionStorage.GetProperties<double>();
            Dictionary<string, ReactiveProperty<string>> propertiesString = _reflectionStorage.GetProperties<string>();


            if (propertiesInt.ContainsKey(_fontSize.SelectedName) && _fontSize.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertiesInt[_fontSize.SelectedName].Subscribe(x =>
                {
                    _component.fontSize = x;
                }));
            }

            if (propertiesInt.ContainsKey(_text.SelectedName) && _text.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertiesInt[_text.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }

            if (propertiesFloat.ContainsKey(_text.SelectedName) && _text.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertiesFloat[_text.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }

            if (propertiesDouble.ContainsKey(_text.SelectedName) && _text.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertiesDouble[_text.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }

            if (propertiesString.ContainsKey(_text.SelectedName) && _text.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(propertiesString[_text.SelectedName].Subscribe(x =>
                {
                    _component.text = ((x != null) ? x.ToString() : string.Empty);
                }));
            }
        }
        
        protected override void OnParseReflections()
        {
            Dictionary<string, ReactiveProperty<int>> propertiesInt = _reflectionStorage.GetProperties<int>();
            Dictionary<string, ReactiveProperty<float>> propertiesFloat = _reflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<double>> propertessDouble = _reflectionStorage.GetProperties<double>();
            Dictionary<string, ReactiveProperty<string>> propertessString = _reflectionStorage.GetProperties<string>();


            _text.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>() {
                propertiesInt.Select(x => x.Key).ToArray(),
                propertiesFloat.Select(x => x.Key).ToArray(),
                propertessDouble.Select(x => x.Key).ToArray(),
                propertessString.Select(x => x.Key).ToArray()
            });
        }
    }
}
