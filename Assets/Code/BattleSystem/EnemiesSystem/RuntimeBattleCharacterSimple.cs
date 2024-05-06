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
                // Надо какой-то общий метод, который будет наносить урон такому-то игроку с такими-то дебафами и такими-то атаками
                _runtimePlayer.DealDamage(CharacterProfile.Damage, false);
            }
        }
    }
}
