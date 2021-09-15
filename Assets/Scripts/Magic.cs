using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magic", menuName = "Creatable/Create Magic")]
public class Magic : ScriptableObject
{
    [Header("Картинка магии")]
    public Texture MagicIcon;
    [Header("Название магического навыка")]
    public string Name;
    [Header("Тип магии")]
    public MagicManager.MagicType magicType;
    [Header("Единица магии")]
    public double actionVar;
    public double actionVarDefault;
    [Header("Время перезарядки (ходов)")]
    public int cooldownTimeMax;
    public int cooldownTimeMaxDefault;
    [HideInInspector()]
    [Header("Текущее время перезарядки")]
    public int currentCooldown;
    [Header("Можно ли использовать магию")]
    public bool canUseMagic;
}
