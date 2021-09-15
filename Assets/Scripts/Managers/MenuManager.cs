using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Аниматор перехода")]
    public Animator animator;

    [Header("Может ли игрок нажать на кнопку")]
    public bool canPress = true;

    public Text verText;

    public void Start()
    {
        verText.text = "v" + Application.version;
    }

    // Открыть экран сохранений
    public void OpenSaveLoadPanel()
    {
        if(canPress)
        {
            canPress = false;
            animator.SetBool("isOpen", true);
        }
    }

    // Закрытие экрана сохранений
    public void CloseSaveLoadPanel()
    {
        animator.SetBool("isOpen", false);
        canPress = true;
    }

    // Продолжить игру с последней сохранённой точки
    public void ContinueGame()
    {
        if (canPress)
        {
            FindObjectOfType<SaveButtonsLoader>().SaveState("AutoSave");
            FindObjectOfType<faderScript>().FadeScreen("LoadingScene");
        }
    }

    // Для загрузки с других скриптов
    public void LoadSave(string loadName)
    {
        if (canPress)
        {
            FindObjectOfType<SaveButtonsLoader>().SaveState(loadName);
            FindObjectOfType<faderScript>().FadeScreen("LoadingScene");
        }
    }

    // Начать новую игру
    public void StartNewGame()
    {
        if (canPress)
        {
            FindObjectOfType<SaveButtonsLoader>().SaveState("NewGame");
            FindObjectOfType<faderScript>().FadeScreen("LoadingScene");
        }
    }

    // Помощь
    public void AboutGame()
    {
        if(canPress)
        {
            //canPress = false;
        }
    }

    // Выход из игры
    public void ExitGame()
    {
        if (canPress)
        {
            //canPress = false;
            Application.Quit();
        }
    }
}
