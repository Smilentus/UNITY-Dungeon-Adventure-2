using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocationInteractionView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private LocationProfile m_locationProfile;

    [SerializeField]
    private int m_travelHours = 1;
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        LocationsController.Instance.TravelToLocation(m_locationProfile, m_travelHours);
    }
}
