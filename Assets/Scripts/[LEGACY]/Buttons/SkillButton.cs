using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Тип скилла для прокачки")]
    public SkillsManager.SkillType Type;

    public void OnPointerClick(PointerEventData eventData)
    {
        FindObjectOfType<SkillsManager>().ShowSkillDescr(Type);
    }
}
