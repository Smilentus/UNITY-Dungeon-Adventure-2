using UnityEditor;
using UnityEngine;

// Ещё одна пасхалка разрабов, хе-хе. Интересно, кто-то когда-нибудь откроет этот код ещё раз? 
// Ну просто интересно ^_^
// P.S> К сожалению - открыли((( (к счастью)

// Оставлю здесь дату этого комментария, просто так 15.02.2018 - 21:56
// Ещё один комментарий меня из будущего: 12.11.2023 - 8:15 (сижу с 2:30 ебашу глобальный рефакторинг)

[CreateAssetMenu(fileName = "BuffProfile", menuName = "Creatable/Create BuffProfile")]
public class BuffProfile : ScriptableObject
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

    [Header("Единица взаимодействия")]
    public int buffPower;

    [Header("Длительность")]
    public int Duration;
    public int MaxDuration;

    [Header("Бесконечный бафф?")]
    public bool isInfinity;


    [SerializeField]
    protected RuntimeBuff m_runtimeBuffPrefab;
    public RuntimeBuff RuntimeBuffPrefab => m_runtimeBuffPrefab;


    [SerializeField]
    protected string m_BuffUID;
    public string BuffUID => m_BuffUID;


    [TextArea(1, 3)]
    [SerializeField]
    protected string m_BuffName;
    public string BuffName => m_BuffName;

    [TextArea(3, 10)]
    [SerializeField]
    protected string m_Description;
    public string BuffDescription => m_Description;

    [SerializeField]
    protected Sprite m_Icon;
    public Sprite BuffIcon => m_Icon;

    [SerializeField]
    protected int m_durationHours;
    public int BuffDurationHours => m_durationHours;
}