using UnityEngine;

[CreateAssetMenu(fileName = "New VillagePart", menuName = "Creatable/Create VillagePart")]
public class VillagePart : ScriptableObject
{
    [Header("Часть деревни")]
    public PlayerVillageActivity.villagePart vilPart;
    [HideInInspector()]
    public int currentLvl;
    [HideInInspector()]
    public int maxLvl;
    [Header("Уровень постройки")]
    public int defaultMaxLvl;

    [HideInInspector()]
    public double upgradeCost;
    [Header("Стоимость улучшения")]
    public double defaultUpgradeCost;
    [HideInInspector()]
    public double upgradeMulty;
    [Header("Модификатор стоимости")]
    public double defaultUpgradeMulty;

    [HideInInspector()]
    public int currentWorkers;
    [HideInInspector()]
    public int maxWorkers;
    [Header("Рабочие")]
    public int defaultMaxWorkers;
    [Header("Прирост рабочих за уровень")]
    public int defaultWorkersUpgrade;

    [HideInInspector()]
    public double currentIncome;
    [Header("Текущий доход")]
    public double defaultCurrentIncome;
    [Header("Прирост дохода за уровень")]
    public int defaultIncomeUpgrade;

    [HideInInspector()]
    public int currentPopularity;
    [Header("Текущая привлекательность")]
    public int defaultCurrentPopularity;
    [Header("Прирост популярности за уровень")]
    public int defaultPopularityUpgrade;

    [Header("Ресурсы для производства")]
    public VillageResource[] productResources;

    [Header("Ресурсы для улучшения")]
    public VillageUpgrade[] neededItemsToUpgrade;
}
