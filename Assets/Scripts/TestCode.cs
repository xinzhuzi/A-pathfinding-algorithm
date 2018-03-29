using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public GameObject startCube, goalCube;

    private Node startNode, goalNode;

    private ArrayList nodePaths;
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("FindPath", 1.0f, 1.0f);
        FindPath();
    }

    private void FindPath()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.name=="StartCube" || child.name=="EndCube") continue;
            Destroy(child);
        }
        startNode = new Node(startCube.transform.position);
        goalNode = new Node(goalCube.transform.position);
        nodePaths = AStar.FindPath(startNode, goalNode);
        
    }

    private void OnDrawGizmos()
    {
        if (nodePaths != null && nodePaths.Count > 0)
        {
            
            for (int i = 0; i < nodePaths.Count-1; i++)
            {
               
                Node n = (Node)nodePaths[i];
                Debug.DrawLine(n.position, ((Node)nodePaths[i + 1]).position, Color.red);
                //GameObject obj = Instantiate(startCube, transform);
                //obj.transform.localPosition = new Vector3(n.position.x, -0.5f, n.position.z);
            }
        }
    }

}
