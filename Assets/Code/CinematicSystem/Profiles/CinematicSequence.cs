using System.Collections.Generic;
using UnityEngine;

namespace Dimasyechka.Code.CinematicSystem.Profiles
{
    [CreateAssetMenu(fileName = "CinematicSequence", menuName = "CinematicSystem/New CinematicSequence")]
    public class CinematicSequence : ScriptableObject
    {
        [field: SerializeField]
        public List<BaseCinematicProfile> Sequence { get; private set; }
    }
}