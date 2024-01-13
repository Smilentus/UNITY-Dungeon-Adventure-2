using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RuntimePlayerStatsView : MonoBehaviour
{
    [Header("Current Step")]
    [SerializeField]
    private TMP_Text m_currentDateTimeTMP;


    [Header("Player Stats")]
    [SerializeField]
    private TMP_Text m_playerHealthTMP;

    [SerializeField]
    private Slider m_playerHealthSlider;


    [SerializeField]
    private TMP_Text m_playerLevelTMP;

    [SerializeField]
    private Slider m_playerLevelSlider;


    [SerializeField]
    private TMP_Text m_playerHealthRegenTMP;

    [SerializeField]
    private TMP_Text m_playerManaTMP;

    [SerializeField]
    private Slider m_playerManaSlider;

    [SerializeField]
    private TMP_Text m_playerManaRegenTMP;


    [SerializeField]
    private TMP_Text m_playerDamageTMP;

    [SerializeField]
    private TMP_Text m_playerAttackSpeedTMP;

    [SerializeField]
    private TMP_Text m_playerArmorTMP;


    [SerializeField]
    private TMP_Text m_playerElementTMP;


    [SerializeField]
    private TMP_Text m_playerGoldTMP;

    [SerializeField]
    private TMP_Text m_playerExtraExpTMP;

    [SerializeField]
    private TMP_Text m_playerExtraMoneyTMP;


    [SerializeField]
    private TMP_Text m_currentLocationTMP;


    [SerializeField]
    private TMP_Text m_dodgeChanceTMP;

    [SerializeField]
    private TMP_Text m_playerLuckTMP;


    [SerializeField]
    private TMP_Text m_playerCriticalChanceTMP;

    [SerializeField]
    private TMP_Text m_playerCriticalDamageTMP;


    [SerializeField]
    private TMP_Text m_playerSkillPointsTMP;

    
    private void Update()
    {
        LegacyCheckers();

        UpdateTexts();
        UpdateSliders();
    }


    private void LegacyCheckers()
    {
        if (RuntimePlayer.Instance.RuntimePlayerStats.Health > RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth)
            RuntimePlayer.Instance.RuntimePlayerStats.Health = RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth;

        if (RuntimePlayer.Instance.RuntimePlayerStats.Mana > RuntimePlayer.Instance.RuntimePlayerStats.MaxMana)
            RuntimePlayer.Instance.RuntimePlayerStats.Mana = RuntimePlayer.Instance.RuntimePlayerStats.MaxMana;
    }


    private void UpdateTexts()
    {
        m_playerSkillPointsTMP.text = "ОН: " + RuntimePlayer.Instance.RuntimePlayerStats.SkillPoints;

        m_currentDateTimeTMP.text = $"{GameTimeFlowController.Instance.DateNow()}\n{GameTimeFlowController.Instance.DayStatusNow()}";

        m_playerHealthTMP.text = $"{RuntimePlayer.Instance.RuntimePlayerStats.Health.ToString("f2")}/{RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth.ToString("f2")} ОЗ";

        m_playerLevelTMP.text = $"Урв. {RuntimePlayer.Instance.RuntimePlayerStats.Lvl}";
        m_playerHealthRegenTMP.text = $"Реген. {RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen.ToString("f2")} ОЗ";


        m_playerManaTMP.text = $"{RuntimePlayer.Instance.RuntimePlayerStats.Mana.ToString("f2")}/{RuntimePlayer.Instance.RuntimePlayerStats.MaxMana.ToString("f2")} ОМ";
        m_playerManaRegenTMP.text = $"Реген. {RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen.ToString("f2")} ОМ";
            
        m_playerDamageTMP.text = $"Урон {RuntimePlayer.Instance.RuntimePlayerStats.Damage.ToString("f2")} ед.";
        m_playerAttackSpeedTMP.text = $"Скорость {RuntimePlayer.Instance.RuntimePlayerStats.AttackSpeed.ToString("f2")} ед.";
        m_playerArmorTMP.text = $"Защита {RuntimePlayer.Instance.RuntimePlayerStats.Armor.ToString("f2")} ед.";

        m_playerElementTMP.text = $"{RuntimePlayer.Instance.RuntimePlayerStats.elementStr}";

        m_playerGoldTMP.text = $"Золото {RuntimePlayer.Instance.RuntimePlayerStats.Money.ToString("f0")} ед.";
        m_playerExtraExpTMP.text = $"Доп. опыт {(RuntimePlayer.Instance.RuntimePlayerStats.ExtraExpMultiplier * 100).ToString("f2")}%";
        m_playerExtraMoneyTMP.text = $"Доп. золото {(RuntimePlayer.Instance.RuntimePlayerStats.ExtraMoneyMultiplier * 100).ToString("f2")}%";

        m_currentLocationTMP.text = $"Локация {(LocationsController.Instance.CurrentLocation == null ? "Неизвестно" : LocationsController.Instance.CurrentLocation.LocationTitle)}";

        m_dodgeChanceTMP.text = $"Уклонение {RuntimePlayer.Instance.RuntimePlayerStats.DodgeChance.ToString("f2")}%";
        m_playerLuckTMP.text = $"Удача {RuntimePlayer.Instance.RuntimePlayerStats.Luck.ToString("f2")}%";

        m_playerCriticalChanceTMP.text = $"Крит. шанс {RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeChance.ToString("f2")}%";
        m_playerCriticalDamageTMP.text = $"Крит. урон {(RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeDamageMultiplier * 100).ToString("f2")}%";
    }

    private void UpdateSliders()
    {
        m_playerHealthSlider.maxValue = (float)RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth;
        m_playerHealthSlider.minValue = 0;
        m_playerHealthSlider.value = (float)RuntimePlayer.Instance.RuntimePlayerStats.Health;

        m_playerLevelSlider.maxValue = (float)RuntimePlayer.Instance.RuntimePlayerStats.MaxExp;
        m_playerLevelSlider.minValue = 0;
        m_playerLevelSlider.value = (float)RuntimePlayer.Instance.RuntimePlayerStats.Exp;

        m_playerManaSlider.maxValue = (float)RuntimePlayer.Instance.RuntimePlayerStats.MaxMana;
        m_playerManaSlider.minValue = 0;
        m_playerManaSlider.value = (float)RuntimePlayer.Instance.RuntimePlayerStats.Mana;
    }
}
