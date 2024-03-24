using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Observerable<T>
{
    [SerializeField] T m_value;
    public T Value
    {
        get => m_value; 
        set
        {
            m_value = value;
            Invoke();
        }
    }
    [SerializeField] UnityEvent<T> OnValueChanged;

    public Observerable(T value, UnityAction<T> callback = null)
    {
        OnValueChanged = new UnityEvent<T>();

        if (callback != null)
            OnValueChanged.AddListener(callback);

        Value = value;
    }

    public void AddListener(UnityAction<T> callback)
    {
        if (callback == null || OnValueChanged == null) return;

        OnValueChanged.AddListener(callback);
    }

    public void RemoveListener(UnityAction<T> callback)
    {
        if (callback == null || OnValueChanged == null) return;
        
        OnValueChanged.RemoveListener(callback);
    }

    public void RemoveAllListeners()
    {
        if (OnValueChanged == null) return;
        
        OnValueChanged.RemoveAllListeners();
    }

    public void Dispose()
    {
        RemoveAllListeners();
        Value = default;
        OnValueChanged = null;
    }

    void Invoke()
    {
        OnValueChanged?.Invoke(m_value);
    }

    public static implicit operator T(Observerable<T> observer) => observer.Value;
}

