using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeLocationObject : MonoBehaviour
{
    [SerializeField]
    private LocationProfile m_locationProfile;
    /// <summary>
    ///     ������ �� ����������� ������� � �������
    /// </summary>
    public LocationProfile LocationProfileReference => m_locationProfile;
}