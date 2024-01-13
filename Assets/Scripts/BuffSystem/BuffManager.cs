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
    public BuffProfile[] AllBuffs;

    [Header("Объекты баффов игрока")]
    public List<GameObject> buffsObjects = new List<GameObject>();
    [Header("Объекты баффов противника")]
    public List<GameObject> enemyBuffsObjects = new List<GameObject>();

    [Header("Активные баффы")]
    public List<BuffProfile> activeBuffs = new List<BuffProfile>();
    [Header("Активные баффы противников")]
    public List<BuffProfile> enemyActiveBuffs = new List<BuffProfile>();

    // Обновляем каждый кадр. P.S.: Написал просто для красоты, да, Я дебил.
    private void Update()
    {
        if (activeBuffs.Count > 0)
            UpdateBuffsUI();
    }

    // Поиск баффа в списке
    public int BuffPos(BuffProfile.BuffType fType)
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
    public void SetBuff(BuffProfile.BuffType sType)
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

            //activeBuffs.Add(new BuffProfile()
            //{
            //    Name = b.Name,
            //    Descr = b.Description, Type = b.Type,
            //    Duration = b.Duration,
            //    MaxDuration = b.MaxDuration,
            //    buffPower = b.buffPower,
            //    isInfinity = b.isInfinity,
            //    Icon = b.Icon,
            //    iconColor = b.iconColor
            //});

            CreateBuffObject(sType, 0);

            BuffsOnceAction(0);
        }
    }

    // Добавление баффа противнику
    public void SetBuffToEnemy(BuffProfile.BuffType sType)
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

                //enemyActiveBuffs.Add(new BuffProfile()
                //{
                //    Name = b.Name,
                //    Descr = b.Description,
                //    Type = b.Type,
                //    Duration = b.Duration,
                //    MaxDuration = b.MaxDuration,
                //    buffPower = b.buffPower,
                //    isInfinity = b.isInfinity,
                //    Icon = b.Icon,
                //    iconColor = b.iconColor
                //});

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
                    case BuffProfile.BuffType.ManaRegenBonus:
                        RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen += var;
                        break;
                    case BuffProfile.BuffType.RegenBonus:
                        RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen += var;
                        break;
                    case BuffProfile.BuffType.HealthBonus:
                        RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth += var;
                        RuntimePlayer.Instance.RuntimePlayerStats.Health += var;
                        break;
                    case BuffProfile.BuffType.ManaBonus:
                        RuntimePlayer.Instance.RuntimePlayerStats.MaxMana += var;
                        RuntimePlayer.Instance.RuntimePlayerStats.Mana += var;
                        break;
                    case BuffProfile.BuffType.DamageBonus:
                        RuntimePlayer.Instance.RuntimePlayerStats.Damage += var;
                        break;
                    case BuffProfile.BuffType.ArmorDecreasing:
                        RuntimePlayer.Instance.RuntimePlayerStats.Armor -= var;
                        break;
                    case BuffProfile.BuffType.ArmorBonus:
                        RuntimePlayer.Instance.RuntimePlayerStats.Armor += var;
                        break;
                    case BuffProfile.BuffType.HealthAndManaRegen:
                        RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen += var;
                        RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen += var;
                        break;
                    case BuffProfile.BuffType.InfinityHealthRegen:
                        RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen += var;
                        break;
                    case BuffProfile.BuffType.InfinityManaRegen:
                        RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen += var;
                        break;
                    case BuffProfile.BuffType.InfinityShield:
                        RuntimePlayer.Instance.RuntimePlayerStats.Armor += var;
                        break;
                    // Зелья превращения
                    case BuffProfile.BuffType.RabbitBuff:
                        GameController.Instance.ShowMessageText("Вы превратились в кролика, милого и пушистого.");
                        break;
                    case BuffProfile.BuffType.SheepBuff:
                        GameController.Instance.ShowMessageText("Вы превратились в овцу. Прямо как Ваша жена.");
                        break;
                    case BuffProfile.BuffType.ChickenBuff:
                        GameController.Instance.ShowMessageText("Вы превратились в курицу! Ко-ко-ко");
                        break;
                    case BuffProfile.BuffType.FrogBuff:
                        if (!isBuffOnAction(BuffProfile.BuffType.FrogBuff, 0))
                        {
                            // Сохранение переменных игрока
                            RuntimePlayer.Instance.RuntimePlayerStats.tempDamage = RuntimePlayer.Instance.RuntimePlayerStats.Damage;
                            RuntimePlayer.Instance.RuntimePlayerStats.tempHealth = RuntimePlayer.Instance.RuntimePlayerStats.Health;
                            RuntimePlayer.Instance.RuntimePlayerStats.tempMaxHealth = RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth;
                            RuntimePlayer.Instance.RuntimePlayerStats.tempMaxMana = RuntimePlayer.Instance.RuntimePlayerStats.MaxMana;
                            RuntimePlayer.Instance.RuntimePlayerStats.tempMana = RuntimePlayer.Instance.RuntimePlayerStats.Mana;
                            RuntimePlayer.Instance.RuntimePlayerStats.tempAttackSpeed = RuntimePlayer.Instance.RuntimePlayerStats.AttackSpeed;
                            RuntimePlayer.Instance.RuntimePlayerStats.tempArmor = RuntimePlayer.Instance.RuntimePlayerStats.Armor;

                            // Действие баффа
                            RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth = 10;
                            RuntimePlayer.Instance.RuntimePlayerStats.Health = 10;
                            RuntimePlayer.Instance.RuntimePlayerStats.MaxMana = 100;
                            RuntimePlayer.Instance.RuntimePlayerStats.Mana = 100;
                            RuntimePlayer.Instance.RuntimePlayerStats.Damage = 1;
                            RuntimePlayer.Instance.RuntimePlayerStats.AttackSpeed = 1;
                            RuntimePlayer.Instance.RuntimePlayerStats.Armor = 0;

                            // Информируем
                            GameController.Instance.ShowMessageText("Вы превратились в лягушку. А я говорил!");
                        }
                        break;
                    case BuffProfile.BuffType.SunstayBuff:
                        RuntimePlayer.Instance.RuntimePlayerStats.MaxMana += 500;
                        RuntimePlayer.Instance.RuntimePlayerStats.Mana += 500;
                        RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen += 125;
                        break;
                    case BuffProfile.BuffType.FullmoonBuff:
                        RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth += 100;
                        RuntimePlayer.Instance.RuntimePlayerStats.Health += 100;
                        RuntimePlayer.Instance.RuntimePlayerStats.Damage += 35;
                        RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen += 10;
                        RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen += 5;
                        break;
                    case BuffProfile.BuffType.PlanetRowBuff:
                        RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth += 2500;
                        RuntimePlayer.Instance.RuntimePlayerStats.Health += 2500;
                        RuntimePlayer.Instance.RuntimePlayerStats.Damage += 750;
                        RuntimePlayer.Instance.RuntimePlayerStats.AttackSpeed += 5;
                        RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen += 150;
                        RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen += 50;
                        RuntimePlayer.Instance.RuntimePlayerStats.MaxMana += 200;
                        RuntimePlayer.Instance.RuntimePlayerStats.Mana += 200;
                        break;
                    case BuffProfile.BuffType.MageAbility:
                        RuntimePlayer.Instance.RuntimePlayerStats.MaxMana += 1000 * RuntimePlayer.Instance.RuntimePlayerStats.HALvl;
                        RuntimePlayer.Instance.RuntimePlayerStats.Mana += 1000 * RuntimePlayer.Instance.RuntimePlayerStats.HALvl;
                        break;
                    case BuffProfile.BuffType.WarriorAbility:
                        RuntimePlayer.Instance.RuntimePlayerStats.Armor += 100 * RuntimePlayer.Instance.RuntimePlayerStats.HALvl;
                        RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth += 200 * RuntimePlayer.Instance.RuntimePlayerStats.HALvl;
                        RuntimePlayer.Instance.RuntimePlayerStats.Health += 200 * RuntimePlayer.Instance.RuntimePlayerStats.HALvl;
                        break;
                    case BuffProfile.BuffType.Happiness:
                        RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth += 1000;
                        RuntimePlayer.Instance.RuntimePlayerStats.Health += 1000;
                        RuntimePlayer.Instance.RuntimePlayerStats.Damage += 50;
                        RuntimePlayer.Instance.RuntimePlayerStats.Armor += 10;
                        break;
                    case BuffProfile.BuffType.Illuminati:
                        RuntimePlayer.Instance.RuntimePlayerStats.ExtraExpMultiplier += var;
                        break;
                    case BuffProfile.BuffType.Magic_HealthRegen:
                        break;
                    case BuffProfile.BuffType.Magic_Shield:
                        var MM = FindObjectOfType<MagicManager>();
                        RuntimePlayer.Instance.RuntimePlayerStats.Armor += (int)MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Shield)].actionVar;
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
                    case BuffProfile.BuffType.Poison:
                        RuntimePlayer.Instance.RuntimePlayerStats.Health -= activeBuffs[i].buffPower;
                        break;
                    case BuffProfile.BuffType.Bleeding:
                        RuntimePlayer.Instance.RuntimePlayerStats.Health -= activeBuffs[i].buffPower;
                        break;
                    case BuffProfile.BuffType.Fire:
                        RuntimePlayer.Instance.RuntimePlayerStats.Health -= activeBuffs[i].buffPower;
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
                case BuffProfile.BuffType.Vampirism:
                    RuntimePlayer.Instance.RuntimePlayerStats.Health += RuntimePlayer.Instance.RuntimePlayerStats.Damage * activeBuffs[i].buffPower;
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
                case BuffProfile.BuffType.ArmorBonus:
                    RuntimePlayer.Instance.RuntimePlayerStats.Armor -= var;
                    break;
                case BuffProfile.BuffType.ArmorDecreasing:
                    RuntimePlayer.Instance.RuntimePlayerStats.Armor += var;
                    break;
                case BuffProfile.BuffType.DamageBonus:
                    RuntimePlayer.Instance.RuntimePlayerStats.Damage -= var;
                    break;
                case BuffProfile.BuffType.HealthBonus:
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth -= var;
                    break;
                case BuffProfile.BuffType.ManaBonus:
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxMana -= var;
                    break;
                case BuffProfile.BuffType.ManaRegenBonus:
                    RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen -= var;
                    break;
                case BuffProfile.BuffType.RegenBonus:
                    RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen -= var;
                    break;
                case BuffProfile.BuffType.HealthAndManaRegen:
                    RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen -= var;
                    RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen -= var;
                    break;
                case BuffProfile.BuffType.InfinityHealthRegen:
                    RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen -= var;
                    break;
                case BuffProfile.BuffType.InfinityManaRegen:
                    RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen -= var;
                    break;
                case BuffProfile.BuffType.InfinityShield:
                    RuntimePlayer.Instance.RuntimePlayerStats.Armor -= var;
                    break;
                case BuffProfile.BuffType.FrogBuff:
                    // Снимаем действие баффа
                    RuntimePlayer.Instance.RuntimePlayerStats.Damage = RuntimePlayer.Instance.RuntimePlayerStats.tempDamage;
                    RuntimePlayer.Instance.RuntimePlayerStats.Health = RuntimePlayer.Instance.RuntimePlayerStats.tempHealth;
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth = RuntimePlayer.Instance.RuntimePlayerStats.tempMaxHealth;
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxMana = RuntimePlayer.Instance.RuntimePlayerStats.tempMaxMana;
                    RuntimePlayer.Instance.RuntimePlayerStats.Mana = RuntimePlayer.Instance.RuntimePlayerStats.tempMana;
                    RuntimePlayer.Instance.RuntimePlayerStats.AttackSpeed = RuntimePlayer.Instance.RuntimePlayerStats.tempAttackSpeed;
                    RuntimePlayer.Instance.RuntimePlayerStats.Armor = RuntimePlayer.Instance.RuntimePlayerStats.tempArmor;
                    break;
                case BuffProfile.BuffType.SunstayBuff:
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxMana -= 500;
                    RuntimePlayer.Instance.RuntimePlayerStats.Mana -= 500;
                    RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen -= 125;
                    GameTimeFlowController.Instance.ForceFinishGameTimeFlowEvent("SunStay_GameTimeFlowEvent");
                    break;
                case BuffProfile.BuffType.FullmoonBuff:
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth -= 100;
                    RuntimePlayer.Instance.RuntimePlayerStats.Health -= 100;
                    RuntimePlayer.Instance.RuntimePlayerStats.Damage -= 35;
                    RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen -= 10;
                    RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen -= 5;
                    GameTimeFlowController.Instance.ForceFinishGameTimeFlowEvent("FullMoon_Sunstay_GameTimeFlowEvent");
                    break;
                case BuffProfile.BuffType.PlanetRowBuff:
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth -= 2500;
                    RuntimePlayer.Instance.RuntimePlayerStats.Health -= 2500;
                    RuntimePlayer.Instance.RuntimePlayerStats.Damage -= 750;
                    RuntimePlayer.Instance.RuntimePlayerStats.AttackSpeed -= 5;
                    RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen -= 150;
                    RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen -= 50;
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxMana -= 200;
                    RuntimePlayer.Instance.RuntimePlayerStats.Mana -= 200;
                    GameTimeFlowController.Instance.ForceFinishGameTimeFlowEvent("PlanetRow_GameTimeFlowEvent");
                    break;
                case BuffProfile.BuffType.MageAbility:
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxMana -= 1000 * RuntimePlayer.Instance.RuntimePlayerStats.HALvl;
                    RuntimePlayer.Instance.RuntimePlayerStats.Mana -= 1000 * RuntimePlayer.Instance.RuntimePlayerStats.HALvl;
                    break;
                case BuffProfile.BuffType.WarriorAbility:
                    RuntimePlayer.Instance.RuntimePlayerStats.Armor -= 100 * RuntimePlayer.Instance.RuntimePlayerStats.HALvl;
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth -= 200 * RuntimePlayer.Instance.RuntimePlayerStats.HALvl;
                    RuntimePlayer.Instance.RuntimePlayerStats.Health -= 200 * RuntimePlayer.Instance.RuntimePlayerStats.HALvl;
                    break;
                case BuffProfile.BuffType.Happiness:
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth -= 1000;
                    RuntimePlayer.Instance.RuntimePlayerStats.Health -= 1000;
                    RuntimePlayer.Instance.RuntimePlayerStats.Damage -= 50;
                    RuntimePlayer.Instance.RuntimePlayerStats.Armor -= 10;
                    break;
                case BuffProfile.BuffType.Magic_HealthRegen:
                    // Добавить потом
                    break;
                case BuffProfile.BuffType.Illuminati:
                    RuntimePlayer.Instance.RuntimePlayerStats.ExtraExpMultiplier -= var;
                    break;
                case BuffProfile.BuffType.Magic_Shield:
                    RuntimePlayer.Instance.RuntimePlayerStats.Armor -= 20;
                    break;
                case BuffProfile.BuffType.Magic_Stun:
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
    public void DeleteBuffAction(BuffProfile.BuffType dType)
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
    public void DeleteBuffActionOnEnemy(BuffProfile.BuffType dType)
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
    public bool isBuffOnAction(BuffProfile.BuffType cType, int character)
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
    public void LoadBuff(BuffProfile.BuffType lType, int lDuration)
    {
        var b = AllBuffs[BuffPos(lType)];
        //activeBuffs.Add(new BuffProfile()
        //{
        //    Name = b.Name,
        //    Descr = b.Description,
        //    Type = b.Type,
        //    Duration = b.Duration,
        //    MaxDuration = b.MaxDuration,
        //    buffPower = b.buffPower,
        //    isInfinity = b.isInfinity,
        //    Icon = b.Icon,
        //    iconColor = b.iconColor
        //});
        CreateBuffObject(lType, 0);
    }

    // Создаём объект баффа
    /// <summary>
    /// 0 - Игрок. 1 - противник.
    /// </summary>
    /// <param name="cType"></param>
    public void CreateBuffObject(BuffProfile.BuffType cType, int character)
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
        //for (int i = 0; i < buffsObjects.Count; i++)
        //{
        //    buffsObjects[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = activeBuffs[i].BuffIcon;
        //    buffsObjects[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = activeBuffs[i].iconColor;
        //    if (!activeBuffs[i].isInfinity)
        //        buffsObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = activeBuffs[i].Duration + "/" + activeBuffs[i].MaxDuration;
        //    else
        //        buffsObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "Беск.";
        //}
        //for (int i = 0; i < enemyBuffsObjects.Count; i++)
        //{
        //    enemyBuffsObjects[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = enemyActiveBuffs[i].BuffIcon;
        //    enemyBuffsObjects[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = enemyActiveBuffs[i].iconColor;
        //    if (!enemyActiveBuffs[i].isInfinity)
        //        enemyBuffsObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = enemyActiveBuffs[i].Duration + "/" + enemyActiveBuffs[i].MaxDuration;
        //    else
        //        enemyBuffsObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "Беск.";
        //}
    }

    // Показать описание баффа
    public void ShowDescr(BuffProfile.BuffType dType)
    {
        var SM = FindObjectOfType<SkillsManager>();
        var b = AllBuffs[BuffPos(dType)];
        if(!b.isInfinity)
            DescrBox.GetComponentInChildren<Text>().text = "Бафф: " + b.BuffName + " \n| " + SM.TransformDescr(b.BuffDescription) + " \n| Действует: " + b.MaxDuration + " ходов.";
        else
            DescrBox.GetComponentInChildren<Text>().text = "Бафф: " + b.BuffName + " \n| " + SM.TransformDescr(b.BuffDescription) + " \n| Действует: Бесконечно";
        DescrBox.SetActive(true);
    }

    // Закрыть описание баффа
    public void HideDescr()
    {
        DescrBox.SetActive(false);
    }
}
