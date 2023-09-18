using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorytellerManager : MonoBehaviour
{
    [Header("Объект для повествования")]
    public GameObject storytellerPanel;
    [Header("Текст для вывода")]
    public Text storytellerText;
    [Header("Блокиратор")]
    public GameObject actionBlocker;

    [Header("Все диалоги в игре")]
    public Dialog[] Dialogs;

    public Queue<string> sentences;

    // Последний действующий диалог
    private string lastDialogID;

    // Закончился ли диалог
    public static bool isEnd;
    // Текущий последний показанный диалог
    public static bool isSkip;

    private void Start()
    {
        isEnd = true;
        sentences = new Queue<string>();
    }

    // Поиск диалога по ID
    private int FindDialogInList(string findID)
    {
        int returnPos = 0;

        for(int i = 0; i < Dialogs.Length; i++)
        {
            if (Dialogs[i].dialogID == findID)
            {
                returnPos = i;
                break;
            }
        }

        return returnPos;
    }

    // Показываем новый диалог или монолог, без разницы
    public void StartDialog(int dialog)
    {
        // Активируем UI элементы
        actionBlocker.SetActive(true);
        storytellerPanel.SetActive(true);

        isEnd = false;

        lastDialogID = Dialogs[dialog].dialogID;

        // Очищаем текущие сообщения
        sentences.Clear();

        // Добавляем новые сообщения
        foreach (string sentence in Dialogs[dialog].dialogText)
        {
            sentences.Enqueue(sentence);
        }

        // Выводим следующее предложение
        DisplayNextSentence();
    }
    public void StartDialog(string dialog)
    {
        // Активируем UI элементы
        actionBlocker.SetActive(true);
        storytellerPanel.SetActive(true);

        isEnd = false;

        if (sentences == null)
            sentences = new Queue<string>();

        // Очищаем текущие сообщения
        sentences.Clear();

        int pos = FindDialogInList(dialog);

        lastDialogID = Dialogs[pos].dialogID;
        
        // Добавляем новые сообщения
        foreach (string sentence in Dialogs[pos].dialogText)
        {
            sentences.Enqueue(sentence);
        }

        // Выводим следующее предложение
        DisplayNextSentence();
    }

    // Пропускаем вывод текста по буквам и открываем другое сообщение, если уже пропустили
    public void SkipLettering()
    {
        if (isSkip)
            DisplayNextSentence();
        else if (!isSkip)
            isSkip = true;
    }

    // Отображение сообщения диалога
    public void DisplayNextSentence()
    {
        // Если сообщений 0, то закрываем диалог
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Убираем из списка это предложение
        string sentence = sentences.Dequeue();

        // Выводим текст на экран
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public IEnumerator TypeSentence(string sentence)
    {
        // Устанавлием пропуск на false
        isSkip = false;

        string tempSentence = sentence;

        // Очищаем текущий текст
        storytellerText.text = "";

        // Выводим побуквенно
        foreach(char letter in sentence.ToCharArray())
        {
            // Если пропустили, то сразу весь текст
            if(isSkip)
            {
                storytellerText.text = sentence;
                break;
            }

            // Добавляем букву
            storytellerText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }

        // Сообщение закончилось
        isSkip = true;
    }

    // Действие после диалога
    public void AfterDialogAction(string dialogID)
    {
        if(dialogID == "TalkToBarmen_01")
        {
            Debug.Log("Покупаю деревню!");
            if(Player.Money >= 100000 && !Player.isHadVillage)
            {
                Player.isHadVillage = true;
                Player.Money -= 100000;
                // Покупка деревни
                //FindObjectOfType<LocationsController>().AddNewExtraLocation(LocationsController.Location.Player_Village);
            }
        }
    }

    // Конец диалога
    public void EndDialogue()
    {
        isEnd = true;

        // Закрываем UI элементы
        actionBlocker.SetActive(false);
        storytellerPanel.SetActive(false);

        // Действие после конца диалога
        AfterDialogAction(lastDialogID);

        lastDialogID = "null";
    }
}
