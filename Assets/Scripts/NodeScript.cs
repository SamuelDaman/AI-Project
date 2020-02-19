using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    public struct Node
    {
        public Vector3 location;
        public GameObject north;
        public GameObject south;
        public GameObject east;
        public GameObject west;
    }

    List<List<Node>> graph;
    public Vector2Int graphSize = new Vector2Int(3, 3);

    public GameObject tile;

    private void Start()
    {
        graph = new List<List<Node>>(graphSize.y);
    }
}
