using UnityEngine;

namespace Dimasyechka.Code._LEGACY_.Buttons
{
    public class AbilityButton : MonoBehaviour
    {
        Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();

            //if (RuntimePlayer.HACharge >= RuntimePlayer.HAMaxCharge && RuntimePlayer.HACharge != 0)
            //    AnimateButton();
        }

        // Начать анимацию кнопки
        public void AnimateButton()
        {
            animator.SetBool("isCharged", true);
        }
        // Закончить анимацию кнопки
        public void StopAnimateButton()
        {
            animator.SetBool("isCharged", false);
        }

        private void Update()
        {
            //transform.GetChild(0).GetComponent<Text>().text = RuntimePlayer.HACharge + "/" + RuntimePlayer.HAMaxCharge;
        }
    }
}
