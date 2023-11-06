using UnityEngine;

public class InventorySaveLoadConverter : SaveLoadBaseConverter<InventorySaveData>
{
    private void Awake()
    {
        SaveLoadSystemController.Instance.AddSaveLoadConverter(this);
    }


    public override InventorySaveData GetConverterData()
    {
        InventorySaveData inventorySaveData = new InventorySaveData();

        // Сохраняем инвентарь

        return inventorySaveData;
    }

    public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
    {

    }
}

[System.Serializable]
public class InventorySaveData
{

}