using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    /// <summary>
    /// 单例字段
    /// </summary>
    private static GridManager s_Instance = null;
    /// <summary>
    /// 总共10行
    /// </summary>
    public int numOfRows;
    /// <summary>
    /// 总共10列
    /// </summary>
    public int numOfColumns;
    /// <summary>
    /// 每一个网格的边长,正方形
    /// </summary>
    public float gridCellSize;
    /// <summary>
    /// 是否展示这个网格
    /// </summary>
    public bool showGrid = true;

    /// <summary>
    /// 障碍物数据
    /// </summary>
    private GameObject[] Obstacles;
    /// <summary>
    /// 二维数据,有几行几列
    /// </summary>
    private Node[,] nodes;

    public static GridManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GridManager)) as GridManager;
                if (s_Instance == null)
                {
                    Debug.Log("没有实例,需要添加脚本到一个物体上");
                }
            }
            return s_Instance;
        }
    }

    // Use this for initialization
    void Awake()
    {
        s_Instance = this;
        Obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        CalculateObstacles();
    }

    /// <summary>
    /// 计算障碍物,填充节点
    /// </summary>
    private void CalculateObstacles()
    {
        ///节点容器
        nodes = new Node[numOfColumns, numOfRows];
        ///将容器充满Node
        for (int i = 0; i < numOfColumns; i++)
        {
            for (int j = 0; j < numOfRows; j++)
            {///节点实例填充
                Node n = new Node(GetGridCellCenter(i, j));
                nodes[i, j] = n;
            }
        }
        ///如果有障碍物,需要将nodes里面的节点转换成障碍物
        if (Obstacles.Length > 0)
        {
            foreach (GameObject item in Obstacles)
            {
                int column, row;
                GetCellIndex(item.transform.position, out column, out row);
                nodes[column, row].MarkAsObstacle();
            }
        }

    }
    /// <summary>
    ///  根据位置,找到所有cell所在的标号,从0-99的标号
    /// </summary>
    /// <param name="position"></param>
    /// <param name="z"></param>
    /// <param name="x"></param>
    public void GetCellIndex(Vector3 position, out int column, out int row)
    {
        column = (int)((position.z - (gridCellSize / 2)) / gridCellSize);
        row = (int)((position.x - (gridCellSize / 2)) / gridCellSize);
    }

    /// <summary>
    /// 根据下标,找到所有nodes的位置
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    private Vector3 GetGridCellCenter(int column, int row)
    {
        float x = (gridCellSize / 2) + row * gridCellSize;
        float z = (gridCellSize / 2) + column * gridCellSize;
        return new Vector3(x, 1, z);//因为所有的Y点都向上拔高了个1
    }
    /// <summary>
    /// 得到一个节点周围的8个节点
    /// </summary>
    /// <param name="node"></param>
    /// <param name="neighbours"></param>
    public void GetNeighbours(Node node,ArrayList neighbours)
    {
        Vector3 neighbourPos = node.position;
        int column, row;
        GetCellIndex(neighbourPos, out column, out row);
        /**
         * 周围的8个节点数据,添加到数组里面
         */

        //Bottom
        int bottomNodeRow = row - 1;
        int bottomNodeColumn = column;
        AssignNeighbour(bottomNodeColumn, bottomNodeRow,  neighbours);

        //top
        int topNodeRow = row + 1;
        int topNodeColumn = column;
        AssignNeighbour(topNodeColumn, topNodeRow, neighbours);

        //left
        int leftNodeRow = row ;
        int leftNodeColumn = column-1;
        AssignNeighbour(leftNodeColumn, leftNodeRow,  neighbours);

        //right
        int rightNodeRow = row;
        int rightNodeColumn = column + 1;
        AssignNeighbour(rightNodeColumn, rightNodeRow, neighbours);


    }
    /// <summary>
    /// 将node的上下左右,装进数组中
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="neighbours"></param>
    private void AssignNeighbour(int column, int row,  ArrayList neighbours)
    {
        if (row < 0 || row > 9 || column < 0 || column > 9) return;
        //Node node = nodes[column, row];
        Node node = nodes[column,row];
        if (node.bObstacle) return;
        neighbours.Add(node); 
    }

    /// <summary>
    /// 画出网格线
    /// </summary>
    private void OnDrawGizmos()
    {
        if (showGrid)
        {
            DebugDrawGrid(transform.position, numOfRows, numOfColumns, gridCellSize, Color.blue);
        }
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
    /// <summary>
    /// 坐标值在0.0.0点 向右为X正方向 向上为Z正方向 
    /// </summary>
    /// <param name="origin">网格原点</param>
    /// <param name="rows">网格第几行</param>
    /// <param name="columns">网格第几列</param>
    /// <param name="cellSize">网格大小</param>
    /// <param name="blue">线的颜色</param>
    private void DebugDrawGrid(Vector3 origin, int rows, int columns, float cellSize, Color color)
    {
        float width = rows * cellSize;//20
        float height = columns * cellSize;//20
        for (int i = 0; i < rows + 1; i++)
        {
            ///向上每一点,再向右每一行
            Vector3 startZXPos = origin + i * cellSize * new Vector3(0.0f, 0.0f, 1.0f);//向上每一小格
            Vector3 endZXPos = startZXPos + width * new Vector3(1.0f, 0.0f, 0.0f);//向右为每一行
            Debug.DrawLine(startZXPos, endZXPos, color);

            ///向右画每一点,再向上画每一行
            Vector3 startXZPos = origin + i * cellSize * new Vector3(1.0f, 0.0f, 0.0f);//向右每一小格
            Vector3 endXZPos = startXZPos + height * new Vector3(0.0f, 0.0f, 1.0f);//向上每一行
            Debug.DrawLine(startXZPos, endXZPos, color);
        }
    }

    private void OnApplicationQuit()
    {
        s_Instance = null;
    }
}
