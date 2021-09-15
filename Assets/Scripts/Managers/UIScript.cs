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
        if (Player.Health > Player.MaxHealth)
            Player.Health = Player.MaxHealth;
        if (Player.Mana > Player.MaxMana)
            Player.Mana = Player.MaxMana;

        // Слайдер опыта
        LvlSlider.maxValue = (float)Player.MaxExp;
        LvlSlider.value = (float)Player.Exp;
        LvlSlider.GetComponentInChildren<Text>().text = "Ур." + Player.Lvl;
           //" | До " + (Player.Lvl + 1) + " ур. " + (Player.MaxExp - Player.Exp) + " ОО";

        // Слайдер здоровья
        HealthSlider.maxValue = (float)Player.MaxHealth;
        if(Player.Health < 0)
            HealthSlider.value = 0;
        else
            HealthSlider.value = (float)Player.Health;
        HealthSlider.GetComponentInChildren<Text>().text = Player.Health + "/" + Player.MaxHealth + " ОЗ";

        // Слайдер маны
        ManaSlider.maxValue = (float)Player.MaxMana;
        if (Player.Mana < 0)
            ManaSlider.value = 0;
        else
            ManaSlider.value = (float)Player.Mana;
        ManaSlider.GetComponentInChildren<Text>().text = Player.Mana + "/" + Player.MaxMana + " ОМ";

        // Тексты
        currentStepText.text = GameTimeScript.DateNow() + "\n" + GameTimeScript.DayStatusNow();
        AttackText.text = "Урон: " + Player.Damage.ToString();
        ArmorText.text = "Защита: " + Player.Armor.ToString();
        SpeedText.text = "Скорость атаки: " + Player.AttackSpeed.ToString();
        HealthRegen.text = "Реген. ОЗ: " + Player.HealthRegen.ToString();
        ManaRegen.text = "Реген. ОМ: " + Player.ManaRegen.ToString();
        elementText.text = Player.elementStr;
        GoldText.text = "Золото: " + Player.Money.ToString();
        LuckText.text = "Удача: " + Player.Luck.ToString("f2") + "%";
        CriticalDamageText.text = "Крит. урон: " + (Player.CriticalStrikeMulty * 100 - 100) + "%";
        CriticalChanceText.text = "Крит. шанс: " + Player.CriticalStrikeChance + "%";
        DodgeChanceText.text = "Шанс уворота: " + Player.DodgeChance + "%";
        ExtraExpModText.text = "Доп. опыт: " + Player.ExtraExpMod + "%";
        ExtraMoneyModText.text = "Доп. золото: " + Player.ExtraMoneyMod + "%";
        currentLocation.text = LocationManager.LocationToText(LocationManager.CurrentLocation);
    }

    // Обновление текстов противника
    public void UpdateEnemyUIText()
    {
        for (int i = 0; i < BattleHelper._BH.allEnemies.Count; i++)
        {
            // Проверка допустимых значений
            if (BattleHelper._BH.allEnemies[i].Health > BattleHelper._BH.allEnemies[i].MaxHealth)
                BattleHelper._BH.allEnemies[i].Health = BattleHelper._BH.allEnemies[i].MaxHealth;
            if (BattleHelper._BH.allEnemies[i].Mana > BattleHelper._BH.allEnemies[i].MaxMana)
                BattleHelper._BH.allEnemies[i].Mana = BattleHelper._BH.allEnemies[i].MaxMana;

            // Установка новых значений
            BattleHelper._BH.allEnemiesObjects[i].transform.GetChild(1).GetComponent<Text>().text = BattleHelper._BH.allEnemies[i].Name.ToString();
            BattleHelper._BH.allEnemiesObjects[i].transform.GetChild(2).GetChild(2).GetComponent<Text>().text = BattleHelper._BH.allEnemies[i].Health.ToString() + "/" + BattleHelper._BH.allEnemies[i].MaxHealth.ToString();
            BattleHelper._BH.allEnemiesObjects[i].transform.GetChild(2).GetComponent<Slider>().value = (float)BattleHelper._BH.allEnemies[i].Health;
            BattleHelper._BH.allEnemiesObjects[i].transform.GetChild(2).GetComponent<Slider>().maxValue = (float)BattleHelper._BH.allEnemies[i].MaxHealth;
            BattleHelper._BH.allEnemiesObjects[i].transform.GetChild(3).GetComponent<Text>().text = BattleHelper._BH.allEnemies[i].Attack.ToString();
            BattleHelper._BH.allEnemiesObjects[i].transform.GetChild(4).GetComponent<Text>().text = BattleHelper._BH.allEnemies[i].AttackSpeed.ToString();
            BattleHelper._BH.allEnemiesObjects[i].transform.GetChild(5).GetComponent<Text>().text = BattleHelper._BH.allEnemies[i].Armor.ToString();
            BattleHelper._BH.allEnemiesObjects[i].transform.GetChild(6).GetComponent<Text>().text = BattleHelper._BH.allEnemies[i].elementStr;

            // Помощник
            if (BattleHelper._BH.allEnemies[i].Health < 0)
                BattleHelper._BH.allEnemiesObjects[i].transform.GetChild(2).GetComponent<Slider>().value = 0;
            else
                BattleHelper._BH.allEnemiesObjects[i].transform.GetChild(2).GetComponent<Slider>().value = (float)BattleHelper._BH.allEnemies[i].Health;
        }
    }
}