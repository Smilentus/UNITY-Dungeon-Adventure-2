using UnityEngine;


[CreateAssetMenu(fileName = "ArtCinematicProfile", menuName = "CinematicSystem/New ArtCinematicProfile")]
public partial class ArtCinematicProfile : BaseCinematicProfile
{
    [field: SerializeField]
    public Sprite ArtBody { get; private set; }
}