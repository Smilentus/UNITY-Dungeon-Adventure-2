using System;
using System.Collections.Generic;
using UnityEngine;


public class Cinematics : MonoBehaviour
{
    private static Cinematics _instance;
    public static Cinematics Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Cinematics>();
            }

            return _instance;
        }
    }


    public event Action<CinematicSequence> onCinematicsStarted;
    public event Action<CinematicSequence> onCinematicsEnded;


    private CinematicSequence _playingSequence;


    private bool _isPlaying;
    public bool IsPlaying => _isPlaying;


    public void StartCinematics(CinematicSequence sequence)
    {
        if (_isPlaying) return;

        _isPlaying = true;
        _playingSequence = sequence;

        onCinematicsStarted?.Invoke(_playingSequence);
    }

    public void ForceStopCinematics()
    {
        _isPlaying = false;
     
        onCinematicsEnded?.Invoke(_playingSequence);

        _playingSequence = null;
    }
}
