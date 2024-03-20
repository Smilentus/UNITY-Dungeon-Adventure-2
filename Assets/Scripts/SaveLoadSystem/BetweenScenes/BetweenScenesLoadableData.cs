using UnityEngine;


[CreateAssetMenu(fileName = "BetweenScenesLoadableData", menuName = "SaveLoadSystem/BetweenScenesLoadableData")]
public class BetweenScenesLoadableData : ScriptableObject
{
    [field: SerializeField]
    public string SelectedSaveFileFullPath { get; set; }
}