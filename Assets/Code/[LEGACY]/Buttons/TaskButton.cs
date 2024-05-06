using Dimasyechka.Code._LEGACY_.Managers;
using UnityEngine;

namespace Dimasyechka.Code._LEGACY_.Buttons
{
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
}
