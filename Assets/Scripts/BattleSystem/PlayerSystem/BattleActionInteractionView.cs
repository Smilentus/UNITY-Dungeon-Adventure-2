using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleActionInteractionView : MonoBehaviour, IPointerClickHandler
{
    public event Action<int> onPressed;


    [SerializeField]
    private TMP_Text m_actionTitleTMP;

    [SerializeField]
    private TMP_Text m_actionPointsCostTMP;

    private int attachedIndex;


    public void SetData(BattleActionProfile profile, int index)
    {
        attachedIndex = index;

        m_actionTitleTMP.text = profile.ActionTitle;
        m_actionPointsCostTMP.text = profile.SpendableActions.ToString();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        onPressed?.Invoke(attachedIndex);
    }
}
