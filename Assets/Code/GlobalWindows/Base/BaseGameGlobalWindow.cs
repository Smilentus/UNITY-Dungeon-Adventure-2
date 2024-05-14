using Dimasyechka.Code.GlobalWindows.Interfaces;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UI;
#endif

namespace Dimasyechka.Code.GlobalWindows.Base
{
    public class BaseGameGlobalWindow : MonoBehaviour, IGlobalWindow
    {
        private IGlobalWindowData _globalWindowData;
        public IGlobalWindowData GlobalWindowData => _globalWindowData;


        public T GetConvertedWindowData<T>() => (T)GlobalWindowData;


        private bool _isShown;
        public bool IsShown => _isShown;


        public void Hide()
        {
            OnHide();

            _isShown = false;
            this.gameObject.SetActive(false);
        }
        protected virtual void OnHide() { }

        public void SetWindowData(IGlobalWindowData globalWindowData)
        {
            _globalWindowData = globalWindowData;
        }

        public void Show(IGlobalWindowData globalWindowData)
        {
            SetWindowData(globalWindowData);
            Show();
        }

        public void Show()
        {
            _isShown = true;
            this.gameObject.SetActive(true);

            OnShow();
        }
        protected virtual void OnShow() { }
    }

    public class BaseGameGlobalWindowData : IGlobalWindowData
    {
        public string GlobalWindowTitle { get; set; } = "Базовое игровое окно";
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(BaseGameGlobalWindow), true)]
    public class BaseGameGlobalWindowEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawGenerationButton();
        }


        private void DrawGenerationButton()
        {
            if (GUILayout.Button("Generate Canvas Components"))
            {
                CreateCanvasComponents();
            }
        }


        private void CreateCanvasComponents()
        {
            MonoBehaviour mono = serializedObject.targetObject as MonoBehaviour;

            if (mono == null)
            {
                Debug.LogError($"Как ты сюда не MonoBehaviour запихнул?", serializedObject.targetObject);
                return;
            }

            Canvas canvas = mono.GetComponent<Canvas>();
            CanvasScaler canvasScaler = mono.GetComponent<CanvasScaler>();
            GraphicRaycaster raycaster = mono.GetComponent<GraphicRaycaster>();

            if (canvas == null)
            {
                canvas = mono.gameObject.AddComponent<Canvas>();

                canvas.sortingOrder = 1;
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.referencePixelsPerUnit = 100;
            }

            if (canvasScaler == null)
            {
                canvasScaler = mono.gameObject.AddComponent<CanvasScaler>();

                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = new Vector2(1920, 1080);
                canvasScaler.referencePixelsPerUnit = 100;
            }

            if (raycaster == null)
            {
                raycaster = mono.gameObject.AddComponent<GraphicRaycaster>();
            }
        }
    }
#endif
}