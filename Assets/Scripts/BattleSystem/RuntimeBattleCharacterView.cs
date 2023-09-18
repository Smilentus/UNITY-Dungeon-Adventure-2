using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeBattleCharacterView : MonoBehaviour
{
    [SerializeField]
    protected RawImage m_characterImage;

    [SerializeField]
    protected TMP_Text m_characterName;


    public virtual void DrawCharacterInfo(CharacterDrawerData drawerData)
    {
        m_characterImage.texture = drawerData.runtimeBattleCharacter.characterProfile.CharacterImage;
        m_characterName.text = $"{drawerData.runtimeBattleCharacter.characterProfile.Name}";
    }
}

public class CharacterDrawerData
{
    public RuntimeBattleCharacter runtimeBattleCharacter;
}