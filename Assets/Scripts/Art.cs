using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Art
{
    [Header("Идентификатор арта")]
    public string artID;
    [Header("Арты в коллекции")]
    public Texture[] artImages;
}