using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskButton : MonoBehaviour
{
    [Header("Идентификатор задания")]
    public string taskID;

    // Нажатие на кнопку
    public void Press()
    {
        FindObjectOfType<TasksManager>().ShowTaskInfo(taskID);
    }
}
