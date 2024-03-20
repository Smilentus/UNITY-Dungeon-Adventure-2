using UnityEngine;

public class InventorySaveLoadConverter : SaveLoadBaseConverter<InventorySaveData>
{
    [SerializeField]
    private BaseInventoryContainerProfile[] m_defaultPlayerContainers;


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
        foreach (BaseInventoryContainerProfile container in m_defaultPlayerContainers)
        {
            InventoryController.Instance.AddInventoryContainer(container);
        }
    }
}

[System.Serializable]
public class InventorySaveData
{

}