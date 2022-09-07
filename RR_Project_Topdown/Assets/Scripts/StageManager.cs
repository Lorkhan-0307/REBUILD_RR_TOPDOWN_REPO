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
            //���⿡ player �Ҵ� �޾Ƽ� �Լ��� ������ ��. 
            //index 0
            //�ܼ� ������ ���� 15%
            case PlugInType.GauntletAttack_1:
                player.UpgradeAttackDamage(1.15f);
                break;
            //������ 30% ����, ���ݼӵ� 10% ����
            case PlugInType.GauntletAttack_2:
                player.UpgradeAttackSpeed(0.9f);
                player.UpgradeAttackDamage(1.3f);
                break;
            //���ݼӵ� 15% ����, ������ 25% ����
            case PlugInType.GauntletAttack_3:
                player.UpgradeAttackSpeed(1.15f);
                player.UpgradeAttackDamage(0.75f);
                break;
            //���ݷ� 50% ����, �������� OR ���ݼӵ� 10% ����[�����]
            case PlugInType.GauntletAttack_4:
                player.UpgradeAttackSpeed(1.1f);
                player.UpgradeAttackDamage(1.5f);
                break;

            // �Ӽ� ���� �ø��� ����

            /*
             * �� �Ӽ������� �ϳ��� ���� �������� �� �� ����.
             * 
             * ȭ��
             * ��Ʈ ������ ��ø �Ұ�, ��Ʈ�� �°� �ִ� �ο����Դ� �ٽñ� ���ʵǴ� ����
             * 1.���ݿ� ȭ�� ������ �߰�[��Ʈ ȭ�� ������ �߰�, �������� ª�� ��Ʈ��] 
             * 2.ȭ���� ��Ʈ ������ ����
             * 3.���� ���� ��ġ�� ȭ���� ���� ����?
             * 4.[ȭ�� ������ �� ����� ����, ����]
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
            //ü�� ���� �÷����� ���ξ��� �̵��ӵ� ������ ������ ���?
            /*Utility Plugin ���� �̸� ����
             * 1. ü�� ��ȭ
             * 2. �ǵ� ����
             * 3. �̵��ӵ� ����
             * 4. ü�� ��ȭ �� ���ؽ� ȭ�� ��ü�� ������ ���� ������
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
            /*��ȯ���� ��� 
             * 
             * ���Ÿ� ���� ������� ����
             * �÷��̾� �߽����� ���ɿ��� �׸��� ȸ��
             * ���� ����� ������ ��ų ���� 
             * ��ų ���� ��ġ ��ġ ���� W
             * 
             * 1. ��ȯ�� ����
             * 2. ��ȯ�� Melee ���� ��ȭ Ȥ�� Player Buff
             * 3. ��ȯ�� Debuff ��ȭ 
             * 4. ��ȯ�� Damage&�Ӽ����� �÷��̾� ��ȭ ���¿� ���缭 ��ȭ
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
