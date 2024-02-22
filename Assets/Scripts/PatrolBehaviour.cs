using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float walkSpeed;
    private int currentPatrolPoint = 0;

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Vector3 moveToPoint = patrolPoints[currentPatrolPoint].position;
        transform.position = Vector3.MoveTowards(transform.position, moveToPoint, walkSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, moveToPoint) < 0.01f)
        {
            currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
        }
    }
}
