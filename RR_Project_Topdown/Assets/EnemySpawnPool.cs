using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnPool : ObjectPool
{
    [Header("Default Settings")]
    [SerializeField] private Enemy[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float EnemySpawnInterval = 3.5f;
    [SerializeField] private int spawnCount = 10;

    [SerializeField] private float meleeAttackEnemyHealth = 5f;
    [SerializeField] private float rangeAttackEnemyHealth = 5f;
    [SerializeField] private float speedMax = 3f;
    [SerializeField] private float speedMin = 1f;



    private List<Enemy> enemies = new List<Enemy>();
    private int spawnedEnemy;
    private float intensity;
    private IEnumerator coroutine;


    private void Awake()
    {
        CreatePool();
    }

    public override void CreatePool()
    {
        base.CreatePool();
    }

    public override void GetPoolObject()
    {
        base.GetPoolObject();
    }

    public override void PushPoolObject(GameObject obj)
    {
        base.PushPoolObject(obj);
    }

    private IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(EnemySpawnInterval);

        float enemyIntensity = Random.Range(0f, 1f);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Enemy enemyPrefab = enemyPrefabs[(int)Random.Range(0f, 2f)];
        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

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

        enemies.Add(enemy);

        enemy.OnDeath += () => enemies.Remove(enemy);
        enemy.OnDeath += () => Destroy(enemy.gameObject);

        coroutine = spawnEnemy();
        StartCoroutine(coroutine);
    }

    private void StopMethod()
    {
        Debug.Log("Function activated");
        StopCoroutine(coroutine);
    }

}
