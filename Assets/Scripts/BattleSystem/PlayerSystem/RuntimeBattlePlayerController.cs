using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    

    [Tooltip("Количество действия на шаг боя у игрока")]
    [SerializeField]
    private int m_defaultPlayerActionPoints = 2;
    public int DefaultPlayerActionPoints => m_defaultPlayerActionPoints;


    [SerializeField]
    private List<BattleActionProfile> m_defaultPlayerBattleActions = new List<BattleActionProfile>();
    /// <summary>
    ///     Дефолтные действия, доступные игроку во время битвы
    /// </summary>
    public List<BattleActionProfile> DefaultPlayerBattleActions => m_defaultPlayerBattleActions;


    private List<AvailableBattleActionData> availablePlayerBattleActions = new List<AvailableBattleActionData>();
    /// <summary>
    ///     Доступные действия игрока во время боя (какие-то предметы, события и т.п. могут добавлять сюда свои действия)
    /// </summary>
    public List<AvailableBattleActionData> AvailablePlayerBattleActions => availablePlayerBattleActions;


    private int extraPlayerActionPoints = 0;
    /// <summary>
    ///     Количество дополнительных ходов у игрока во время шага боя
    /// </summary>
    public int ExtraPlayerActionsCounter => extraPlayerActionPoints;


    private int playerActionPoints = 0;
    public int PlayerActionPoints => playerActionPoints;

    public int PlayerActionPointsTotalPerRound => m_defaultPlayerActionPoints + extraPlayerActionPoints;


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

    public void InitializeBattlePlayer()
    {
        InitializeBattleActions();
    }

    public void InitializeBattleActions()
    {
        // Очищаем существующие действия
        for (int i = availablePlayerBattleActions.Count - 1; i >= 0; i--)
        {
            Destroy(availablePlayerBattleActions[i].tempObject);
        }
        availablePlayerBattleActions.Clear();


        // Заполняем новые действия для боя
        foreach (BattleActionProfile profile in m_defaultPlayerBattleActions)
        {
            AddBattleAction(profile);   
        }


        // Активируем наши действия единожды перед каждым боем
        foreach (AvailableBattleActionData actionData in availablePlayerBattleActions)
        {
            actionData.executer.Initialize();
        }


        // Говорим всем, что наши действия обновились
        onBattleActionsUpdated?.Invoke();
    }


    public List<BattleActionProfile> GetAvailableBattleActionProfiles()
    {
        List<BattleActionProfile> profiles = new List<BattleActionProfile>();

        foreach (AvailableBattleActionData actionData in availablePlayerBattleActions)
        {
            profiles.Add(actionData.profile);
        }

        return profiles;
    }
    

    public void AddBattleAction(BattleActionProfile profile)
    {
        if (profile == null)
        {
            Debug.LogError($"Обнаружена попытка добавить пустой BattleAction!");
            return;
        }

        if (profile.ActionExecuter != null)
        {
            AvailableBattleActionData searching = availablePlayerBattleActions.Find(x => x.profile == profile);

            if (searching != null)
            {
                Debug.LogError($"Была обнаружена попытка добавить действие, которое уже существует '{profile.name}'");
                return;
            }

            AvailableBattleActionData availableBattleActionData = new AvailableBattleActionData();
            availableBattleActionData.profile = profile;

            GameObject tempObject = new GameObject($"{profile.ActionExecuter.Type.ToString()}");
            tempObject.transform.SetParent(this.transform);

            availableBattleActionData.tempObject = tempObject;
            availableBattleActionData.executer = profile.ActionExecuter.AddToGameObject(tempObject);

            availablePlayerBattleActions.Add(availableBattleActionData);
        }
        else
        {
            Debug.LogError($"BattleActionProfile '{profile.name}' не содержит IBattleActionExecuter!");
        }


        onBattleActionsUpdated?.Invoke();
    }

    public void RemoveBattleAction(BattleActionProfile profile)
    {
        if (profile == null)
        {
            Debug.LogError($"Обнаружена попытка удалить пустое BattleAction!", this.gameObject);
            return;
        }

        AvailableBattleActionData actionData = availablePlayerBattleActions.Find(x => x.profile == profile);

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
            if (playerActionPoints >= 0 && playerActionPoints >= availablePlayerBattleActions[executerIndex].profile.SpendableActions)
            {
                playerActionPoints -= availablePlayerBattleActions[executerIndex].profile.SpendableActions;

                onPlayerActionPointsChanged?.Invoke(playerActionPoints);

                availablePlayerBattleActions[executerIndex].executer.ExecuteAction();

                FindObjectOfType<SkillsManager>().SkillsAction();
                FindObjectOfType<BuffManager>().BuffsAction();
                FindObjectOfType<MagicManager>().UpdateMagicCooldown();

                BattleController.Instance.CheckEndBattleConditions();
                BattleController.Instance.UpdateAllEnemiesUI();
            }
        }
        else
        {
            Debug.LogError($"Обнаружена попытка выполнить пустое действие");
        }
    }


    private void OnBattleTurnStatusChanged(BattleController.TurnStatus turnStatus)
    {
        if (turnStatus == BattleController.TurnStatus.PlayerTurn)
        {
            playerActionPoints = PlayerActionPointsTotalPerRound;
            onPlayerActionPointsChanged?.Invoke(playerActionPoints);
        }
    }

    public bool CriticalStrike()
    {
        int rndChance = UnityEngine.Random.Range(0, 101);
        double dmg = RuntimePlayer.Instance.RuntimePlayerStats.Damage * RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeMulty;

        if (rndChance <= RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeChance)
        {
            BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Health -= dmg;
            GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - Вы нанесли урон критическим ударом: " + dmg + " ед.");
            return true;
        }
        return false;
    }

    // Попытка сбежать
    public void TryToEscape()
    {
        if (UnityEngine.Random.Range(0, 101) <= 50 + RuntimePlayer.Instance.RuntimePlayerStats.Luck)
        {
            BattleController.Instance.IsBattle = false;
            BattleController.Instance.IsWin = false;
            GameController.Instance.AddEventText("Вы сбежали.");
            BattleController.Instance.EndBattle();
        }
        else
        {
            GameController.Instance.AddEventText("Вы не смогли избежать боя.");
        }
    }
}

[System.Serializable]
public class AvailableBattleActionData
{
    public BattleActionProfile profile;
    public IBattleActionExecuter executer;
    public GameObject tempObject;
}