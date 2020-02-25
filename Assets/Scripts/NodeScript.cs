using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    Node[][] graph;
    public Vector2Int graphSize = new Vector2Int(3, 3);

    public GameObject tile;

    public Node start;
    private Node startCheck = new Node();
    public Node end;
    private Node endCheck = new Node();

    private void Start()
    {
        graph = new Node[graphSize.y][];
        for (uint i = 0; i < graph.Length; i++)
        {
            graph[i] = new Node[graphSize.x];
            for (uint j = 0; j < graph[i].Length; j++)
            {
                GameObject nodeTile = Instantiate(tile, new Vector3(j - (graphSize.x / 2), 0, i - (graphSize.y / 2)), new Quaternion());
                graph[i][j] = new Node(nodeTile, nodeTile.transform.position);
            }
        }
        for (int i = 0; i < graph.Length; i++)
        {
            for (int j = 0; j < graph[i].Length; j++)
            {
                // Set North Node
                if (i + 1 < graph.Length)
                {
                    graph[i][j].north = graph[i + 1][j];
                }
                else
                {
                    graph[i][j].north = null;
                }
                // Set South Node
                if (i - 1 > 0)
                {
                    graph[i][j].south = graph[i - 1][j];
                }
                else
                {
                    graph[i][j].south = null;
                }
                // Set East Node
                if (j + 1 < graph[i].Length)
                {
                    graph[i][j].east = graph[i][j + 1];
                }
                else
                {
                    graph[i][j].east = null;
                }
                // Set West Node
                if (j - 1 > 0)
                {
                    graph[i][j].west = graph[i][j - 1];
                }
                else
                {
                    graph[i][j].west = null;
                }
            }
        }
        start = graph[0][0];
        end = graph[2][2];
        DijkstrasAlgorithm(start, end);
    }

    //private void Update()
    //{
    //    if (startCheck != start || endCheck != end)
    //    {
    //        DijkstrasAlgorithm(start, end);
    //        startCheck = start;
    //        endCheck = end;
    //    }
    //}

    void DijkstrasAlgorithm(Node start, Node end)
    {
        Node cur;
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        openList.Add(start);
        int failSafe = 0;
        while (openList.Count != 0 && failSafe < 100)
        {
            cur = openList[0];
            openList.RemoveAt(0);
            closedList.Add(cur);
            
            if (cur.north != null)
            {
                cur.north.gScore += cur.gScore;
                cur.north.previous = cur;
                openList.Add(cur.north);
            }
            if (cur.south != null)
            {
                cur.south.gScore += cur.gScore;
                cur.south.previous = cur;
                openList.Add(cur.south);
            }
            if (cur.east != null)
            {
                cur.east.gScore += cur.gScore;
                cur.east.previous = cur;
                openList.Add(cur.east);
            }
            if (cur.west != null)
            {
                cur.west.gScore += cur.gScore;
                cur.west.previous = cur;
                openList.Add(cur.west);
            }

            for (int i = 0; i < openList.Count; i++)
            {
                bool isSorted = true;
                for (int j = i; j >= 0; j--)
                {
                    if (j != i)
                    {
                        if (openList[j].gScore > openList[i].gScore)
                        {
                            Node tempNode = openList[j];
                            openList[j] = openList[i];
                            openList[i] = tempNode;
                            isSorted = false;
                        }
                    }
                }
                if ((i == openList.Count - 1) && (isSorted == false))
                {
                    i = 0;
                }
            }

            if (cur == end)
            {

            }

            failSafe++;
        }
    }
}

public class Node
{
    public GameObject tile;
    public Vector3 location;
    public float gScore = 1;

    public Node north;
    public Node south;
    public Node east;
    public Node west;

    public Node previous;

    public Node()
    {
        tile = null;
        location = Vector3.zero;
        north = null;
        south = null;
        east = null;
        west = null;
    }
    public Node(GameObject nodeObject, Vector3 position)
    {
        tile = nodeObject;
        location = position;
        north = null;
        south = null;
        east = null;
        west = null;
    }
    public Node(Node copy)
    {
        tile = copy.tile;
        location = copy.location;
        north = copy.north;
        south = copy.south;
        east = copy.east;
        west = copy.west;
    }
}
