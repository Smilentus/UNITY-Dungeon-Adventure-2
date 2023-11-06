using UnityEngine;

public class PlayerSaveLoadConverter : SaveLoadBaseConverter<PlayerSaveData>
{
    private void Awake()
    {
        SaveLoadSystemController.Instance.AddSaveLoadConverter(this);
    }


    public override PlayerSaveData GetConverterData()
    {
        PlayerSaveData playerData = new PlayerSaveData();

        // Сохраняем игрока

        return playerData;
    }

    public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
    {

    }
}

[System.Serializable]
public class PlayerSaveData
{
    
}