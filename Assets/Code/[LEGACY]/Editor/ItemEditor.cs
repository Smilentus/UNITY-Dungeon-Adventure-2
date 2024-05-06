using System.Collections;
using System.Collections.Generic;
using Dimasyechka.Code._LEGACY_.Inventory;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemProfile))]
public class ItemEditor : Editor
{
    // Ссылка на предметный скрипт
    private ItemProfile currentItem;
    // Сериализируемый объект
    private SerializedObject soItem;

    // Переменные
    private SerializedProperty takedSlot;
    private SerializedProperty Type;
    private SerializedProperty Icon;
    private SerializedProperty Name;
    private SerializedProperty Descr;
    private SerializedProperty WhereToFind;
    private SerializedProperty Cost;
    private SerializedProperty ChanceToFind;
    private SerializedProperty Damage;
    private SerializedProperty Health;
    private SerializedProperty Armor;
    private SerializedProperty AttackSpeed;
    private SerializedProperty Mana;
    private SerializedProperty ExtraExp;
    private SerializedProperty ExtraMoney;
    private SerializedProperty equipBuffs;
    private SerializedProperty Stack;
    private SerializedProperty MaxStack;
    private SerializedProperty DropStack;
    //[Header("Список ID:")]
    //[Header("0-99 - Книжки. 100-199 - Зелья.")]
    //[Header("200-800 - Любое оружие.")]
    //[Header("800-1199 - Одежда и аксессуары.")]
    //[Header("1200-inf - Прочее")]
    //[Header("ID предмета")]
    public SerializedProperty ItemID;

    public int toolbarTab;
    public string currentTab;

    // Инициализация
    public void OnEnable()
    {
        currentItem = (ItemProfile)target;
        soItem = new SerializedObject(target);

        takedSlot = soItem.FindProperty("takedSlot");
        ItemID = soItem.FindProperty("ItemID");
        Type = soItem.FindProperty("Type");
        Icon = soItem.FindProperty("Icon");
        Name = soItem.FindProperty("Name");
        Descr = soItem.FindProperty("Descr");
        WhereToFind = soItem.FindProperty("WhereToFind");
        Cost = soItem.FindProperty("Cost");
        ChanceToFind = soItem.FindProperty("ChanceToFind");
        Damage = soItem.FindProperty("Damage");
        Health = soItem.FindProperty("Health");
        Armor = soItem.FindProperty("Armor");
        AttackSpeed = soItem.FindProperty("AttackSpeed");
        Mana = soItem.FindProperty("Mana");
        ExtraExp = soItem.FindProperty("ExtraExp");
        ExtraMoney = soItem.FindProperty("ExtraMoney");
        equipBuffs = soItem.FindProperty("equipBuffs");
        Stack = soItem.FindProperty("Stack");
        MaxStack = soItem.FindProperty("MaxStack");
        DropStack = soItem.FindProperty("DropStack");
    }

    // Отрисовка кастомного редактора
    public override void OnInspectorGUI()
    {
        // Обновляем предмет
        soItem.Update();

        // Проверка для изменения
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(ItemID, new GUIContent("ID: "));
        EditorGUILayout.PropertyField(Type, new GUIContent("Тип: "));
        EditorGUILayout.PropertyField(Icon, new GUIContent("Иконка: "));
        EditorGUILayout.PropertyField(Name, new GUIContent("Название: "));
        EditorGUILayout.PropertyField(Descr, new GUIContent("Описание: "));
        EditorGUILayout.PropertyField(WhereToFind, new GUIContent("Где найти: "));
        EditorGUILayout.PropertyField(ChanceToFind, new GUIContent("Шанс найти: "));
        EditorGUILayout.PropertyField(Cost, new GUIContent("Цена: "));
        EditorGUILayout.PropertyField(MaxStack, new GUIContent("Макс. стак: "));
        EditorGUILayout.PropertyField(DropStack, new GUIContent("Кол-во дропа."));
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(Health, new GUIContent("Здоровье: "));
        EditorGUILayout.PropertyField(Mana, new GUIContent("Мана: "));
        EditorGUILayout.PropertyField(Damage, new GUIContent("Урон: "));
        EditorGUILayout.PropertyField(AttackSpeed, new GUIContent("Скорость атаки: "));
        EditorGUILayout.PropertyField(Armor, new GUIContent("Защита: "));
        EditorGUILayout.PropertyField(ExtraExp, new GUIContent("Доп. опыт: "));
        EditorGUILayout.PropertyField(ExtraMoney, new GUIContent("Доп. золото: "));
        EditorGUILayout.PropertyField(equipBuffs, new GUIContent("Баффы: "));

        if (EditorGUI.EndChangeCheck())
        {
            // Подтверждаем изменения
            soItem.ApplyModifiedProperties();
        }
    }
}
