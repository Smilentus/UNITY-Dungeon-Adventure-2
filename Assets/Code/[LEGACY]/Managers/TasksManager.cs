using System.Collections.Generic;
using Dimasyechka.Code._LEGACY_.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code._LEGACY_.Managers
{
    public class TasksManager : MonoBehaviour
    {
        [Header("Все задания")]
        public List<Task> allTasks = new List<Task>();

        // Все задания игрока, которые уже можно отобразить
        [Header("Все задания игрока")]
        public List<Task> playerTasks = new List<Task>();
        // Дебажная переменная для оптимизации?
        // Считает сколько заданий уже отображено и сколько у игрока
        private int counter, oldCounter;

        [Header("Кнопки")]
        public GameObject[] buttons;

        [Header("Панели заданий")]
        public GameObject[] panels;
        [Header("Панель информации")]
        public GameObject taskInfoPanel;

        [Header("Префаб задания")]
        public GameObject taskPrefab;
        [Header("Родители для спавна заданий")]
        public Transform[] parentsToCreate;

        [Header("Цвета")]
        public Color normalColor;
        public Color pressedColor;

        // Подсчёт количества разных заданий
        public int countActiveTasks()
        {
            int counter = 0;

            for (int i = 0; i < playerTasks.Count; i++)
            {
                if (playerTasks[i].status == Task.taskType.Active)
                {
                    counter++;
                }
            }

            return counter;
        }
        public int countCompletedTasks()
        {
            int counter = 0;

            for (int i = 0; i < playerTasks.Count; i++)
            {
                if (playerTasks[i].status == Task.taskType.Completed)
                {
                    counter++;
                }
            }

            return counter;
        }

        /*ОЧЕНЬ НЕОПТИМИЗИРОВАННЫЙ МЕТОД, ИСПРАВЬ!*/
        // Создаём кнопку задания для отображения игроку
        // Полностью удаляет все задания и заново добавляет их
        public void CreateTasksButtons()
        {
            // Считаем сколько отображено заданий
            counter = parentsToCreate[0].childCount + parentsToCreate[1].childCount;
            // Очищаем отображённые задания
            for (int i = 0; i < parentsToCreate[0].childCount; i++)
            {
                Destroy(parentsToCreate[0].GetChild(i).gameObject);
            }
            for (int i = 0; i < parentsToCreate[1].childCount; i++)
            {
                Destroy(parentsToCreate[1].GetChild(i).gameObject);
            }

            for (int i = 0; i < playerTasks.Count; i++)
            {
                // Создаём кнопку задания
                GameObject task = Instantiate(taskPrefab);
                task.GetComponent<TaskButton>().taskID = playerTasks[i].taskID;
                task.transform.GetChild(0).GetComponent<Text>().text = playerTasks[i].taskHeader;

                // Даём определённую панель каждому заданию
                if (playerTasks[i].status == Task.taskType.Active)
                {
                    task.transform.parent = parentsToCreate[0];
                }
                else
                {
                    task.transform.parent = parentsToCreate[1];
                }
            }
        }

        // Ищем номер задания в общем списке
        public int taskPos(string taskID)
        {
            for(int i = 0; i < allTasks.Count; i++)
            {
                if(allTasks[i].taskID == taskID)
                {
                    return i;
                }
            }

            return 0;
        }

        #region Взаимодействие с панелью заданий
        // Открытие и закрытие панели в одном действии
        public void ShowHideTaskPanel()
        {
            FindObjectOfType<PanelsManager>().CloseAllPlayerPanels(4);
            FindObjectOfType<PanelsManager>().OpenHidePlayerPanel(4, !FindObjectOfType<PanelsManager>().PlayerPanels[4].activeSelf);

            OpenPanel(0);
            taskInfoPanel.SetActive(false);
        }

        // Только открытие панели
        public void ShowTaskPanel()
        {
            FindObjectOfType<PanelsManager>().CloseAllPlayerPanels(4);
            FindObjectOfType<PanelsManager>().OpenHidePlayerPanel(4, true);
        }

        // Только закрытие панели
        public void HideTaskPanel()
        {
            FindObjectOfType<PanelsManager>().CloseAllPlayerPanels(4);
            FindObjectOfType<PanelsManager>().OpenHidePlayerPanel(4, false);
        }

        // Открытие панели и закрытие других
        public void OpenPanel(int panelNum)
        {
            for(int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(false);
            }

            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<Image>().color = normalColor;
            }

            buttons[panelNum].GetComponent<Image>().color = pressedColor;
            panels[panelNum].SetActive(true);

            if(countActiveTasks() == 0) { panels[0].transform.GetChild(2).gameObject.SetActive(true); } else { panels[0].transform.GetChild(2).gameObject.SetActive(false); }
            if(countCompletedTasks() == 0) { panels[1].transform.GetChild(2).gameObject.SetActive(true); } else { panels[1].transform.GetChild(2).gameObject.SetActive(false); }
        }
        #endregion

        #region Взаимодействие с заданиями
        // Получение задания
        public void AddTask(string taskID)
        {
            playerTasks.Add(allTasks[taskPos(taskID)]);
            playerTasks[playerTasks.Count - 1].status = Task.taskType.Active;

            // Добавляем задания
            CreateTasksButtons();
        }

        // Проверка выполнения задания
        public void CheckTaskEnd()
        {

        }

        // Показать информацию о задании
        public void ShowTaskInfo(string taskID)
        {
            taskInfoPanel.transform.GetChild(0).GetComponent<Text>().text = playerTasks[taskPos(taskID)].taskDescr;
            taskInfoPanel.SetActive(true);
        }

        // Меняем статус
        public void ChangeTaskStatus(Task.taskType status)
        {
        
        }
        #endregion
    }
}
