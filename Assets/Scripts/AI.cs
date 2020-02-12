using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public Transform target;
    Rigidbody body;
    Vector3 v;
    Vector3 a;
    Vector3 w;
    Vector3 wanderPoint;
    float speed = 5f;
    public LayerMask obstacles;

    public bool AIPursuit;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        StartCoroutine("SetValue");
    }

    // Update is called once per frame
    void Update()
    {
        v = (target.position - transform.position).normalized;
        if (Avoid() != new Vector3())
        {
            a += Avoid();
        }
        else if (a.sqrMagnitude > 0)
        {
            a -= a / 60;
        }
        if (a.magnitude < 1)
        {
            a = new Vector3(0, 0, 0);
        }
        if (AIPursuit == true)
        {
            body.velocity = Pursue() + Arrival();
        }
        else if (AIPursuit == false)
        {
            if (Vector3.Distance(target.position, transform.position) < 10)
            {
                body.velocity = Evade() + Wander() + Avoid();
            }
            else
            {
                w = (wanderPoint - transform.position).normalized;
                body.velocity = Wander() + Avoid();
            }
            Debug.DrawLine(transform.position, wanderPoint, Color.grey);
        }
        Debug.DrawRay(transform.position, body.velocity, Color.green);
        Debug.DrawLine(transform.position, target.position, Color.grey);
    }

    void PursueRotateAI()
    {
        float angle = Mathf.Atan2(v.x + a.x, v.z + a.z) * (180/Mathf.PI);
        Quaternion targetAngle = Quaternion.Euler(0, angle, 0);
        float turnSpeed = Mathf.Abs(targetAngle.eulerAngles.y - transform.eulerAngles.y) / 100;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, turnSpeed);
    }
    void EvadeRotateAI()
    {
        float angle = Mathf.Atan2(v.x + a.x, v.z + a.z) * (180 / Mathf.PI);
        Quaternion targetAngle = Quaternion.Euler(0, angle, 0);
        float turnSpeed = -Mathf.Abs(targetAngle.eulerAngles.y - transform.eulerAngles.y) / (Vector3.Distance(target.position, transform.position) * 100);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, turnSpeed);
    }
    void WanderRotateAI()
    {
        float angle = Mathf.Atan2(w.x + a.x, w.z + a.z) * (180 / Mathf.PI);
        Quaternion targetAngle = Quaternion.Euler(0, angle, 0);
        float turnSpeed = Mathf.Abs(targetAngle.eulerAngles.y - transform.eulerAngles.y) / 100;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, turnSpeed);
    }

    Vector3 Pursue()
    {
        PursueRotateAI();
        return (transform.TransformDirection(0, 0, speed) + (v * speed)) / 1.5f;
    }

    Vector3 Evade()
    {
        EvadeRotateAI();
        return (transform.TransformDirection(0, 0, speed) - (v * speed)) / 1.5f;
    }

    Vector3 Arrival()
    {
        return -(v / Vector3.Distance(target.position, transform.position)) * 20;
    }

    Vector3 Avoid()
    {
        for (int i = 0; i < 2; i++)
        {
            RaycastHit hit;
            if (i == 0)
            {
                Debug.DrawRay(transform.position, (transform.forward * 4) + transform.right, Color.yellow);
                if (Physics.Raycast(transform.position, (transform.forward * 4) + transform.right, out hit, 4, obstacles))
                {
                    return -((transform.right * 3) / Vector3.Distance(transform.position, hit.point)) * speed;
                }
                Debug.DrawRay(transform.position, (transform.forward * 4) - transform.right, Color.yellow);
                if (Physics.Raycast(transform.position, (transform.forward * 4) - transform.right, out hit, 4, obstacles))
                {
                    return ((transform.right * 3) / Vector3.Distance(transform.position, hit.point)) * speed;
                }
                Debug.DrawRay(transform.position, (transform.forward * 3) - transform.up + transform.right, Color.yellow);
                if (!Physics.Raycast(transform.position, (transform.forward * 3) - transform.up + transform.right, out hit, obstacles))
                {
                    return -(transform.right) * speed;
                }
                Debug.DrawRay(transform.position, (transform.forward * 3) - transform.up - transform.right, Color.yellow);
                if (!Physics.Raycast(transform.position, (transform.forward * 3) - transform.up - transform.right, out hit, obstacles))
                {
                    return (transform.right) * speed;
                }
            }
            else if (i == 1)
            {
                Debug.DrawRay(transform.position, (transform.forward * 5), Color.yellow);
                if (Physics.Raycast(transform.position, (transform.forward * 5), out hit, 5, obstacles))
                {
                    return -((transform.forward * 5) / Vector3.Distance(transform.position, hit.point)) * speed;
                }
                Debug.DrawRay(transform.position, (transform.forward * 4) - transform.up, Color.yellow);
                if (!Physics.Raycast(transform.position, (transform.forward * 4) - transform.up, out hit, obstacles))
                {
                    return -(transform.forward * 5) * speed;
                }
            }
        }
        return new Vector3();
    }

    Vector3 WanderRandomize()
    {
        return new Vector3(Random.Range(-15, 15), 0, Random.Range(-15, 15));
    }

    Vector3 Wander()
    {
        WanderRotateAI();
        return (transform.TransformDirection(0, 0, speed) + (w * speed)) / 2;
    }

    IEnumerator SetValue()
    {
        wanderPoint = WanderRandomize();
        yield return new WaitForSeconds(2);
        StartCoroutine("SetValue");
    }
}