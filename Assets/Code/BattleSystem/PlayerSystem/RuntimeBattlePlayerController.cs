using System;
using System.Collections.Generic;
using Dimasyechka.Code.BattleSystem.BattleActions.Interfaces;
using Dimasyechka.Code.BattleSystem.BattleActions.Profiles;
using Dimasyechka.Code.BattleSystem.Controllers;
using Dimasyechka.Code.BattleSystem.EnemiesSystem;
using Dimasyechka.Code.Utilities;
using Dimasyechka.Code.ZenjectFactories;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.BattleSystem.PlayerSystem
{
    public class RuntimeBattlePlayerController : MonoBehaviour
    {
        public event Action<int> onPlayerActionPointsChanged;
        public event Action onBattleActionsUpdated;


        [Tooltip("���������� �������� �� ��� ��� � ������")]
        [SerializeField]
        private int _defaultPlayerActionPoints = 2;
        public int DefaultPlayerActionPoints => _defaultPlayerActionPoints;


        [SerializeField]
        private List<BattleActionProfile> _defaultPlayerBattleActions = new List<BattleActionProfile>();


        private List<IBattleActionInteraction> _defaultPlayerBattleInteractions = new List<IBattleActionInteraction>();
        /// <summary>
        ///     ��������� ��������, ��������� ������ �� ����� �����
        /// </summary>
        public List<IBattleActionInteraction> defaultPlayerBattleInteractions => _defaultPlayerBattleInteractions;


        private List<AvailableBattleActionData> _availablePlayerBattleActions = new List<AvailableBattleActionData>();
        /// <summary>
        ///     ��������� �������� ������ �� ����� ��� (�����-�� ��������, ������� � �.�. ����� ��������� ���� ���� ��������)
        /// </summary>
        public List<AvailableBattleActionData> AvailablePlayerBattleActions => _availablePlayerBattleActions;


        private int _extraPlayerActionPoints = 0;
        /// <summary>
        ///     ���������� �������������� ����� � ������ �� ����� ���� ���
        /// </summary>
        public int ExtraPlayerActionsCounter => _extraPlayerActionPoints;


        private int _playerActionPoints = 0;
        public int PlayerActionPoints
        { 
            get => _playerActionPoints;
            set 
            {
                if (_playerActionPoints < 0)
                {
                    _playerActionPoints = 0;
                }
                else
                {
                    _playerActionPoints = value;
                }
             
                onPlayerActionPointsChanged?.Invoke(_playerActionPoints);
            }
        }   
    

        public int PlayerActionPointsTotalPerRound => _defaultPlayerActionPoints + _extraPlayerActionPoints;


        private BattleController _battleController;
        private RuntimePlayer _runtimePlayer;

        private RuntimeBattleActionFactory _runtimeBattleActionFactory;

        [Inject]
        public void Construct(
            BattleController battleController, 
            RuntimePlayer runtimePlayer, 
            RuntimeBattleActionFactory runtimeBattleActionFactory)
        {
            _battleController = battleController;
            _runtimePlayer = runtimePlayer;

            _runtimeBattleActionFactory = runtimeBattleActionFactory;
        }


        private void Awake()
        {
            CreateDefaultPlayerBattleActions();
        }

        private void Start()
        {
            _battleController.onBattleTurnStatusChanged += OnBattleTurnStatusChanged;
        }

        private void OnDestroy()
        {
            if (_battleController != null)
            {
                _battleController.onBattleTurnStatusChanged -= OnBattleTurnStatusChanged;
            }
        }


        private void CreateDefaultPlayerBattleActions()
        {
            foreach (BattleActionProfile profile in _defaultPlayerBattleActions)
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
            for (int i = _availablePlayerBattleActions.Count - 1; i >= 0; i--)
            {
                Destroy(_availablePlayerBattleActions[i].TempObject);
            }
            _availablePlayerBattleActions.Clear();


            // ��������� ����� �������� ��� ���
            foreach (IBattleActionInteraction profile in _defaultPlayerBattleInteractions)
            {
                AddAvailableBattleAction(profile);
            }


            // ���������� ���� �������� �������� ����� ������ ����
            foreach (AvailableBattleActionData actionData in _availablePlayerBattleActions)
            {
                actionData.Executer.Initialize();
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

            if (_defaultPlayerBattleInteractions.Contains(interaction)) return;

            _defaultPlayerBattleInteractions.Add(interaction);

            onBattleActionsUpdated?.Invoke();
        }
        public void RemoveDefaultBattleAction(IBattleActionInteraction interaction)
        {
            if (interaction == null)
            {
                Debug.LogError($"���������� ������� ������� ������ BattleAction!", this.gameObject);
                return;
            }

            _defaultPlayerBattleInteractions.Remove(interaction);

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
                AvailableBattleActionData searching = _availablePlayerBattleActions.Find(x => x.Interaction == interaction);

                if (searching != null)
                {
                    Debug.LogError($"���� ���������� ������� �������� ��������, ������� ��� ���������� '{interaction.GetType().Name}'");
                    return;
                }

                AvailableBattleActionData availableBattleActionData = new AvailableBattleActionData();
                availableBattleActionData.Interaction = interaction;

                GameObject tempObject = _runtimeBattleActionFactory.InstantiateBattleAction(interaction.ActionExecuter.Type.ToString(), this.transform);
                Component executerComponent = _runtimeBattleActionFactory.AddComponent(tempObject, interaction.ActionExecuter);

                availableBattleActionData.TempObject = tempObject;
                availableBattleActionData.Executer = executerComponent.GetComponent<IBattleActionExecuter>();
                availableBattleActionData.Executer.SetInteraction(interaction);

                _availablePlayerBattleActions.Add(availableBattleActionData);
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

            AvailableBattleActionData actionData = _availablePlayerBattleActions.Find(x => x.Interaction == interaction);

            if (actionData != null)
            {
                Destroy(actionData.TempObject);
                _availablePlayerBattleActions.Remove(actionData);

                onBattleActionsUpdated?.Invoke();
            }
        }


        public void ExecuteAction(int executerIndex)
        {
            if (executerIndex >= 0 && executerIndex < _availablePlayerBattleActions.Count)
            {
                if (_availablePlayerBattleActions[executerIndex].Executer.CanExecuteAction())
                {
                    _availablePlayerBattleActions[executerIndex].Executer.ExecuteAction();

                    _runtimePlayer.PerformHealthRegeneration();
                    _runtimePlayer.PerformManaRegeneration();
                    //FindObjectOfType<SkillsManager>().SkillsAction();
                    //FindObjectOfType<BuffManager>().BuffsAction();
                    //FindObjectOfType<MagicManager>().UpdateMagicCooldown();

                    _battleController.CheckEndBattleConditions();
                    _battleController.UpdateAllEnemiesUI();
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
                _playerActionPoints = PlayerActionPointsTotalPerRound;
                onPlayerActionPointsChanged?.Invoke(_playerActionPoints);
            }

            foreach (AvailableBattleActionData availableBattleActionData in _availablePlayerBattleActions)
            {
                availableBattleActionData.Executer.EveryTurnCheck(turnStatus);
            }

            onBattleActionsUpdated?.Invoke();
        }

        public bool CriticalStrike()
        {
            int rndChance = UnityEngine.Random.Range(0, 101);
            double dmg = _runtimePlayer.RuntimePlayerStats.Damage.Value * _runtimePlayer.RuntimePlayerStats.CriticalStrikeDamageMultiplier.Value;

            if (rndChance <= _runtimePlayer.RuntimePlayerStats.CriticalStrikeChance.Value)
            {
                _battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].Health -= dmg;
                GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - �� ������� ���� ����������� ������: " + dmg + " ��.");
                return true;
            }
            return false;
        }

        public void TryToEscape()
        {
            if (UnityEngine.Random.Range(0, 101) <= 50 + _runtimePlayer.RuntimePlayerStats.Luck.Value)
            {
                _battleController.IsBattle = false;
                _battleController.IsWin = false;
                GameController.Instance.AddEventText("�� �������.");
                _battleController.EndBattle();
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
        public IBattleActionInteraction Interaction;
        public IBattleActionExecuter Executer;
        public GameObject TempObject;
    }

    public class RuntimeBattleActionFactory : DiContainerFactory
    {
        public GameObject InstantiateBattleAction(string objectName, Transform spawnPoint)
        {
            GameObject gameObject = _diContainer.Instantiate<GameObject>();
            gameObject.transform.SetParent(spawnPoint);
            gameObject.name = objectName;
            return gameObject;
        }

        public Component AddComponent(GameObject gameObject, SerializableMonoScript serializableMonoScript)
        {
            return _diContainer.InstantiateComponent(serializableMonoScript.Type, gameObject);
        }
    }
}