using Dimasyechka.Lubribrary.RxMV.UniRx.Adapters.Base;
using Dimasyechka.Lubribrary.RxMV.Utilities;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.Adapters
{
    public class RawImageRxAdapter : BaseComponentRxAdapter<RawImage>
    {
        [SerializeField]
        private UniRxReflectionField _texture = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _color = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _material = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _raycastTarget = new UniRxReflectionField();



        protected override void OnParseReflections()
        {
            Dictionary<string, ReactiveProperty<Texture>> textureProperty = _reflectionStorage.GetProperties<Texture>();
            Dictionary<string, ReactiveProperty<Color>> colorProperty = _reflectionStorage.GetProperties<Color>();
            Dictionary<string, ReactiveProperty<Material>> materialProperty = _reflectionStorage.GetProperties<Material>();
            Dictionary<string, ReactiveProperty<bool>> raycastTargetProperty = _reflectionStorage.GetProperties<bool>();

            _texture.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                textureProperty.Select(x => x.Key).ToArray(),
            });

            _material.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                materialProperty.Select(x => x.Key).ToArray()
            });

            _color.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                colorProperty.Select(x => x.Key).ToArray()
            });

            _raycastTarget.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                raycastTargetProperty.Select(x => x.Key).ToArray()
            });
        }

        protected override void OnConnectedToView()
        {
            Dictionary<string, ReactiveProperty<Texture>> textureProperty = _reflectionStorage.GetProperties<Texture>();
            Dictionary<string, ReactiveProperty<Color>> colorProperty = _reflectionStorage.GetProperties<Color>();
            Dictionary<string, ReactiveProperty<Material>> materialProperty = _reflectionStorage.GetProperties<Material>();
            Dictionary<string, ReactiveProperty<bool>> raycastTargetProperty = _reflectionStorage.GetProperties<bool>();

            if (textureProperty.ContainsKey(_texture.SelectedName) && _texture.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(textureProperty[_texture.SelectedName].Subscribe(x =>
                {
                    _component.texture = x;
                }));
            }

            if (colorProperty.ContainsKey(_color.SelectedName) && _color.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(colorProperty[_color.SelectedName].Subscribe(x =>
                {
                    _component.color = x;
                }));
            }

            if (materialProperty.ContainsKey(_material.SelectedName) && _material.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(materialProperty[_material.SelectedName].Subscribe(x =>
                {
                    _component.material = x;
                }));
            }

            if (raycastTargetProperty.ContainsKey(_raycastTarget.SelectedName) && _raycastTarget.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(raycastTargetProperty[_raycastTarget.SelectedName].Subscribe(x =>
                {
                    _component.raycastTarget = x;
                }));
            }
        }
    }
}
