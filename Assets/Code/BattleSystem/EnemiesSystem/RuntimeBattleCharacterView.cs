using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code.BattleSystem.EnemiesSystem
{
    public class RuntimeBattleCharacterView : MonoBehaviour
    {
        [Header("Base References")]
        [SerializeField]
        protected RawImage _characterImage;

        [SerializeField]
        protected TMP_Text _characterName;


        [SerializeField]
        protected TMP_Text _characterHealthText;

        [SerializeField]
        protected Image _characterHealthBar;


        public virtual void DrawCharacterInfo(CharacterDrawerData drawerData)
        {
            _characterImage.texture = drawerData.RuntimeBattleCharacter.CharacterProfile.CharacterImage;
            _characterName.text = $"{drawerData.RuntimeBattleCharacter.CharacterProfile.Name}";

            double healthRatio = (drawerData.RuntimeBattleCharacter.Health / drawerData.RuntimeBattleCharacter.MaxHealth);

            _characterHealthText.text = $"{drawerData.RuntimeBattleCharacter.Health.ToString("f0")}/{drawerData.RuntimeBattleCharacter.MaxHealth.ToString("f0")}  ({(healthRatio * 100f).ToString("f2")})%";
            _characterHealthBar.fillAmount = (float)healthRatio;
        }
    }

    public class CharacterDrawerData
    {
        public RuntimeBattleCharacter RuntimeBattleCharacter;
    }
}