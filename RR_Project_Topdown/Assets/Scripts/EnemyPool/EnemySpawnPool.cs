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

    public override void GetPoolObject()
    {
        base.GetPoolObject();
    }

    public override void PushPoolObject(GameObject obj)
    {
        base.PushPoolObject(obj);
    }
}
