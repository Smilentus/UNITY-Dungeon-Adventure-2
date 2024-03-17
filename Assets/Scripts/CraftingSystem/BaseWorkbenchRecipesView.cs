using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BaseWorkbenchRecipesView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_recipeNameTMP;


    [SerializeField]
    private Button m_craftButton;


    private CraftingRecipe _recipe;
    private Action<CraftingRecipe> _handler;


    private void OnEnable()
    {
        m_craftButton?.onClick.AddListener(OnCraftButtonPressed);
    }

    private void OnDisable()
    {
        m_craftButton?.onClick.RemoveListener(OnCraftButtonPressed);
    }


    public void SetData(CraftingRecipe craftingRecipe, Action<CraftingRecipe> callback)
    {
        _handler = callback;

        _recipe = craftingRecipe;

        m_recipeNameTMP.text = _recipe.RecipeName;
    }


    private void OnCraftButtonPressed()
    {
        _handler?.Invoke(_recipe);
    }
}
