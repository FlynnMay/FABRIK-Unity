using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public static class ExtensionMethods
{
    public static void ExecuteAfterFrames(this MonoBehaviour mono, int delay, Action action)
    {
        mono.StartCoroutine(ExecuteAfterFramesCoroutine(delay, action));
    }
    public static void ExecuteAfterSeconds(this MonoBehaviour mono, float delay, Action action)
    {
        mono.StartCoroutine(ExecuteAfterSecondsCoroutine(delay, action));
    }

    private static IEnumerator ExecuteAfterFramesCoroutine(int delay, Action action)
    {
        for (int i = 0; i < delay; i++)
            yield return null;

        action?.Invoke();
    }
    private static IEnumerator ExecuteAfterSecondsCoroutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public static Vector3 With(this Vector3 vector3, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vector3.x, y ?? vector3.y, z ?? vector3.z);
    }

    public static Vector2 ToVector2(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    public static Vector3 ToVector3(this Vector2 vector2, float y = 0)
    {
        return new Vector3(vector2.x, y, vector2.y);
    }

    public static Vector2Int RoundToVector2Int(this Vector2 vector3)
    {
        return new Vector2Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));
    }

    /// <summary>
    /// Adds the <paramref name="call"/> action to the <paramref name="currentEvent"/> and removes it after the first time it is called.
    /// </summary>
    /// <param name="currentEvent"></param>
    /// <param name="call"></param>
    public static void AddTemporaryListener(this UnityEvent currentEvent, UnityAction call)
    {
        currentEvent.AddListener(call);
        currentEvent.AddListener(() => currentEvent.RemoveListener(call));
    }
    
    /// <summary>
    /// Removes the temporary <paramref name="call"/> action from <paramref name="currentEvent"/> even if it hasn't been called before.
    /// </summary>
    /// <param name="currentEvent"></param>
    /// <param name="call"></param>
    public static void RemoveTemporaryListener(this UnityEvent currentEvent, UnityAction call)
    {
        currentEvent.RemoveListener(call);
        currentEvent.RemoveListener(() => currentEvent.RemoveListener(call));
    }

    public static GameObject[] GetActiveChildren(this Transform parent)
    {
        return parent.GetChildren(c => c.activeInHierarchy);
    }
    
    public static GameObject[] GetChildren(this Transform parent, Predicate<GameObject> condition = null)
    {
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            bool usingCondition = condition != null;
            if (!usingCondition || condition.Invoke(child.gameObject))
                children.Add(child.gameObject);
        }
        return children.ToArray();
    }

    public static T GetOrAddComponent<T>(this GameObject current) where T : Component
    {
        if(!current.TryGetComponent<T>(out var component)) component = current.AddComponent<T>();
        
        return component;
    }

    public static T OrNull<T> (this T obj) where T : Object
    {
        return obj ? obj : null;
    }
}

