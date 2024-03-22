using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "CinematicSequence", menuName = "CinematicSystem/New CinematicSequence")]
public class CinematicSequence : ScriptableObject
{
    [field: SerializeField]
    public BaseCinematicProfile Sequence { get; private set; }
}