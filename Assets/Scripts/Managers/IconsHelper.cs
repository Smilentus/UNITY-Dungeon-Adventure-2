using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconsHelper : MonoBehaviour
{
    public enum Icon
    {
        Exp, Health, Mana, Damage, AttackSpeed, Armor,
        Luck, Gold, Dodge, HealthRegen, ManaRegen, Element,
        CritChance, CritDamage, Data,
        // Инвентарь
        INV_Melee, INV_Ranged, INV_Magic, INV_Helmet, INV_Chestplate, INV_Boots, INV_Shield,
        INV_Ring, INV_Gloves, INV_Necklace, INV_Orb
    }

    [Header("Все описания иконок")]
    public Dictionary<Icon, string> iconsDescr = new Dictionary<Icon, string>();

    [Header("Панель описания")]
    public GameObject descrPanel;

    private void Start()
    {
        InitDescrs();
    }

    // Инициализируем описания
    private void InitDescrs()
    {
        iconsDescr.Add(Icon.Exp, "Опыт служит единицой прокачки персонажа. Зарабатывайте его в битве или выполняя задания.");
        iconsDescr.Add(Icon.Health, "Здоровье это основной показатель Вашей живучести. Следите за этой полоской чаще, всякое может произойти.");
        iconsDescr.Add(Icon.Mana, "Мана служит Вашей основной магической силой. Какой из Вас маг без маны? Используйте и контролируйте с умом.");
        iconsDescr.Add(Icon.Damage, "Урон - это Ваша сила и максимальное возможное повреждение, которое Вы можете нанести противнику физическим уроном.");
        iconsDescr.Add(Icon.AttackSpeed, "Скорость атаки показывает сколько раз за удар Вы сможете нанести урон.");
        iconsDescr.Add(Icon.Armor, "Защита спасает Вас в сложных ситуациях блокируя урон противника равный показателю брони.");
        iconsDescr.Add(Icon.Luck, "Удача помогает в необычных ситуациях, когда, казалось, всё потеряно. Подкручивает некоторые вероятности.");
        iconsDescr.Add(Icon.Gold, "Золото является валютой обмена среди жителей и торговцев.");
        iconsDescr.Add(Icon.Dodge, "Шанс увернуться от атаки противника.");
        iconsDescr.Add(Icon.HealthRegen, "Регенерация здоровья. Восстанавливает Х ОЗ за ход.");
        iconsDescr.Add(Icon.ManaRegen, "Восстановление маны. Восстанавливает Х ОМ за ход.");
        iconsDescr.Add(Icon.Element, "Ваш текущий преобладающий элемент стихии.");
        iconsDescr.Add(Icon.CritChance, "Шанс нанести критический удар.");
        iconsDescr.Add(Icon.CritDamage, "Множитель урона критического удара.");
        iconsDescr.Add(Icon.Data, "Текущая игровая дата. В определённый момент времени может произойти случайное событие. Будьте начеку!");
        iconsDescr.Add(Icon.INV_Ranged, "Оружие дальнего боя. ");
        iconsDescr.Add(Icon.INV_Melee, "Оружие ближнего боя. ");
        iconsDescr.Add(Icon.INV_Magic, "Оружие магического происхождения. ");
        iconsDescr.Add(Icon.INV_Boots, "Снаряжение: Поножи. ");
        iconsDescr.Add(Icon.INV_Chestplate, "Снаряжение: Нагрудник. ");
        iconsDescr.Add(Icon.INV_Gloves, "Снаряжение: Перчатки. ");
        iconsDescr.Add(Icon.INV_Helmet, "Снаряжение: Шлем. ");
        iconsDescr.Add(Icon.INV_Necklace, "Снаряжение: Ожерелье. ");
        iconsDescr.Add(Icon.INV_Orb, "Снаряжение: Сфера. ");
        iconsDescr.Add(Icon.INV_Ring, "Снаряжение: Кольцо. ");
        iconsDescr.Add(Icon.INV_Shield, "Снаряжение: Щит. ");
    }

    // Отображение описания иконки
    public void ShowIconDescr(Icon icon)
    {
        string iconDescr = "Описание: \n" + iconsDescr[icon];
        descrPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = iconDescr;
        descrPanel.SetActive(true);
    }
    // Закрытие панели описания
    public void HideIconDescr()
    {
        descrPanel.SetActive(false);
    }
}
