// https://sourcemaking.com/design_patterns/object_pool
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoSingleton<ObjectPool>
{
    [Header("Prefab Settings")]
    public GameObject prefab;
    public GameObject[] GameObjects;

    
    public List<PoolObject> listPoolObject;

    [SerializeField] private int iEnemyCount = 5;

    private void Awake()
    {
        CreatePool();
    }

    private void Update()
    {

    }

    // 오브젝트 풀을 미리생성한다.
    public virtual void CreatePool() 
    {
        
        if(!prefab.TryGetComponent(out PoolObject poolObject))
        {
            prefab.AddComponent<PoolObject>();
        }
        
        for (int i=0; i<iEnemyCount; i++)
        {
            GameObject newGameObject = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;


            listPoolObject.Add(newGameObject.GetComponent<PoolObject>());
            newGameObject.gameObject.SetActive(false);
            newGameObject.transform.SetParent(transform);
            newGameObject.name = "TestPrefab" + i.ToString();
        }

    }

    // 풀 에서, 사용 가능한 오브젝트를 가져온다.
    /*public GameObject GetPoolObject() 
    {
        listPoolObject[0].gameObject.SetActive(true);
        return listPoolObject[0].GetComponent<GameObject>();
    }*/
    public virtual void GetPoolObject()
    {
        if(listPoolObject.Count == 0)
        {
            GameObject newGameObject = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            listPoolObject.Add(newGameObject.GetComponent<PoolObject>());
            newGameObject.gameObject.SetActive(false);
            newGameObject.transform.SetParent(transform);
        }
        listPoolObject[0].gameObject.SetActive(true);
        listPoolObject.Remove(listPoolObject[0]);
        
    }

    // 풀에 사용이 끝난 오브젝트를 다시 집어 넣는다.
    public virtual void PushPoolObject(GameObject obj) 
    {
        listPoolObject.Add(obj.GetComponent<PoolObject>());
        obj.SetActive(false);
    }
}





