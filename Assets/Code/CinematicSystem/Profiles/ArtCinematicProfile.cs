using UnityEngine;

namespace Dimasyechka.Code.CinematicSystem.Profiles
{
    [CreateAssetMenu(fileName = "ArtCinematicProfile", menuName = "CinematicSystem/New ArtCinematicProfile")]
    public partial class ArtCinematicProfile : BaseCinematicProfile
    {
        [field: SerializeField]
        public Sprite ArtBody { get; private set; }
    }
}