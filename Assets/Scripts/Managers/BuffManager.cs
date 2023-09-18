using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    [Header("Коробка описания")]
    public GameObject DescrBox;

    [Header("Полная панель баффов игрока")]
    public GameObject FullBuffPanelPlayer;

    [Header("Полная панель баффов противника")]
    public GameObject FullBuffPanelEnemy;

    [Header("Родитель для спавна")]
    public Transform BuffsParent;
    [Header("Префаб баффа")]
    public GameObject BuffPrefab;
    [Header("Родитель для спавна баффов противника")]
    public Transform EnemyBuffsParent;

    [Header("Все баффы")]
    public Buff[] AllBuffs;

    [Header("Объекты баффов игрока")]
    public List<GameObject> buffsObjects = new List<GameObject>();
    [Header("Объекты баффов противника")]
    public List<GameObject> enemyBuffsObjects = new List<GameObject>();

    [Header("Активные баффы")]
    public List<Buff> activeBuffs = new List<Buff>();
    [Header("Активные баффы противников")]
    public List<Buff> enemyActiveBuffs = new List<Buff>();

    // Обновляем каждый кадр. P.S.: Написал просто для красоты, да, Я дебил.
    private void Update()
    {
        if (activeBuffs.Count > 0)
            UpdateBuffsUI();
    }

    // Поиск баффа в списке
    public int BuffPos(Buff.BuffType fType)
    {
        int num = -1;
        for (int i = 0; i < AllBuffs.Length; i++)
        {
            if (AllBuffs[i].Type == fType)
                return num = i;
        }
        return num;
    }

    // Добавление баффа игроку
    public void SetBuff(Buff.BuffType sType)
    {
        bool isFound = false;

        FullBuffPanelPlayer.gameObject.SetActive(true);

        // Поиск баффа, если есть, то конец скрипта
        for (int i = 0; i < activeBuffs.Count; i++)
        {
            if (activeBuffs[i].Type == sType)
            {
                isFound = true;
                break;
            }
        }

        // Если бафф не найден => вешаем этот бафф
        if (!isFound)
        {
            var b = AllBuffs[BuffPos(sType)];

            activeBuffs.Add(new Buff()
            {
                Name = b.Name,
                Descr = b.Descr, Type = b.Type,
                Duration = b.Duration,
                MaxDuration = b.MaxDuration,
                buffPower = b.buffPower,
                isInfinity = b.isInfinity,
                Icon = b.Icon,
                iconColor = b.iconColor
            });

            CreateBuffObject(sType, 0);

            BuffsOnceAction(0);
        }
    }

    // Добавление баффа противнику
    public void SetBuffToEnemy(Buff.BuffType sType)
    {
        if (BattleController.Instance.IsBattle)
        {
            bool isFound = false;

            FullBuffPanelEnemy.gameObject.SetActive(true);

            // Поиск баффа, если есть, то конец скрипта
            for (int i = 0; i < enemyActiveBuffs.Count; i++)
            {
                if (enemyActiveBuffs[i].Type == sType)
                {
                    isFound = true;
                    break;
                }
            }

            // Если бафф не найден => вешаем этот бафф
            if (!isFound)
            {
                var b = AllBuffs[BuffPos(sType)];

                enemyActiveBuffs.Add(new Buff()
                {
                    Name = b.Name,
                    Descr = b.Descr,
                    Type = b.Type,
                    Duration = b.Duration,
                    MaxDuration = b.MaxDuration,
                    buffPower = b.buffPower,
                    isInfinity = b.isInfinity,
                    Icon = b.Icon,
                    iconColor = b.iconColor
                });

                CreateBuffObject(sType, 1);

                BuffsOnceAction(1);
            }
        }
    }

    #region Отдельный регион для действия баффов
    // Действие баффов при получении
    public void BuffsOnceAction(int character)
    {
        int var = 0;

        // Игрок
        if (character == 0)
        {
            for (int i = 0; i < activeBuffs.Count; i++)
            {
                var = activeBuffs[i].buffPower;
                switch (activeBuffs[i].Type)
                {
                    case Buff.BuffType.ManaRegenBonus:
                        Player.ManaRegen += var;
                        break;
                    case Buff.BuffType.RegenBonus:
                        Player.HealthRegen += var;
                        break;
                    case Buff.BuffType.HealthBonus:
                        Player.MaxHealth += var;
                        Player.Health += var;
                        break;
                    case Buff.BuffType.ManaBonus:
                        Player.MaxMana += var;
                        Player.Mana += var;
                        break;
                    case Buff.BuffType.DamageBonus:
                        Player.Damage += var;
                        break;
                    case Buff.BuffType.ArmorDecreasing:
                        Player.Armor -= var;
                        break;
                    case Buff.BuffType.ArmorBonus:
                        Player.Armor += var;
                        break;
                    case Buff.BuffType.HealthAndManaRegen:
                        Player.HealthRegen += var;
                        Player.ManaRegen += var;
                        break;
                    case Buff.BuffType.InfinityHealthRegen:
                        Player.HealthRegen += var;
                        break;
                    case Buff.BuffType.InfinityManaRegen:
                        Player.ManaRegen += var;
                        break;
                    case Buff.BuffType.InfinityShield:
                        Player.Armor += var;
                        break;
                    // Зелья превращения
                    case Buff.BuffType.RabbitBuff:
                        GameController.Instance.ShowMessageText("Вы превратились в кролика, милого и пушистого.");
                        break;
                    case Buff.BuffType.SheepBuff:
                        GameController.Instance.ShowMessageText("Вы превратились в овцу. Прямо как Ваша жена.");
                        break;
                    case Buff.BuffType.ChickenBuff:
                        GameController.Instance.ShowMessageText("Вы превратились в курицу! Ко-ко-ко");
                        break;
                    case Buff.BuffType.FrogBuff:
                        if (!isBuffOnAction(Buff.BuffType.FrogBuff, 0))
                        {
                            // Сохранение переменных игрока
                            Player.tempDamage = Player.Damage;
                            Player.tempHealth = Player.Health;
                            Player.tempMaxHealth = Player.MaxHealth;
                            Player.tempMaxMana = Player.MaxMana;
                            Player.tempMana = Player.Mana;
                            Player.tempAttackSpeed = Player.AttackSpeed;
                            Player.tempArmor = Player.Armor;

                            // Действие баффа
                            Player.MaxHealth = 10;
                            Player.Health = 10;
                            Player.MaxMana = 100;
                            Player.Mana = 100;
                            Player.Damage = 1;
                            Player.AttackSpeed = 1;
                            Player.Armor = 0;

                            // Информируем
                            GameController.Instance.ShowMessageText("Вы превратились в лягушку. А я говорил!");
                        }
                        break;
                    case Buff.BuffType.SunstayBuff:
                        Player.MaxMana += 500;
                        Player.Mana += 500;
                        Player.ManaRegen += 125;
                        break;
                    case Buff.BuffType.FullmoonBuff:
                        Player.MaxHealth += 100;
                        Player.Health += 100;
                        Player.Damage += 35;
                        Player.HealthRegen += 10;
                        Player.ManaRegen += 5;
                        break;
                    case Buff.BuffType.PlanetRowBuff:
                        Player.MaxHealth += 2500;
                        Player.Health += 2500;
                        Player.Damage += 750;
                        Player.AttackSpeed += 5;
                        Player.HealthRegen += 150;
                        Player.ManaRegen += 50;
                        Player.MaxMana += 200;
                        Player.Mana += 200;
                        break;
                    case Buff.BuffType.MageAbility:
                        Player.MaxMana += 1000 * Player.HALvl;
                        Player.Mana += 1000 * Player.HALvl;
                        break;
                    case Buff.BuffType.WarriorAbility:
                        Player.Armor += 100 * Player.HALvl;
                        Player.MaxHealth += 200 * Player.HALvl;
                        Player.Health += 200 * Player.HALvl;
                        break;
                    case Buff.BuffType.Happiness:
                        Player.MaxHealth += 1000;
                        Player.Health += 1000;
                        Player.Damage += 50;
                        Player.Armor += 10;
                        break;
                    case Buff.BuffType.Illuminati:
                        Player.ExtraExpMod += var;
                        break;
                    case Buff.BuffType.Magic_HealthRegen:
                        break;
                    case Buff.BuffType.Magic_Shield:
                        var MM = FindObjectOfType<MagicManager>();
                        Player.Armor += (int)MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Shield)].actionVar;
                        break;
                }
            }
        }

        //// Противник
        //if (character == 1)
        //{
        //    for (int i = 0; i < enemyActiveBuffs.Count; i++)
        //    {
        //        var = enemyActiveBuffs[i].buffPower;
        //        switch (enemyActiveBuffs[i].Type)
        //        {
        //            case Buff.BuffType.RegenBonus:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].HealthRegen += var;
        //                break;
        //            case Buff.BuffType.HealthBonus:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].MaxHealth += var;
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Health += var;
        //                break;
        //            case Buff.BuffType.ManaBonus:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].MaxMana += var;
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Mana += var;
        //                break;
        //            case Buff.BuffType.DamageBonus:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Damage += var;
        //                break;
        //            case Buff.BuffType.ArmorDecreasing:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Armor -= var;
        //                break;
        //            case Buff.BuffType.ArmorBonus:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Armor += var;
        //                break;
        //            case Buff.BuffType.Magic_Stun:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].isStun = true;
        //                Debug.Log(BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Name + " - Имя - " + BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].isStun);
        //                break;
        //        }
        //    }
        //}

        CheckBuffEnd();
    }
    
    // Действие баффов каждый ход
    public void BuffsAction()
    {
        // Игрок
        for (int i = 0; i < activeBuffs.Count; i++)
        {
            // На всякий случай проверим должен ли бафф работать
            if (activeBuffs[i].Duration > 0)
            {
                // Эффект баффа 
                switch (activeBuffs[i].Type)
                {
                    case Buff.BuffType.Poison:
                        Player.Health -= activeBuffs[i].buffPower;
                        break;
                    case Buff.BuffType.Bleeding:
                        Player.Health -= activeBuffs[i].buffPower;
                        break;
                    case Buff.BuffType.Fire:
                        Player.Health -= activeBuffs[i].buffPower;
                        break;
                }

                if (!activeBuffs[i].isInfinity)
                {
                    // Изменяем пару переменных
                    activeBuffs[i].Duration--;
                }
            }
        }

        //// Противник
        //for (int i = 0; i < enemyActiveBuffs.Count; i++)
        //{
        //    // На всякий случай проверим должен ли бафф работать
        //    if (enemyActiveBuffs[i].Duration > 0)
        //    {
        //        int var = enemyActiveBuffs[i].buffPower;
        //        // Эффект баффа 
        //        switch (enemyActiveBuffs[i].Type)
        //        {
        //            case Buff.BuffType.Poison:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Health -= var;
        //                break;
        //            case Buff.BuffType.Bleeding:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Health -= var;
        //                break;
        //            case Buff.BuffType.Fire:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Health -= var;
        //                break;
        //            case Buff.BuffType.Magic_Poison:
        //                var MM = FindObjectOfType<MagicManager>();
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Health -= MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Poison)].actionVar;
        //                break;
        //        }

        //        if (!enemyActiveBuffs[i].isInfinity)
        //        {
        //            // Изменяем пару переменных
        //            enemyActiveBuffs[i].Duration--;
        //        }
        //    }
        //}

        // Проверяем закончился ли бафф
        CheckBuffEnd();
    }

    // Действие баффа при надевании вещи
    public void BuffsEquipAction()
    {
        for (int i = 0; i < activeBuffs.Count; i++)
        {
            switch (activeBuffs[i].Type)
            {

            }
        }
    }

    // Удаление баффа при снятии вещи
    public void BuffsUnequipAction()
    {
        for (int i = 0; i < activeBuffs.Count; i++)
            DeleteBuffAction(i, 0);
    }

    // Действие баффов в битве
    public void BuffsBattleAction()
    {
        for (int i = 0; i < activeBuffs.Count; i++)
        {
            switch (activeBuffs[i].Type)
            {
                case Buff.BuffType.Vampirism:
                    Player.Health += Player.Damage * activeBuffs[i].buffPower;
                    break;
            }
        }

        CheckBuffEnd();
    }
    #endregion

    // Общее удаление эффекта баффа по типу и номеру
    public void DeleteBuffAction(int i, int character)
    {
        int var = 0;

        if(character == 0)
        {
            var = activeBuffs[i].buffPower;
            switch (activeBuffs[i].Type)
            {
                case Buff.BuffType.ArmorBonus:
                    Player.Armor -= var;
                    break;
                case Buff.BuffType.ArmorDecreasing:
                    Player.Armor += var;
                    break;
                case Buff.BuffType.DamageBonus:
                    Player.Damage -= var;
                    break;
                case Buff.BuffType.HealthBonus:
                    Player.MaxHealth -= var;
                    break;
                case Buff.BuffType.ManaBonus:
                    Player.MaxMana -= var;
                    break;
                case Buff.BuffType.ManaRegenBonus:
                    Player.ManaRegen -= var;
                    break;
                case Buff.BuffType.RegenBonus:
                    Player.HealthRegen -= var;
                    break;
                case Buff.BuffType.HealthAndManaRegen:
                    Player.HealthRegen -= var;
                    Player.ManaRegen -= var;
                    break;
                case Buff.BuffType.InfinityHealthRegen:
                    Player.HealthRegen -= var;
                    break;
                case Buff.BuffType.InfinityManaRegen:
                    Player.ManaRegen -= var;
                    break;
                case Buff.BuffType.InfinityShield:
                    Player.Armor -= var;
                    break;
                case Buff.BuffType.FrogBuff:
                    // Снимаем действие баффа
                    Player.Damage = Player.tempDamage;
                    Player.Health = Player.tempHealth;
                    Player.MaxHealth = Player.tempMaxHealth;
                    Player.MaxMana = Player.tempMaxMana;
                    Player.Mana = Player.tempMana;
                    Player.AttackSpeed = Player.tempAttackSpeed;
                    Player.Armor = Player.tempArmor;
                    break;
                case Buff.BuffType.SunstayBuff:
                    Player.MaxMana -= 500;
                    Player.Mana -= 500;
                    Player.ManaRegen -= 125;
                    GameTimeFlowController.Instance.ForceFinishGameTimeFlowEvent("SunStay_GameTimeFlowEvent");
                    break;
                case Buff.BuffType.FullmoonBuff:
                    Player.MaxHealth -= 100;
                    Player.Health -= 100;
                    Player.Damage -= 35;
                    Player.HealthRegen -= 10;
                    Player.ManaRegen -= 5;
                    GameTimeFlowController.Instance.ForceFinishGameTimeFlowEvent("FullMoon_Sunstay_GameTimeFlowEvent");
                    break;
                case Buff.BuffType.PlanetRowBuff:
                    Player.MaxHealth -= 2500;
                    Player.Health -= 2500;
                    Player.Damage -= 750;
                    Player.AttackSpeed -= 5;
                    Player.HealthRegen -= 150;
                    Player.ManaRegen -= 50;
                    Player.MaxMana -= 200;
                    Player.Mana -= 200;
                    GameTimeFlowController.Instance.ForceFinishGameTimeFlowEvent("PlanetRow_GameTimeFlowEvent");
                    break;
                case Buff.BuffType.MageAbility:
                    Player.MaxMana -= 1000 * Player.HALvl;
                    Player.Mana -= 1000 * Player.HALvl;
                    break;
                case Buff.BuffType.WarriorAbility:
                    Player.Armor -= 100 * Player.HALvl;
                    Player.MaxHealth -= 200 * Player.HALvl;
                    Player.Health -= 200 * Player.HALvl;
                    break;
                case Buff.BuffType.Happiness:
                    Player.MaxHealth -= 1000;
                    Player.Health -= 1000;
                    Player.Damage -= 50;
                    Player.Armor -= 10;
                    break;
                case Buff.BuffType.Magic_HealthRegen:
                    // Добавить потом
                    break;
                case Buff.BuffType.Illuminati:
                    Player.ExtraExpMod -= var;
                    break;
                case Buff.BuffType.Magic_Shield:
                    Player.Armor -= 20;
                    break;
                case Buff.BuffType.Magic_Stun:
                    // Выход из оглушения
                    break;
            }
        }
        //if(character == 1)
        //{
        //    var = enemyActiveBuffs[i].buffPower;
        //    if (BattleController.Instance.allEnemies.Count != 0)
        //    {
        //        switch (enemyActiveBuffs[i].Type)
        //        {
        //            case Buff.BuffType.RegenBonus:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].HealthRegen -= var;
        //                break;
        //            case Buff.BuffType.HealthBonus:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].MaxHealth -= var;
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Health -= var;
        //                break;
        //            case Buff.BuffType.ManaBonus:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].MaxMana -= var;
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Mana -= var;
        //                break;
        //            case Buff.BuffType.DamageBonus:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Damage -= var;
        //                break;
        //            case Buff.BuffType.ArmorDecreasing:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Armor += var;
        //                break;
        //            case Buff.BuffType.ArmorBonus:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].Armor -= var;
        //                break;
        //            case Buff.BuffType.Magic_Stun:
        //                BattleController.Instance.allEnemies[BattleController.Instance.allEnemies.Count - 1].isStun = false;
        //                break;
        //        }
        //    }
        //}
    }
    public void DeleteBuffAction(Buff.BuffType dType)
    {
        int pos = -1; 

        for(int i = 0; i < activeBuffs.Count; i++)
        {
            if(activeBuffs[i].Type == dType)
            {
                pos = i;
                break;
            }
        }

        if (pos == -1)
        {
            FindObjectOfType<GameController>().ShowMessageText("Вы не зачарованы! Попробуйте позже.");
            return;
        }

        DeleteBuffAction(pos, 0);

        CheckBuffEnd();
    }
    public void DeleteBuffActionOnEnemy(Buff.BuffType dType)
    {
        int pos = -1;

        for (int i = 0; i < enemyActiveBuffs.Count; i++)
        {
            if (enemyActiveBuffs[i].Type == dType)
            {
                pos = i;
                break;
            }
        }

        if (pos == -1)
        {
            return;
        }

        DeleteBuffAction(pos, 1);

        CheckBuffEnd();
    }

    // Проверка действует ли сейчас бафф
    public bool isBuffOnAction(Buff.BuffType cType, int character)
    {
        if (character == 0)
        {
            for (int i = 0; i < activeBuffs.Count; i++)
            {
                if (activeBuffs[i].Type == cType)
                    return true;
            }
        }
        if(character == 1)
        {
            for (int i = 0; i < enemyActiveBuffs.Count; i++)
            {
                if (enemyActiveBuffs[i].Type == cType)
                    return true;
            }
        }
        return false;
    }

    // Проверка на конец баффа
    public void CheckBuffEnd()
    {
        // Игрок
        for (int i = 0; i < activeBuffs.Count; i++)
        {
            // Игрок
            if (activeBuffs[i].Duration <= 0)
            {
                // Убираем эффект по типу нашего баффа
                DeleteBuffAction(i, 0);

                // Удаляем из списка
                Destroy(buffsObjects[i]);
                buffsObjects.Remove(buffsObjects[i]);
                activeBuffs.Remove(activeBuffs[i]);

                i--;
            }
        }

        // Противник
        for (int i = 0; i < enemyActiveBuffs.Count; i++)
        {
            if (enemyActiveBuffs[i].Duration <= 0)
            {
                // Убираем эффект по типу нашего баффа
                DeleteBuffActionOnEnemy(enemyActiveBuffs[i].Type);

                // Удаляем из списка
                Destroy(enemyBuffsObjects[i]);
                enemyBuffsObjects.Remove(enemyBuffsObjects[i]);
                enemyActiveBuffs.Remove(enemyActiveBuffs[i]);

                i--;
            }
        }

        if (enemyActiveBuffs.Count == 0)
            FullBuffPanelEnemy.gameObject.SetActive(false);

        if (activeBuffs.Count == 0)
            FullBuffPanelPlayer.gameObject.SetActive(false);
    }
    
    // Изменение времени действия ВСЕХ баффов на указаное значение
    public void ChangeBuffActionTime(int actionTime)
    {
        for(int i = 0; i < activeBuffs.Count; i++)
        {
            activeBuffs[i].Duration += actionTime;
            // Защита
            if (activeBuffs[i].Duration > activeBuffs[i].MaxDuration)
                activeBuffs[i].Duration = activeBuffs[i].MaxDuration;
        }

        CheckBuffEnd();
        UpdateBuffsUI();
    }

    // Обнуление ВСЕХ баффов
    public void NullAllBuffs()
    {
        for (int i = 0; i < activeBuffs.Count; i++)
        {
            DeleteBuffAction(i, 0);
        }
        activeBuffs.Clear();

        for (int i = 0; i < enemyActiveBuffs.Count; i++)
        {
            DeleteBuffAction(i, 1);
        }
        enemyActiveBuffs.Clear();

        foreach(var buff in enemyBuffsObjects)
        {
            Destroy(buff.gameObject);
        }
        enemyBuffsObjects.Clear();

        foreach (var buff in buffsObjects)
        {
            Destroy(buff.gameObject);
        }
        buffsObjects.Clear();
    }

    // Обнуление всех баффов ПРОТИВНИКА
    public void NullAllEnemyBuffs()
    {
        for (int i = 0; i < enemyActiveBuffs.Count; i++)
        {
            DeleteBuffAction(i, 1);
        }
        enemyActiveBuffs.Clear();

        foreach (var buff in enemyBuffsObjects)
        {
            Destroy(buff.gameObject);
        }
        enemyBuffsObjects.Clear();
    }

    // Обнуление всех баффов у ИГРОКА
    public void NullAllPlayerBuffs()
    {
        for (int i = 0; i < activeBuffs.Count; i++)
        {
            DeleteBuffAction(i, 0);
        }
        activeBuffs.Clear();
        foreach (var buff in buffsObjects)
        {
            Destroy(buff.gameObject);
        }
        buffsObjects.Clear();
    }

    // Инициализация баффа и добавление в список после загрузки
    public void LoadBuff(Buff.BuffType lType, int lDuration)
    {
        var b = AllBuffs[BuffPos(lType)];
        activeBuffs.Add(new Buff()
        {
            Name = b.Name,
            Descr = b.Descr,
            Type = b.Type,
            Duration = b.Duration,
            MaxDuration = b.MaxDuration,
            buffPower = b.buffPower,
            isInfinity = b.isInfinity,
            Icon = b.Icon,
            iconColor = b.iconColor
        });
        CreateBuffObject(lType, 0);
    }

    // Создаём объект баффа
    /// <summary>
    /// 0 - Игрок. 1 - противник.
    /// </summary>
    /// <param name="cType"></param>
    public void CreateBuffObject(Buff.BuffType cType, int character)
    {
        if (character == 0)
        {
            GameObject newBuff = Instantiate(BuffPrefab, BuffsParent);
            newBuff.GetComponent<BuffButton>().bType = cType;
            buffsObjects.Add(newBuff);
        }
        if(character == 1)
        {
            GameObject newBuff = Instantiate(BuffPrefab, EnemyBuffsParent);
            newBuff.GetComponent<BuffButton>().bType = cType;
            enemyBuffsObjects.Add(newBuff);
        }
    }

    // Перерисовка баффов
    public void UpdateBuffsUI()
    {
        for (int i = 0; i < buffsObjects.Count; i++)
        {
            buffsObjects[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = activeBuffs[i].Icon;
            buffsObjects[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = activeBuffs[i].iconColor;
            if (!activeBuffs[i].isInfinity)
                buffsObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = activeBuffs[i].Duration + "/" + activeBuffs[i].MaxDuration;
            else
                buffsObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "Беск.";
        }
        for (int i = 0; i < enemyBuffsObjects.Count; i++)
        {
            enemyBuffsObjects[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = enemyActiveBuffs[i].Icon;
            enemyBuffsObjects[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = enemyActiveBuffs[i].iconColor;
            if (!enemyActiveBuffs[i].isInfinity)
                enemyBuffsObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = enemyActiveBuffs[i].Duration + "/" + enemyActiveBuffs[i].MaxDuration;
            else
                enemyBuffsObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "Беск.";
        }
    }

    // Показать описание баффа
    public void ShowDescr(Buff.BuffType dType)
    {
        var SM = FindObjectOfType<SkillsManager>();
        var b = AllBuffs[BuffPos(dType)];
        if(!b.isInfinity)
            DescrBox.GetComponentInChildren<Text>().text = "Бафф: " + b.Name + " \n| " + SM.TransformDescr(b.Descr) + " \n| Действует: " + b.MaxDuration + " ходов.";
        else
            DescrBox.GetComponentInChildren<Text>().text = "Бафф: " + b.Name + " \n| " + SM.TransformDescr(b.Descr) + " \n| Действует: Бесконечно";
        DescrBox.SetActive(true);
    }

    // Закрыть описание баффа
    public void HideDescr()
    {
        DescrBox.SetActive(false);
    }
}
