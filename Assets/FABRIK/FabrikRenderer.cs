using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabrikRenderer : MonoBehaviour
{
    [SerializeField] FabrikSolver solver;
    [SerializeField] LineRenderer lineRenderer;
    //[SerializeField] float speed = 4.0f;
    //Vector3[] points;

    private void Start()
    {
        //points = solver.Points.ToArray();
    }

    private void Update()
    {
        //for (int i = 0; i < points.Length; i++)
        //{
        //    Vector3 current = points[i];
        //    Vector3 target = solver.Points[i];

        //    if (Vector3.Distance(current, target) > .01f)
        //        points[i] = Vector3.MoveTowards(current, target, Time.deltaTime * speed);
        //    else
        //        points[i] = target;
        //}
    }

    private void OnEnable()
    {
        Application.onBeforeRender += UpdateRendererPositions;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= UpdateRendererPositions;
    }

    private void UpdateRendererPositions()
    {
        lineRenderer.positionCount = solver.Points.Count;
        lineRenderer.SetPositions(solver.Points.ToArray());
    }
}
