// https://sourcemaking.com/design_patterns/object_pool
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum PoolType
{
    Melee,
    SmallMelee,
    Robot,
    Kamikace,
    C3PO,
    Turret,
    Humanoid
}*/


public class ObjectPool : MonoSingleton<ObjectPool>
{
    [Header("Prefab Settings")]
    public GameObject prefab;
    public List<PoolObject> listPoolObject;
    //public PoolType poolType;

    [SerializeField] private int iCount = 5;

    private void Awake()
    {
        CreatePool();
    }

    // 오브젝트 풀을 미리생성한다.
    public virtual void CreatePool() 
    {
        
        if(!prefab.TryGetComponent(out PoolObject poolObject))
        {
            prefab.AddComponent<PoolObject>();
        }
        
        for (int i=0; i<iCount; i++)
        {
            
            CreatePoolObject();
            
        }

    }

    // 풀 에서, 사용 가능한 오브젝트를 가져온다.
    /*public GameObject GetPoolObject() 
    {
        listPoolObject[0].gameObject.SetActive(true);
        return listPoolObject[0].GetComponent<GameObject>();
    }*/
    public virtual PoolObject GetPoolObject()
    {
        if(listPoolObject.Count == 0)
        {
            CreatePoolObject();
        }

        listPoolObject[0].gameObject.SetActive(true);
        PoolObject targetPoolObject = listPoolObject[0];
        listPoolObject.Remove(listPoolObject[0]);
        return targetPoolObject;

    }

    // 풀에 사용이 끝난 오브젝트를 다시 집어 넣는다.
    public virtual void PushPoolObject(GameObject obj) 
    {
        listPoolObject.Add(obj.GetComponent<PoolObject>());
        obj.transform.SetParent(this.transform);
        //obj.SetActive(false);
    }

    public PoolObject CreatePoolObject()
    {
        GameObject newGameObject = GameObject.Instantiate(prefab, this.transform) as GameObject;
        PoolObject newPoolObject = newGameObject.GetComponent<PoolObject>();
        newPoolObject.pool = this;
        newPoolObject.gameObject.SetActive(false);
        listPoolObject.Add(newPoolObject);
        return newPoolObject;
    }
}





