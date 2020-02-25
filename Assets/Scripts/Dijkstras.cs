using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstras : MonoBehaviour
{
    NodeScript nodeScript;

    List<Node> openList;
    List<Node> closedList;

    // Start is called before the first frame update
    void Start()
    {
        nodeScript = FindObjectOfType<NodeScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
