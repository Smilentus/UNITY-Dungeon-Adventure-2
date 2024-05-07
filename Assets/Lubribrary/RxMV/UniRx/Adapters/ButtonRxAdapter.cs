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
    public class ButtonRxAdapter : BaseComponentRxAdapter<Button>
    {
        [SerializeField]
        private UniRxReflectionField _interactable = new UniRxReflectionField();

        [SerializeField]
        private UniRxReflectionField _onClickHandler = new UniRxReflectionField();


        protected override void OnParseReflections()
        {
            Dictionary<string, ReactiveProperty<bool>> interactableProperty = _reflectionStorage.GetProperties<bool>();
            Dictionary<string, Action> actionDelegates = _reflectionStorage.GetDelegates();

            _interactable.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                interactableProperty.Select(x => x.Key).ToArray()
            });

            _onClickHandler.ReflectiveNames = _reflectionStorage.GetDictionaryNames(new List<string[]>()
            {
                actionDelegates.Select(x => x.Key).ToArray()
            });
        }

        protected override void OnConnectedToView()
        {
            Dictionary<string, ReactiveProperty<bool>> interactableProperty = _reflectionStorage.GetProperties<bool>();
            Dictionary<string, Action> actionDelegates = _reflectionStorage.GetDelegates();

            if (interactableProperty.ContainsKey(_interactable.SelectedName) && _interactable.SelectedIndex != -1)
            {
                _disposablesStorage.AddToDisposables(interactableProperty[_interactable.SelectedName].Subscribe(x =>
                {
                    _component.interactable = x;
                }));
            }

            if (actionDelegates.ContainsKey(_onClickHandler.SelectedName) && _onClickHandler.SelectedIndex != -1)
            {
                _component.onClick.AddListener(actionDelegates[_onClickHandler.SelectedName].Invoke);
            }
        }
    }
}
