using UnityEngine;


public class MagicContainerPresenter : MonoBehaviour
{
    [SerializeField]
    private MagicContainer m_activeMagicContainer;
    public MagicContainer ActiveMagicContainer => m_activeMagicContainer;


    [SerializeField]
    private RuntimeMagicObjectView m_magicObjectViewPrefab;

    [SerializeField]
    private Transform m_contentParent;


    private void Start()
    {
        if (m_activeMagicContainer != null)
        {
            InitializeMagicContainer(m_activeMagicContainer);
        }
    }


    public void SetMagicContainer(MagicContainer _container)
    {
        InitializeMagicContainer(_container);
    }

    private void InitializeMagicContainer(MagicContainer _container)
    {
        if (m_activeMagicContainer != null)
        {
            m_activeMagicContainer.onMagicProfileAdded -= UpdateMagicContainerVisuals;
            m_activeMagicContainer.onMagicProfileRemoved -= UpdateMagicContainerVisuals;
            m_activeMagicContainer.onMagicObjectsUpdated -= UpdateMagicContainerVisuals;

            Destroy(m_activeMagicContainer.gameObject);
        }

        m_activeMagicContainer = Instantiate(_container, this.transform);

        m_activeMagicContainer.onMagicProfileAdded += UpdateMagicContainerVisuals;
        m_activeMagicContainer.onMagicProfileRemoved += UpdateMagicContainerVisuals;
        m_activeMagicContainer.onMagicObjectsUpdated += UpdateMagicContainerVisuals;
    }


    private void UpdateMagicContainerVisuals(RuntimeMagicObject _blank)
    {
        UpdateMagicContainerVisuals();
    }

    private void UpdateMagicContainerVisuals()
    {
        for (int i = m_contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(m_contentParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < m_activeMagicContainer.AvailableMagicObjects.Count; i++)
        {
            RuntimeMagicObjectView viewPrefab = Instantiate(m_magicObjectViewPrefab, m_contentParent);

            viewPrefab.SetFillAmountRatio(
                m_activeMagicContainer.AvailableMagicObjects[i].CooldownValue, 
                m_activeMagicContainer.AvailableMagicObjects[i].CooldownRatio
            );
        }
    }
}
