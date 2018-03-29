using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : IComparable
{

    /**
     * 计算路径公式  estimatedCost = estimatedCost + nodeTotalCost;
     * 成本使用 2点之间直线最短的计算方式进行计算
     */
    /// <summary>
    /// 总成本,表示 当前起始点 到 当前目标点的成本 加上 当前起始点的总成本 
    /// </summary>
    public float nodeTotalCost;
    /// <summary>
    /// 估算的成本,表示当前的总成本 加上 最终目标点的成本
    /// </summary>
    public float estimatedCost;

    /// <summary>
    /// 是不是障碍物,如果是障碍物,则不加入相邻节点计算
    /// </summary>
    public bool bObstacle;
    /// <summary>
    /// 父节点,表示单项链表,每一个node都有父节点,初始节点没有,为了最后找到最短节点路径
    /// 通过之后的节点一直向上遍历找到最终父节点为null的初始节点,找完最短路径.
    /// </summary>
    public Node parent;
    /// <summary>
    /// 每一个节点的位置,使用位置 计算成本距离
    /// </summary>
    public Vector3 position;

    public Node() : this(Vector3.zero) { }

    public Node(Vector3 position)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
        this.position = position;
    }
    /// <summary>
    /// 将一个节点变成障碍物
    /// </summary>
    public void MarkAsObstacle()
    {
        this.bObstacle = true;
    }
    /// <summary>
    /// 计算2个节点之间的成本大小,成本小的排在最前面
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    int IComparable.CompareTo(object obj)
    {
        Node objNode = obj as Node;
        if (this.estimatedCost < objNode.estimatedCost)
        {
            return -1;
        }
        else if (this.estimatedCost > objNode.estimatedCost)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
