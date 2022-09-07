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
    [SerializeField] PlayerScriptableObject playerScriptableObject;
    [SerializeField] HealthBar playerHealth;

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
        selectedUpgrades.AddRange(GetUpgrades(4));
        upgradePanel.OpenPanel(selectedUpgrades);
    }

    public List<UpgradeData> GetUpgrades(int count)
    {
        List<UpgradeData> upgradeList = new List<UpgradeData>();
        

        int totalNum = 0;

        for(int i = 0; i < upgrades.Count; i++)
        {
            totalNum += upgrades[i].datas.Count;
        }

        if (count > totalNum)
        {
            count = totalNum;
        }

        /*int j = 0;
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
        }*/

        List<int> numList = new List<int>();
        for(int i = 0; i < typeOfPlugIn; i++)
        {
            numList.Add(i);
        }

        while (count != 0)
        {
            if (numList.Count == 0) break;

            int j = numList[Random.Range(0, numList.Count)];
            if (upgrades[j].datas.Count == 0)
            {
                numList.Remove(j);
                continue;
            }

            UpgradeData upgradeData = upgrades[j].datas[Random.Range(0, upgrades[j].datas.Count)];
            upgradeList.Add(upgradeData);
            numList.Remove(j);
            count--;
        }

        return upgradeList;
    }

    public void AddUpgradesIntoCurrentUpgradeList(List<UpgradeData> upgradesToAdd, int index)
    {
        //this.upgrades.AddRange(upgradesToAdd);
        if (upgradesToAdd.Count == 0) return;
        if (upgradesToAdd.Count == 1 && (this.upgrades[index].datas.Contains(upgradesToAdd[0]) || acquiredUpgrades.Contains(upgradesToAdd[0]))) return;

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
            //단순 데미지 증가 15%
            case PlugInType.GauntletAttack_1:
                player.UpgradeAttackDamage(1.15f);
                break;
            //데미지 30% 증가, 공격속도 10% 감소
            case PlugInType.GauntletAttack_2:
                player.UpgradeAttackSpeed(0.9f);
                player.UpgradeAttackDamage(1.3f);
                break;
            //공격속도 15% 증가, 데미지 25% 감소
            case PlugInType.GauntletAttack_3:
                player.UpgradeAttackSpeed(1.15f);
                player.UpgradeAttackDamage(0.75f);
                break;
            //공격력 50% 증가, 범위증가 OR 공격속도 10% 증가[고민중]
            case PlugInType.GauntletAttack_4:
                player.UpgradeAttackSpeed(1.1f);
                player.UpgradeAttackDamage(1.5f);
                break;

            // 속성 공격 시리즈 생성

            /*
             * 각 속성공격은 하나를 고르면 나머지를 고를 수 없다.
             * 
             * 화염
             * 도트 데미지 중첩 불가, 도트를 맞고 있는 인원에게는 다시금 리필되는 형식
             * 1.공격에 화염 데미지 추가[도트 화염 데미지 추가, 강하지만 짧은 도트뎀] 
             * 2.화염의 도트 데미지 증가
             * 3.적이 죽은 위치에 화염이 남아 이전?
             * 4.[화염 데미지 중 사망시 폭발, 이전]
             * 
             */

            //index 1
            case PlugInType.CorrosionAttack_1:
                upgrades[upgradeData.index + 1].datas.Clear();
                upgrades[upgradeData.index + 2].datas.Clear();
                player.EnableElementAttack(PlayerMove.Element.Corrosion);
                break;
            case PlugInType.CorrosionAttack_2:
                //upgrades[upgradeData.index].datas.Clear();
                
                break;
            case PlugInType.CorrosionAttack_3:
                break;
            case PlugInType.CorrosionAttack_4:
                break;
            //index 2
            case PlugInType.FireAttack_1:
                upgrades[upgradeData.index - 1].datas.Clear();
                upgrades[upgradeData.index + 1].datas.Clear();
                player.EnableElementAttack(PlayerMove.Element.Fire);
                break;
            case PlugInType.FireAttack_2:
                playerScriptableObject.burnDamage *= 1.5f;
                break;
            case PlugInType.FireAttack_3:
                break;
            case PlugInType.FireAttack_4:
                break;
            //index 3
            case PlugInType.IceAttack_1:
                upgrades[upgradeData.index - 2].datas.Clear();
                upgrades[upgradeData.index - 1].datas.Clear();
                player.EnableElementAttack(PlayerMove.Element.Ice);
                break;
            case PlugInType.IceAttack_2:
                break;
            case PlugInType.IceAttack_3:
                break;
            case PlugInType.IceAttack_4:
                break;
            //index 4
            //체력 증가 플러그인 라인업에 이동속도 증가를 넣으면 어떨까?
            /*Utility Plugin 으로 이름 변경
             * 1. 체력 강화
             * 2. 실드 생성
             * 3. 이동속도 증가
             * 4. 체력 강화 및 피해시 화면 전체의 적에게 적은 데미지
             */
            case PlugInType.Utility_1:
                break;
            case PlugInType.Utility_2:
                break;
            case PlugInType.Utility_3:
                break;
            case PlugInType.Utility_4:
                break;
            //index 5
            /*소환수의 경우 
             * 
             * 원거리 공격 방식으로 고정
             * 플레이어 중심으로 동심원을 그리며 회전
             * 가장 가까운 적에게 스킬 시전 
             * 스킬 시전 위치 마치 제라스 W
             * 
             * 1. 소환수 생성
             * 2. 소환수 Melee 공격 강화 혹은 Player Buff
             * 3. 소환수 Debuff 강화 
             * 4. 소환수 Damage&속성공격 플레이어 강화 상태에 맞춰서 강화
             */
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
        //upgrades[upgradeData.index].datas.Clear();
        upgrades[upgradeData.index].datas.Remove(upgradeData);
        AddUpgradesIntoCurrentUpgradeList(upgradeData.upgrades, upgradeData.index);
    }
}
