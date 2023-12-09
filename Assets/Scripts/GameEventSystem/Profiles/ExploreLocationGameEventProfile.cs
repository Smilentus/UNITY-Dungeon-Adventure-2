using UnityEngine;


[CreateAssetMenu(menuName = "Creatable/GameEventSystem/New ExploreLocationGameEventProfile", fileName = "ExploreLocationGameEventProfile_")]
public class ExploreLocationGameEventProfile : BaseGameEventProfile
{
    [SerializeField]
    private LocationProfile m_ExplorableLocationProfile;
    public LocationProfile ExplorableLocationProfile => m_ExplorableLocationProfile;


    protected override void OnAutoGenerateNames()
    {
        if (m_ExplorableLocationProfile != null)
        {
            this.m_eventTitle = $"Вы исследовали область '{m_ExplorableLocationProfile.LocationTitle}'";
        }
    }
}
