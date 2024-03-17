using System;
using UnityEngine;
using UnityEngine.UI;


public class WorkbenchButtonView : MonoBehaviour
{
    [SerializeField]
    private Image m_previewImage;

    [SerializeField]
    private Button m_button;


    private CraftingWorkbenchProfile _profile;
    private Action<CraftingWorkbenchProfile> _handler;


    private void OnEnable()
    {
        m_button.onClick.AddListener(Press);
    }

    private void OnDisable()
    {
        m_button.onClick.RemoveListener(Press);
    }


    public void SetData(CraftingWorkbenchProfile profile, Action<CraftingWorkbenchProfile> callback)
    {
        _handler = callback;

        _profile = profile;
        m_previewImage.sprite = _profile.WorkbenchMiniSprite;
    }


    private void Press()
    {
        _handler?.Invoke(_profile);
    }
}
