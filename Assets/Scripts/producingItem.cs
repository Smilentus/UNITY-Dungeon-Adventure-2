using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class producingItem : MonoBehaviour
{
    [Header("Предмет")]
    public Item item;
    [Header("Производственное количество")]
    public int producingStack;
}
