using UnityEngine;

public class InventorySaveLoadConverter : SaveLoadBaseConverter<InventorySaveData>
{
    public override InventorySaveData GetConverterData(string saveFileName)
    {
        InventorySaveData inventorySaveData = new InventorySaveData();

        // ��������� ���������

        return inventorySaveData;
    }

    public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
    {

    }

    public override void SetDefaultData()
    {

    }
}

[System.Serializable]
public class InventorySaveData
{

}