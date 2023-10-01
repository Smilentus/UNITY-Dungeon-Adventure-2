using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creatable/LocationSystem/New LocationProfile", fileName = "LocationProfile")]
[System.Serializable]
public class LocationProfile : ScriptableObject
{
    [TextArea(3, 5)]
    [SerializeField]
    private string m_locationTitle;
    /// <summary>
    ///     ������������ �������
    /// </summary>
    public string LocationTitle => m_locationTitle;
    

    [TextArea(5, 10)]
    [SerializeField]
    private string m_locationDescription;
    /// <summary>
    ///     ��������� �������� �������
    /// </summary>
    public string LocationDescription => m_locationDescription;


    [SerializeField]
    private List<BaseGameEventProfile> m_locationEvents = new List<BaseGameEventProfile>();
    /// <summary>
    ///     ������ ��������� ������� �� �������
    /// </summary>
    public List<BaseGameEventProfile> LocationEvents => m_locationEvents;


    [SerializeField]
    private List<ItemProfile> m_droppableItems = new List<ItemProfile>();
    /// <summary>
    ///     ������ ���������, ������� ����� ������� �� �������
    /// </summary>
    public List<ItemProfile> DroppableItems => m_droppableItems;
}
