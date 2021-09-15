using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Creatable/Create Skill")]
public class Skill : ScriptableObject
{
    [Header("Нужный уровень для покупки или улучшения")]
    public int neededLvl;
    [Header("Модификатор нужного уровня")]
    public int neededLvlMulty;
    [Header("Тип скилла")]
    public SkillsManager.SkillType Type;
    [Header("Название скилла")]
    public string Name;
    [Header("Описание скилла")]
    [TextArea(3, 5)]
    public string Descr;
    [Header("Цена")]
    public int Cost;
    public int DefaultCost;
    [Header("Модификатор цены")]
    public int CostMulty;
    [Header("Изменение цены каждый ...")]
    public int CostChangeLvl;
    [Header("Дефолтный показатель")]
    public double defaultVariable;
    [Header("Истинный показатель")]
    public double currentVariable;
    [Header("Изменение показателя с уровнем")]
    public double changableVariable;
    [Header("Текущий уровень")]
    public int Lvl;
    [Header("Бесконечный ли скилл")]
    public bool isInfinity;
    [Header("Макс. уровень")]
    public int MaxLvl;
    [Header("Иконка")]
    public Texture Icon;
}
