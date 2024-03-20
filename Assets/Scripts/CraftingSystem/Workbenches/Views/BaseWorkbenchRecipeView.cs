using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BaseWorkbenchRecipeView : MonoBehaviour
{
    [SerializeField]
    private Image m_recipePreview;

    [SerializeField]
    private TMP_Text m_recipeNameTMP;

    [SerializeField]
    private TMP_Text m_recipeOutputTMP;

    [SerializeField]
    private Button m_craftButton;


    [SerializeField]
    private CraftInputItemView m_craftInputItemView;

    [SerializeField]
    private Transform m_inputItemsContentParent;


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

        m_recipePreview.sprite = _recipe.CraftedItem.ItemSprite;
        m_recipeOutputTMP.text = _recipe.CraftedItemAmount.ToString();

        PlaceInputCraftItems();
    }

    private void PlaceInputCraftItems()
    {
        ClearInputCraftItems();
        
        for (int i = 0; i < _recipe.InputCraftItems.Count; i++)
        {
            CraftInputItemView inputView = Instantiate(m_craftInputItemView, m_inputItemsContentParent);

            inputView.SetData(_recipe.InputCraftItems[i]);
        }
    }

    private void ClearInputCraftItems()
    {
        for (int i = m_inputItemsContentParent.childCount - 1; i >= 0; i--) 
        {
            Destroy(m_inputItemsContentParent.GetChild(i).gameObject);
        }
    }

    private void OnCraftButtonPressed()
    {
        _handler?.Invoke(_recipe);
    }
}
