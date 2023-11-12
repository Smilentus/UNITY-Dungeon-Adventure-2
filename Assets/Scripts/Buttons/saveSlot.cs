using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine.SceneManagement;


public class saveSlot : MonoBehaviour, IPointerClickHandler
{
    public string hiddenSaveName;
    public string saveName;
    public DateTime saveRealDate;
    public string saveGameDate;
    public bool isSaved;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isSaved && hiddenSaveName != "AutoSave" && SceneManager.GetActiveScene().name == "GameScene")
        {
            isSaved = true;

            saveRealDate = DateTime.Now;
            saveGameDate = GameTimeFlowController.Instance.DateNow();

            FindObjectOfType<SaveButtonsLoader>().SaveState(hiddenSaveName);
            //FindObjectOfType<SavingManager>().SaveGame(hiddenSaveName);

            UpdateText();
        }

        if (isSaved)
            transform.GetChild(1).gameObject.SetActive(!transform.GetChild(1).gameObject.activeSelf);
    }

    public void LoadGame()
    {
        Debug.Log("Загружаю состояние: " + hiddenSaveName + " | " + SceneManager.GetActiveScene().name);
        FindObjectOfType<SaveButtonsLoader>().LoadGame(hiddenSaveName);
    }

    public void RemoveGame()
    {
        Debug.Log("Удаляю сохранение: " + hiddenSaveName);

        isSaved = false;
        
        FindObjectOfType<SaveButtonsLoader>().RemoveSaveFile(hiddenSaveName);

        transform.GetChild(1).gameObject.SetActive(!transform.GetChild(1).gameObject.activeSelf);

        UpdateText();
    }

    public void UpdateText()
    {
        if (isSaved)
            transform.GetChild(0).GetComponent<Text>().text = saveName + "\n\n Дата: " + saveRealDate.ToShortDateString() +
                "\nВремя: " + saveRealDate.ToShortTimeString() + "\n\nИгровое время: " + saveGameDate;
        else
            transform.GetChild(0).GetComponent<Text>().text = "Пустой слот";
    }
}
