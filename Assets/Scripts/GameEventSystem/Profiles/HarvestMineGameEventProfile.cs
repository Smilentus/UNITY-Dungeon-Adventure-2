using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Creatable/GameEventSystem/New HarvestMineGameEventProfile", fileName = "HarvestMineGameEventProfile_")]
[System.Serializable]
public class HarvestMineGameEventProfile : BaseGameEventProfile
{
    [Tooltip("���������� ��������")]
    [SerializeField]
    private BaseItemProfile[] m_BaseHarvestables;
    public BaseItemProfile[] BaseHarvestables => m_BaseHarvestables;


    [Tooltip("���������� ��������� ���������� ������ �� ���")]
    [SerializeField]
    private int m_HarvestableAmountMin;
    public int HarvestableAmountMin => m_HarvestableAmountMin;

    [Tooltip("����������� ��������� ���������� ������ �� ���")]
    [SerializeField]
    private int m_HarvestableAmountMax;
    public int HarvestableAmountMax => m_HarvestableAmountMax;


    [Tooltip("���� ����� �� ������")]
    [SerializeField]
    private float m_HarvestableExp;
    public float HarvestableExt => m_HarvestableExp;


    [SerializeField]
    [Range(0, 100)]
    private int m_ChanceToHarvest;
    public int ChanceToHarvest => m_ChanceToHarvest;
}
