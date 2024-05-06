using System.Collections.Generic;
using Dimasyechka.Code.BuffSystem.Views;
using UnityEngine;

namespace Dimasyechka.Code.BuffSystem.Containers
{
    public class BuffsContainerPresenter : MonoBehaviour
    {
        [Header("(Optional)")]
        [SerializeField]
        private BuffsContainer m_defaultContainerReference;


        [SerializeField]
        private RuntimeBuffView m_runtimeBuffViewPrefab;

        [SerializeField]
        private Transform m_contentParent;


        private BuffsContainer activeContainer;

        private List<RuntimeBuffView> runtimeBuffViews = new List<RuntimeBuffView>();


        private void Start()
        {
            if (m_defaultContainerReference != null)
            {
                SetBuffsContainer(m_defaultContainerReference);
            }
        }


        private void OnDestroy()
        {
            if (activeContainer != null)
            {
                activeContainer.onRuntimeBuffsChanged -= UpdateBuffs;
            }
        }


        public void SetBuffsContainer(BuffsContainer _container)
        {
            if (activeContainer != null)
            {
                activeContainer.onRuntimeBuffsChanged -= UpdateBuffs;
            }

            activeContainer = _container;

            activeContainer.onRuntimeBuffsChanged += UpdateBuffs;

            UpdateBuffs();
        }


        private void UpdateBuffs()
        {
            for (int i = runtimeBuffViews.Count - 1; i >= 0; i--)
            {
                Destroy(runtimeBuffViews[i].gameObject);
            }
            runtimeBuffViews.Clear();

            if (activeContainer != null)
            {
                foreach (RuntimeBuff runtimeBuff in activeContainer.RuntimeBuffs)
                {
                    RuntimeBuffView buffView = Instantiate(m_runtimeBuffViewPrefab, m_contentParent);
                    buffView.SetTitleAndIcon(runtimeBuff.BuffProfile.BuffName, runtimeBuff.BuffProfile.BuffIcon);
                    buffView.SetMaxDuration(runtimeBuff.BuffProfile.BuffDurationHours);
                    buffView.SetDuration(runtimeBuff.DurationHours);

                    runtimeBuffViews.Add(buffView);
                }
            }
        }
    }
}
