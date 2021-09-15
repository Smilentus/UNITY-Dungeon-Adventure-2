using UnityEngine;

public class ExtraLocationSlot : MonoBehaviour
{
    [Header("Локация на слоте")]
    public LocationManager.Location thisLocation;

    // Короткое название локации для картинки
    public string locShortName
    {
        get {
            return thisLocation.ToString();
        }
    }

    public void Press()
    {
        FindObjectOfType<LocationManager>().ShowInfoAbout(thisLocation);
    }
}
