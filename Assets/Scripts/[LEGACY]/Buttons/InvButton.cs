using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Номер слота")]
    public int slotNum;
    [Header("Открыт ли слот инвентаря?")]
    public bool isActive;

    public void UpdateState()
    {
        if(isActive)
        {
            transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isActive)
        {
            FindObjectOfType<Inventory>().PressSlot(slotNum);
        }
    }
}
