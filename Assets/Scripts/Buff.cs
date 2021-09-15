using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ещё одна пасхалка разрабов, хе-хе. Интересно, кто-то когда-нибудь откроет этот код ещё раз? 
// Ну просто интересно ^_^

// Оставлю здесь дату этого комментария, просто так 15.02.2018 - 21:56

[CreateAssetMenu(fileName = "New Buff", menuName = "Creatable/Create Buff")]
public class Buff : ScriptableObject
{
    // Добавить: РАНДОМНЫЙ, ПРЕВРАЩЕНИЕ
    public enum BuffType
    {
        Poison, // <-- Винцеслав, если увидишь это, просто вспомни <:3 
        Bleeding, // <-- Health Decreasing
        ArmorDecreasing, 
        HealthBonus, // <-- Health Increasing
        ArmorBonus, // <-- Armor Increasing
        ManaBonus,
        DamageBonus,
        RegenBonus,
        ManaRegenBonus,
        HealthAndManaRegen,
        InfinityShield,
        InfinityManaRegen,
        InfinityHealthRegen,
        Vampirism,

        // Баффы превращения
        FrogBuff,
        ChickenBuff,
        SheepBuff,
        RabbitBuff,

        // Астрономические баффы
        PlanetRowBuff,
        SunstayBuff,
        FullmoonBuff,
        
        Fire,
        WarriorAbility,
        MageAbility,
        
        Happiness,
        
        // Магия
        Magic_Shield,
        Magic_Poison,
        Magic_HealthRegen,
        Magic_Stun,

        Illuminati,
    }

    [Header("Тип баффа")]
    public BuffType Type;
    [Header("Название баффа")]
    public string Name;
    [TextArea(3, 5)]
    [Header("Описание баффа")]
    public string Descr;
    [Header("Иконка")]
    public Texture Icon;
    [Header("Цвет иконки (Для корректности)")] // ... И из-за лени
    public Color iconColor = Color.white;
    [Header("Единица взаимодействия")]
    public int buffPower;
    [Header("Длительность")]
    public int Duration;
    public int MaxDuration;
    [Header("Бесконечный бафф?")]
    public bool isInfinity;
}
