using System;
using UnityEngine;


public abstract class RuntimeMagicObject : MonoBehaviour
{
    public event Action<int> OnCooldownPassed;


    private MagicProfile magicProfile;
    public MagicProfile MagicProfile => magicProfile;


    private int cooldownValue;
    public int CooldownValue => cooldownValue;

    public float CooldownRatio => magicProfile != null 
        ? cooldownValue / magicProfile.DefaultCooldownHours
        : 0;

    
    public virtual void SetupMagicObject(MagicProfile _profile)
    {
        magicProfile = _profile;

        SetSkillCooldown(0);

        OnSetup();
    }


    public virtual void SetSkillCooldown(int _cooldown)
    {
        cooldownValue = _cooldown;

        OnCooldownPassed?.Invoke(cooldownValue);
    }


    public virtual void UpdateCooldown()
    {
        SetSkillCooldown(cooldownValue - 1);
    }


    public virtual bool IsCooldownPassed()
    {
        if (cooldownValue <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual bool IsManaCostAvailable()
    {
        if (RuntimePlayer.Instance.RuntimePlayerStats.Mana >= magicProfile.DefaultManaPointsCost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void SpendPlayerMana()
    {
        RuntimePlayer.Instance.RuntimePlayerStats.Mana -= magicProfile.DefaultManaPointsCost;
    }
    

    public virtual bool TryUseMagic()
    {
        if (IsCooldownPassed() && IsManaCostAvailable())
        {
            SpendPlayerMana();

            SetSkillCooldown(magicProfile.DefaultCooldownHours);

            OnUseMagic();

            return true;
        }
        else
        {
            return false;
        }
    }


    public abstract void OnSetup();
    public abstract void OnUseMagic();
}