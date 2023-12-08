using System.Collections.Generic;

public class BuffsSaveLoadConverter : SaveLoadBaseConverter<BuffSaveLoadData>
{
    public override BuffSaveLoadData GetConverterData()
    {
        BuffSaveLoadData saveLoadData = new BuffSaveLoadData();

        saveLoadData.RuntimeBuffsSaveData = new List<RuntimeBuffSaveData>();

        for (int i = 0; i < PlayerBuffsController.Instance.PlayerBuffsContainer.RuntimeBuffs.Count; i++)
        {
            saveLoadData.RuntimeBuffsSaveData.Add(PlayerBuffsController.Instance.PlayerBuffsContainer.RuntimeBuffs[i].GetSaveBuffData());
        }

        return saveLoadData;
    }

    public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
    {
        BuffSaveLoadData buffSaveLoadData = ExtractDataType(generalSaveData);

        if (buffSaveLoadData != null)
        {
            PlayerBuffsController.Instance.DisableAndRemoveAllBuffs();

            PlayerBuffsController.Instance.LoadSaveBuffData(buffSaveLoadData);
        }
        else
        {
            SetDefaultData();
        }
    }

    public override void SetDefaultData()
    {
        PlayerBuffsController.Instance.DisableAndRemoveAllBuffs();
    }
}

[System.Serializable]
public class BuffSaveLoadData
{
    public List<RuntimeBuffSaveData> RuntimeBuffsSaveData = new List<RuntimeBuffSaveData>();
}