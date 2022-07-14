using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private float EnemySpawnInterval = 3.5f;


    void Start()
    {
        StartCoroutine(spawnEnemy(EnemySpawnInterval, EnemyPrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject EnemyPrefab)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(EnemyPrefab, new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, EnemyPrefab));
    }

}
