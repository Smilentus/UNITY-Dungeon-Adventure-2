using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Creatable/GameEventSystem/New BattleGameEventProfile", fileName = "New BattleGameEventProfile")]
public class BattleGameEventProfile : BaseGameEventProfile
{
    [SerializeField]
    private List<CharacterProfile> m_characters = new List<CharacterProfile>();
    public List<CharacterProfile> Characters => m_characters;


    [Tooltip("Когда установлен флаг True наименования будут генерироваться сами")]
    [SerializeField]
    private bool m_autoGenerateNames = true;


    public BattleGameEventProfile()
    {
        this.m_eventTitle = "Битва";
        this.m_eventDescription = "Небольшое сражение.";
    }

    private void OnValidate()
    {
        // TODO: Переделать 
        if (m_autoGenerateNames)
        { 
            if (m_characters.Count == 0)
            {
                this.m_eventTitle = "Бой с тенью";
                this.m_eventDescription = $"Воздух вокруг вас содрогнулся.";
            }
            else if (m_characters.Count == 1)
            {
                this.m_eventTitle = "Дуэль";
                this.m_eventDescription = $"Вы вступили в дуэль с '{m_characters[0].Name}'";
            }
            else if (m_characters.Count < 5)
            {
                this.m_eventTitle = "Драка";
                this.m_eventDescription = $"Небольшая драка с {m_characters.Count} противниками";
            }
            else if (m_characters.Count < 15)
            {
                this.m_eventTitle = "Потасовка";
                this.m_eventDescription = $"Вы вступили в небольшую потасовку с {m_characters.Count} противниками!";
            }
            else if (m_characters.Count < 30)
            {
                this.m_eventTitle = "Крупное сражение";
                this.m_eventDescription = $"Вы начали крупное сражение с {m_characters.Count} противниками!";
            }
            else if (m_characters.Count < 50)
            {
                this.m_eventTitle = "Побоище";
                this.m_eventDescription = $"Вы вступили в смертельное побоище с {m_characters.Count} противниками!";
            }
            else if (m_characters.Count > 50)
            {
                this.m_eventTitle = "Легендарная битва";
                this.m_eventDescription = $"Ваша битва будет легендарной! Перед вами {m_characters.Count} противников...";
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
