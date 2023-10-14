using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseItem
{
    protected BaseItemProfile baseItemProfile;
    public BaseItemProfile BaseItemProfile => baseItemProfile;


    public BaseItem(BaseItemProfile _baseItemProfile)
    {
        baseItemProfile = _baseItemProfile;
    }
}
