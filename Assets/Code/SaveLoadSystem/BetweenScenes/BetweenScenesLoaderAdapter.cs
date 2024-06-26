using UnityEngine;

namespace Dimasyechka.Code.SaveLoadSystem.BetweenScenes
{
    public class BetweenScenesLoaderAdapter : MonoBehaviour
    {
        private static BetweenScenesLoaderAdapter _instance;
        public static BetweenScenesLoaderAdapter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<BetweenScenesLoaderAdapter>();  
                }

                return _instance;
            }
        }


        [field: SerializeField]
        public BetweenScenesLoadableData LoadableData { get; private set; }


        public void SetLoadablePath(string loadablePath)
        {
            LoadableData.SelectedSaveFileFullPath = loadablePath;
        }
    }
}