using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public PoolObject poolObject;

    // 이 오브젝트가 어느 풀에서 나왔는지 레퍼런스를 이 오브젝트를 가져오는 시점에 assign 해준다.
    public ObjectPool pool;

    private void Awake()
    {
        poolObject = this.GetComponent<PoolObject>();
        pool = GetComponentInParent<ObjectPool>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            ReturnToPool();
        }
    }

    // 풀이 사용이 끝나면 오브젝트 풀에 다시 돌려 준다.
    void ReturnToPool()
    {
        pool.PushPoolObject(this.gameObject);
    }
}
