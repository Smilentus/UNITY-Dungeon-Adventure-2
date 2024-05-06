using System;
using System.Collections.Generic;
using Dimasyechka.Code.BattleSystem.BattleActions.Views;
using UnityEngine;

namespace Dimasyechka.Code.BattleSystem.PlayerSystem
{
    public class BattleActionsListView : MonoBehaviour
    {
        [SerializeField]
        private Transform _contentParent;


        private Action<int> onCallbackAction;


        public void UpdateData(List<AvailableBattleActionData> availableBattleActions)
        {
            // Очищаем старую информацию
            for (int i = _contentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(_contentParent.GetChild(i).gameObject);
            }

            // Заполняем новую информацию
            for (int i = 0; i < availableBattleActions.Count; i++)
            {
                BaseBattleInteractionView view = Instantiate(availableBattleActions[i].Interaction.ActionProfileViewPrefab, _contentParent);
                view.SetData(availableBattleActions[i], i);
                view.onPressed += OnViewActionPressed;
            }
        }

        public void SetPressedCallback(Action<int> callback)
        {
            onCallbackAction = callback;
        }

        private void OnViewActionPressed(int pressedIndex)
        {
            if (onCallbackAction != null)
            {
                onCallbackAction(pressedIndex);
            }
        }
    }
}
