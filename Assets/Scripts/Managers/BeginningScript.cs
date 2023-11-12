using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginningScript : MonoBehaviour
{
    [Header("Ссылка на StoryManager")]
    public StorytellerManager SM;

    private void Start()
    {
        if(RuntimePlayer.Instance.RuntimePlayerStats.isFirstEnter)
        {
            StartEducation();
        }
    }

    // Начало обучения и сюжета
    private void StartEducation()
    {
        SM.StartDialog("Learning_01");
        StartCoroutine(WaitForDialogEnd("Learning_01"));
    }

    // Ожидание окончания диалога
    private IEnumerator WaitForDialogEnd(string dialogID)
    {
        while (true)
        {
            if (StorytellerManager.isEnd)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        // Действие дальше
        switch(dialogID)
        {
            case "Learning_01":

                break;
        }
    }
}
