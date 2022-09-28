using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = (GameObject)GameObject.FindObjectOfType(typeof(T));
                if (obj == null)
                {
                    var instanceObject = new GameObject();
                    instanceObject.name = "(Singleton)"+typeof(T).ToString();
                    instance = instanceObject.AddComponent<T>();
                }
                else
                {
                    instance = (T)obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }
    private static T instance;
}