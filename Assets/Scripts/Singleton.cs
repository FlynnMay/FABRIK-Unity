using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance => instance;
    static T instance;

    private void Awake()
    {
        if (instance == null) 
            instance = GetComponent<T>();
        else
            Destroy(this);

        OnAwake();
    }

    protected virtual void OnAwake() { }
}
