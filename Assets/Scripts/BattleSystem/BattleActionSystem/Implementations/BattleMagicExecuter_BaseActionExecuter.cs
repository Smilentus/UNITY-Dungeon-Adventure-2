using System;
using UnityEngine;


public class BattleMagicExecuter_BaseActionExecuter : MonoBehaviour, IBattleActionExecuter
{
    public event Action<int> OnCooldownPassed;


    protected IBattleActionInteraction battleActionInteraction;
    public IBattleActionInteraction BattleActionInteraction { get => battleActionInteraction; set => battleActionInteraction = value; }


    protected MagicProfile magicProfile;


    protected int cooldownValue;
    public int CooldownValue => cooldownValue;


    public float CooldownRatio => magicProfile != null 
        ? (float)cooldownValue / (float)magicProfile.DefaultCooldownHours
        : 0f;


    public virtual bool CanExecuteAction()
    {
        return IsCooldownPassed() && IsManaCostAvailable();
    }

    public virtual void Initialize()
    {
        SetSkillCooldown(0);
    }

    public virtual void SetInteraction(IBattleActionInteraction _interaction)
    {
        if (battleActionInteraction == null)
        {
            battleActionInteraction = _interaction;
        }

        magicProfile = _interaction as MagicProfile;
    }

    public virtual void EveryTurnCheck(BattleController.TurnStatus turnStatus)
    {
        if (turnStatus == BattleController.TurnStatus.PlayerTurn)
        {
            SetSkillCooldown(cooldownValue - 1);
        }
    }

    public virtual void ExecuteAction()
    {
        SpendPlayerMana();

        SetSkillCooldown(magicProfile.DefaultCooldownHours);
    }


    public virtual void SetSkillCooldown(int _cooldown)
    {
        cooldownValue = _cooldown;

        OnCooldownPassed?.Invoke(cooldownValue);
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
}