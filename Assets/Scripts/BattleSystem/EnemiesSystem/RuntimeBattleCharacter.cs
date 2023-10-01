using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     ќсновной класс, который содержит информацию и прочее о игровом персонаже противника, с которым мы сражаемс€
/// </summary>
public class RuntimeBattleCharacter : MonoBehaviour
{
    public event Action onCharacterDeath;
    public event Action onCharacterActionsCompleted;


    [SerializeField]
    private RuntimeBattleCharacterView m_characterView;


    [HideInInspector()]
    public CharacterProfile characterProfile;


    // ќсновные переменные персонажа
    [HideInInspector()]
    public double MaxHealth, Health, MaxMana, Mana, HealthRegen, ManaRegen;

    [HideInInspector()]
    public double Armor, AttackSpeed, Damage, DodgeChance;


    public virtual void CreateBattleCharacter(CharacterProfile profile)
    {
        characterProfile = profile;

        MaxHealth = Health = profile.MaxHealth;
        MaxMana = Mana = profile.MaxMana;
        HealthRegen = profile.HealthRegen;
        ManaRegen = profile.ManaRegen;

        Armor = profile.Armor;
        AttackSpeed = profile.AttackSpeed;
        Damage = profile.Damage;
        DodgeChance = profile.DodgeChance;

        m_characterView.DrawCharacterInfo(new CharacterDrawerData()
        {
            runtimeBattleCharacter = this
        });

        UpdateCharacterView();
    }

    public virtual void ProcessCharacterActions()
    {
        onCharacterActionsCompleted?.Invoke();

        UpdateCharacterView();
    }

    public virtual void UpdateCharacterView()
    {
        m_characterView.DrawCharacterInfo(new CharacterDrawerData()
        {
            runtimeBattleCharacter = this
        });
    }
}
