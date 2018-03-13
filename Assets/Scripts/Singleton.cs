using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : Component{

    private static T t;

    public static T instance
    {
        get
        {
            if (t == null)
            {               
                t = FindObjectOfType<T>();
                if (t == null)
                {
                    t = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }

                t.Init();
                return t;
            }

            return t;
        }
    }

}
