using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSlotButton : MonoBehaviour
{
    public MagicManager.MagicType currentType;

    public void Press()
    {
        FindObjectOfType<MagicManager>().UseMagic(currentType);
    }
}
