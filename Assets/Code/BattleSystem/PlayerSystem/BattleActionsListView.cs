using System;
using System.Collections.Generic;
using Dimasyechka.Code.BattleSystem.BattleActions.Views;
using UnityEngine;

namespace Dimasyechka.Code.BattleSystem.PlayerSystem
{
    public class BattleActionsListView : MonoBehaviour
    {
        [SerializeField]
        private Transform m_contentParent;


        private Action<int> callbackAction;


        public void UpdateData(List<AvailableBattleActionData> availableBattleActions)
        {
            // Очищаем старую информацию
            for (int i = m_contentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(m_contentParent.GetChild(i).gameObject);
            }

            // Заполняем новую информацию
            for (int i = 0; i < availableBattleActions.Count; i++)
            {
                BaseBattleInteractionView view = Instantiate(availableBattleActions[i].interaction.ActionProfileViewPrefab, m_contentParent);
                view.SetData(availableBattleActions[i], i);
                view.onPressed += OnViewActionPressed;
            }
        }

        public void SetPressedCallback(Action<int> callback)
        {
            callbackAction = callback;
        }

        private void OnViewActionPressed(int pressedIndex)
        {
            if (callbackAction != null)
            {
                callbackAction(pressedIndex);
            }
        }
    }
}
