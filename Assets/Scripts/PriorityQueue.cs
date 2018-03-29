using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 优先级队列,从小到大
/// </summary>
public class PriorityQueue 
{
    private ArrayList nodes = new ArrayList();

    public int Length
    {
        get { return this.nodes.Count; }
    }

    public bool Contains(Node node)
    {
        return this.nodes.Contains(node);
    }

    public Node First()
    {
        if (this.nodes.Count>0)
        {
            return ( Node )this.nodes[0];
        }
        return null;
    }

    public void Push(Node node)
    {
        this.nodes.Add(node);
        this.nodes.Sort();
    }

    public void Remove(Node node)
    {
        this.nodes.Remove(node);
        this.nodes.Sort();
    }

}
