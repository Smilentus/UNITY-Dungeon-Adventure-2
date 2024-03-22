using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObtainSkillGlobalWindow : BaseGameGlobalWindow
{
    [SerializeField]
    private ObtainSkillDataView m_currentSkillView;

    [SerializeField]
    private ObtainSkillDataView m_nextSkillView;



    [SerializeField]
    private SkillUpgradeDescriptionView m_neededUpgradePrefab;

    [SerializeField]
    private Transform m_neededUpgradesContentParent;


    [SerializeField]
    private Button m_closeButton;

    [SerializeField]
    private Button m_applyButton;


    private void OnEnable()
    {
        m_closeButton.onClick.AddListener(Hide);
        m_applyButton.onClick.AddListener(OnApplyButtonPressed);
    }

    private void OnDisable()
    {
        m_closeButton.onClick.RemoveListener(Hide);
        m_applyButton.onClick.RemoveListener(OnApplyButtonPressed);
    }


    protected override void OnShow()
    {
        DrawSkillData();
    }

    private void DrawSkillData()
    {
        ObtainSkillGlobalWindowData data = GetConvertedWindowData<ObtainSkillGlobalWindowData>();

        PlayerSkill playerSkill = PlayerSkillsController.instance.GetPlayerSkillByGUID(data.Profile.skillGUID);

        int currentSkillLevel = -1;
        List<string> deltaValues = new List<string>();

        if (playerSkill != null)
        {
            currentSkillLevel = playerSkill.runtimeSkillCore.UpgradeableComponent.currentLevel;
            deltaValues = playerSkill.runtimeSkillCore.GetSkillDeltaValues(currentSkillLevel + 1);
        }
        else
        {
            deltaValues = data.Profile.skillCorePrefab.GetSkillDeltaValues(1);
        }

        SkillLevelData currentLevelData = currentSkillLevel == -1 ? null : data.Profile.GetLevelData(currentSkillLevel);
        SkillLevelData nextLevelData = data.Profile.GetLevelData(currentSkillLevel + 1);

        m_currentSkillView.SetData(currentLevelData, currentSkillLevel);
        m_nextSkillView.SetData(nextLevelData, currentSkillLevel + 1, deltaValues);

        DrawUpgradesDescription(playerSkill);

    }

    private void DrawUpgradesDescription(PlayerSkill playerSkill)
    {
        for (int i = m_neededUpgradesContentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(m_neededUpgradesContentParent.GetChild(i).gameObject);
        }

        List<SkillUpgradeDescriptionData> skillUpgrades = new List<SkillUpgradeDescriptionData>();

        if (playerSkill == null)
        {
            ObtainSkillGlobalWindowData data = GetConvertedWindowData<ObtainSkillGlobalWindowData>();
            skillUpgrades = data.Profile.skillCorePrefab.UpgradeableComponent.GetUpgradeDescriptions();
        }
        else
        {
            skillUpgrades = playerSkill.runtimeSkillCore.UpgradeableComponent.GetUpgradeDescriptions();
        }

        foreach (SkillUpgradeDescriptionData upgradeDescription in skillUpgrades)
        {
            SkillUpgradeDescriptionView view = Instantiate(m_neededUpgradePrefab, m_neededUpgradesContentParent);

            view.SetData(upgradeDescription);
        }
    }


    private void OnApplyButtonPressed()
    {
        ObtainSkillGlobalWindowData data = GetConvertedWindowData<ObtainSkillGlobalWindowData>();

        data?.OnApply();

        DrawSkillData();
    }
}

[System.Serializable]
public class ObtainSkillGlobalWindowData : BaseGameGlobalWindowData
{
    public SkillProfile Profile;

    public Action OnApply;
}