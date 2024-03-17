using UnityEngine;


public class WorkbenchesContainerPresenter : MonoBehaviour
{
    [field: SerializeField]
    public WorkbenchesContainer WorkbenchesContainer { get; private set; }


    [SerializeField]
    private WorkbenchButtonView m_buttonViewPrefab;

    [SerializeField]
    private Transform m_workbenchesContentParent;


    [SerializeField]
    private BaseWorkbenchRecipesView m_baseRecipeViewPrefab;

    [SerializeField]
    private Transform m_recipesContentParent;


    private void Awake()
    {
        WorkbenchesContainer.onWorkbenchesChanged += UpdateWorkbenchesUI;
    }

    private void OnDestroy()
    {
        WorkbenchesContainer.onWorkbenchesChanged -= UpdateWorkbenchesUI;
    }


    public void UpdateWorkbenchesUI()
    {
        ClearWorkbenches();
        DrawWorkbenches();
    }

    private void DrawWorkbenches()
    {
        for (int i = 0; i < WorkbenchesContainer.AvailableCraftingWorkbenches.Count; i++)
        {
            WorkbenchButtonView buttonView = Instantiate(m_buttonViewPrefab, m_workbenchesContentParent);

            buttonView.SetData(WorkbenchesContainer.AvailableCraftingWorkbenches[i], OpenWorkbench);
        }
    }

    private void ClearWorkbenches()
    {
        for (int i = m_workbenchesContentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(m_workbenchesContentParent.GetChild(i).gameObject);
        }
    }


    private void OpenWorkbench(CraftingWorkbenchProfile profile)
    {
        UpdateRecipesUI(profile);
    }


    private void UpdateRecipesUI(CraftingWorkbenchProfile profile)
    {
        ClearRecipes();
        DrawRecipes(profile);
    }

    private void DrawRecipes(CraftingWorkbenchProfile profile)
    {
        foreach (CraftingRecipe recipe in profile.GetAllCraftingRecipes())
        {
            BaseWorkbenchRecipesView recipeView = Instantiate(m_baseRecipeViewPrefab, m_recipesContentParent);

            recipeView.SetData(recipe, OnCraftRecipeButtonPressed);
        }
    }

    private void ClearRecipes()
    {
        for (int i = m_recipesContentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(m_recipesContentParent.GetChild(i).gameObject);
        }
    }


    private void OnCraftRecipeButtonPressed(CraftingRecipe recipe)
    {
        CraftSystem.Instance.TryCraftRecipe(recipe);
    }
}