using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagicManager : MonoBehaviour
{
    // Магия
    public enum MagicType
    {
        Lighting,
        Poison,
        Shield,
        Fireball,
        Stun,
        ZapperLighting,
        SlamStun,
    }

    // Ссылка на BattleHelper
    private BattleController BH;

    [Header("Вся магия в игре")]
    public Magic[] allMagicInGame;
    [Header("Магия игрока")]
    public List<Magic> playerMagic = new List<Magic>();

    [Header("Родитель для спавна магии")]
    public Transform magicSpawnParent;
    [Header("Префаб слота магии")]
    public GameObject magicSlotPrefab;

    [Header("Объект спавна магии")]
    public GameObject magicSpawnObject;

    private void Awake()
    {
        BH = FindObjectOfType<BattleController>();
    }

    // Поиск магии в списке игрока
    public int FindMagicPos(MagicType fType)
    {
        for (int i = 0; i < playerMagic.Count; i++)
        {
            if (playerMagic[i].magicType == fType)
                return i;
        }
        return 0;
    }
    public int FindMagicPosInAllMagic(MagicType fType)
    {
        for (int i = 0; i < allMagicInGame.Length; i++)
        {
            if (allMagicInGame[i].magicType == fType)
                return i;
        }
        return 0;
    }

    // Загрузка магии в битву
    public void LoadMagicInBattle()
    {
        // Очищаем уже установленную магию
        for (int i = 0; i < magicSpawnParent.childCount; i++)
        {
            Destroy(magicSpawnParent.GetChild(i).gameObject);
        }

        if (playerMagic.Count > 0)
            magicSpawnObject.SetActive(true);
        else
            magicSpawnObject.SetActive(false);

        // Устанавливаем новые слоты магии
        for (int i = 0; i < playerMagic.Count; i++)
        {
            playerMagic[i].currentCooldown = 0;
            playerMagic[i].canUseMagic = true;

            // Создаём новую магию
            GameObject magicSlot = Instantiate(magicSlotPrefab, magicSpawnParent);

            magicSlot.GetComponent<MagicSlotButton>().currentType = playerMagic[i].magicType;

            // УСТАНАВЛИВАЕМ ЗНАЧЕНИЯ, КАРТИНКИ И ПРОЧЕЕ
            magicSlot.transform.GetChild(0).GetComponent<RawImage>().texture = playerMagic[i].MagicIcon;
            magicSlot.transform.GetChild(1).GetComponent<Text>().text = playerMagic[i].Name;
        }
    }

    // Обнуление всей магии
    public void NullAllMagic()
    {
        playerMagic.Clear();

        for(int i = 0; i < allMagicInGame.Length; i++)
        {
            allMagicInGame[i].actionVar = allMagicInGame[i].actionVarDefault;
            allMagicInGame[i].cooldownTimeMax = allMagicInGame[i].cooldownTimeMaxDefault;
        }

        magicSpawnObject.SetActive(false);
    }

    // Добавление магии игроку
    public void AddMagicToPlayer(MagicType cType)
    {
        int pos = FindMagicPosInAllMagic(cType);

        playerMagic.Add(allMagicInGame[pos]);
        playerMagic[playerMagic.Count - 1].canUseMagic = true;

        magicSpawnObject.SetActive(true);
    }

    // Использование магии
    public void UseMagic(MagicType usableMagic)
    {
        //if (playerMagic[FindMagicPos(usableMagic)].canUseMagic)
        //{
        //    playerMagic[FindMagicPos(usableMagic)].currentCooldown = playerMagic[FindMagicPos(usableMagic)].cooldownTimeMax;

        //    switch (usableMagic)
        //    {
        //        case MagicType.Fireball:
        //            // Наносим урон от фаербола первому противнику в ряду
        //            Debug.Log("Фаерболл");
        //            Debug.Log("Наносим " + playerMagic[FindMagicPos(usableMagic)].actionVar + " ед  урона противнику " + BH.allEnemies[BH.allEnemies.Count - 1].Name);
        //            BH.allEnemies[BH.allEnemies.Count - 1].Health -= playerMagic[FindMagicPos(usableMagic)].actionVar;
        //            break;
        //        case MagicType.Lighting:
        //            // Наносим урон всем противникам в ряду
        //            for (int i = 0; i < BH.allEnemies.Count; i++)
        //            {
        //                BH.allEnemies[i].Health -= playerMagic[FindMagicPos(usableMagic)].actionVar;
        //            }
        //            break;
        //        case MagicType.Poison:
        //            // Каждый ход наносится урон противнику
        //            FindObjectOfType<BuffManager>().SetBuffToEnemy(Buff.BuffType.Magic_Poison);
        //            break;
        //        case MagicType.Shield:
        //            // Магический щит, защищающий от физ. и маг. урона
        //            FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.Magic_Shield);
        //            break;
        //        case MagicType.Stun:
        //            // Противник не может ходить Х ходов
        //            FindObjectOfType<BuffManager>().SetBuffToEnemy(Buff.BuffType.Magic_Stun);
        //            break;
        //        case MagicType.SlamStun:
        //            // Наносим урон всем противникам в ряду и станим их
        //            for(int i = 0; i < FindObjectOfType<BattleController>().allEnemies.Count; i++)
        //            {
        //                FindObjectOfType<BuffManager>().SetBuffToEnemy(Buff.BuffType.Magic_Stun);
        //            }
        //            break;
        //        case MagicType.ZapperLighting:
        //            float percentage = 1f;
        //            // Наносим урон всем противникам в ряду, каждый последующий получает на 10 процентов меньше
        //            for (int i = 0; i < BH.allEnemies.Count; i++)
        //            {
        //                if (percentage > 0)
        //                {
        //                    BH.allEnemies[i].Health -= playerMagic[FindMagicPos(usableMagic)].actionVar * percentage;
        //                    percentage -= 0.1f;
        //                }
        //                else
        //                    break;
        //            }
        //            break;
        //    }

        //    SetCooldownUI(FindMagicPos(usableMagic));
        //    UIScript.Instance.UpdateEnemyUIText();
        //    BH.CheckEnemyDeath();

        //    playerMagic[FindMagicPos(usableMagic)].canUseMagic = false;
        //}
    }

    // Установка кулдауна 
    public void SetCooldownUI(int i)
    {
        if(playerMagic[i].canUseMagic)
        {
            // Если КД равен максимуму, просто обновляем
            if (playerMagic[i].currentCooldown == playerMagic[i].cooldownTimeMax)
            {
                playerMagic[i].canUseMagic = false;
                magicSpawnParent.GetChild(i).GetChild(2).gameObject.SetActive(true);
                magicSpawnParent.GetChild(i).GetChild(3).gameObject.SetActive(true);
                // Текст кулдауна
                magicSpawnParent.GetChild(i).GetChild(3).GetChild(0).GetComponent<Text>().text = playerMagic[i].currentCooldown.ToString();

                playerMagic[i].currentCooldown--;
            }
        }
    }

    // Обновление перезарядки
    public void UpdateMagicCooldown()
    {
        for (int i = 0; i < playerMagic.Count; i++)
        {
            if (playerMagic[i].currentCooldown == 0)
            {
                playerMagic[i].canUseMagic = true;
                magicSpawnParent.GetChild(i).GetChild(2).GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                magicSpawnParent.GetChild(i).GetChild(3).GetComponent<Image>().fillAmount = 1;
                magicSpawnParent.GetChild(i).GetChild(2).gameObject.SetActive(false);
                magicSpawnParent.GetChild(i).GetChild(3).gameObject.SetActive(false);
            }

            // Если кулдаун меньше максимума и больше нуля
            if (playerMagic[i].currentCooldown < playerMagic[i].cooldownTimeMax && playerMagic[i].currentCooldown > 0)
            {
                // Текст кулдауна
                magicSpawnParent.GetChild(i).GetChild(3).GetChild(0).GetComponent<Text>().text = playerMagic[i].currentCooldown.ToString();

                // Обновление картинок
                float timer = 100f / (float)playerMagic[i].cooldownTimeMax / 100f;
                // Осветление картинки на фоне
                magicSpawnParent.GetChild(i).GetChild(2).GetComponent<RawImage>().color -= new Color(1, 1, 1, timer);
                magicSpawnParent.GetChild(i).GetChild(3).GetComponent<Image>().fillAmount -= timer;

                playerMagic[i].currentCooldown--;
            }
        }
    }
}