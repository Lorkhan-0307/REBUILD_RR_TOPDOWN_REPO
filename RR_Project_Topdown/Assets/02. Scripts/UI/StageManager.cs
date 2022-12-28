using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
    [SerializeField] EnemySpawnPoolController enemySpawnPoolController;

    [Header("Load Main Menu")]
    [SerializeField] private GameObject FadeImage;
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    /*
    [Header("Dialogues")]
    [SerializeField] public DialogueTrigger dialogueTrigger;*/

    public bool isGameOver { get; private set; }

    private int typeOfPlugIn = 6;
    private List<UpgradeData> selectedUpgrades;

    public event EventHandler UpgradeHealth;
    public event EventHandler UpgradeSetShieldActive;
    public event EventHandler UpgradeMoveSpeed;
    public event EventHandler UpgradeEntireAttack;
    public event EventHandler UpgradeSetPetActive;
    public event EventHandler UpgradePetAbility;
    public event EventHandler UpgradeSetForceFieldActive;
    public event EventHandler UpgradePetCoolTime;
    public event EventHandler GameOverEvent;

    private void Start()
    {
        //enemySpawnPoolController.OnStageEnd += UpdatePlugIn;
        player.OnTriggerPortal += UpdatePlugIn;
        enemySpawnPoolController.UpgradePlugIn += UpdatePlugIn;
        FindObjectOfType<Health>().OnDeath += EndGame;
        PlayerVariable pv = new PlayerVariable();


        playerScriptableObject.meleeAttackDamage = pv.meleeAttackDamage;
        playerScriptableObject.skillActiveMeleeAttackDamage = pv.skillActiveMeleeAttackDamage;
        playerScriptableObject.burnTicks = pv.burnTicks;
        playerScriptableObject.maxBurnTicks = pv.maxBurnTicks;
        playerScriptableObject.burnDamage = pv.burnDamage;
        playerScriptableObject.corrosionTicks = pv.corrosionTicks;
        playerScriptableObject.corrosionDamage = pv.corrosionDamage;
        playerScriptableObject.maxCorrosionTicks = pv.maxCorrosionTicks;
        playerScriptableObject.enabledSecondUpgrade = pv.enabledSecondUpgrade;
        playerScriptableObject.enabledThirdUpgrade = pv.enabledThirdUpgrade;
        playerScriptableObject.enabledFourthUpgrade = pv.enabledFourthUpgrade;
        playerScriptableObject.isPetUpgraded = pv.isPetUpgraded;
        

    }
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdatePlugIn_Temp();
        }*/
    }

    private void EndGame()
    {
        isGameOver = true;
        //Debug.Log("End game");
        GameOverEvent?.Invoke(this, EventArgs.Empty);
        //FindObjectOfType<UIManager>().OpenGameOverUI();
    }

    public void LoadMainScene()
    {
        StartCoroutine(LoadScene(0));
    }

    private IEnumerator LoadScene(int sceneIndex)
    {
        FadeImage.SetActive(true);
        transition.SetBool("Start", true);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }


    // For Plug In Test
    private void UpdatePlugIn_Temp()
    {
        if (selectedUpgrades == null)
        {
            selectedUpgrades = new List<UpgradeData>();
        }
        selectedUpgrades.Clear();
        selectedUpgrades.AddRange(GetUpgrades(4));
        upgradePanel.OpenPanel(selectedUpgrades);
    }

    //발동 조건 필요
    private void UpdatePlugIn(object sender, EventArgs e)
    {
        if (selectedUpgrades == null)
        {
            selectedUpgrades = new List<UpgradeData>();
        }
        selectedUpgrades.Clear();
        selectedUpgrades.AddRange(GetUpgrades(4));
        upgradePanel.OpenPanel(selectedUpgrades);
        //enemySpawnPoolController.UpgradePlugIn -= UpdatePlugIn;
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

        //Debug.Log(upgradeData.plugInType);

        switch (upgradeData.plugInType)
        {
            //여기에 player 할당 받아서 함수로 넣으면 됨. 
            //index 0
            //단순 데미지 증가 15%
            case PlugInType.GauntletAttack_1:
                player.UpgradeAttackDamage(playerScriptableObject.upgradeDamage_Gauntlet1);
                playerScriptableObject.latestGauntletDamageRate = playerScriptableObject.upgradeDamage_Gauntlet1;

                // 펫 2번째 업그레이드 이미 되어있는 경우, 펫 능력치를 올린다. 
                if (playerScriptableObject.isPetUpgraded) UpgradePetAbility?.Invoke(this, EventArgs.Empty);
                break;
            //데미지 30% 증가, 공격속도 10% 감소
            case PlugInType.GauntletAttack_2:
                player.UpgradeAttackSpeed(playerScriptableObject.upgradeSpeed_Gauntlet2);
                player.UpgradeAttackDamage(playerScriptableObject.upgradeDamage_Gauntlet2);
                playerScriptableObject.latestGauntletDamageRate = playerScriptableObject.upgradeDamage_Gauntlet2;
                if (playerScriptableObject.isPetUpgraded) UpgradePetAbility?.Invoke(this, EventArgs.Empty);

                break;
            //공격속도 15% 증가, 데미지 25% 감소
            case PlugInType.GauntletAttack_3:
                player.UpgradeAttackSpeed(playerScriptableObject.upgradeSpeed_Gauntlet3);
                player.UpgradeAttackDamage(playerScriptableObject.upgradeDamage_Gauntlet3);
                playerScriptableObject.latestGauntletDamageRate = playerScriptableObject.upgradeDamage_Gauntlet3;

                if (playerScriptableObject.isPetUpgraded) UpgradePetAbility?.Invoke(this, EventArgs.Empty);
                break;
            //공격력 50% 증가, 범위증가 OR 공격속도 10% 증가[고민중]
            case PlugInType.GauntletAttack_4:
                //player.UpgradeAttackSpeed(1.1f);
                player.UpgradeAttackDamage(playerScriptableObject.upgradeDamage_Gauntlet4);
                playerScriptableObject.latestGauntletDamageRate = playerScriptableObject.upgradeDamage_Gauntlet4;

                if (playerScriptableObject.isPetUpgraded) UpgradePetAbility?.Invoke(this, EventArgs.Empty);
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
                playerScriptableObject.corrosionTicks += 10;
                break;
            case PlugInType.CorrosionAttack_3: 
                playerScriptableObject.enabledThirdUpgrade = true;
                break;
            case PlugInType.CorrosionAttack_4: 
                playerScriptableObject.enabledFourthUpgrade = true;
                break;
            //index 2
            case PlugInType.FireAttack_1: 
                upgrades[upgradeData.index - 1].datas.Clear();
                upgrades[upgradeData.index + 1].datas.Clear();
                player.EnableElementAttack(PlayerMove.Element.Fire);
                Debug.Log("Fire1");
                break;
            case PlugInType.FireAttack_2: 
                playerScriptableObject.burnDamage *= 2f;
                Debug.Log("Fire2");
                break;
            case PlugInType.FireAttack_3: 
                playerScriptableObject.enabledThirdUpgrade = true;
                Debug.Log("Fire3");
                break;
            case PlugInType.FireAttack_4: 
                playerScriptableObject.enabledFourthUpgrade = true;
                player.SetFireFourthUpgrade();
                Debug.Log("Fire4");
                break;



            //여기까지 완료
            //index 3
            case PlugInType.IceAttack_1:
                upgrades[upgradeData.index - 2].datas.Clear();
                upgrades[upgradeData.index - 1].datas.Clear();
                player.EnableElementAttack(PlayerMove.Element.Ice);
                break;
            case PlugInType.IceAttack_2:
                playerScriptableObject.slowDownSpeed *= 1.4f;
                playerScriptableObject.enabledThirdUpgrade = true;
                break;
            case PlugInType.IceAttack_3:
                playerScriptableObject.enabledThirdUpgrade = true;
                break;
            case PlugInType.IceAttack_4:
                playerScriptableObject.enabledFourthUpgrade = true;
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
                //Health or Barrier ++
                UpgradeHealth?.Invoke(this, EventArgs.Empty);
                break;
            case PlugInType.Utility_2:
                //ShieldEffect.SetActive(true);
                UpgradeSetShieldActive?.Invoke(this, EventArgs.Empty);
                break;
            case PlugInType.Utility_3:
                UpgradeMoveSpeed?.Invoke(this, EventArgs.Empty);
                break;
            case PlugInType.Utility_4:
                UpgradeEntireAttack?.Invoke(this, EventArgs.Empty);
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
                UpgradeSetPetActive?.Invoke(this, EventArgs.Empty);
                break;
            case PlugInType.SummonAttack_2:
                playerScriptableObject.isPetUpgraded = true;
                UpgradePetAbility?.Invoke(this, EventArgs.Empty);
                break;
            case PlugInType.SummonAttack_3:
                UpgradeSetForceFieldActive?.Invoke(this, EventArgs.Empty);
                break;
            case PlugInType.SummonAttack_4:
                UpgradePetCoolTime?.Invoke(this, EventArgs.Empty);
                break;
        }

        acquiredUpgrades.Add(upgradeData);
        //upgrades[upgradeData.index].datas.Clear();
        upgrades[upgradeData.index].datas.Remove(upgradeData);
        AddUpgradesIntoCurrentUpgradeList(upgradeData.upgrades, upgradeData.index);
    }
}
