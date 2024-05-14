using Dimasyechka.Lubribrary.RxMV.UniRx.Adapters.Base;
using Dimasyechka.Lubribrary.RxMV.Utilities;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.Adapters
{
    public class TransformRxAdapter : BaseComponentRxAdapter<Transform>
    {
        [Header("Positions")]
        [SerializeField]
        private UniRxReflectionField _worldPosition = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _localPosition = new UniRxReflectionField();


        [Header("EulerAngles")]
        [SerializeField]
        private UniRxReflectionField _worldEulerAngles = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _localEulerAngles = new UniRxReflectionField();


        [Header("Quaternion")]
        [SerializeField]
        private UniRxReflectionField _worldRotation = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _localRotation = new UniRxReflectionField();


        protected override void OnParseReflections()
        {
            Dictionary<string, ReactiveProperty<Vector3>> vector3Property = _reflectionStorage.GetProperties<Vector3>();
            Dictionary<string, ReactiveProperty<Quaternion>> quaternionProperty = _reflectionStorage.GetProperties<Quaternion>();

            string[] vectorNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                vector3Property.Select(x => x.Key).ToArray(),
            });

            string[] quaternionNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                quaternionProperty.Select(x => x.Key).ToArray(),
            });

            _worldPosition.ReflectiveNames = vectorNames;
            _localPosition.ReflectiveNames = vectorNames;

            _worldEulerAngles.ReflectiveNames = vectorNames;
            _localEulerAngles.ReflectiveNames = vectorNames;

            _worldRotation.ReflectiveNames = quaternionNames;
            _localRotation.ReflectiveNames = quaternionNames;
        }

        protected override void OnConnectedToView()
        {
            Dictionary<string, ReactiveProperty<Vector3>> vector3Property = _reflectionStorage.GetProperties<Vector3>();
            Dictionary<string, ReactiveProperty<Quaternion>> quaternionProperty = _reflectionStorage.GetProperties<Quaternion>();

            if (vector3Property.ContainsKey(_worldPosition.SelectedName) && _worldPosition.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(vector3Property[_worldPosition.SelectedName].Subscribe(x =>
                {
                    _component.position = x;
                }));
            }

            if (vector3Property.ContainsKey(_localPosition.SelectedName) && _localPosition.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(vector3Property[_localPosition.SelectedName].Subscribe(x =>
                {
                    _component.localPosition = x;
                }));
            }


            if (vector3Property.ContainsKey(_worldEulerAngles.SelectedName) && _worldEulerAngles.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(vector3Property[_worldEulerAngles.SelectedName].Subscribe(x =>
                {
                    _component.eulerAngles = x;
                }));
            }

            if (vector3Property.ContainsKey(_localEulerAngles.SelectedName) && _localEulerAngles.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(vector3Property[_localEulerAngles.SelectedName].Subscribe(x =>
                {
                    _component.localEulerAngles = x;
                }));
            }


            if (quaternionProperty.ContainsKey(_worldRotation.SelectedName) && _worldRotation.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(quaternionProperty[_worldRotation.SelectedName].Subscribe(x =>
                {
                    _component.rotation = x;
                }));
            }

            if (quaternionProperty.ContainsKey(_localRotation.SelectedName) && _localRotation.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(quaternionProperty[_localRotation.SelectedName].Subscribe(x =>
                {
                    _component.localRotation = x;
                }));
            }
        }
    }
}
