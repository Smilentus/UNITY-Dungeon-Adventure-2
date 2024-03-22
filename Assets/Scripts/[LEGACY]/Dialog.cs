using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [Header("Идентификатор диалога")]
    public string dialogID;
    [Header("Текст повествования")]
    [TextArea(3, 5)]
    public string[] dialogText;
}
