using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnPool : ObjectPool
{
    public override void CreatePool()
    {
        base.CreatePool();
    }

    public override PoolObject GetPoolObject()
    {
        return base.GetPoolObject();
    }

    public override void PushPoolObject(GameObject obj)
    {
        base.PushPoolObject(obj);
        //obj.GetComponent<SpriteRenderer>().enabled = false;
        obj.GetComponent<Enemy>().enabled = false;
        obj.SetActive(false);
    }

    public PoolObject GetEnemyPoolObject(Transform targetTransform)
    {
        PoolObject newPoolObject = GetPoolObject();
        newPoolObject.gameObject.transform.position = targetTransform.position;
        return newPoolObject;
    }
}
