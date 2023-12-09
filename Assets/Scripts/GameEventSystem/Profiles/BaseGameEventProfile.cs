using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Creatable/GameEventSystem/New GameEventProfile", fileName = "GameEventProfile_")]
[System.Serializable]
public class BaseGameEventProfile : ScriptableObject
{
    [TextArea(2, 5)]
    [SerializeField]
    protected string m_eventTitle;
    /// <summary>
    ///     Наименование игрового ивента
    /// </summary>
    public string EventTitle => m_eventTitle;


    [TextArea(5, 10)]
    [SerializeField]
    protected string m_eventDescription;
    /// <summary>
    ///     Описание игрового ивента
    /// </summary>
    public string EventDescription => m_eventDescription;


    [Tooltip("Когда установлен флаг True наименования будут генерироваться сами")]
    [SerializeField]
    private bool m_autoGenerateNames = true;


    private void OnValidate()
    {
        if (m_autoGenerateNames)
        {
            OnAutoGenerateNames();

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }

    protected virtual void OnAutoGenerateNames() { }
}
