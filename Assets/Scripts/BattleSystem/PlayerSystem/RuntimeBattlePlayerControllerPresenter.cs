using TMPro;
using UnityEngine;

public class RuntimeBattlePlayerControllerPresenter : MonoBehaviour
{
    [SerializeField]
    private RuntimeBattlePlayerController m_controller;


    [SerializeField]
    private TMP_Text m_playersActionPointsTMP;

    [SerializeField]
    private BattleActionsListView m_battleActionsView;


    private void Start()
    {
        m_controller.onPlayerActionPointsChanged += OnPlayerActionsChanged;
        m_controller.onBattleActionsUpdated += OnBattleActionsUpdated;
    }

    private void OnDestroy()
    {
        m_controller.onPlayerActionPointsChanged -= OnPlayerActionsChanged;
        m_controller.onBattleActionsUpdated -= OnBattleActionsUpdated;
    }

    private void OnBattleActionsUpdated()
    {
        m_battleActionsView.SetPressedCallback(m_controller.ExecuteAction);
        m_battleActionsView.UpdateData(m_controller.GetAvailableBattleActionProfiles());
    }

    private void OnPlayerActionsChanged(int value)
    {
        m_playersActionPointsTMP.text = $"Очки действий: {value} из {m_controller.PlayerActionPointsTotalPerRound}";
    }
}
