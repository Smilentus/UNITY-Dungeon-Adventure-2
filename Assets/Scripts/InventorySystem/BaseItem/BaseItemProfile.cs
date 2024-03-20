using UnityEngine;

[CreateAssetMenu(fileName = "BaseItemProfile", menuName = "Creatable/New BaseItemProfile")]
public class BaseItemProfile : ScriptableObject
{
    public enum BaseItemRarity { None = 0, Basic = 1, Common = 2, Rare = 3, Epic = 4, Legendary = 5, Unseenable = 6 };


    [SerializeField]
    [TextArea(3, 5)]
    protected string m_itemName;
    public string ItemName => m_itemName;


    [SerializeField]
    protected BaseItemRarity m_Rarity;
    public BaseItemRarity Rarity => m_Rarity;


    [SerializeField]
    protected Sprite m_itemSprite;
    public Sprite ItemSprite => m_itemSprite;


    [SerializeField]
    [TextArea(5, 10)]
    protected string m_itemDescription;
    public string ItemDescription => m_itemDescription;


    [SerializeField]
    protected int m_maximumStack;
    public int MaximumStack => m_maximumStack;
}
