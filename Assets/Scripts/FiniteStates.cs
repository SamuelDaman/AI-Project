using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStates : MonoBehaviour
{
    public enum States
    {
        Patrol,
        Seek,
        Flee
    }

    public Vector3[] patrolLocations;
    public int currentPatrolIndex;

    public Transform fleeTarget;
    public Transform seekTarget;

    private States currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = States.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case States.Patrol:
                Patrol();
                break;
            case States.Seek:
                Seek();
                break;
            case States.Flee:
                Flee();
                break;
            default:
                Debug.LogError("Invalid State");
                break;
        }
        foreach (Vector3 location in patrolLocations)
        {
            Debug.DrawRay(location, transform.up, Color.red);
        }
    }

    void FixedUpdate()
    {
        StartCoroutine("FleeCheck");
        if (currentState != States.Flee)
        {
            StartCoroutine("LineOfSight");
        }
    }

    void Patrol()
    {
        Vector3 v = (patrolLocations[currentPatrolIndex] - transform.position).normalized;
        float angle = Mathf.Atan2(v.x, v.z) * (180 / Mathf.PI);
        Quaternion targetAngle = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, 1);

        float dot = Vector3.Dot(transform.TransformDirection(0, 0, 1), v);

        transform.position += (transform.TransformDirection(0, 0, 1) + v).normalized / 20 * (dot + 1) / 2;

        if (Vector3.Distance(transform.position, patrolLocations[currentPatrolIndex]) < 1)
        {
            if (currentPatrolIndex != patrolLocations.Length - 1)
            {
                currentPatrolIndex++;
            }
            else
            {
                currentPatrolIndex = 0;
            }
        }
    }
    void Seek()
    {
        Vector3 v = (seekTarget.position - transform.position).normalized;
        float angle = Mathf.Atan2(v.x, v.z) * (180 / Mathf.PI);
        Quaternion targetAngle = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, 1);

        float dot = Vector3.Dot(transform.TransformDirection(0, 0, 1), v);

        transform.position += (transform.TransformDirection(0, 0, 1) + v).normalized / 20 * (dot + 1) / 2;
    }
    void Flee()
    {
        Vector3 v = -(seekTarget.position - transform.position).normalized;
        float angle = Mathf.Atan2(v.x, v.z) * (180 / Mathf.PI);
        Quaternion targetAngle = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, 1);

        float dot = Vector3.Dot(transform.TransformDirection(0, 0, 1), v);

        transform.position += (transform.TransformDirection(0, 0, 1) + v).normalized / 10 * (dot + 1) / (Vector3.Distance(transform.position, fleeTarget.position));
    }

    IEnumerator LineOfSight()
    {
        Vector3 v = (seekTarget.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.TransformDirection(0,0,1), v);
        Debug.DrawRay(transform.position, transform.TransformDirection(0, 0, 30), Color.green);
        Debug.DrawRay(transform.position, transform.TransformDirection(-1, 0, 1).normalized * 30, Color.grey);
        Debug.DrawRay(transform.position, transform.TransformDirection(1, 0, 1).normalized * 30, Color.grey);
        if (currentState == States.Patrol)
        {
            if (dot > 0.7f)
            {
                Debug.Log("Now Seeking " + dot);
                currentState = States.Seek;
            }
        }
        else if (currentState == States.Seek)
        {
            if (dot < 0.7f)
            {
                Debug.Log("Now Patrolling");
                currentState = States.Patrol;
            }
        }
        return null;
    }

    IEnumerator FleeCheck()
    {
        if (currentState != States.Flee)
        {
            if (Vector3.Distance(transform.position, fleeTarget.position) < 2)
            {
                Debug.Log("Now Fleeing");
                currentState = States.Flee;
            }
        }
        else if (Vector3.Distance(transform.position, fleeTarget.position) > 3)
        {
            Debug.Log("Now Patrolling");
            currentState = States.Patrol;
        }
        return null;
    }
}
