using Dimasyechka.Lubribrary.RxMV.UniRx.Adapters.Base;
using Dimasyechka.Lubribrary.RxMV.Utilities;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.Adapters
{
    public class GameObjectRxAdapter : BaseRxAdapter
    {
        [SerializeField]
        private UniRxReflectionField _activeState = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _tag = new UniRxReflectionField();
        
        [SerializeField]
        private UniRxReflectionField _layer = new UniRxReflectionField();


        protected override void OnParseReflections()
        {
            Dictionary<string, ReactiveProperty<bool>> activeStateProperty = _reflectionStorage.GetProperties<bool>();
            Dictionary<string, ReactiveProperty<string>> tagProperty = _reflectionStorage.GetProperties<string>();
            Dictionary<string, ReactiveProperty<int>> layerProperty = _reflectionStorage.GetProperties<int>();

            _activeState.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                activeStateProperty.Select(x => x.Key).ToArray(),
            });

            _tag.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                tagProperty.Select(x => x.Key).ToArray()
            });

            _layer.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                layerProperty.Select(x => x.Key).ToArray()
            });
        }

        protected override void OnConnectedToView()
        {
            Dictionary<string, ReactiveProperty<bool>> activeStateProperty = _reflectionStorage.GetProperties<bool>();
            Dictionary<string, ReactiveProperty<string>> tagProperty = _reflectionStorage.GetProperties<string>();
            Dictionary<string, ReactiveProperty<int>> layerProperty = _reflectionStorage.GetProperties<int>();

            if (activeStateProperty.ContainsKey(_activeState.SelectedName) && _activeState.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(activeStateProperty[_activeState.SelectedName].Subscribe(x =>
                {
                    this.gameObject.SetActive(x);
                }));
            }

            if (tagProperty.ContainsKey(_tag.SelectedName) && _tag.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(tagProperty[_tag.SelectedName].Subscribe(x =>
                {
                    this.gameObject.tag = x;
                }));
            }

            if (layerProperty.ContainsKey(_layer.SelectedName) && _layer.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(layerProperty[_layer.SelectedName].Subscribe(x =>
                {
                    this.gameObject.layer = x;
                }));
            }
        }
    }
}
