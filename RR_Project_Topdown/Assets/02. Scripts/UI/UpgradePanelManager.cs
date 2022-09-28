using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject UpgradePanel;
    [SerializeField] private List<UpgradeButton> upgradeButtons;
    public int lastIndex;
    PauseManager pauseManager;

    private void Awake()
    {
        pauseManager = GetComponent<PauseManager>();
    }

    private void Start()
    {
        HideButtons();
    }

    public void OpenPanel(List<UpgradeData> upgradeDatas)
    {
        Clean();
        pauseManager.PauseGame();
        UpgradePanel.SetActive(true);

        //업그레이드 버튼들에 업그레이드 데이터의 icon 할당
        for(int i = 0; i < upgradeDatas.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(true);
            upgradeButtons[i].Set(upgradeDatas[i]);
        }

        //가장 첫번째 버튼의 이미지 생성
        upgradeButtons[0].plugInList[upgradeButtons[0].index].SetActive(true);
        lastIndex = upgradeButtons[0].index;
    }

    public void ClosePanel()
    {
        //StageManager.Instance.isStageEnd = false;

        HideButtons();
        pauseManager.ResumeGame();
        UpgradePanel.SetActive(false);
    }

    private void HideButtons()
    {
        for (int i = 0; i<upgradeButtons.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(false);
        }
    }

    public void Clean()
    {
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].Clean();
        }
    }

    public void UpgradePlugIn(int pressedButtonId)
    {
        //StageManager.Instance.Upgrade(pressedButtonId);
        FindObjectOfType<StageManager>().Upgrade(pressedButtonId);
        ClosePanel();
    }
}
