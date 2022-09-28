using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolObject : MonoBehaviour
{
    public EnemyPoolObject enemyPoolObject;
    public EnemySpawnPoolController enemySpawnPool;

    private void Awake()
    {
        enemyPoolObject = this.GetComponent<EnemyPoolObject>();
    }
    private void ReturnToPool()
    {
        //enemySpawnPool.PushPoolObject(this.gameObject);
    }



}
