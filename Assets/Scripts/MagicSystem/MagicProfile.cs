using UnityEngine;


[CreateAssetMenu(menuName = "Creatable/New MagicProfile", fileName = "MagicProfile")]
public class MagicProfile : ScriptableObject
{
    [SerializeField]
    private RuntimeMagicObject m_MagicObjectPrefab;
    public RuntimeMagicObject MagicObject => m_MagicObjectPrefab;


    [SerializeField]
    private string m_MagicName;
    public string MagicName => m_MagicName;


    [SerializeField]
    private string m_MagicDescription;
    public string MagicDescription => m_MagicDescription;


    [Tooltip("Дефолтная стоимость магии")]
    [SerializeField]
    private int m_DefaultManaPointsCost;
    public int DefaultManaPointsCost => m_DefaultManaPointsCost;


    [SerializeField]
    private int m_DefaultCooldownHours;
    public int DefaultCooldownHours => m_DefaultCooldownHours;
}
