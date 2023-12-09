using UnityEngine;
using UnityEngine.EventSystems;


public class LocationInteractableButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField]
    private LocationProfile m_LocationProfile;


    public void Press()
    {
        if (m_LocationProfile == null)
        {
            Debug.LogError($"������� �� ���������� �� ��������!", this.gameObject);
            return;
        }

        if (m_LocationProfile.LocationEvents.Count == 0)
        {
            Debug.LogError($"�� ������� �� ��������� ��������!", this.gameObject);
            return;
        }

        BaseGameEventProfile baseGameEventProfile = m_LocationProfile.GetRandomLocationEvent;

        if (baseGameEventProfile == null)
        {
            Debug.LogError($"�� ������ �������� ��������� ������� �� �������!", this.gameObject);
            return;
        }

        GameEventsController.Instance.StartGameEvent(baseGameEventProfile);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Press();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
