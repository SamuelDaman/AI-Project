using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    Rigidbody body;
    float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = transform.TransformDirection(0, 0, speed);
    }

    void Move()
    {
        
    }

    void Separation()
    {

    }

    void Alignment()
    {
        
    }

    void Cohesion()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Boid"))
        {
            Transform target = other.gameObject.transform;
            body.angularVelocity = (target.eulerAngles - transform.eulerAngles);
        }
    }
}
