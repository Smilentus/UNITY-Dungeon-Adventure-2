using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    #region Для обращений к классу
    private static UIScript UI;
    public static UIScript Instance
    {
        get { return UI; }
    }
    #endregion

    [Header("Текущий ход")]
    public Text currentStepText;
    [Header("Слайдер уровня")]
    public Slider LvlSlider;
    [Header("Текст и слайдер здоровья")]
    public Slider HealthSlider;
    [Header("Текст и слайдер маны")]
    public Slider ManaSlider;
    [Header("Текст атаки")]
    public Text AttackText;
    [Header("Текст защиты")]
    public Text ArmorText;
    [Header("Текст скорости атаки")]
    public Text SpeedText;
    [Header("Текст регенерации")]
    public Text HealthRegen;
    [Header("Текст мана. регена")]
    public Text ManaRegen;
    [Header("Текст элемента")]
    public Text elementText;
    [Header("Текст текущей локации")]
    public Text currentLocation;

    [Header("Текст золота")]
    public Text GoldText;
    [Header("Текст удачи")]
    public Text LuckText;
    [Header("Текст крит. урона")]
    public Text CriticalDamageText;
    [Header("Текст крит. шанса")]
    public Text CriticalChanceText;
    [Header("Текст шанса уворота")]
    public Text DodgeChanceText;
    [Header("Текст множителя опыта")]
    public Text ExtraExpModText;
    [Header("Текст множителя монет")]
    public Text ExtraMoneyModText;

    private void Start()
    {
        UI = this;
    }

    private void Update()
    {
        UpdateUIText();
    }

    // Обновление текстов игрока
    public void UpdateUIText()
    {
        if (RuntimePlayer.Instance.RuntimePlayerStats.Health > RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth)
            RuntimePlayer.Instance.RuntimePlayerStats.Health = RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth;
        if (RuntimePlayer.Instance.RuntimePlayerStats.Mana > RuntimePlayer.Instance.RuntimePlayerStats.MaxMana)
            RuntimePlayer.Instance.RuntimePlayerStats.Mana = RuntimePlayer.Instance.RuntimePlayerStats.MaxMana;

        // Слайдер опыта
        LvlSlider.maxValue = (float)RuntimePlayer.Instance.RuntimePlayerStats.MaxExp;
        LvlSlider.value = (float)RuntimePlayer.Instance.RuntimePlayerStats.Exp;
        LvlSlider.GetComponentInChildren<Text>().text = "Ур." + RuntimePlayer.Instance.RuntimePlayerStats.Lvl;
           //" | До " + (Player.Lvl + 1) + " ур. " + (Player.MaxExp - Player.Exp) + " ОО";

        // Слайдер здоровья
        HealthSlider.maxValue = (float)RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth;
        if(RuntimePlayer.Instance.RuntimePlayerStats.Health < 0)
            HealthSlider.value = 0;
        else
            HealthSlider.value = (float)RuntimePlayer.Instance.RuntimePlayerStats.Health;
        HealthSlider.GetComponentInChildren<Text>().text = RuntimePlayer.Instance.RuntimePlayerStats.Health + "/" + RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth + " ОЗ";

        // Слайдер маны
        ManaSlider.maxValue = (float)RuntimePlayer.Instance.RuntimePlayerStats.MaxMana;
        if (RuntimePlayer.Instance.RuntimePlayerStats.Mana < 0)
            ManaSlider.value = 0;
        else
            ManaSlider.value = (float)RuntimePlayer.Instance.RuntimePlayerStats.Mana;
        ManaSlider.GetComponentInChildren<Text>().text = RuntimePlayer.Instance.RuntimePlayerStats.Mana + "/" + RuntimePlayer.Instance.RuntimePlayerStats.MaxMana + " ОМ";

        // Тексты
        currentStepText.text = GameTimeFlowController.Instance.DateNow() + "\n" + GameTimeFlowController.Instance.DayStatusNow();
        AttackText.text = "Урон: " + RuntimePlayer.Instance.RuntimePlayerStats.Damage.ToString();
        ArmorText.text = "Защита: " + RuntimePlayer.Instance.RuntimePlayerStats.Armor.ToString();
        SpeedText.text = "Скорость атаки: " + RuntimePlayer.Instance.RuntimePlayerStats.AttackSpeed.ToString();
        HealthRegen.text = "Реген. ОЗ: " + RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen.ToString();
        ManaRegen.text = "Реген. ОМ: " + RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen.ToString();
        elementText.text = RuntimePlayer.Instance.RuntimePlayerStats.elementStr;
        GoldText.text = "Золото: " + RuntimePlayer.Instance.RuntimePlayerStats.Money.ToString();
        LuckText.text = "Удача: " + RuntimePlayer.Instance.RuntimePlayerStats.Luck.ToString("f2") + "%";
        CriticalDamageText.text = "Крит. урон: " + (RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeMulty * 100 - 100) + "%";
        CriticalChanceText.text = "Крит. шанс: " + RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeChance + "%";
        DodgeChanceText.text = "Шанс уворота: " + RuntimePlayer.Instance.RuntimePlayerStats.DodgeChance + "%";
        ExtraExpModText.text = "Доп. опыт: " + RuntimePlayer.Instance.RuntimePlayerStats.ExtraExpMod + "%";
        ExtraMoneyModText.text = "Доп. золото: " + RuntimePlayer.Instance.RuntimePlayerStats.ExtraMoneyMod + "%";
        currentLocation.text = LocationsController.Instance.CurrentLocation ? LocationsController.Instance.CurrentLocation.LocationTitle : "[Location]";
    }
}