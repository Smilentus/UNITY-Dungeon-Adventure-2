using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "CinematicSequence", menuName = "CinematicSystem/New CinematicSequence")]
public class CinematicSequence : ScriptableObject
{
    [field: SerializeField]
    public List<BaseCinematicProfile> Sequence { get; private set; }
}