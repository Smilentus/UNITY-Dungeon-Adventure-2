using Dimasyechka.Code.BattleSystem.Controllers;
using TMPro;
using UnityEngine;

namespace Dimasyechka.Code.BattleSystem.Presenters
{
    public class BattleControllerPresenter : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text m_turnStatusTMP;


        private void Awake()
        {
            BattleController.Instance.onBattleStatusChanged += OnBattleStatusChanged;
            BattleController.Instance.onBattleTurnStatusChanged += OnBattleTurnStatusChanged;
        }

        private void OnDestroy()
        {
            if (BattleController.Instance != null)
            {
                BattleController.Instance.onBattleStatusChanged += OnBattleStatusChanged;
                BattleController.Instance.onBattleTurnStatusChanged += OnBattleTurnStatusChanged;
            }
        }


        private void OnBattleTurnStatusChanged(BattleController.TurnStatus turnStatus)
        {
            if (turnStatus == BattleController.TurnStatus.PlayerTurn)
            {
                m_turnStatusTMP.text = "��� ������";
            }
            else if (turnStatus == BattleController.TurnStatus.EnemiesTurn)
            {
                m_turnStatusTMP.text = "��� ����������";
            }
            else
            {
                m_turnStatusTMP.text = "���������� ��� ���";
            }
        }

        private void OnBattleStatusChanged(bool value)
        {

        }
    }
}
