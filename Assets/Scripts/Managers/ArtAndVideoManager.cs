using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// А здесь Аниме ^_^
public class ArtAndVideoManager : MonoBehaviour
{
    public StorytellerManager SM;

    [Header("Все арты в игре")]
    public Art[] allArts;

    [Header("Текущие арты для отображения")]
    public Queue<Texture> artsToShow;

    [Header("Панель артов")]
    public GameObject artPanel;
    [Header("Затемнялка")]
    public RawImage fadeImage;
    [Header("Родитель для арта")]
    public RawImage artParent;

    // Закончилось ли шоу
    public static bool isEnd;
    // Пропустили ли показ артов
    public static bool isSkip;
    public static bool canContinue;

    // Последнее арт шоу
    private string lastArtID;

    private void Awake()
    {
        SM = FindObjectOfType<StorytellerManager>();
    }

    private void Start()
    {
        isSkip = false;
        artsToShow = new Queue<Texture>();
        isEnd = true;
    }

    // Поиск в листе артов
    private int FindArtInList(string artID)
    {
        int pos = -1;

        for(int i = 0; i < allArts.Length; i++)
        {
            if(allArts[i].artID == artID)
            {
                pos = i;
                break;
            }
        }

        return pos;
    }

    // Арт шоу, история, сюжет
    public void StartArtShow(string artID)
    {
        // Активируем UI элементы
        artPanel.SetActive(true);

        isEnd = false;

        int pos = FindArtInList(artID);

        lastArtID = allArts[pos].artID;

        // Добавляем новые сообщения
        foreach (Texture image in allArts[pos].artImages)
        {
            artsToShow.Enqueue(image);
        }

        artParent.color = new Color(0, 0, 0, 1);

        // Выводим следующее предложение
        DisplayNextArt();
    }

    // Отображение следующего арта
    public void DisplayNextArt()
    {
        // Если сообщений 0, то закрываем диалог
        if (artsToShow.Count == 0)
        {
            EndArtShow();
            return;
        }

        // Убираем из списка это предложение
        Texture art = artsToShow.Dequeue();

        StartCoroutine(fadeAndShowArt(art));
    }

    // Пропускаем плавный показ артов
    public void SkipLettering()
    {
        if (isSkip)
            DisplayNextArt();
        else if(!isSkip && canContinue)
        {
            DisplayNextArt();
        }
        else if (!isSkip)
        {
            isSkip = true;
        }
    }

    // Затемнение и отображение арта
    private IEnumerator fadeAndShowArt(Texture art)
    {
        isSkip = false;
        canContinue = false;

        fadeImage.gameObject.SetActive(true);
        Color alphaColor = new Color(0, 0, 0, 0);
        // Затемняем
        while (fadeImage.color.a < 1)
        {
            if(isSkip)
            {
                alphaColor = new Color(0, 0, 0, 1);
                fadeImage.color = alphaColor;
                break;
            }
            alphaColor += new Color(0, 0, 0, 0.04f);
            fadeImage.color = alphaColor;
            yield return null;
        }

        // Отображаем
        artParent.color = new Color(1, 1, 1, 1);
        artParent.texture = art;
        isSkip = false;

        alphaColor = new Color(0, 0, 0, 1);
        // Затемняем
        while (fadeImage.color.a > 0)
        {
            if (isSkip)
            {
                alphaColor = new Color(0, 0, 0, 0);
                fadeImage.color = alphaColor;
                break;
            }
            alphaColor -= new Color(0, 0, 0, 0.04f);
            fadeImage.color = alphaColor;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
        isSkip = false;
        canContinue = true;
    }

    // Действие после окончания арт шоу
    public void AfterShowAction(string artID)
    {
        Debug.Log(artID);
        
        switch(artID)
        {
            case "FirstEnter_01":
                StartArtShow("FirstEnter_02");
                SM.StartDialog("Learning_02");
                break;
            case "FirstEnter_02":
                StartArtShow("FirstEnter_03");
                SM.StartDialog("Learning_03");
                break;
            case "FirstEnter_03":
                StartArtShow("FirstEnter_04");
                SM.StartDialog("Learning_04");
                break;
            case "FirstEnter_04":
                StartArtShow("FirstEnter_05");
                SM.StartDialog("Learning_05");
                break;
            case "FirstEnter_05":
                StartArtShow("FirstEnter_06");
                SM.StartDialog("Learning_06");
                break;
            case "IlluminatiArt":
                FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Illuminati);
                break;
        }
    }

    // Конец отображения артов
    public void EndArtShow()
    {
        isEnd = true;
        artPanel.SetActive(false);

        AfterShowAction(lastArtID);
    }
}
