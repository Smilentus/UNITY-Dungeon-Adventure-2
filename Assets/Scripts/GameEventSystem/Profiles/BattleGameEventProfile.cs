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


    [Tooltip("����� ���������� ���� True ������������ ����� �������������� ����")]
    [SerializeField]
    private bool m_autoGenerateNames = true;


    public BattleGameEventProfile()
    {
        this.m_eventTitle = "�����";
        this.m_eventDescription = "��������� ��������.";
    }

    private void OnValidate()
    {
        // TODO: ���������� 
        if (m_autoGenerateNames)
        { 
            if (m_characters.Count == 0)
            {
                this.m_eventTitle = "��� � �����";
                this.m_eventDescription = $"������ ������ ��� �����������.";
            }
            else if (m_characters.Count == 1)
            {
                this.m_eventTitle = "�����";
                this.m_eventDescription = $"�� �������� � ����� � '{m_characters[0].Name}'";
            }
            else if (m_characters.Count < 5)
            {
                this.m_eventTitle = "�����";
                this.m_eventDescription = $"��������� ����� � {m_characters.Count} ������������";
            }
            else if (m_characters.Count < 15)
            {
                this.m_eventTitle = "���������";
                this.m_eventDescription = $"�� �������� � ��������� ��������� � {m_characters.Count} ������������!";
            }
            else if (m_characters.Count < 30)
            {
                this.m_eventTitle = "������� ��������";
                this.m_eventDescription = $"�� ������ ������� �������� � {m_characters.Count} ������������!";
            }
            else if (m_characters.Count < 50)
            {
                this.m_eventTitle = "�������";
                this.m_eventDescription = $"�� �������� � ����������� ������� � {m_characters.Count} ������������!";
            }
            else if (m_characters.Count > 50)
            {
                this.m_eventTitle = "����������� �����";
                this.m_eventDescription = $"���� ����� ����� �����������! ����� ���� {m_characters.Count} �����������...";
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
