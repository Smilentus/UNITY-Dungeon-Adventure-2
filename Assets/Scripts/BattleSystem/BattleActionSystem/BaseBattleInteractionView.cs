using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class BaseBattleInteractionView : MonoBehaviour, IPointerClickHandler
{
    public event Action<int> onPressed;


    [SerializeField]
    protected TMP_Text m_actionTitleTMP;


    protected int attachedIndex;


    public virtual void SetData(AvailableBattleActionData availableBattleActionData, int index)
    {
        attachedIndex = index;

        m_actionTitleTMP.text = availableBattleActionData.interaction.InteractionTitle;
    }


    public virtual void OnPointerClick(PointerEventData eventData)
    {
        onPressed?.Invoke(attachedIndex);
    }
}