using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creatable/GameEventSystem/New GameEventProfile", fileName = "New GameEventProfile")]
public class BaseGameEventProfile : ScriptableObject
{
    [SerializeField]
    protected string m_eventTitle;
    /// <summary>
    ///     Наименование игрового ивента
    /// </summary>
    public string EventTitle => m_eventTitle;


    [SerializeField]
    protected string m_eventDescription;
    /// <summary>
    ///     Описание игрового ивента
    /// </summary>
    public string EventDescription => m_eventDescription;
}
