using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (Player.HACharge >= Player.HAMaxCharge && Player.HACharge != 0)
            AnimateButton();
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
        transform.GetChild(0).GetComponent<Text>().text = Player.HACharge + "/" + Player.HAMaxCharge;
    }
}
