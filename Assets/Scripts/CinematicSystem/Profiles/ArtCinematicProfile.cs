using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ArtCinematicProfile", menuName = "CinematicSystem/New ArtCinematicProfile")]
public partial class ArtCinematicProfile : ScriptableObject
{
    [field: SerializeField]
    public Sprite ArtBody { get; private set; }
}