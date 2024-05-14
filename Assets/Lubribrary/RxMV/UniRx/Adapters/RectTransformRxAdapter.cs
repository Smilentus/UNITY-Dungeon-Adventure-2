using Dimasyechka.Lubribrary.RxMV.UniRx.Adapters.Base;
using Dimasyechka.Lubribrary.RxMV.Utilities;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.Adapters
{
    public class RectTransformRxAdapter : BaseComponentRxAdapter<RectTransform>
    {
        [SerializeField]
        private UniRxReflectionField _anchoredPosition3D = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _anchoredPosition = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _pivotPoint = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _size = new UniRxReflectionField();


        protected override void OnParseReflections()
        {
            Dictionary<string, ReactiveProperty<Vector3>> vector3Property = _reflectionStorage.GetProperties<Vector3>();
            Dictionary<string, ReactiveProperty<Vector2>> vector2Property = _reflectionStorage.GetProperties<Vector2>();

            string[] allVectorNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                vector3Property.Select(x => x.Key).ToArray(),
                vector2Property.Select(x => x.Key).ToArray(),
            });

            _anchoredPosition3D.ReflectiveNames = allVectorNames;
            _anchoredPosition.ReflectiveNames = allVectorNames;

            _pivotPoint.ReflectiveNames = allVectorNames;
            _size.ReflectiveNames = allVectorNames;
        }

        protected override void OnConnectedToView()
        {
            Dictionary<string, ReactiveProperty<Vector3>> vector3Property = _reflectionStorage.GetProperties<Vector3>();
            Dictionary<string, ReactiveProperty<Vector2>> vector2Property = _reflectionStorage.GetProperties<Vector2>();


            if (vector3Property.ContainsKey(_anchoredPosition3D.SelectedName) && _anchoredPosition3D.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(vector3Property[_anchoredPosition3D.SelectedName].Subscribe(x =>
                {
                    _component.anchoredPosition3D = x;
                }));
            }
            if (vector2Property.ContainsKey(_anchoredPosition3D.SelectedName) && _anchoredPosition3D.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(vector2Property[_anchoredPosition3D.SelectedName].Subscribe(x =>
                {
                    _component.anchoredPosition3D = x;
                }));
            }

            if (vector3Property.ContainsKey(_anchoredPosition.SelectedName) && _anchoredPosition.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(vector3Property[_anchoredPosition.SelectedName].Subscribe(x =>
                {
                    _component.anchoredPosition = x;
                }));
            }
            if (vector2Property.ContainsKey(_anchoredPosition.SelectedName) && _anchoredPosition.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(vector2Property[_anchoredPosition.SelectedName].Subscribe(x =>
                {
                    _component.anchoredPosition = x;
                }));
            }

            if (vector3Property.ContainsKey(_pivotPoint.SelectedName) && _pivotPoint.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(vector3Property[_pivotPoint.SelectedName].Subscribe(x =>
                {
                    _component.pivot = x;
                }));
            }
            if (vector2Property.ContainsKey(_pivotPoint.SelectedName) && _pivotPoint.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(vector2Property[_pivotPoint.SelectedName].Subscribe(x =>
                {
                    _component.pivot = x;
                }));
            }

            if (vector3Property.ContainsKey(_size.SelectedName) && _size.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(vector3Property[_size.SelectedName].Subscribe(x =>
                {
                    _component.sizeDelta = x;
                }));
            }
            if (vector2Property.ContainsKey(_size.SelectedName) && _size.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(vector2Property[_size.SelectedName].Subscribe(x =>
                {
                    _component.sizeDelta = x;
                }));
            }
        }
    }
}
