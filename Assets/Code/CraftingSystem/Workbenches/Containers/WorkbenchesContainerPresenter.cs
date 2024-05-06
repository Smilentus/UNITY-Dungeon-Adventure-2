using Dimasyechka.Code.CraftingSystem.Recipes;
using Dimasyechka.Code.CraftingSystem.Workbenches.Profiles;
using Dimasyechka.Code.CraftingSystem.Workbenches.Views;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.CraftingSystem.Workbenches.Containers
{
    public class WorkbenchesContainerPresenter : MonoBehaviour
    {
        [field: SerializeField]
        public WorkbenchesContainer WorkbenchesContainer { get; private set; }


        [SerializeField]
        private WorkbenchButtonView _buttonViewPrefab;

        [SerializeField]
        private Transform _workbenchesContentParent;


        [SerializeField]
        private BaseWorkbenchRecipeView _baseRecipeViewPrefab;

        [SerializeField]
        private Transform _recipesContentParent;


        private CraftSystem _craftSystem;

        [Inject]
        public void Construct(CraftSystem craftSystem)
        {
            _craftSystem = craftSystem;
        }


        private void Awake()
        {
            WorkbenchesContainer.onWorkbenchesChanged += UpdateWorkbenchesUI;
        }

        private void OnDestroy()
        {
            WorkbenchesContainer.onWorkbenchesChanged -= UpdateWorkbenchesUI;
        }


        private void OnEnable()
        {
            UpdateWorkbenchesUI();
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
                WorkbenchButtonView buttonView = Instantiate(_buttonViewPrefab, _workbenchesContentParent);

                buttonView.SetData(WorkbenchesContainer.AvailableCraftingWorkbenches[i], OpenWorkbench);
            }
        }

        private void ClearWorkbenches()
        {
            for (int i = _workbenchesContentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(_workbenchesContentParent.GetChild(i).gameObject);
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
                BaseWorkbenchRecipeView recipeView = Instantiate(_baseRecipeViewPrefab, _recipesContentParent);

                recipeView.SetData(recipe, OnCraftRecipeButtonPressed);
            }
        }

        private void ClearRecipes()
        {
            for (int i = _recipesContentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(_recipesContentParent.GetChild(i).gameObject);
            }
        }


        private void OnCraftRecipeButtonPressed(CraftingRecipe recipe)
        {
            _craftSystem.TryCraftRecipe(recipe);
        }
    }
}