using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Creatable/New Item")]
public class ItemProfile : ScriptableObject
{
    // Последний выбранный слот
    public int takedSlot;
    // Тип предмета
    public Inventory.ItemType Type;
    // Иконка предмета
    public Texture Icon;
    // Название предмета
    public string Name;
    // Описание предмета
    [TextArea(3, 5)]
    public string Descr;
    // Где найти предмет
    [TextArea(3, 5)]
    public string WhereToFind;
    // Цена предмета
    public int Cost;
    // Шанс найти предмет
    public double ChanceToFind;
    // Характеристики предмета
    public int Damage;
    public int Health;
    public int Armor;
    public int AttackSpeed;
    public int Mana;
    public int ExtraExp;
    public int ExtraMoney;
    public Buff[] equipBuffs;
    // Текущий стак предмета
    public int Stack;
    // Макс. стак предмета
    public int MaxStack;
    // Дроп. стак
    public int DropStack;
    //[Header("Список ID:")]
    //[Header("0-99 - Книжки. 100-199 - Зелья.")]
    //[Header("200-800 - Любое оружие.")]
    //[Header("800-1199 - Одежда и аксессуары.")]
    //[Header("1200-inf - Прочее")]
    //[Header("ID предмета")]
    public string ItemID;
}
