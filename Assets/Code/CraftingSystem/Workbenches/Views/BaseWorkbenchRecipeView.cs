using Dimasyechka.Code.CraftingSystem.Recipes;
using Dimasyechka.Code.CraftingSystem.Recipes.Views;
using Dimasyechka.Code.ZenjectFactories;
using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.CraftingSystem.Workbenches.Views
{
    public class BaseWorkbenchRecipeView : MonoViewModel<CraftingRecipe>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<Sprite> RecipePreview = new ReactiveProperty<Sprite>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> RecipeName = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<string> RecipeOutput = new ReactiveProperty<string>();


        [SerializeField]
        private CraftInputItemView _craftInputItemView;

        [SerializeField]
        private Transform _inputItemsContentParent;


        private Action<CraftingRecipe> _pressCallback;


        private CraftInputItemViewFactory _factory;

        [Inject]
        public void Construct(CraftInputItemViewFactory factory)
        {
            _factory = factory;
        }


        protected override void OnSetupModel()
        {
            RecipeName.Value = Model.RecipeName;

            RecipePreview.Value = Model.CraftedItem.ItemSprite;
            RecipeOutput.Value = Model.CraftedItemAmount.ToString();

            PlaceInputCraftItems();
        }


        public void SetPressCallback(Action<CraftingRecipe> callback)
        {
            _pressCallback = callback;
        }

        private void PlaceInputCraftItems()
        {
            ClearInputCraftItems();

            for (int i = 0; i < Model.InputCraftItems.Count; i++)
            {
                CraftInputItemView inputView = _factory.InstantiateForComponent(_craftInputItemView.gameObject, _inputItemsContentParent);

                inputView.SetupModel(Model.InputCraftItems[i]);
            }
        }

        private void ClearInputCraftItems()
        {
            for (int i = _inputItemsContentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(_inputItemsContentParent.GetChild(i).gameObject);
            }
        }

        [RxAdaptableMethod]
        public void OnCraftButtonPressed()
        {
            _pressCallback?.Invoke(Model);
        }
    }


    public class CraftInputItemViewFactory : DiContainerCreationFactory<CraftInputItemView> { }
}
