using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameTimeFlowEvent", menuName = "Creatable/Create GameTimeFlow")]
public class GameTimeFlowEvent : ScriptableObject
{
    public string EventUID;

    [TextArea(3, 5)]
    public string EventTitle;

    [TextArea(5, 15)]
    public string EventDescription;
}