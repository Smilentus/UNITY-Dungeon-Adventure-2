using UnityEngine;

public class PlayerSaveLoadConverter : SaveLoadBaseConverter<PlayerSaveData>
{
    public override PlayerSaveData GetConverterData()
    {
        PlayerSaveData playerData = new PlayerSaveData();

        // Сохраняем игрока
        playerData.PlayerStats = RuntimePlayer.Instance.RuntimePlayerStats;

        return playerData;
    }

    public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
    {
        PlayerSaveData playerSaveData = ExtractDataType(generalSaveData);

        if (playerSaveData != null)
        {
            RuntimePlayer.Instance.RuntimePlayerStats = playerSaveData.PlayerStats;
        }
        else
        {
            SetDefaultData();
        }
    }

    public override void SetDefaultData()
    {
        RuntimePlayer.Instance.SetDefaultPlayerStats();
    }
}

[System.Serializable]
public class PlayerSaveData
{
    public RuntimePlayerStats PlayerStats;
}