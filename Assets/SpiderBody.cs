using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class SpiderBody : MonoBehaviour
{
    [SerializeField] GameObject legPrefab;
    [SerializeField] int legCount = 4;
    [SerializeField, Range(0, 360)] float startingAngle;
    [SerializeField] float offset = 1;
    [SerializeField] float rayDistance = 10.0f;
    [SerializeField] float maxStepDistance = 0.5f;
    [SerializeField] float stepCooldown = 1;
    [SerializeField] float legSpreadRadius = 1;

    List<FabrikSolver> solvers = new List<FabrikSolver>();
    List<Vector3> raycastPositions = new List<Vector3>();

    bool canStep = true;

    int[][] indiciesToStepTogether;
    int stepIndex = 0;

    private void Awake()
    {
        float angleBetweenLegs = 360.0f / legCount;

        SetupSynchronizedSteps();

        for (int i = 0; i < legCount; i++)
        {
            GameObject leg = Instantiate(legPrefab, transform);
            FabrikSolver solver = leg.GetComponent<FabrikSolver>();

            float angleRad = Mathf.Deg2Rad * (startingAngle + angleBetweenLegs * i);
            Vector3 dir = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
            leg.transform.position = transform.position + dir * offset;

            solvers.Add(solver);
            raycastPositions.Add((dir * (offset + legSpreadRadius)).With(y: 1));
        }
    }

    private void SetupSynchronizedSteps()
    {

        if (legCount == 2)
        {
            indiciesToStepTogether = new int[2][];

            indiciesToStepTogether[0] = new int[1];
            indiciesToStepTogether[1] = new int[1];

            indiciesToStepTogether[0][0] = 0;
            indiciesToStepTogether[1][0] = 1;
            
            return;
        }

        int halfLegCount = legCount / 2;
        indiciesToStepTogether = new int[halfLegCount][];

        for (int i = 0; i < halfLegCount; i++)
        {
            indiciesToStepTogether[i] = new int[2];
            indiciesToStepTogether[i][0] = i;
            indiciesToStepTogether[i][1] = i + halfLegCount;
        }
    }

    private void Update()
    {
        //if (!canStep) return;

        var legIndicies = indiciesToStepTogether[stepIndex];

        for (int i = 0; i < legIndicies.Length; i++)
        {
            int index = legIndicies[i];
            var solver = solvers[index];
            var raycastPosition = raycastPositions[index];

            bool collided = Physics.Raycast(transform.position + raycastPosition, -transform.up, out RaycastHit hit, rayDistance);

            var currentTarget = solver.GetTarget();
            var newTarget = hit.point;

            if (!IsExceedingStepDistance(currentTarget, newTarget)) continue;

            if (collided)
                solver.SetTarget(hit.point);
            else
                solver.SetTarget(rayDistance * -transform.up);

            //canStep = false;
            //this.ExecuteAfterSeconds(stepCooldown, () => canStep = true);
            stepIndex++;
            stepIndex %= indiciesToStepTogether.Length;
        }
    }

    private bool IsExceedingStepDistance(Vector3 currentTarget, Vector3 newTarget)
    {
        return Vector3.Distance(currentTarget, newTarget) > maxStepDistance;
    }

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, offset);

        float angleBetweenLegs = 360.0f / legCount;

        for (int i = 0; i < legCount; i++)
        {
            float angleRad = Mathf.Deg2Rad * (startingAngle + angleBetweenLegs * i);
            Vector3 dir = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));

            Gizmos.color = i == 0 ? Color.red : Color.green;
            Gizmos.DrawSphere(transform.position + dir * offset, 0.1f);
        }
    }
}
