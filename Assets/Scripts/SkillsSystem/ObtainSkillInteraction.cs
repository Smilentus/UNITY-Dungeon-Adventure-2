using UnityEngine;
using UnityEngine.EventSystems;

public class ObtainSkillInteraction : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    protected SkillProfile m_skillProfile;
    public SkillProfile SkillProfile { get => m_skillProfile; set => m_skillProfile = value; }


    private bool isObtained = false;
    public bool IsObtained => isObtained;


    /// <summary>
    ///     ����� �������� ��� ����, ����� ������ ��������, ������������ ����� �� �������� ��� ��� � �.�.
    /// </summary>
    public void Interaction()
    {
        if (m_skillProfile == null)
        {
            Debug.Log($"����������� ������� ������!", this.gameObject);
            return;
        }

        GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(ObtainSkillGlobalWindow), new ObtainSkillGlobalWindowData()
        {
            Profile = m_skillProfile,
            OnApply = Apply
        });

        void Apply()
        {
            PlayerSkillsController.instance.TryObtainPlayerSkill(m_skillProfile);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Interaction();
    }
}
