using UnityEngine;


public class PlayerBuffsController : MonoBehaviour
{
    private static PlayerBuffsController instance;
    public static PlayerBuffsController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerBuffsController>(true);
            }

            return instance;
        }
    }


    [SerializeField]
    private BuffsContainer m_playerBuffsContainer;
    public BuffsContainer PlayerBuffsContainer => m_playerBuffsContainer;


    private void Start()
    {
        GameTimeFlowController.Instance.onTimeHoursPassed += OnTimeHoursPassed;
    }

    private void OnDestroy()
    {
        GameTimeFlowController.Instance.onTimeHoursPassed -= OnTimeHoursPassed;
    }


    private void OnTimeHoursPassed(int passedHours)
    {
        // За каждый прошедший час - обновляем действия баффов
        for (int i = 0; i < passedHours; i++)
        {
            UpdatePlayerBuffs();
        }
    }


    public void DisableAndRemoveAllBuffs()
    {
        m_playerBuffsContainer.DisableAndRemoveAllBuffs();
    }

    public void LoadSaveBuffData(BuffSaveLoadData saveData)
    {
        foreach (RuntimeBuffSaveData buffSaveData in saveData.RuntimeBuffsSaveData)
        {
            m_playerBuffsContainer.LoadBuff(BuffsWarehouse.Instance.GetBuffProfileByUID(buffSaveData.BuffUID), buffSaveData.BuffDurationHours);
        }
    }


    public void AddPlayerBuff(BuffProfile _profile)
    {
        m_playerBuffsContainer.AddBuff(_profile);
    }

    public void RemovePlayerBuff(BuffProfile _profile)
    {
        m_playerBuffsContainer.RemoveBuff(_profile);
    }

    public void UpdatePlayerBuffs()
    {
        m_playerBuffsContainer.UpdateContainedBuffs();
    }


    [SerializeField]
    private BuffProfile debugProfile;


    [ContextMenu("AddBuff")]
    public void AddDebugBuff()
    {
        AddPlayerBuff(debugProfile);
    }

    [ContextMenu("RemoveBuff")]
    public void RemoveDebugBuff()
    {
        RemovePlayerBuff(debugProfile);
    }
}