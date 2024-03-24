using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FabrikSolver : MonoBehaviour
{
    public List<Vector3> Points { get; protected set; } = new();
    
    [SerializeField] List<float> lengths = new();
    [SerializeField] Vector3 target;
    [SerializeField] int attemptsPerFrame = 10;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        //if (target == null) return;

        lengths ??= new List<float>();
        Points = new List<Vector3>
        {
            transform.position
        };

        float sum = 0;
        foreach (var l in lengths)
        {
            sum += l;
            Points.Add(new Vector3(sum, 0, 0));
        }
    }

    private void Update()
    {
        if ((transform.position - target).magnitude > lengths.Sum())
            ProcessFabrik();
        else
            for (int i = 0; i < attemptsPerFrame; i++)
                ProcessFabrik();
    }

    public void ProcessFabrik()
    {
        if (target == null) return;

        ProcessFabrikBackward();
        ProcessFabrikForward();
    }

    public void ProcessFabrikForward()
    {
        Points[0] = transform.position;

        for (int i = 0; i < Points.Count - 1; i++)
        {
            Vector3 current = Points[i];
            Vector3 next = Points[i + 1];
            Vector3 dir = (next - current).normalized;
            Points[i + 1] = current + dir * lengths[i];
        }
    }

    public void ProcessFabrikBackward()
    {
        Points[^1] = target;

        for (int i = Points.Count - 1; i > 0; i--)
        {
            Vector3 current = Points[i];
            Vector3 prev = Points[i - 1];
            Vector3 dir = (prev - current).normalized;

            Points[i - 1] = current + dir * lengths[i - 1];
        }
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    private void OnValidate()
    {
        Init();
    }

    private void OnDrawGizmos()
    {
        if (target == null) return;

        for (int i = 0; i < attemptsPerFrame; i++)
            ProcessFabrik();

        for (int i = 0; i < Points.Count -1; i++)
        {
            Vector3 current = Points[i];
            Vector3 next = Points[i + 1];
            Gizmos.DrawLine(current, next);
            Gizmos.DrawSphere(current, 0.1f);
        }
    }

    public Vector3 GetTarget()
    {
        return target;
    }
}
