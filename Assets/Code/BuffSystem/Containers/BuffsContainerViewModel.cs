using System.Collections.Generic;
using Dimasyechka.Code.BuffSystem.Views;
using Dimasyechka.Code.ZenjectFactories;
using Dimasyechka.Lubribrary.RxMV.Core;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.BuffSystem.Containers
{
    public class BuffsContainerViewModel : MonoViewModel<BuffsContainer>
    {
        [SerializeField]
        private RuntimeBuffView _runtimeBuffViewPrefab;

        [SerializeField]
        private Transform _contentParent;


        private List<RuntimeBuffView> _runtimeBuffViews = new List<RuntimeBuffView>();


        private RuntimeBuffViewFactory _factory;

        [Inject]
        public void Construct(RuntimeBuffViewFactory factory)
        {
            _factory = factory;
        }


        protected override void OnSetupModel()
        {
            Model.onRuntimeBuffsChanged += UpdateBuffs;

            UpdateBuffs();
        }

        protected override void OnRemoveModel()
        {
            if (Model != null)
            {
                Model.onRuntimeBuffsChanged -= UpdateBuffs;
            }
        }


        private void UpdateBuffs()
        {
            for (int i = _runtimeBuffViews.Count - 1; i >= 0; i--)
            {
                Destroy(_runtimeBuffViews[i].gameObject);
            }
            _runtimeBuffViews.Clear();

            if (Model != null)
            {
                foreach (RuntimeBuff runtimeBuff in Model.RuntimeBuffs)
                {
                    RuntimeBuffView buffView = _factory.InstantiateForComponent(_runtimeBuffViewPrefab.gameObject, _contentParent);
                    buffView.SetupModel(runtimeBuff);

                    _runtimeBuffViews.Add(buffView);
                }
            }
        }
    }


    public class RuntimeBuffViewFactory : DiContainerCreationFactory<RuntimeBuffView> { }
}
