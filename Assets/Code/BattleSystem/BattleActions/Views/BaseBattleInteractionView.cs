using Dimasyechka.Code.BattleSystem.PlayerSystem;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using Dimasyechka.Lubribrary.RxMV.UniRx.RxLink;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dimasyechka.Code.BattleSystem.BattleActions.Views
{
    public class BaseBattleInteractionView : MonoBehaviour, IPointerClickHandler, IRxLinkable
    {
        public event Action<int> onPressed;


        [RxAdaptableProperty]
        public ReactiveProperty<string> ActionTitle = new ReactiveProperty<string>();


        protected int attachedIndex;


        public virtual void SetData(AvailableBattleActionData availableBattleActionData, int index)
        {
            attachedIndex = index;

            ActionTitle.Value = availableBattleActionData.Interaction.InteractionTitle;
        }


        public virtual void OnPointerClick(PointerEventData eventData)
        {
            onPressed?.Invoke(attachedIndex);
        }
    }
}