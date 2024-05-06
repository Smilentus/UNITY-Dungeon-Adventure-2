using Dimasyechka.Code.CoreComponentSystem.Core;
using Dimasyechka.Code.UpgradeableSystem.Interfaces;
using UnityEngine;

namespace Dimasyechka.Code.UpgradeableSystem.Base
{
    public class BaseUpgradeableCheckerComponent : CoreComponent, IUpgradeableChecker
    {
        [SerializeField]
        protected int _checkLevel = 0;
        public int CheckLevel => _checkLevel;


        [Tooltip("True -> бесконечное увеличение уровней проверки")]
        [SerializeField]
        protected bool _autoRaiseCheckLevel = false;


        public virtual void PostUpgrade()
        {
            ProcessUpgrade();

            if (_autoRaiseCheckLevel)
            {
                _checkLevel++;
            }
        }

        public virtual void LoadUpgradeableLevel(int level)
        {
            // ¬ажно добавл€ть +1 к уровню, потому что мы загружаем текущий уровень навыка
            // ј здесь нам необходимо устанавливать тот уровень, который мы провер€ем дл€ следующего улучшени€
            _checkLevel = level + 1;
        }

        public virtual string GetDescription() 
        {
            return $"[”словие улучшени€ не описано]";
        }

        protected virtual void ProcessUpgrade() { }

        public virtual bool CanUpgrade() { return true; }
    }
}