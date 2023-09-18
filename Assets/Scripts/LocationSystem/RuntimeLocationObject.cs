using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeLocationObject : MonoBehaviour
{
    [SerializeField]
    private LocationProfile m_locationProfile;
    /// <summary>
    ///     —сылка на прив€занную локацию к области
    /// </summary>
    public LocationProfile LocationProfileReference => m_locationProfile;
}