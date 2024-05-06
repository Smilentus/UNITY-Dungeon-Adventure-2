using System.Collections.Generic;
using Dimasyechka.Code.BattleSystem;
using UnityEngine;

namespace Dimasyechka.Code.GameEventSystem.Profiles
{
    [CreateAssetMenu(menuName = "Creatable/GameEventSystem/New BattleGameEventProfile", fileName = "New BattleGameEventProfile")]
    public class BattleGameEventProfile : BaseGameEventProfile
    {
        [SerializeField]
        private List<CharacterProfile> m_characters = new List<CharacterProfile>();
        public List<CharacterProfile> Characters => m_characters;


        public BattleGameEventProfile()
        {
            this.m_eventTitle = "�����";
            this.m_eventDescription = "��������� ��������.";
        }


        protected override void OnAutoGenerateNames()
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
        }
    }
}
