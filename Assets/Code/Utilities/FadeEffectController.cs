using System;
using DG.Tweening;
using UnityEngine;

namespace Dimasyechka.Code.Utilities
{
    public class FadeEffectController : MonoBehaviour
    {
        public event Action onFadeIn = null;
        public event Action onFadeOut = null;
    

        [field: SerializeField] 
        public CanvasGroup FaderCanvasGroup { get; protected set; }


        [field: SerializeField]
        public bool ChangeInteractableStates { get; protected set; }


        [field: SerializeField] 
        public float FadeTimeInSeconds { get; protected set; }


        private Sequence faderSequence;


        public void ForceFadeIn()
        {
            FaderCanvasGroup.alpha = 1;

            if (ChangeInteractableStates)
            {
                FaderCanvasGroup.interactable = true;
                FaderCanvasGroup.blocksRaycasts = true;
            }
        }

        public void ForceFadeOut()
        {
            FaderCanvasGroup.alpha = 0;

            if (ChangeInteractableStates)
            {
                FaderCanvasGroup.interactable = false;
                FaderCanvasGroup.blocksRaycasts = false;
            }
        }


        public void FadeIn()
        {
            if (faderSequence != null)
                faderSequence.Kill();

            FaderCanvasGroup.alpha = 0;

            if (ChangeInteractableStates)
            {
                FaderCanvasGroup.interactable = true;
                FaderCanvasGroup.blocksRaycasts = true;
            }

            faderSequence = DOTween.Sequence();

            faderSequence.Append(FaderCanvasGroup.DOFade(1, FadeTimeInSeconds));
            faderSequence.OnComplete(() => {
                onFadeIn?.Invoke();
            });

            faderSequence.Play();
        }
     

        public void FadeOut()
        {
            if (faderSequence != null)
                faderSequence.Kill();

            FaderCanvasGroup.alpha = 1;

            if (ChangeInteractableStates)
            {
                FaderCanvasGroup.interactable = true;
                FaderCanvasGroup.blocksRaycasts = true;
            }

            faderSequence = DOTween.Sequence();

            faderSequence.Append(FaderCanvasGroup.DOFade(0, FadeTimeInSeconds));
            faderSequence.OnComplete(() => {
                if (ChangeInteractableStates)
                {
                    FaderCanvasGroup.interactable = false;
                    FaderCanvasGroup.blocksRaycasts = false;
                }

                onFadeOut?.Invoke();
            });

            faderSequence.Play();
        }
    }
}