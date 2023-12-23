using UnityEngine;


[CreateAssetMenu(menuName = "Creatable/Magic System/New MagicProfile", fileName = "MagicProfile_")]
public class MagicProfile : ScriptableObject, IBattleActionInteraction
{
    [SerializeField]
    private BaseBattleInteractionView m_actionProfileViewPrefab;
    public BaseBattleInteractionView ActionProfileViewPrefab => m_actionProfileViewPrefab;


    [SerializeField]
    private SerializableMonoScript<IBattleActionExecuter> m_actionExecuter;
    public SerializableMonoScript<IBattleActionExecuter> ActionExecuter => m_actionExecuter;


    [TextArea(3, 5)]
    [SerializeField]
    private string m_MagicName;
    public string InteractionTitle => m_MagicName;


    [TextArea(5, 10)]
    [SerializeField]
    private string m_MagicDescription;
    public string MagicDescription => m_MagicDescription;


    [Tooltip("��������� ��������� �����")]
    [SerializeField]
    private int m_DefaultManaPointsCost;
    public int DefaultManaPointsCost => m_DefaultManaPointsCost;


    [SerializeField]
    private int m_DefaultCooldownHours;
    public int DefaultCooldownHours => m_DefaultCooldownHours;
}
