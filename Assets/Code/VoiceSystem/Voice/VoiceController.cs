using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dimasyechka.Code.VoiceSystem.Voice
{
    public class VoiceController : MonoBehaviour
    {
        private static VoiceController _instance;
        public static VoiceController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<VoiceController>();
                }

                return _instance;
            }
        }


        public event Action<BaseVoiceClipData, float> onVoiceClipDataPlayed;


        [SerializeField]
        private AudioSource m_playingAudioSource;

        [SerializeField]
        private float m_minClipLength = 0.05f;


        private Queue<BaseVoiceClipData> _playingQueue = new Queue<BaseVoiceClipData>();


        private bool _isDelayed;
        private float _delayTime = 1f;
        private float _delayTimeout = 0f;


        private bool _isPlayingAudio;


        private void FixedUpdate()
        {
            if (_isDelayed)
            {
                _delayTimeout += Time.fixedDeltaTime;

                if (_delayTimeout >= _delayTime)
                {
                    _isDelayed = false;
                    _delayTimeout = 0.0f;
                }
            }

            _isPlayingAudio = m_playingAudioSource.isPlaying;

            CheckQueueAndPlay();
        }

        private void CheckQueueAndPlay()
        {
            if (_isDelayed) return;
            if (_isPlayingAudio) return;
            if (_playingQueue.Count == 0) return;

            Debug.Log("CheckQueueAndPlay");

            PlaySingleVoiceClip(_playingQueue.Dequeue());
        }


        public void PlayVoiceClips(List<BaseVoiceClipData> voiceClips)
        {
            foreach (BaseVoiceClipData data in voiceClips)
            {
                _playingQueue.Enqueue(data);
            }
        }

        public void PlayVoiceClip(BaseVoiceClipData data)
        {
            _playingQueue.Enqueue(data);
        }

        private void PlaySingleVoiceClip(BaseVoiceClipData data) 
        {
            float clipLength = CalculateSubtitlesShownSeconds(data);

            clipLength = Mathf.Clamp(clipLength, m_minClipLength, float.MaxValue);

            _isDelayed = true;
            _delayTime = clipLength;
            _delayTimeout = 0;

            Debug.Log(_delayTime);

            SetAndPlayAudioSourceClip(data.VoiceClip);

            onVoiceClipDataPlayed?.Invoke(data, clipLength);
        }

        private float CalculateSubtitlesShownSeconds(BaseVoiceClipData data)
        {
            if (data != null)
            {
                if (data.VoiceClip != null)
                {
                    return data.VoiceClip.length + 0.25f;
                }
                else
                {
                    return (data.SubtitlesBody.Trim().Replace(" ", "").Length * 0.05f) + 0.25f;
                }
            }

            return 1f;
        }


        public void ForceStop()
        {
            ClearPlayingQueue();
            ResetAudioSource();
        }


        public void ClearPlayingQueue()
        {
            _playingQueue.Clear();
        }


        public void ResetAudioSource()
        {
            m_playingAudioSource.Stop();
        }

        public void SetAndPlayAudioSourceClip(AudioClip clip)
        {
            ResetAudioSource();

            m_playingAudioSource.clip = clip;
            m_playingAudioSource.Play();
        }
    }
}