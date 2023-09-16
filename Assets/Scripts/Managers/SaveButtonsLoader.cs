using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveButtonsLoader : MonoBehaviour
{
    [System.Serializable]
    public class State
    {
        public string savedState;

        public string[] saveGameDate;
        public DateTime[] saveRealDate;
        public bool[] isSaved;
    }

    public GameObject[] saveLoadButtons;

    public string loadGameState;

    private void Start()
    {
        LoadState();

        for(int i = 0; i < saveLoadButtons.Length; i++)
        {
            saveLoadButtons[i].GetComponent<saveSlot>().UpdateText();
        }

        if (SceneManager.GetActiveScene().name == "GameScene")
            FindObjectOfType<SavingManager>().LoadAfterLoading();
    }

    // Сохранение состояния загрузки
    public void SaveState(string stateToSave)
    {
        State save = new State();

        save.savedState = stateToSave;

        save.saveGameDate = new string[saveLoadButtons.Length];
        save.saveRealDate = new DateTime[saveLoadButtons.Length];
        save.isSaved = new bool[saveLoadButtons.Length];
        for (int i = 0; i < saveLoadButtons.Length; i++)
        {
            save.saveGameDate[i] = saveLoadButtons[i].GetComponent<saveSlot>().saveGameDate;
            save.saveRealDate[i] = saveLoadButtons[i].GetComponent<saveSlot>().saveRealDate;
            save.isSaved[i] = saveLoadButtons[i].GetComponent<saveSlot>().isSaved;
            saveLoadButtons[i].GetComponent<saveSlot>().UpdateText();
        }

        if (!Directory.Exists(Application.persistentDataPath + "/files"))
            Directory.CreateDirectory(Application.persistentDataPath + "/files");

        FileStream fs = new FileStream(Application.persistentDataPath + "/files/state.sv", FileMode.Create);

        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fs, save);
        fs.Close();

        Debug.Log("Состояние сохранено! - " + stateToSave);
    }

    // Загрузка состояния загрузки
    public void LoadState()
    {
        if (File.Exists(Application.persistentDataPath + "/files/state.sv"))
        {
            FileStream fs = new FileStream(Application.persistentDataPath + "/files/state.sv", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                State save = (State)formatter.Deserialize(fs);

                loadGameState = save.savedState;

                for (int i = 0; i < saveLoadButtons.Length; i++)
                {
                    saveLoadButtons[i].GetComponent<saveSlot>().saveGameDate = save.saveGameDate[i];
                    saveLoadButtons[i].GetComponent<saveSlot>().saveRealDate = save.saveRealDate[i];
                    saveLoadButtons[i].GetComponent<saveSlot>().isSaved = save.isSaved[i];
                    saveLoadButtons[i].GetComponent<saveSlot>().UpdateText();
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
            finally
            {
                fs.Close();
            }
            Debug.Log("Состояние загружено! - " + loadGameState);
        }
    }

    // Обновляем слот автосохранения
    public void UpdateAutoSaveSlot()
    {
        if (File.Exists(Application.persistentDataPath + "/files/AutoSave.sv"))
            saveLoadButtons[saveLoadButtons.Length - 1].GetComponent<saveSlot>().isSaved = true;
        else
            saveLoadButtons[saveLoadButtons.Length - 1].GetComponent<saveSlot>().isSaved = false;

        saveLoadButtons[saveLoadButtons.Length - 1].GetComponent<saveSlot>().saveRealDate = DateTime.Now;
        saveLoadButtons[saveLoadButtons.Length - 1].GetComponent<saveSlot>().saveGameDate = GameTimeFlowController.Instance.DateNow();
        saveLoadButtons[saveLoadButtons.Length - 1].GetComponent<saveSlot>().UpdateText();
    }

    // Удаляет файл сохранения
    public void RemoveSaveFile(string fileToRemove)
    {
        if (File.Exists(Application.persistentDataPath + "/files/" + fileToRemove + ".sv"))
        {
            File.Delete(Application.persistentDataPath + "/files/" + fileToRemove + ".sv");
            Debug.Log("Файл сохранения " + fileToRemove + " удалён!");
        }
        else
            Debug.Log("Такого файла не существует!");

        // Сохраняем состояние
        SaveState("AutoSave");
    }

    // Загрузка игры при клике на ячейку сохранения
    public void LoadGame(string saveLoad)
    {
        SaveState(saveLoad);
        FindObjectOfType<faderScript>().FadeScreen("LoadingScene");
    }
}
