using Dimasyechka.Code.CraftingSystem.Recipes;
using Dimasyechka.Code.CraftingSystem.Workbenches.Profiles;
using Dimasyechka.Code.CraftingSystem.Workbenches.Views;
using Dimasyechka.Code.ZenjectFactories;
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
        private WorkbenchButtonViewFactory _workbenchButtonViewFactory;
        private BaseWorkbenchRecipeViewFactory _baseWorkbenchRecipeViewFactory;

        [Inject]
        public void Construct(
            CraftSystem craftSystem, 
            BaseWorkbenchRecipeViewFactory baseWorkbenchRecipeViewFactory,
            WorkbenchButtonViewFactory workbenchButtonViewFactory)
        {
            _craftSystem = craftSystem;
            _workbenchButtonViewFactory = workbenchButtonViewFactory;
            _baseWorkbenchRecipeViewFactory = baseWorkbenchRecipeViewFactory;
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
                WorkbenchButtonView buttonView = _workbenchButtonViewFactory.InstantiateForComponent(_buttonViewPrefab.gameObject, _workbenchesContentParent);

                buttonView.SetupModel(WorkbenchesContainer.AvailableCraftingWorkbenches[i]);
                buttonView.SetPressCallback(OpenWorkbench);
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
                BaseWorkbenchRecipeView recipeView = _baseWorkbenchRecipeViewFactory.InstantiateForComponent(_baseRecipeViewPrefab.gameObject, _recipesContentParent);
                
                recipeView.SetupModel(recipe);
                recipeView.SetPressCallback(OnCraftRecipeButtonPressed);
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


    public class WorkbenchButtonViewFactory : DiContainerCreationFactory<WorkbenchButtonView> { }

    public class BaseWorkbenchRecipeViewFactory : DiContainerCreationFactory<BaseWorkbenchRecipeView> { }
}