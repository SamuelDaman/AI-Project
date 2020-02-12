using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    public GameObject target;
    Rigidbody rb;
    Vector3 velocity;
    public float speed = 5f;
    float maxVelocity = 10f;
    float maxForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = (target.transform.position - transform.position).normalized;
        velocity = v * speed;
        rb.velocity += velocity;
        rb.AddForce(Mathf.Clamp(velocity.x, -maxForce, maxForce), Mathf.Clamp(velocity.y, -maxForce, maxForce), Mathf.Clamp(velocity.z, -maxForce, maxForce));
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxVelocity, maxVelocity), Mathf.Clamp(rb.velocity.y, -maxVelocity, maxVelocity), Mathf.Clamp(rb.velocity.z, -maxVelocity, maxVelocity));
        //transform.position += velocity * Time.deltaTime;
        Vector3 q = Vector3.MoveTowards(transform.position, v, 0.01f);
        transform.rotation = Quaternion.LookRotation(v);
    }
}
