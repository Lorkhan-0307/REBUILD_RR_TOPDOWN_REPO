using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KeyType = System.String;

public class EnemySpawnPoolController : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private EnemySpawnPool[] enemySpawnPools;
    [SerializeField] private float EnemySpawnInterval = 3.5f;
    [SerializeField] private int spawnCount = 10;

    [SerializeField] private float meleeAttackEnemyHealth = 5f;
    [SerializeField] private float rangeAttackEnemyHealth = 5f;
    [SerializeField] private float speedMax = 3f;
    [SerializeField] private float speedMin = 1f;


    [SerializeField] public GameObject boss;

    private int spawnedEnemy = 0;
    private float intensity;
    private IEnumerator coroutine;
    private int killCount = 0;

    void Start()
    {
        coroutine = spawnEnemy();
        StartCoroutine(coroutine);
    }

    private void Update()
    {
        //Debug.Log(spawnedEnemy);
        //Debug.Log(killCount);

        if (spawnedEnemy == spawnCount)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(EnemySpawnInterval);

        float enemyIntensity = Random.Range(0f, 1f);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        EnemySpawnPool enemySpawnPool = enemySpawnPools[(int)Random.Range(0f, enemySpawnPools.Length)];
        PoolObject enemyPoolObject = enemySpawnPool.GetEnemyPoolObject(spawnPoint);
        Enemy enemy = enemyPoolObject.GetComponent<Enemy>();
        //enemy.enabled = true;

        if (enemy.name == "R")
        {
            enemy.SetUp(rangeAttackEnemyHealth, speedMin);
        }
        else if (enemy.name == "M")
        {
            float speed = Mathf.Lerp(speedMin, speedMax, enemyIntensity);
            enemy.SetUp(meleeAttackEnemyHealth, speed);
        }

        spawnedEnemy++;


        //TESTCODE
        enemy.OnDeath += () => killCount++;
        if(killCount == 1)
        {
            boss.SetActive(true);
        }



        coroutine = spawnEnemy();

        StartCoroutine(coroutine);
    }
    private void StopMethod()
    {
        Debug.Log("Function activated");
        StopCoroutine(coroutine);
    }



}

