using System;
using System.Collections.Generic;
using UnityEngine;


public class BaseCinematicsObserver : MonoBehaviour, IObserver<BaseCinematicProfile>
{
    public void OnCompleted()
    {
        VoiceController.Instance.ForceStop();
        //SubtitlesController.Instance.ForceHide();
    }

    public void OnError(Exception error)
    {
        Debug.Log($"{error.Message}");
    }

    public void OnNext(BaseCinematicProfile profile)
    {
        VoiceController.Instance.PlayVoiceClips(profile.SubtitleDatas);
    }
}