using Dimasyechka.Code.GlobalWindows.Controllers;
using Dimasyechka.Code.SkillsSystem.Controllers;
using Dimasyechka.Code.SkillsSystem.Core;
using Dimasyechka.Code.SkillsSystem.GlobalWindow;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Dimasyechka.Code.SkillsSystem.Interactions
{
    public class ObtainSkillInteraction : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        protected SkillProfile _skillProfile;
        public SkillProfile SkillProfile { get => _skillProfile; set => _skillProfile = value; }


        private bool _isObtained = false;
        public bool IsObtained => _isObtained;


        private PlayerSkillsController _playerSkillsController;

        [Inject]
        public void Construct(PlayerSkillsController playerSkillsController)
        {
            _playerSkillsController = playerSkillsController;
        }


        /// <summary>
        ///     Здесь проверки ещё надо, чтобы окошки выводить, проапгрейжен скилл на максимум или нет и т.п.
        /// </summary>
        public void Interaction()
        {
            if (_skillProfile == null)
            {
                Debug.Log($"Отсутствует профиль навыка!", this.gameObject);
                return;
            }

            GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(ObtainSkillGlobalWindow), new ObtainSkillGlobalWindowData()
            {
                SkillProfile = _skillProfile,
                OnApply = Apply
            });

            void Apply()
            {
                _playerSkillsController.TryObtainPlayerSkill(_skillProfile);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Interaction();
        }
    }
}
