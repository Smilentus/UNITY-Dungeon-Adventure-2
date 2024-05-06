namespace Dimasyechka.Code.BattleSystem.EnemiesSystem
{
    public class RuntimeBattleCharacterSimple : RuntimeBattleCharacter
    {
        public override void ProcessCharacterActions()
        {
            BaseAttack();

            base.ProcessCharacterActions();
        }


        private void BaseAttack()
        {
            for (int i = 0; i < ActionPoints; i++)
            {
                // ���� �����-�� ����� �����, ������� ����� �������� ���� ������-�� ������ � ������-�� �������� � ������-�� �������
                _runtimePlayer.DealDamage(CharacterProfile.Damage, false);
            }
        }
    }
}
