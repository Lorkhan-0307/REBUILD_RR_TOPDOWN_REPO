using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public PoolObject poolObject;

    // �� ������Ʈ�� ��� Ǯ���� ���Դ��� ���۷����� �� ������Ʈ�� �������� ������ assign ���ش�.
    public ObjectPool pool;

    private void Awake()
    {
        poolObject = this.GetComponent<PoolObject>();
        pool = GetComponentInParent<ObjectPool>();
        StartCoroutine(DestroyInFive());
    }

    private IEnumerator DestroyInFive()
    {
        yield return new WaitForSeconds(5f);
        ReturnToPool();
    }

    // Ǯ�� ����� ������ ������Ʈ Ǯ�� �ٽ� ���� �ش�.
    void ReturnToPool()
    {
        pool.PushPoolObject(this.gameObject);
    }
}
