using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Creatable/BattleActionSystem/BattleActionProfile", fileName = "BattleActionProfile_")]
public class BattleActionProfile : ScriptableObject
{
    [SerializeField]
    private SerializableMonoScript<IBattleActionExecuter> m_actionExecuter;
    public SerializableMonoScript<IBattleActionExecuter> ActionExecuter => m_actionExecuter;


    [TextArea(3, 5)]
    [SerializeField]
    private string m_actionTitle;
    public string ActionTitle => m_actionTitle;


    [TextArea(5, 10)]
    [SerializeField]
    private string m_actionDescription;
    public string ActionDescription => m_actionDescription;


    [Tooltip("Очки действий, которые необходимо затратить для того, чтобы применить это действие")]
    [SerializeField]
    private int m_spendableActions;
    /// <summary>
    ///     Очки действий, которые необходимо затратить для того, чтобы применить это действие
    /// </summary>
    public int SpendableActions => m_spendableActions;
}
