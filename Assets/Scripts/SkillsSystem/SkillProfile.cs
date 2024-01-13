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
    ///     Главный префаб навыка, со всеми улучшениями и т.п.
    /// </summary>
    public SkillCore skillCorePrefab { get => m_skillCorePrefab; }


    [SerializeField] 
    private List<SkillLevelData> m_skillLevelDatas;
    /// <summary>
    ///     Список улучшений навыка
    /// </summary>
    public List<SkillLevelData> skillLevelDatas { get => m_skillLevelDatas; }

    
    public SkillLevelData GetLevelData(int level)
    {
        SkillLevelData returnableData = null;

        foreach (SkillLevelData skillLevelData in m_skillLevelDatas)
        {
            if (skillLevelData.PassSkillLevel <= level)
            {
                returnableData = skillLevelData;
            }
        }

        return returnableData;
    }
}

[System.Serializable]
public class SkillLevelData
{
    [SerializeField] 
    private int m_passSkillLevel;
    /// <summary>
    ///     Тот уровень, после которого основной навык заменит своё описание на этот навык
    /// </summary>
    public int PassSkillLevel => m_passSkillLevel;


    [SerializeField] 
    private string m_skillLevelTitle;
    /// <summary>
    ///     Наименование улучшения 
    /// </summary>
    public string skillLevelTitle { get => m_skillLevelTitle; }


    [TextArea(5, 10)]
    [SerializeField] 
    private string m_skillLevelDescription;
    /// <summary>
    ///     Подробное описание улучшения навыка
    /// </summary>
    public string skillLevelDescription { get => m_skillLevelDescription; }


    [SerializeField] 
    private Sprite m_skillLevelIcon;
    /// <summary>
    ///     Главная иконка улучшения (если её нет используется иконка навыка)
    /// </summary>
    public Sprite skillLevelIcon { get => m_skillLevelIcon; }
}