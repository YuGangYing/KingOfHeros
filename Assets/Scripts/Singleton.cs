using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Singleton<T> where T : class, new()
{
    static T this_obj;

    public static T me
    {
        get
        {
            if (this_obj == null)
                CreateInstance();
            return this_obj;
        }
    }

    public static void CreateInstance()
    {
        if (this_obj != null)
            return;

        this_obj = new T();
    }

    public static void ReleaseInstance()
    {
        this_obj = default(T);
        this_obj = null;
    }
}

public abstract class SingletonMonoBehaviourNoCreate<T> : MonoBehaviour where T : SingletonMonoBehaviourNoCreate<T>
{
    private static T this_obj = null;
    public static T me
    {
        get
        {
            return this_obj;
        }
    }

    void Awake()
    {
        if (this_obj==null)
        {
            DontDestroyOnLoad(this.gameObject);
            this_obj = this as T;
            this_obj.Init();
        }
    }

    protected virtual void Init()
    {

    }

    private void OnApplicationQuit()
    {
        //this_obj = null;
    }

    public static void ReleaseInstance()
    {
        if (this_obj != null)
        {
            Destroy(this_obj.gameObject);
            this_obj = null;
        }
    }
}

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T this_obj = null;
 
    public static T me
    {
        get
        {
            CreateInstance();
            return this_obj;
        }
    }

    private void Awake()
    {
        if (this_obj == null)
        {
            this_obj = this as T;
            this_obj.Init();
        }
    }

    protected virtual void Init()
    {

    }

    private void OnApplicationQuit()
    {
        this_obj = null;
    }

    public static void CreateInstance()
    {
        if (this_obj != null)
            return;

        T[] managers = GameObject.FindObjectsOfType(typeof(T)) as T[];
        if (managers.Length != 0)
        {
            if (managers.Length == 1)
            {
                this_obj = managers[0];
                this_obj.gameObject.name = typeof(T).Name;
                DontDestroyOnLoad(this_obj.gameObject);
                return;
            }
            else
            {
                Logger.LogError("You have more than one " + typeof(T).Name + " in the scene. You only need 1, it's a singleton!");
                foreach (T manager in managers)
                {
                    Destroy(manager.gameObject);
                }
            }
        }

        GameObject gO = new GameObject(typeof(T).Name, typeof(T));
        this_obj = gO.GetComponent<T>();
        this_obj.Init();
        DontDestroyOnLoad(gO);
    }

    public static void ReleaseInstance()
    {
        if (this_obj != null)
        {
            Destroy(this_obj.gameObject);
            this_obj = null;
        }
    }
}