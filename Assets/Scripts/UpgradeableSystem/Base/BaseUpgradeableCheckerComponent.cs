using UnityEngine;


public class BaseUpgradeableCheckerComponent : CoreComponent, IUpgradeableChecker
{
    [SerializeField]
    protected int m_checkLevel = 0;
    public int CheckLevel => m_checkLevel;


    [Tooltip("True -> ����������� ���������� ������� ��������")]
    [SerializeField]
    protected bool m_autoRaiseCheckLevel = false;


    public virtual void PostUpgrade()
    {
        ProcessUpgrade();

        if (m_autoRaiseCheckLevel)
        {
            m_checkLevel++;
        }
    }

    public virtual void LoadUpgradeableLevel(int level)
    {
        // ����� ��������� +1 � ������, ������ ��� �� ��������� ������� ������� ������
        // � ����� ��� ���������� ������������� ��� �������, ������� �� ��������� ��� ���������� ���������
        m_checkLevel = level + 1;
    }

    public virtual string GetDescription() 
    {
        return $"[������� ��������� �� �������]";
    }

    protected virtual void ProcessUpgrade() { }

    public virtual bool CanUpgrade() { return true; }
}