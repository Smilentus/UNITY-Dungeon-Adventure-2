using System;
using System.Collections.Generic;
using Dimasyechka.Code.BattleSystem.BattleActions.Interfaces;
using Dimasyechka.Code.BattleSystem.BattleActions.Profiles;
using Dimasyechka.Code.BattleSystem.Controllers;
using UnityEngine;

namespace Dimasyechka.Code.BattleSystem.PlayerSystem
{
    public class RuntimeBattlePlayerController : MonoBehaviour
    {
        private static RuntimeBattlePlayerController instance;
        public static RuntimeBattlePlayerController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<RuntimeBattlePlayerController>(true);
                }

                return instance;
            }
        }


        public event Action<int> onPlayerActionPointsChanged;
        public event Action onBattleActionsUpdated;


        [Tooltip("���������� �������� �� ��� ��� � ������")]
        [SerializeField]
        private int m_defaultPlayerActionPoints = 2;
        public int DefaultPlayerActionPoints => m_defaultPlayerActionPoints;


        [SerializeField]
        private List<BattleActionProfile> m_defaultPlayerBattleActions = new List<BattleActionProfile>();


        private List<IBattleActionInteraction> defaultPlayerBattleActions = new List<IBattleActionInteraction>();
        /// <summary>
        ///     ��������� ��������, ��������� ������ �� ����� �����
        /// </summary>
        public List<IBattleActionInteraction> DefaultPlayerBattleActions => defaultPlayerBattleActions;


        private List<AvailableBattleActionData> availablePlayerBattleActions = new List<AvailableBattleActionData>();
        /// <summary>
        ///     ��������� �������� ������ �� ����� ��� (�����-�� ��������, ������� � �.�. ����� ��������� ���� ���� ��������)
        /// </summary>
        public List<AvailableBattleActionData> AvailablePlayerBattleActions => availablePlayerBattleActions;


        private int extraPlayerActionPoints = 0;
        /// <summary>
        ///     ���������� �������������� ����� � ������ �� ����� ���� ���
        /// </summary>
        public int ExtraPlayerActionsCounter => extraPlayerActionPoints;


        private int playerActionPoints = 0;
        public int PlayerActionPoints
        { 
            get => playerActionPoints;
            set 
            {
                if (playerActionPoints < 0)
                {
                    playerActionPoints = 0;
                }
                else
                {
                    playerActionPoints = value;
                }
             
                onPlayerActionPointsChanged?.Invoke(playerActionPoints);
            }
        }   
    

        public int PlayerActionPointsTotalPerRound => m_defaultPlayerActionPoints + extraPlayerActionPoints;


        private void Awake()
        {
            CreateDefaultPlayerBattleActions();
        }

        private void Start()
        {
            BattleController.Instance.onBattleTurnStatusChanged += OnBattleTurnStatusChanged;
        }

        private void OnDestroy()
        {
            if (BattleController.Instance != null)
            {
                BattleController.Instance.onBattleTurnStatusChanged -= OnBattleTurnStatusChanged;
            }
        }


        private void CreateDefaultPlayerBattleActions()
        {
            foreach (BattleActionProfile profile in m_defaultPlayerBattleActions)
            {
                AddDefaultBattleAction(profile);
            }
        }


        public void InitializeBattlePlayer()
        {
            InitializeBattleActions();
        }

        public void InitializeBattleActions()
        {
            // ������� ������������ ��������
            for (int i = availablePlayerBattleActions.Count - 1; i >= 0; i--)
            {
                Destroy(availablePlayerBattleActions[i].tempObject);
            }
            availablePlayerBattleActions.Clear();


            // ��������� ����� �������� ��� ���
            foreach (IBattleActionInteraction profile in defaultPlayerBattleActions)
            {
                AddAvailableBattleAction(profile);
            }


            // ���������� ���� �������� �������� ����� ������ ����
            foreach (AvailableBattleActionData actionData in availablePlayerBattleActions)
            {
                actionData.executer.Initialize();
            }


            // ������� ����, ��� ���� �������� ����������
            onBattleActionsUpdated?.Invoke();
        }


        public void AddDefaultBattleAction(IBattleActionInteraction interaction)
        {
            if (interaction == null)
            {
                Debug.LogError($"���������� ������� �������� ������ IBattleActionInteraction!");
                return;
            }

            if (defaultPlayerBattleActions.Contains(interaction)) return;

            defaultPlayerBattleActions.Add(interaction);

            onBattleActionsUpdated?.Invoke();
        }
        public void RemoveDefaultBattleAction(IBattleActionInteraction interaction)
        {
            if (interaction == null)
            {
                Debug.LogError($"���������� ������� ������� ������ BattleAction!", this.gameObject);
                return;
            }

            defaultPlayerBattleActions.Remove(interaction);

            onBattleActionsUpdated?.Invoke();
        }


        private void AddAvailableBattleAction(IBattleActionInteraction interaction)
        {
            if (interaction == null)
            {
                Debug.LogError($"���������� ������� �������� ������ IBattleActionInteraction!");
                return;
            }

            if (interaction.ActionExecuter != null)
            {
                AvailableBattleActionData searching = availablePlayerBattleActions.Find(x => x.interaction == interaction);

                if (searching != null)
                {
                    Debug.LogError($"���� ���������� ������� �������� ��������, ������� ��� ���������� '{interaction.GetType().Name}'");
                    return;
                }

                AvailableBattleActionData availableBattleActionData = new AvailableBattleActionData();
                availableBattleActionData.interaction = interaction;

                GameObject tempObject = new GameObject($"{interaction.ActionExecuter.Type.ToString()}");
                tempObject.transform.SetParent(this.transform);

                availableBattleActionData.tempObject = tempObject;
                availableBattleActionData.executer = interaction.ActionExecuter.AddToGameObject(tempObject);
                availableBattleActionData.executer.SetInteraction(interaction);

                availablePlayerBattleActions.Add(availableBattleActionData);
            }
            else
            {
                Debug.LogError($"BattleActionProfile '{interaction.GetType().Name}' �� �������� IBattleActionExecuter!");
            }

            onBattleActionsUpdated?.Invoke();
        }
        private void RemoveAvailableBattleAction(IBattleActionInteraction interaction)
        {
            if (interaction == null)
            {
                Debug.LogError($"���������� ������� ������� ������ BattleAction!", this.gameObject);
                return;
            }

            AvailableBattleActionData actionData = availablePlayerBattleActions.Find(x => x.interaction == interaction);

            if (actionData != null)
            {
                Destroy(actionData.tempObject);
                availablePlayerBattleActions.Remove(actionData);

                onBattleActionsUpdated?.Invoke();
            }
        }


        public void ExecuteAction(int executerIndex)
        {
            if (executerIndex >= 0 && executerIndex < availablePlayerBattleActions.Count)
            {
                if (availablePlayerBattleActions[executerIndex].executer.CanExecuteAction())
                {
                    availablePlayerBattleActions[executerIndex].executer.ExecuteAction();

                    RuntimePlayer.Instance.PerformHealthRegeneration();
                    RuntimePlayer.Instance.PerformManaRegeneration();
                    //FindObjectOfType<SkillsManager>().SkillsAction();
                    //FindObjectOfType<BuffManager>().BuffsAction();
                    //FindObjectOfType<MagicManager>().UpdateMagicCooldown();

                    BattleController.Instance.CheckEndBattleConditions();
                    BattleController.Instance.UpdateAllEnemiesUI();
                }
            }
            else
            {
                Debug.LogError($"���������� ������� ��������� ������ ��������");
            }
        }


        private void OnBattleTurnStatusChanged(BattleController.TurnStatus turnStatus)
        {
            if (turnStatus == BattleController.TurnStatus.PlayerTurn)
            {
                playerActionPoints = PlayerActionPointsTotalPerRound;
                onPlayerActionPointsChanged?.Invoke(playerActionPoints);
            }

            foreach (AvailableBattleActionData availableBattleActionData in availablePlayerBattleActions)
            {
                availableBattleActionData.executer.EveryTurnCheck(turnStatus);
            }

            onBattleActionsUpdated?.Invoke();
        }

        public bool CriticalStrike()
        {
            int rndChance = UnityEngine.Random.Range(0, 101);
            double dmg = RuntimePlayer.Instance.RuntimePlayerStats.Damage * RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeDamageMultiplier;

            if (rndChance <= RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeChance)
            {
                BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Health -= dmg;
                GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - �� ������� ���� ����������� ������: " + dmg + " ��.");
                return true;
            }
            return false;
        }

        // ������� �������
        public void TryToEscape()
        {
            if (UnityEngine.Random.Range(0, 101) <= 50 + RuntimePlayer.Instance.RuntimePlayerStats.Luck)
            {
                BattleController.Instance.IsBattle = false;
                BattleController.Instance.IsWin = false;
                GameController.Instance.AddEventText("�� �������.");
                BattleController.Instance.EndBattle();
            }
            else
            {
                GameController.Instance.AddEventText("�� �� ������ �������� ���.");
            }
        }
    }

    [System.Serializable]
    public class AvailableBattleActionData
    {
        public IBattleActionInteraction interaction;
        public IBattleActionExecuter executer;
        public GameObject tempObject;
    }
}