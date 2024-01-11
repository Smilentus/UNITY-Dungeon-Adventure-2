using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SkillProfile_", menuName = "Creatable/SkillsSystem/Create New Skill Profile")]
public class SkillProfile : ScriptableObject
{
    [SerializeField] 
    private string m_skillGUID;
    public string skillGUID => m_skillGUID;


    [SerializeField] 
    private SkillCore m_skillCorePrefab;
    /// <summary>
    ///     ������� ������ ������, �� ����� ����������� � �.�.
    /// </summary>
    public SkillCore skillCorePrefab { get => m_skillCorePrefab; }


    [SerializeField] 
    private string m_skillName;
    /// <summary>
    ///     ������������ ������
    /// </summary>
    public string skillName { get => m_skillName; }


    [TextArea(5, 10)]
    [SerializeField] 
    private string m_skillDescription;
    /// <summary>
    ///     �������������� ��������� �������� ������
    /// </summary>
    public string skillDescription { get => m_skillDescription; }


    [SerializeField] 
    private Sprite m_skillIcon;
    /// <summary>
    ///     ������� ������ ������
    /// </summary>
    public Sprite skillIcon { get => m_skillIcon; }


    [SerializeField] 
    private List<SkillLevelData> m_skillLevelDatas;
    /// <summary>
    ///     ������ ��������� ������
    /// </summary>
    public List<SkillLevelData> skillLevelDatas { get => m_skillLevelDatas; }
}

[System.Serializable]
public class SkillLevelData
{
    [SerializeField] 
    private int m_passSkillLevel;
    /// <summary>
    ///     ��� �������, ����� �������� �������� ����� ������� ��� �������� �� ���� �����
    /// </summary>
    public int PassSkillLevel => m_passSkillLevel;


    [SerializeField] 
    private string m_skillLevelTitle;
    /// <summary>
    ///     ������������ ��������� 
    /// </summary>
    public string skillLevelTitle { get => m_skillLevelTitle; }


    [TextArea(5, 10)]
    [SerializeField] 
    private string m_skillLevelDescription;
    /// <summary>
    ///     ��������� �������� ��������� ������
    /// </summary>
    public string skillLevelDescription { get => m_skillLevelDescription; }


    [SerializeField] 
    private Sprite m_skillLevelIcon;
    /// <summary>
    ///     ������� ������ ��������� (���� � ��� ������������ ������ ������)
    /// </summary>
    public Sprite skillLevelIcon { get => m_skillLevelIcon; }
}