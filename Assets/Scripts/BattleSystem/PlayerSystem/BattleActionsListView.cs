using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionsListView : MonoBehaviour
{
    [SerializeField]
    private BattleActionInteractionView m_battleActionViewPrefab;

    [SerializeField]
    private Transform m_contentParent;


    private Action<int> callbackAction;

    
    public void UpdateData(List<BattleActionProfile> profiles)
    {
        // Очищаем старую информацию
        for (int i = m_contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(m_contentParent.GetChild(i).gameObject);
        }


        // Заполняем новую информацию
        for (int i = 0; i < profiles.Count; i++)
        {
            BattleActionInteractionView view = Instantiate(m_battleActionViewPrefab, m_contentParent);
            view.SetData(profiles[i], i);
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
