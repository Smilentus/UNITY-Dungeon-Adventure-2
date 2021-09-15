using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class faderScript : MonoBehaviour
{
    [Header("Картинка затемнения")]
    public RawImage fadeImage;

    // Затемнение экрана
    public void FadeScreen(string scene)
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeIn(scene));
    }
    public IEnumerator FadeIn(string scene)
    {
        fadeImage.raycastTarget = true;
        Color alphaColor = new Color(0, 0, 0, 0);

        while(fadeImage.color.a < 1)
        {
            alphaColor += new Color(0, 0, 0, 0.04f);
            fadeImage.color = alphaColor;
            yield return null;
        }

        SceneManager.LoadScene(scene);
    }

    // Осветленее экрана
    public void FadeScreenOut()
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }
    public IEnumerator FadeOut()
    {
        Color alphaColor = new Color(0, 0, 0, 1);
        fadeImage.raycastTarget = true;

        while (fadeImage.color.a > 0)
        {
            alphaColor -= new Color(0, 0, 0, 0.04f);
            fadeImage.color = alphaColor;
            yield return null;
        }

        fadeImage.raycastTarget = false;
    }
}
