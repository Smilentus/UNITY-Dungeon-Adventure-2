using UnityEngine;

namespace Dimasyechka.Code._LEGACY_
{
    [System.Serializable]
    public class Art
    {
        [Header("Идентификатор арта")]
        public string artID;
        [Header("Арты в коллекции")]
        public Texture[] artImages;
    }
}