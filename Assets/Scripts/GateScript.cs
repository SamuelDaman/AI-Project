using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    public bool isClosed = true;
    private bool isMoving = false;
    Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != targetPosition)
        {
            isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.25f);
        }
        else
        {
            isMoving = false;
        }
    }

    public void OpenClose()
    {
        if (isMoving == false)
        {
            if (isClosed == true)
            {
                targetPosition = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
                isClosed = false;
            }
            else if (isClosed == false)
            {
                targetPosition = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
                isClosed = true;
            }
        }
    }
}
