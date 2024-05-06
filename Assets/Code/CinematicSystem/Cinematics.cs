using System;
using System.Collections.Generic;
using Dimasyechka.Code.CinematicSystem.InputHandlers;
using Dimasyechka.Code.CinematicSystem.Profiles;
using UnityEngine;

namespace Dimasyechka.Code.CinematicSystem
{
    public class Cinematics : MonoBehaviour, IObservable<BaseCinematicProfile>
    {
        public event Action<CinematicSequence> onCinematicsStarted;
        public event Action<CinematicSequence> onCinematicsEnded;


        [SerializeField]
        private List<GameObject> _observersGameObjects = new List<GameObject>();


        private CinematicSequence _playingSequence;
        private int _playingCinematicIndex;


        private bool _isPlaying;
        public bool IsPlaying => _isPlaying;


        private List<IObserver<BaseCinematicProfile>> _cinematicObservers = new List<IObserver<BaseCinematicProfile>>();

        private ICinematicsInputHandler _cinematicsInputHandler;


        private void Awake()
        {
            ExtractObserversFromGameObjects();
            SubscribeObservers();

            ExtractInputHandler();
            SubscribeInputHandler();
        }
        private void OnDestroy()
        {
            UnSubscrubeInputHandler();
        }


        private void ExtractObserversFromGameObjects()
        {
            foreach (GameObject go in _observersGameObjects)
            {
                IObserver<BaseCinematicProfile> observerComponent = null;
                if (go.TryGetComponent<IObserver<BaseCinematicProfile>>(out observerComponent))
                {
                    _cinematicObservers.Add(observerComponent);
                }
            }
        }
        private void SubscribeObservers()
        {
            foreach (IObserver<BaseCinematicProfile> observer in _cinematicObservers)
            {
                Subscribe(observer);
            }
        }


        private void ExtractInputHandler()
        {
            _cinematicsInputHandler = GetComponentInChildren<ICinematicsInputHandler>();
        }
        private void SubscribeInputHandler()
        {
            if (_cinematicsInputHandler != null)
            {
                _cinematicsInputHandler.onForceSkip += PlayNextCinematicProfile;
            }
        }
        private void UnSubscrubeInputHandler()
        {
            if (_cinematicsInputHandler != null)
            {
                _cinematicsInputHandler.onForceSkip -= PlayNextCinematicProfile;
            }
        }


        public void StartCinematics(CinematicSequence sequence)
        {
            if (_isPlaying) return;

            _isPlaying = true;
            _playingSequence = sequence;

            _playingCinematicIndex = -1;

            PlayNextCinematicProfile();

            ToggleInputHandlers(true);

            onCinematicsStarted?.Invoke(_playingSequence);
        }
        public void ForceStopCinematics()
        {
            _isPlaying = false;
            _playingCinematicIndex = -1;

            CompleteObservers();

            ToggleInputHandlers(false);

            onCinematicsEnded?.Invoke(_playingSequence);

            _playingSequence = null;
        }
        public void PlayNextCinematicProfile()
        {
            if (!_isPlaying) return;

            CompleteObservers();

            _playingCinematicIndex++;

            if (_playingCinematicIndex >= _playingSequence.Sequence.Count)
            {
                ForceStopCinematics();
            }
            else
            {
                foreach (IObserver<BaseCinematicProfile> observer in _cinematicObservers)
                {
                    observer.OnNext(_playingSequence.Sequence[_playingCinematicIndex]);
                }
            }
        }


        private void ToggleInputHandlers(bool status)
        {
            _cinematicsInputHandler.Toggle(status);
        }


        private void CompleteObservers()
        {
            foreach (IObserver<BaseCinematicProfile> observer in _cinematicObservers)
            {
                observer.OnCompleted();
            }
        }
        public T GetObserver<T>()
        {
            IObserver<BaseCinematicProfile> observer = _cinematicObservers.Find(x => x.GetType().Equals(typeof(T)));

            return (T)observer;
        }
        public IDisposable Subscribe(IObserver<BaseCinematicProfile> observer)
        {
            return new CinematicSubscriber();
        }
    }

    public class CinematicSubscriber : IDisposable
    {
        public virtual void Dispose() { }
    }
}