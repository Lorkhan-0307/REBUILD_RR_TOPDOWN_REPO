// https://sourcemaking.com/design_patterns/object_pool

public class ObjectPool : MonoSingleton<ObjectPool>
{
    public GameObject prefab;
    
    public List<PoolObject> listPoolObject;

    // 오브젝트 풀을 미리생성한다.
    public void CreatePool() {}

    // 풀 에서, 사용 가능한 오브젝트를 가져온다.
    public GameObject GetPoolObject() { return ... }

    // 풀에 사용이 끝난 오브젝트를 다시 집어 넣는다.
    public void PushPoolObject(GameObject obj) {}
}

public class PoolObject : MonoBehaviour
{
    // 이 오브젝트가 어느 풀에서 나왔는지 레퍼런스를 이 오브젝트를 가져오는 시점에 assign 해준다.
    public ObjectPool pool;

    // 풀이 사용이 끝나면 오브젝트 풀에 다시 돌려 준다.
    void ReturnToPool() {}
}