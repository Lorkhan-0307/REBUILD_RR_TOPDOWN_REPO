using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

using KeyType = System.String;

[System.Serializable]
public class SpawnTransform
{
    public List<Transform> transforms = new List<Transform>();
}

public enum StageType
{
    Main,
    Infinity,
    jatoichi
}
public class EnemySpawnPoolController : MonoBehaviour
{
    [SerializeField] public List<SpawnTransform> spawnPoints;
    [SerializeField] private EnemySpawnPool[] enemySpawnPools;
    [SerializeField] private int spawnCount_infinity = 35;
    [SerializeField] private float EnemySpawnInterval = 3.5f;
    [SerializeField] private float StageChangeInterval = 2.0f;
    [SerializeField] public List<int> spawnCountList;
    [SerializeField] private StageType stageType;
    [SerializeField] private Transform bossInstantiateTransform;
    public GameObject boss;
    public int stageNum = 0;

    private int enemyKillScore = 0;
    private int bossKillScore = 0;
    private int enemyKillScore_temp;

    //public event EventHandler OnStageEnd;
    public event EventHandler UpgradePlugIn;
    public event EventHandler<OnSetActivePotalEventArgs> OnSetActivePotal;

    public class OnSetActivePotalEventArgs : EventArgs
    {
        public int stageNum;
    }

    private int spawnCount;
    private int spawnedEnemy = 0;
    private int killCount = 0;
    private float intensity;
    private float StageChangeTime;
    private float timeBtwSpawn;
    private bool isStageChanged;
    private IEnumerator coroutine;

    void Start()
    {
        StageChangeTime = StageChangeInterval;
        timeBtwSpawn = EnemySpawnInterval;
        spawnCount = spawnCountList[stageNum];
        killCount = 0;
        FindObjectOfType<StageManager>().GameOverEvent += UpdateScore;

        coroutine = spawnEnemy();
        StartCoroutine(coroutine);
    }

    private void Update()
    {
        switch(stageType)
        {
            case StageType.Main:
            //Set Timer When Stage Changes
                if (isStageChanged)
                {
                    StageChangeTime -= Time.deltaTime;

                    //Start coroutine when timer ends
                    if (StageChangeTime <= 0f)
                    {
                        StartCoroutine(coroutine);
                        StageChangeTime = StageChangeInterval;
                        isStageChanged = false;
                    }
                }

                if (spawnedEnemy == spawnCount)
                {
                    StopCoroutine(coroutine);
                }

                if(killCount == spawnCount)
                {
                    //OnStageEnd?.Invoke(this, EventArgs.Empty);
                    OnSetActivePotal?.Invoke(this, new OnSetActivePotalEventArgs { stageNum = stageNum });
                }
                break;

            case StageType.Infinity:

                if (FindObjectOfType<StageManager>().isGameOver) return;

                if(killCount == spawnCount_infinity)
                {
                    UpgradePlugIn?.Invoke(this, EventArgs.Empty);
                    killCount = 0;
                }

                if(enemyKillScore_temp == spawnCount_infinity * 2)
                {
                    Instantiate(boss, bossInstantiateTransform.position, Quaternion.identity);
                    enemyKillScore_temp = 0;
                }

                break;
        }
    }

    private void UpdateScore(object sender, EventArgs e)
    {
        //Debug.Log("update score");
        FindObjectOfType<UIManager>().UpdateKillScore(enemyKillScore, bossKillScore);
    }

    private IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(EnemySpawnInterval);

        float enemyIntensity = Random.Range(0f, 1f);
        //Transform spawnPoint = spawnPoints[stageNum].transforms[Random.Range(0, spawnPoints[stageNum].transforms.Count)];

        Vector3 spawnPoint = GenerateRandomPosition();
        //Debug.Log(spawnPoint);
        EnemySpawnPool enemySpawnPool = enemySpawnPools[(int)Random.Range(0f, enemySpawnPools.Length)];
        PoolObject enemyPoolObject = enemySpawnPool.GetEnemyPoolObject(spawnPoint);
        Enemy enemy = enemyPoolObject.GetComponent<Enemy>();


        float speed = Mathf.Lerp(enemy.enemyScriptableObject.enemySpeedMax, enemy.enemyScriptableObject.enemySpeedMin, enemyIntensity);
        enemy.SetUp(enemy.enemyScriptableObject.enemyHealth, speed);

        spawnedEnemy++;

        //TESTCODE
        enemy.OnDeath += () => killCount++;
        enemy.OnDeath += () => enemyKillScore++;
        enemy.OnDeath += () => enemyKillScore_temp++;

        //killCount가 스테이지 클리어 조건을 만족하면, 스테이지 클리어 bool true.
        /*if(killCount == spawnCount)
        {
            //boss.SetActive(true);
            Debug.Log("stage end");
            isStageEnd = true;
        }*/

        coroutine = spawnEnemy();

        StartCoroutine(coroutine);
    }

    private Vector3 GenerateRandomPosition()
    {
        Vector3 position = new Vector3();

        /*
        float f = (UnityEngine.Random.value > 0.5f) ? -1f : 1f;
        if(UnityEngine.Random.value>0.5f)
        {
            position.x = UnityEngine.Random.Range(-spawnArea.x, spawnArea.x);
            position.y = spawnArea.y * f;
        }
        else
        {
            position.y= UnityEngine.Random.Range(-spawnArea.y, spawnArea.y);
            position.x = spawnArea.x * f;
        }
        */
        int randint;
        randint = UnityEngine.Random.Range(0, 4);
        if(randint == 0) //x 랜덤 y0
        {
            position.x = UnityEngine.Random.Range(spawnPoints[stageNum].transforms[0].position.x, spawnPoints[stageNum].transforms[1].position.x);
            position.y = spawnPoints[stageNum].transforms[0].position.y;
        }
        else if (randint == 1)//x 랜덤 y1
        {
            position.x = UnityEngine.Random.Range(spawnPoints[stageNum].transforms[0].position.x, spawnPoints[stageNum].transforms[1].position.x);
            position.y = spawnPoints[stageNum].transforms[1].position.y;
        }
        else if (randint == 2)//x0 y랜덤
        {
            position.x = spawnPoints[stageNum].transforms[0].position.x;
            position.y = UnityEngine.Random.Range(spawnPoints[stageNum].transforms[1].position.y, spawnPoints[stageNum].transforms[0].position.y);
        }
        else//x1 y랜덤
        {
            position.x = spawnPoints[stageNum].transforms[1].position.x;
            position.y = UnityEngine.Random.Range(spawnPoints[stageNum].transforms[1].position.y, spawnPoints[stageNum].transforms[0].position.y);
        }
        position.z = 0f;

        return position;
    }

    public void StartMethod()
    {
        //initialize
        killCount = 0;
        spawnedEnemy = 0;
        spawnCount = spawnCountList[stageNum];
        //Debug.Log("Start Method");
        isStageChanged = true;
        //StartCoroutine(coroutine);
    }
    public void StopMethod()
    {
        //Debug.Log("Function activated");
        StopCoroutine(coroutine);
    }

}

