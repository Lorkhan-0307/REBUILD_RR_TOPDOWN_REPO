using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeDatas
{
    public List<UpgradeData> datas;
}


public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField] UpgradePanelManager upgradePanel;
    //[SerializeField] List<UpgradeData> upgrades;
    [SerializeField] List<UpgradeDatas> upgrades;
    [SerializeField] List<UpgradeData> acquiredUpgrades;
    [SerializeField] PlayerMove player;
    private int typeOfPlugIn = 5;

    public bool isStageEnd;
    private List<UpgradeData> selectedUpgrades;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdatePlugIn();
        }
    }

    private void UpdatePlugIn()
    {
        if (selectedUpgrades == null)
        {
            selectedUpgrades = new List<UpgradeData>();
        }
        selectedUpgrades.Clear();
        selectedUpgrades.AddRange(GetUpgrades(5));
        upgradePanel.OpenPanel(selectedUpgrades);
    }



    public List<UpgradeData> GetUpgrades(int count)
    {
        List<UpgradeData> upgradeList = new List<UpgradeData>();

        /*if(count > upgrades.Count)
        {
            count = upgrades.Count;
        }

        while (count != 0)
        {
            UpgradeData upgradeData = upgrades[Random.Range(0, upgrades.Count)];

            if (upgradeList.Contains(upgradeData)) continue;
            else
            {
                upgradeList.Add(upgradeData);
                count--;
            }

        }*/
        int totalNum = 0;

        for(int i = 0; i < upgrades.Count; i++)
        {
            totalNum += upgrades[i].datas.Count;
        }

        if (count > totalNum)
        {
            count = totalNum;
        }

        int j = 0;
        while (count != 0)
        {
            if (j == typeOfPlugIn) break;
            if (upgrades[j].datas.Count == 0)
            {
                j++;
                continue;
            }

            UpgradeData upgradeData = upgrades[j].datas[Random.Range(0, upgrades[j].datas.Count)];
            upgradeList.Add(upgradeData);
            j++;
            count--;
        }

        //Debug.Log("end");
        return upgradeList;
    }

    public void AddUpgradesIntoCurrentUpgradeList(List<UpgradeData> upgradesToAdd, int index)
    {
        //this.upgrades.AddRange(upgradesToAdd);
        this.upgrades[index].datas.AddRange(upgradesToAdd);
    }

    public void Upgrade(int selectedUpgradeId)
    {
        UpgradeData upgradeData = selectedUpgrades[selectedUpgradeId];

        if (acquiredUpgrades == null)
        {
            acquiredUpgrades = new List<UpgradeData>();
        }

        switch (upgradeData.plugInType)
        {
            //여기에 player 할당 받아서 함수로 넣으면 됨. 
            //index 0
            case PlugInType.GauntletAttack_1:
                break;
            case PlugInType.GauntletAttack_2:
                break;
            case PlugInType.GauntletAttack_3:
                break;
            case PlugInType.GauntletAttack_4:
                break;
            //index 1
            case PlugInType.CorrosionAttack_1:
                upgrades[upgradeData.index + 1].datas.Clear();
                upgrades[upgradeData.index + 2].datas.Clear();
                break;
            case PlugInType.CorrosionAttack_2:
                break;
            case PlugInType.CorrosionAttack_3:
                break;
            case PlugInType.CorrosionAttack_4:
                break;
            //index 2
            case PlugInType.FireAttack_1:
                upgrades[upgradeData.index - 1].datas.Clear();
                upgrades[upgradeData.index + 1].datas.Clear();
                break;
            case PlugInType.FireAttack_2:
                break;
            case PlugInType.FireAttack_3:
                break;
            case PlugInType.FireAttack_4:
                break;
            //index 3
            case PlugInType.IceAttack_1:
                upgrades[upgradeData.index - 2].datas.Clear();
                upgrades[upgradeData.index - 1].datas.Clear();
                break;
            case PlugInType.IceAttack_2:
                break;
            case PlugInType.IceAttack_3:
                break;
            case PlugInType.IceAttack_4:
                break;
            //index 4
            case PlugInType.Utility_1:
                break;
            case PlugInType.Utility_2:
                break;
            case PlugInType.Utility_3:
                break;
            case PlugInType.Utility_4:
                break;
            //index 5
            case PlugInType.SummonAttack_1:
                break;
            case PlugInType.SummonAttack_2:
                break;
            case PlugInType.SummonAttack_3:
                break;
            case PlugInType.SummonAttack_4:
                break;
        }

        acquiredUpgrades.Add(upgradeData);
        upgrades[upgradeData.index].datas.Clear();
        AddUpgradesIntoCurrentUpgradeList(upgradeData.upgrades, upgradeData.index);
    }
}
