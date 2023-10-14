using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// TODO: ... ???
public class BaseItemView : MonoBehaviour
{
    protected BaseItem baseItem;
    public BaseItem BaseItem => baseItem;

    
    public virtual void SetViewData(BaseItem _baseItem)
    {
        baseItem = _baseItem;

        UpdateViewData();
    }


    protected virtual void UpdateViewData()
    {
        
    }
}
