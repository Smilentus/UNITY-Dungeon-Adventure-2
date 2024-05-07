using Dimasyechka.Code.BattleSystem.BattleActions.Interfaces;
using Dimasyechka.Code.BattleSystem.Controllers;
using Dimasyechka.Code.MagicSystem.Profiles;
using System;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.BattleSystem.BattleActions.Implementations
{
    public class BattleMagicExecuterBaseActionExecuter : MonoBehaviour, IBattleActionExecuter
    {
        public event Action<int> OnCooldownPassed;


        protected IBattleActionInteraction _battleActionInteraction;
        public IBattleActionInteraction BattleActionInteraction { get => _battleActionInteraction; set => _battleActionInteraction = value; }


        protected BaseMagicProfile _magicProfile;


        protected int _cooldownValue;
        public int CooldownValue => _cooldownValue;


        public float CooldownRatio => _magicProfile != null
            ? (float)_cooldownValue / (float)_magicProfile.DefaultCooldownHours
            : 0f;


        protected RuntimePlayer _runtimePlayer;
        protected BattleController _battleController;

        [Inject]
        public void Construct(RuntimePlayer runtimePlayer, BattleController battleController)
        {
            _runtimePlayer = runtimePlayer;
            _battleController = battleController;
        }


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
            if (_battleActionInteraction == null)
            {
                _battleActionInteraction = _interaction;
            }

            _magicProfile = _interaction as BaseMagicProfile;
        }

        public virtual void EveryTurnCheck(BattleController.TurnStatus turnStatus)
        {
            if (turnStatus == BattleController.TurnStatus.PlayerTurn)
            {
                SetSkillCooldown(_cooldownValue - 1);
            }
        }

        public virtual void ExecuteAction()
        {
            SpendPlayerMana();

            SetSkillCooldown(_magicProfile.DefaultCooldownHours);
        }


        public virtual void SetSkillCooldown(int _cooldown)
        {
            _cooldownValue = _cooldown;

            OnCooldownPassed?.Invoke(_cooldownValue);
        }


        public virtual bool IsCooldownPassed()
        {
            if (_cooldownValue <= 0)
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
            if (_runtimePlayer.RuntimePlayerStats.Mana.Value >= _magicProfile.DefaultManaPointsCost)
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
            _runtimePlayer.RuntimePlayerStats.Mana.Value -= _magicProfile.DefaultManaPointsCost;
        }
    }
}