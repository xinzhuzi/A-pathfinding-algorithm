using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public static class AStar
{

    public static PriorityQueue openQueue, closeQueue;

    /// <summary>
    /// 从一个起点,到一个目标,寻找到一个最短的路径
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    public static ArrayList FindPath(Node start,Node goal)
    {
        ///从第一开始的起点计算成本值
        openQueue = new PriorityQueue();
        start.nodeTotalCost = 0.0f;
        start.estimatedCost = NodeCost(start, goal);
        openQueue.Push(start);

        closeQueue = new PriorityQueue();
        Node node = null;
        while (openQueue.Length != 0)
        {
            node = openQueue.First();
            if (node.position == goal.position)
            {
                return CalculatePath(node);
            }
            /// 找到周围的邻居网格
            ArrayList neighbours = new ArrayList();
            GridManager.Instance.GetNeighbours(node, neighbours);

            for (int i = 0; i < neighbours.Count; i++)
            {
                Node neighbour = (Node)neighbours[i];
                ///如果关闭的邻居节点和开放的邻居节点都已经包含在里面,则进入下一个循环
                if (closeQueue.Contains(neighbour) || openQueue.Contains(neighbour)) continue;
                ///计算邻居的成本
                ///2个node之间的总成本,就是由node的成本  加上  node走到neighbour的成本 即表示neighbour的总成本
                float cost = NodeCost(node, neighbour);
                float totalCost = node.nodeTotalCost + cost;
                neighbour.nodeTotalCost = totalCost;
                ///从邻居的节点到最终目标的节点,表示为估算的节点
                float neighbourEstCost = NodeCost(neighbour, goal);
                neighbour.estimatedCost = neighbourEstCost + totalCost;
                neighbour.parent = node;
                ///周围的邻居计算完成本之后,加入队列中
                openQueue.Push(neighbour);
            }

            ///将当前起点加入关闭的优先级队列里面
            closeQueue.Push(node);
            ///移除当前起点
            openQueue.Remove(node);
        }
        Debug.Log(node.position + "  " + goal.position);

        if (node.position != goal.position)
        {
            Debug.LogError("没有找到最终目标"); 
            return null;
        }
        return CalculatePath(node);
    }
    /// <summary>
    /// 装载最短距离的寻路节点,然后返回
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private static ArrayList CalculatePath(Node node)
    {
        ArrayList paths = new ArrayList();
        while (node != null)
        {
            paths.Add(node);
            node = node.parent;
        }
        paths.Reverse();
        return paths;
    }

    /// <summary>
    /// 从一个点到另一个点的模,距离大小
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static float NodeCost(Node a,Node b)
    {
        return (a.position - b.position).magnitude;
    }
}
