using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameGlobalWindowSelector : MonoBehaviour
{
    [SerializeField]
    private BaseGameGlobalWindowButton inventoryButton;

    [SerializeField]
    private BaseGameGlobalWindowButton worldMapButton;

    [SerializeField]
    private BaseGameGlobalWindowButton playerSkillsButton;

    [SerializeField]
    private BaseGameGlobalWindowButton craftingButton;


    private void Awake()
    {
        inventoryButton.SetPressCallback(OpenInventoryWindow);
        worldMapButton.SetPressCallback(OpenWorldMapWindow);
        playerSkillsButton.SetPressCallback(OpenPlayerSkillsWindow);
        craftingButton.SetPressCallback(OpenCraftingWindow);
    }
    

    public void OpenInventoryWindow()
    {
        bool isShown = ToggleBaseGameGlobalWindow(typeof(InventoryGlobalWindow));
        inventoryButton.SetHighlighted(isShown);
    }

    public void OpenWorldMapWindow()
    {
        bool isShown = ToggleBaseGameGlobalWindow(typeof(WorldMapGlobalWindow));
        worldMapButton.SetHighlighted(isShown);
    }

    public void OpenPlayerSkillsWindow()
    {
        bool isShown = ToggleBaseGameGlobalWindow(typeof(PlayerSkillsGlobalWindow));
        playerSkillsButton.SetHighlighted(isShown);
    }

    public void OpenCraftingWindow()
    {
        bool isShown = ToggleBaseGameGlobalWindow(typeof(CraftingGlobalWindow));
        craftingButton.SetHighlighted(isShown);
    }

    
    private bool ToggleBaseGameGlobalWindow(Type baseGameGlobalWindowType)
    {
        inventoryButton.SetHighlighted(false);
        worldMapButton.SetHighlighted(false);
        playerSkillsButton.SetHighlighted(false);
        craftingButton.SetHighlighted(false);

        GlobalWindowsController.Instance.CloseEveryBaseGameGlobalWindowExceptOne(baseGameGlobalWindowType);
        GlobalWindowsController.Instance.TryToggleGlobalWindow(baseGameGlobalWindowType);
        
        return GlobalWindowsController.Instance.IsWindowShown(baseGameGlobalWindowType);
    }
}
