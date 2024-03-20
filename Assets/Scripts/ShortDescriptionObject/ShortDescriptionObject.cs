using UnityEngine;
using UnityEngine.EventSystems;


public class ShortDescriptionObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    [TextArea(5, 10)]
    private string m_shortDescription;


    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(InfoGlobalWindow), new InfoGlobalWindowData()
        {
            ApplyButtonText = "Принять",
            GlobalWindowTitle = "Описание",
            InfoMessage = m_shortDescription
        });
    }
}
