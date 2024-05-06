using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dimasyechka.Code._LEGACY_.Managers
{
    public class LoadingSceneScript : MonoBehaviour
    {
        [Header("Текст загрузки")]
        public TextMeshProUGUI loadingText;

        private int letterCounter = 3;

        private void Start()
        {
            StartCoroutine(_animateText());
            StartCoroutine(_loadScene());
        }

        // Загрузка уровня
        public IEnumerator _loadScene()
        {
            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("GameScene");

            while(!asyncLoadLevel.isDone)
            {
                yield return null;
            }

            yield return asyncLoadLevel;
        }

        // Анимация текста загрузки
        public IEnumerator _animateText()
        {
            while(true)
            {
                switch (letterCounter)
                {
                    case 0:
                        loadingText.text = "Загрузка";
                        break;
                    case 1:
                        loadingText.text = "Загрузка.";
                        break;
                    case 2:
                        loadingText.text = "Загрузка..";
                        break;
                    case 3:
                        loadingText.text = "Загрузка...";
                        break;
                }

                letterCounter++;
                if (letterCounter > 3)
                    letterCounter = 0;
                if (letterCounter < 0)
                    letterCounter = 3;

                yield return new WaitForSeconds(1);
            }
        }
    }
}
