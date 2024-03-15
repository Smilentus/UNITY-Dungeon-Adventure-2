using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BetweenScenesLoaderAdapter : MonoBehaviour
{
    private static BetweenScenesLoaderAdapter _instance;
    public static BetweenScenesLoaderAdapter Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BetweenScenesLoaderAdapter>();  
            }

            return _instance;
        }
    }


    [field: SerializeField]
    public BetweenScenesLoadableData LoadableData { get; private set; }


    private void Awake()
    {
        if (LoadableData == null || 
            string.IsNullOrEmpty(LoadableData.SelectedSaveFileFullPath)
           ) return;

        Debug.Log($"[BetweenScenesLoaderAdapter] --> Попытка загрузить сохранение: '{LoadableData.SelectedSaveFileFullPath}'");

        SaveLoadSystemController.Instance.TryLoadAndParseDataFromFile(LoadableData.SelectedSaveFileFullPath);
    }


    public void SetLoadablePath(string loadablePath)
    {
        LoadableData.SelectedSaveFileFullPath = loadablePath;
    }
}