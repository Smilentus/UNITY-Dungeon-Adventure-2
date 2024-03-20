using UnityEngine;
using UnityEngine.UI;

public class PanelsManager : MonoBehaviour
{
    [Header("Все панели персонажа")]
    public GameObject[] PlayerPanels;

    [Header("Кнопки панелей персонажа")]
    public GameObject[] PlayerButtons;

    [Header("Панели действий (не локации)")]
    public GameObject[] ActionPanels;

    [Header("Цвет нажатой кнопки")]
    public Color pressedColor;

    [Header("Цвет обычной кнопки")]
    public Color normalColor;

    // Закрытие всех панелей игрока
    public void CloseAllPlayerPanels(int except)
    {
        FindObjectOfType<CraftingManager>().HideActions();
        FindObjectOfType<Inventory>().HideDescr();
        FindObjectOfType<Inventory>().EmptyMouseSlot();
        FindObjectOfType<Inventory>().isOpened = false;

        for(int i = 0; i < PlayerPanels.Length; i++)
        {
            if (i == except)
                continue;
            else
            {
                PlayerPanels[i].SetActive(false);
            }
            PlayerButtons[i].GetComponent<Image>().color = normalColor;
        }
    }

    // Закрытие всех панелей действия
    public void CloseAllActionPanels()
    {
        for (int i = 0; i < ActionPanels.Length; i++)
        {
            ActionPanels[i].SetActive(false);
        }
    }

    // Открытие панели игрока
    public void OpenHidePlayerPanel(int num, bool act)
    {
        CloseAllPlayerPanels(num);
        PlayerPanels[num].SetActive(act);
        if(act)
            PlayerButtons[num].GetComponent<Image>().color = pressedColor;
        else
            PlayerButtons[num].GetComponent<Image>().color = normalColor;
    }

    // Открытие панели действия
    /// <summary>
    /// 0 - Битва.
    /// </summary>
    /// <param name="num"></param>
    public void OpenHideActionPanel(int num, bool act)
    {
        ActionPanels[num].SetActive(act);
    }

    // Открытие и закрытие карты
    public void OpenHideMap()
    {
        OpenHidePlayerPanel(2, !PlayerPanels[2].activeSelf);
    }
}