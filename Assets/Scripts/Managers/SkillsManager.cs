using UnityEngine;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{
    public enum SkillType
    {
        None,
        MaxHealth,
        MaxMana,
        HealthRegen,
        ManaRegen,
        Damage,
        Armor,
        AttackSpeed,
        Luck,
        AntiHole,
        ExtraExp,
        ExtraMoney,
        ChanceNotToDelete,
        ChanceToCraftTwice,
        DodgeChance,
        CriticalChance,
        CriticalMulty,
        // Магические навыки
        Magic_Lighting,
        Magic_Poison,
        Magic_Shield, 
        Magic_Fireball,
        Magic_Stun,
        Magic_SlamStun,
        Magic_ZapperLighting,
        ExtraInvSlot,
        ExtraRuneSlot,
    }

    public SkillType currentSkill;

    // Ссылка на MagicManager
    private MagicManager MM;

    [Header("Все скиллы")]
    public Skill[] AllSkills;

    [Header("Кнопки скиллов")]
    public GameObject[] SkillsButtons;

    [Header("Панель улучшения")]
    public GameObject upgradePanel;

    [Header("Текст очков навыков")]
    public Text SkillScoreText;

    public GameObject acceptButton;

    private void Awake()
    {
        MM = FindObjectOfType<MagicManager>();
    }

    private void Start()
    {
        currentSkill = SkillType.None;
        InvokeRepeating("UpdateTexts", 1, 0.2f);
    }

    // Работа скиллов
    public void SkillsAction()
    {
        if(RuntimePlayer.Instance.RuntimePlayerStats.Health < RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth)
            RuntimePlayer.Instance.RuntimePlayerStats.Health += RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen;
        if(RuntimePlayer.Instance.RuntimePlayerStats.Mana < RuntimePlayer.Instance.RuntimePlayerStats.MaxMana)
            RuntimePlayer.Instance.RuntimePlayerStats.Mana += RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen;
    }

    // Обнуление скиллов
    public void NullSkills()
    {
        foreach (var skill in AllSkills)
        {
            skill.Cost = skill.DefaultCost;
            skill.Lvl = 0;
            skill.currentVariable = skill.defaultVariable;
        }
    }

    // Открытие или закрытие панели скиллов
    public void ShowHideSkillPanel()
    {
        FindObjectOfType<PanelsManager>().CloseAllPlayerPanels(1);
        FindObjectOfType<PanelsManager>().OpenHidePlayerPanel(1, !FindObjectOfType<PanelsManager>().PlayerPanels[1].activeSelf);

        UpdateTexts();
    }

    // Поиск скилла в общем списке
    public int SkillPos(SkillType fType)
    {
        for(int i = 0; i < AllSkills.Length; i++)
        {
            if (AllSkills[i].Type == fType)
                return i;
        }
        return 0;
    }

    // Обновление скилла и ссылка на улучшение
    public void AddSkill()
    {
        int pos = SkillPos(currentSkill);

        if(RuntimePlayer.Instance.RuntimePlayerStats.SkillPoints >= AllSkills[pos].Cost)
        {
            if (AllSkills[pos].isInfinity || AllSkills[pos].Lvl < AllSkills[pos].MaxLvl)
            {
                RuntimePlayer.Instance.RuntimePlayerStats.SkillPoints -= AllSkills[pos].Cost;

                UpgradeSkill(currentSkill);

                AllSkills[pos].Lvl++;

                if (AllSkills[pos].Lvl % AllSkills[pos].CostChangeLvl == 0)
                {
                    AllSkills[pos].Cost += AllSkills[pos].CostMulty;
                    AllSkills[pos].currentVariable += AllSkills[pos].changableVariable;
                }
            }
            else
            {
                FindObjectOfType<GameController>().ShowMessageText("Достигнут максимальный уровень навыка!");
                acceptButton.SetActive(false);
            }
        }
        else
        {
            FindObjectOfType<GameController>().ShowMessageText("Недостаточно очков навыков для улучшения! \nНеобходимо минимум " + AllSkills[pos].Cost.ToString() + " ОН");
            HideSkillDescr();
        }

        UpdateDescr(currentSkill);
        UpdateTexts();
    }

    // Улучшение скилла
    private void UpgradeSkill(SkillType uType)
    {
        var var = AllSkills[SkillPos(uType)].currentVariable;
        if (uType != SkillType.None)
            switch (uType)
            {
                case SkillType.HealthRegen:
                    RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen += (int)var;
                    break;
                case SkillType.MaxHealth:
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxHealth += (int)var;
                    break;
                case SkillType.MaxMana:
                    RuntimePlayer.Instance.RuntimePlayerStats.MaxMana += (int)var;
                    break;
                case SkillType.ManaRegen:
                    RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen += (int)var;
                    break;
                case SkillType.Luck:
                    RuntimePlayer.Instance.RuntimePlayerStats.Luck += var;
                    break;
                case SkillType.Damage:
                    RuntimePlayer.Instance.RuntimePlayerStats.Damage += (int)var;
                    break;
                case SkillType.AttackSpeed:
                    RuntimePlayer.Instance.RuntimePlayerStats.AttackSpeed += (int)var;
                    break;
                case SkillType.Armor:
                    RuntimePlayer.Instance.RuntimePlayerStats.Armor += (int)var;
                    break;
                case SkillType.AntiHole:
                    RuntimePlayer.Instance.RuntimePlayerStats.AntiHole = true;
                    break;
                case SkillType.ExtraExp:
                    RuntimePlayer.Instance.RuntimePlayerStats.ExtraExpMultiplier += (int)var;
                    break;
                case SkillType.ExtraMoney:
                    RuntimePlayer.Instance.RuntimePlayerStats.ExtraMoneyMultiplier += (int)var;
                    break;
                case SkillType.ChanceNotToDelete:
                    RuntimePlayer.Instance.RuntimePlayerStats.ChanceNotToDelete += var;
                    break;
                case SkillType.ChanceToCraftTwice:
                    RuntimePlayer.Instance.RuntimePlayerStats.ChanceToCraftTwice += var;
                    break;
                case SkillType.CriticalChance:
                    RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeChance += var;
                    break;
                case SkillType.CriticalMulty:
                    RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeDamageMultiplier += var;
                    break;
                case SkillType.DodgeChance:
                    RuntimePlayer.Instance.RuntimePlayerStats.DodgeChance += var;
                    break;
                case SkillType.Magic_Fireball:
                    if (AllSkills[SkillPos(uType)].Lvl == 0)
                    {
                        MM.AddMagicToPlayer(MagicManager.MagicType.Fireball);
                    }
                    else
                    {
                        Debug.Log("Навык: " + MM.playerMagic[MM.FindMagicPos(MagicManager.MagicType.Fireball)].Name);
                        MM.playerMagic[MM.FindMagicPos(MagicManager.MagicType.Fireball)].actionVar += var;
                        // Каждый 10-ый уровень -1 ход к КД
                        if (AllSkills[SkillPos(uType)].Lvl % 10 == 0)
                            MM.playerMagic[MM.FindMagicPos(MagicManager.MagicType.Fireball)].cooldownTimeMax--;
                    }
                    break;
                case SkillType.Magic_Lighting:
                    if (AllSkills[SkillPos(uType)].Lvl == 0)
                    {
                        MM.AddMagicToPlayer(MagicManager.MagicType.Lighting);
                    }
                    else
                    {
                        MM.playerMagic[MM.FindMagicPos(MagicManager.MagicType.Lighting)].actionVar += var;
                        // Каждый 5-ый уровень -1 ход к КД
                        if (AllSkills[SkillPos(uType)].Lvl % 6 == 0)
                            MM.playerMagic[MM.FindMagicPos(MagicManager.MagicType.Lighting)].cooldownTimeMax--;
                    }
                    break;
                case SkillType.Magic_ZapperLighting:
                    if (AllSkills[SkillPos(uType)].Lvl == 0)
                    {
                        MM.AddMagicToPlayer(MagicManager.MagicType.ZapperLighting);
                    }
                    else
                    {
                        MM.playerMagic[MM.FindMagicPos(MagicManager.MagicType.ZapperLighting)].actionVar += var;
                        // Каждый 5-ый уровень -1 ход к КД
                        if (AllSkills[SkillPos(uType)].Lvl % 5 == 0)
                            MM.playerMagic[MM.FindMagicPos(MagicManager.MagicType.ZapperLighting)].cooldownTimeMax--;
                    }
                    break;
                case SkillType.Magic_Poison:
                    if(AllSkills[SkillPos(uType)].Lvl == 0)
                    {
                        MM.AddMagicToPlayer(MagicManager.MagicType.Poison);
                    }
                    else
                    {
                        MM.playerMagic[MM.FindMagicPos(MagicManager.MagicType.Poison)].actionVar += var;
                        // Каждый 2-й уровень -1 ход к КД
                        if (AllSkills[SkillPos(uType)].Lvl % 2 == 0)
                            MM.playerMagic[MM.FindMagicPos(MagicManager.MagicType.Poison)].cooldownTimeMax--;
                    }
                    break;
                case SkillType.Magic_Shield:
                    if (AllSkills[SkillPos(uType)].Lvl == 0)
                    {
                        MM.AddMagicToPlayer(MagicManager.MagicType.Shield);
                    }
                    else
                    {
                        MM.playerMagic[MM.FindMagicPos(MagicManager.MagicType.Shield)].actionVar += var;
                        // Каждый 3-й уровень -1 ход к КД
                        if (AllSkills[SkillPos(uType)].Lvl % 3 == 0)
                            MM.playerMagic[MM.FindMagicPos(MagicManager.MagicType.Shield)].cooldownTimeMax--;
                    }
                    break;
                case SkillType.Magic_Stun:
                    if (AllSkills[SkillPos(uType)].Lvl == 0)
                    {
                        MM.AddMagicToPlayer(MagicManager.MagicType.Stun);
                    }
                    else
                    {
                        // Каждый 5-ый уровень -1 ход к КД
                        if (AllSkills[SkillPos(uType)].Lvl % 2 == 0)
                            MM.playerMagic[MM.FindMagicPos(MagicManager.MagicType.Stun)].cooldownTimeMax--;
                    }
                    break;
                case SkillType.Magic_SlamStun:
                    break;
                case SkillType.ExtraInvSlot:
                    FindObjectOfType<Inventory>().OpenInvSlot(RuntimePlayer.Instance.RuntimePlayerStats.openedInvCases);
                    RuntimePlayer.Instance.RuntimePlayerStats.openedInvCases += (int)var;
                    break;
                case SkillType.ExtraRuneSlot:
                    FindObjectOfType<Inventory>().OpenRuneSlot(FindObjectOfType<Inventory>().inventory.Length - 8 + RuntimePlayer.Instance.RuntimePlayerStats.openedRuneCases);
                    RuntimePlayer.Instance.RuntimePlayerStats.openedRuneCases += (int)var;
                    break;
            }
    }

    // Возвращает нужную строку из любого подскрипта
    public string TransformDescr(string stringToFormat)
    {
        string newString = stringToFormat;

        // Если содержит название магии, то преобразуем
        if(newString.Contains("{magic:fireball}"))
        {
            string formatted = MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Fireball)].actionVar.ToString();
            newString = newString.Replace("{magic:fireball}", formatted);
            Debug.Log(formatted);
            Debug.Log(newString);
        }
        if (newString.Contains("{magic:fireballcd}"))
        {
            string formatted = MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Fireball)].cooldownTimeMax.ToString();
            newString = newString.Replace("{magic:fireballcd}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{magic:lighting}"))
        {
            string formatted = MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Lighting)].actionVar.ToString();
            newString = newString.Replace("{magic:lighting}", formatted);
        }
        if (newString.Contains("{magic:lightingcd}"))
        {
            string formatted = MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Lighting)].cooldownTimeMax.ToString();
            newString = newString.Replace("{magic:lightingcd}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{magic:zapperlighting}"))
        {
            string formatted = MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.ZapperLighting)].actionVar.ToString();
            newString = newString.Replace("{magic:zapperlighting}", formatted);
        }
        if (newString.Contains("{magic:zapperlightingcd}"))
        {
            string formatted = MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.ZapperLighting)].cooldownTimeMax.ToString();
            newString = newString.Replace("{magic:zapperlightingcd}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{skill:extraexp}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.ExtraExp)].currentVariable).ToString();
            newString = newString.Replace("{skill:extraexp}", formatted);
        }
        if (newString.Contains("{skill:extramoney}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.ExtraMoney)].currentVariable).ToString();
            newString = newString.Replace("{skill:extramoney}", formatted);
        }
        // ------------------------------------
        if(newString.Contains("{skill:armor}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.Armor)].currentVariable).ToString();
            newString = newString.Replace("{skill:armor}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{skill:maxmana}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.MaxMana)].currentVariable).ToString();
            newString = newString.Replace("{skill:maxmana}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{skill:maxhealth}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.MaxHealth)].currentVariable).ToString();
            newString = newString.Replace("{skill:maxhealth}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{skill:speed}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.AttackSpeed)].currentVariable).ToString();
            newString = newString.Replace("{skill:speed}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{skill:critchance}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.CriticalChance)].currentVariable).ToString();
            newString = newString.Replace("{skill:critchance}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{skill:criticalstrike}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.CriticalMulty)].currentVariable * 100).ToString();
            newString = newString.Replace("{skill:criticalstrike}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{skill:damage}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.Damage)].currentVariable).ToString();
            newString = newString.Replace("{skill:damage}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{skill:dodge}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.DodgeChance)].currentVariable).ToString();
            newString = newString.Replace("{skill:dodge}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{skill:regen}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.HealthRegen)].currentVariable).ToString();
            newString = newString.Replace("{skill:regen}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{skill:manaregen}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.ManaRegen)].currentVariable).ToString();
            newString = newString.Replace("{skill:manaregen}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{skill:luck}"))
        {
            string formatted = (AllSkills[SkillPos(SkillType.Luck)].currentVariable).ToString();
            newString = newString.Replace("{skill:luck}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{magic:stun}"))
        {
            string formatted = MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Stun)].actionVar.ToString();
            newString = newString.Replace("{magic:stun}", formatted);
        }
        if (newString.Contains("{magic:stuncd}"))
        {
            string formatted = MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Stun)].cooldownTimeMax.ToString();
            newString = newString.Replace("{magic:stuncd}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{magic:shield}"))
        {
            string formatted = MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Shield)].actionVar.ToString();
            newString = newString.Replace("{magic:shield}", formatted);
        }
        if (newString.Contains("{magic:shieldcd}"))
        {
            string formatted = MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Shield)].cooldownTimeMax.ToString();
            newString = newString.Replace("{magic:shieldcd}", formatted);
        }
        // ------------------------------------
        if (newString.Contains("{magic:poison}"))
        {
            string formatted = MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Poison)].actionVar.ToString();
            newString = newString.Replace("{magic:poison}", formatted);
        }
        if (newString.Contains("{magic:poisoncd}"))
        {
            string formatted = MM.allMagicInGame[MM.FindMagicPosInAllMagic(MagicManager.MagicType.Poison)].cooldownTimeMax.ToString();
            newString = newString.Replace("{magic:poisoncd}", formatted);
        }

        return newString;
    }

    // Показать описание скилла
    public void ShowSkillDescr(SkillType dType)
    {
        // Если уровень больше нужного - показываем инфу
        if (RuntimePlayer.Instance.RuntimePlayerStats.Lvl >= AllSkills[SkillPos(dType)].neededLvl)
        {
            upgradePanel.SetActive(true);

            // Если уровень скилла максимальный - убираем кнопку
            if (AllSkills[SkillPos(dType)].Lvl == AllSkills[SkillPos(dType)].MaxLvl && AllSkills[SkillPos(dType)].MaxLvl > 0)
                acceptButton.SetActive(false);
            else
                acceptButton.SetActive(true);

            UpdateDescr(dType);

            currentSkill = dType;
        }
        else
        {
            FindObjectOfType<GameController>().ShowMessageText(("Ваш уровень слишком мал для улучшения! \nНеобходим минимальный уровень: " + AllSkills[SkillPos(dType)].neededLvl.ToString()));
            acceptButton.SetActive(false);
        }
    }

    // Скрыть описание скилла
    public void HideSkillDescr()
    {
        upgradePanel.SetActive(false);
        currentSkill = SkillType.None;
    }

    // Обновление текста описания
    public void UpdateDescr(SkillType dType)
    {
        // Если уровень скилла максимальный - убираем кнопку
        if (AllSkills[SkillPos(dType)].Lvl == AllSkills[SkillPos(dType)].MaxLvl && AllSkills[SkillPos(dType)].MaxLvl > 0)
            acceptButton.SetActive(false);
        else
            acceptButton.SetActive(true);

        string text = "Навык: " + AllSkills[SkillPos(dType)].Name + "\nОписание: "
        + AllSkills[SkillPos(dType)].Descr + "\nЦена: "
        + AllSkills[SkillPos(dType)].Cost + " ОН";

        if (AllSkills[SkillPos(dType)].isInfinity)
            text += "\nУровень навыка: " + AllSkills[SkillPos(dType)].Lvl;
        else
            text += "\nУровень навыка: " + AllSkills[SkillPos(dType)].Lvl + "/" + AllSkills[SkillPos(dType)].MaxLvl;

        int neededLvl = AllSkills[SkillPos(dType)].neededLvl;

        if (RuntimePlayer.Instance.RuntimePlayerStats.Lvl < neededLvl)
            text += ("\nНужен " + neededLvl + " для улучшения!");

        text = TransformDescr(text);

        upgradePanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = text;
    }

    // Обновление текстов
    public void UpdateTexts()
    {

        foreach(GameObject slot in SkillsButtons)
        {
            slot.GetComponentsInChildren<RawImage>()[1].texture = AllSkills[SkillPos(slot.GetComponent<SkillButton>().Type)].Icon;
            if(AllSkills[SkillPos(slot.GetComponent<SkillButton>().Type)].isInfinity)
                slot.GetComponentInChildren<Text>().text = AllSkills[SkillPos(slot.GetComponent<SkillButton>().Type)].Lvl.ToString();
            else
                slot.GetComponentInChildren<Text>().text = AllSkills[SkillPos(slot.GetComponent<SkillButton>().Type)].Lvl + "/" +
                     AllSkills[SkillPos(slot.GetComponent<SkillButton>().Type)].MaxLvl;
        }
    }
}