using Dimasyechka.Lubribrary.RxMV.UniRx.Adapters.Base;
using Dimasyechka.Lubribrary.RxMV.Utilities;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.Adapters
{
    public class ImageRxAdapter : BaseComponentRxAdapter<Image>
    {
        [SerializeField]
        private UniRxReflectionField _sprite = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _color = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _material = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _fillAmount = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _raycastTarget = new UniRxReflectionField();



        protected override void OnParseReflections()
        {
            Dictionary<string, ReactiveProperty<Sprite>> spriteProperty = _reflectionStorage.GetProperties<Sprite>();
            Dictionary<string, ReactiveProperty<Color>> colorProperty = _reflectionStorage.GetProperties<Color>();
            Dictionary<string, ReactiveProperty<Material>> materialProperty = _reflectionStorage.GetProperties<Material>();
            Dictionary<string, ReactiveProperty<float>> fillAmountProperty = _reflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<bool>> raycastTargetProperty = _reflectionStorage.GetProperties<bool>();

            _sprite.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                spriteProperty.Select(x => x.Key).ToArray(),
            });

            _material.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                materialProperty.Select(x => x.Key).ToArray()
            });

            _color.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                colorProperty.Select(x => x.Key).ToArray()
            });

            _fillAmount.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                fillAmountProperty.Select(x => x.Key).ToArray()
            });

            _raycastTarget.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                raycastTargetProperty.Select(x => x.Key).ToArray()
            });
        }

        protected override void OnConnectedToView()
        {
            Dictionary<string, ReactiveProperty<Sprite>> spriteProperty = _reflectionStorage.GetProperties<Sprite>();
            Dictionary<string, ReactiveProperty<Color>> colorProperty = _reflectionStorage.GetProperties<Color>();
            Dictionary<string, ReactiveProperty<Material>> materialProperty = _reflectionStorage.GetProperties<Material>();
            Dictionary<string, ReactiveProperty<float>> fillAmountProperty = _reflectionStorage.GetProperties<float>();
            Dictionary<string, ReactiveProperty<bool>> raycastTargetProperty = _reflectionStorage.GetProperties<bool>();

            if (spriteProperty.ContainsKey(_sprite.SelectedName) && _sprite.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(spriteProperty[_sprite.SelectedName].Subscribe(x =>
                {
                    _component.sprite = x;
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

            if (fillAmountProperty.ContainsKey(_fillAmount.SelectedName) && _fillAmount.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(fillAmountProperty[_fillAmount.SelectedName].Subscribe(x =>
                {
                    _component.fillAmount = x;
                }));
            }
        }
    }
}
