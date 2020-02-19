using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    GameObject[] boids;
    float speed = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        boids = GameObject.FindGameObjectsWithTag("Boid");
        foreach (GameObject boid in boids)
        {
            boid.transform.position = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            boid.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v1, v2, v3 = new Vector3();
        foreach (GameObject boid in boids)
        {
            v1 = Cohesion(boid);
            v2 = Separation(boid);
            v3 = Alignment(boid);
            Vector3 v = ((v1 + v2 + v3) - boid.transform.position).normalized;
            float angle = Mathf.Atan2(v.x, v.z) * (180 / Mathf.PI);
            Quaternion targetAngle = Quaternion.Euler(0, angle, 0);

            boid.GetComponent<Rigidbody>().velocity = boid.transform.TransformDirection(0, 0, speed);
            boid.transform.rotation = Quaternion.RotateTowards(boid.transform.rotation, targetAngle, 0.5f);
            //boid.transform.rotation = Quaternion.LookRotation(v, boid.transform.up);

            if (boid.transform.position.x > 15)
            {
                boid.transform.position = new Vector3(-14, 0, boid.transform.position.z);
            }
            if (boid.transform.position.x < -15)
            {
                boid.transform.position = new Vector3(14, 0, boid.transform.position.z);
            }

            if (boid.transform.position.z > 15)
            {
                boid.transform.position = new Vector3(boid.transform.position.x, 0, -14);
            }
            if (boid.transform.position.z < -15)
            {
                boid.transform.position = new Vector3(boid.transform.position.x, 0, 14);
            }
        }
    }

    Vector3 Separation(GameObject boid)
    {
        Vector3 c = new Vector3();
        foreach (GameObject boi in boids)
        {
            if (boi != boid)
            {
                if (Vector3.Distance(boi.transform.position, boid.transform.position) < 30)
                {
                    c -= (boi.transform.position - boid.transform.position).normalized / (Vector3.Distance(boi.transform.position, boid.transform.position) * Vector3.Distance(boi.transform.position, boid.transform.position));
                }
            }
        }
        return c * 30;
    }

    Vector3 Alignment(GameObject boid)
    {
        Vector3 v = new Vector3();
        foreach (GameObject boi in boids)
        {
            if (boi != boid)
            {
                v += boid.GetComponent<Rigidbody>().velocity;
            }
        }
        v /= boids.Length - 1;
        return (v - boid.GetComponent<Rigidbody>().velocity).normalized * 5;
    }

    Vector3 Cohesion(GameObject boid)
    {
        Vector3 c = new Vector3();
        foreach (GameObject boi in boids)
        {
            if (boi != boid)
            {
                c += boi.transform.position;
            }
        }
        c /= boids.Length - 1;
        return (c - boid.transform.position).normalized / 100;
    }
}
