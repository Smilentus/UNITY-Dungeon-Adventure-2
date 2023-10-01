using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeBattleCharacterView : MonoBehaviour
{
    [Header("Base References")]
    [SerializeField]
    protected RawImage m_characterImage;

    [SerializeField]
    protected TMP_Text m_characterName;


    [SerializeField]
    protected TMP_Text m_characterHealthText;

    [SerializeField]
    protected Image m_characterHealthBar;


    public virtual void DrawCharacterInfo(CharacterDrawerData drawerData)
    {
        m_characterImage.texture = drawerData.runtimeBattleCharacter.characterProfile.CharacterImage;
        m_characterName.text = $"{drawerData.runtimeBattleCharacter.characterProfile.Name}";

        double healthRatio = (drawerData.runtimeBattleCharacter.Health / drawerData.runtimeBattleCharacter.MaxHealth);

        m_characterHealthText.text = $"{drawerData.runtimeBattleCharacter.Health}/{drawerData.runtimeBattleCharacter.MaxHealth}  ({(healthRatio * 100).ToString("f2")})%";
        m_characterHealthBar.fillAmount = (float)healthRatio;
    }
}

public class CharacterDrawerData
{
    public RuntimeBattleCharacter runtimeBattleCharacter;
}