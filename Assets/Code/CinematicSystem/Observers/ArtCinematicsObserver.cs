using System;
using Dimasyechka.Code.CinematicSystem.Profiles;
using UnityEngine;

namespace Dimasyechka.Code.CinematicSystem.Observers
{
    public class ArtCinematicsObserver : MonoBehaviour, IObserver<BaseCinematicProfile>
    {
        public event Action<ArtCinematicProfile> onProfileReceived;
        public event Action onCompleted;


        public void OnCompleted()
        {
            onCompleted?.Invoke();
        }

        public void OnError(Exception error)
        {
            Debug.Log($"{error.Message}");
        }

        public void OnNext(BaseCinematicProfile profile)
        {
            if (profile.GetType().Equals(typeof(ArtCinematicProfile)))
            {
                ArtCinematicProfile artProfile = (ArtCinematicProfile)profile;

                onProfileReceived?.Invoke(artProfile);
            }
        }
    }
}