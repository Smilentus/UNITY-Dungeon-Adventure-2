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


        [Tooltip("True -> ����������� ���������� ������� ��������")]
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
            // ����� ��������� +1 � ������, ������ ��� �� ��������� ������� ������� ������
            // � ����� ��� ���������� ������������� ��� �������, ������� �� ��������� ��� ���������� ���������
            _checkLevel = level + 1;
        }

        public virtual string GetDescription() 
        {
            return $"[������� ��������� �� �������]";
        }

        protected virtual void ProcessUpgrade() { }

        public virtual bool CanUpgrade() { return true; }
    }
}