using UnityEngine;


public class PlayerMagicController : MonoBehaviour
{
    private static PlayerMagicController instance;
    public static PlayerMagicController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerMagicController>();
            }

            return instance;
        }
    }


    [SerializeField]
    private MagicContainer m_playerMagicContainer;
    public MagicContainer PlayerMagicContainer => m_playerMagicContainer;


    private void Start()
    {
        m_playerMagicContainer.onMagicProfileAdded += OnMagicAdded;
        m_playerMagicContainer.onMagicProfileRemoved += OnMagicRemoved;
    }


    private void OnMagicAdded(BaseMagicProfile magicProfile)
    {
        
    }

    private void OnMagicRemoved(BaseMagicProfile magicProfile)
    {
        
    }
}