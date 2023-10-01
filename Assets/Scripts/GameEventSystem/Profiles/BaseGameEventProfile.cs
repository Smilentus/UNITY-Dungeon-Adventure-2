using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creatable/GameEventSystem/New GameEventProfile", fileName = "GameEventProfile_")]
[System.Serializable]
public class BaseGameEventProfile : ScriptableObject
{
    [TextArea(2, 5)]
    [SerializeField]
    protected string m_eventTitle;
    /// <summary>
    ///     Наименование игрового ивента
    /// </summary>
    public string EventTitle => m_eventTitle;


    [TextArea(5, 10)]
    [SerializeField]
    protected string m_eventDescription;
    /// <summary>
    ///     Описание игрового ивента
    /// </summary>
    public string EventDescription => m_eventDescription;
}
