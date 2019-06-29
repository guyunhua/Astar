using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {
    public Node[,] nodes;
    public int size;

    private void Awake()
    {
        InitMap();
    }

    private void InitMap(int size = 10) {
        this.size = size;
        nodes = new Node[size, size];
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                nodes[i, j] = new Node(i, j);
            }
        }

        List<Node> path = FindPath(nodes[0, 0], nodes[size - 1, size - 1]);
        for (int i = 0; i < path.Count; i++) {
            path[i].inList = true;
        }
        Debug.Log(path);
    }

    //初始化OpenList CloseList;
    //将起点加入OpenList中,
    //如果OpenList不为空,则从OpenList中找到F最高的节点;
    public List<Node> FindPath(Node start, Node end) {
        List<Node> openList = new List<Node>();//带遍历的节点
        List<Node> closeList = new List<Node>();//已经遍历的节点
        Node curNode = null;
        openList.Add(start);
        while(openList.Count > 0)
        {
            curNode = GetMinF(openList);//在OpenList中找到F最小的节点
            openList.Remove(curNode);//从OpenList中移除
            closeList.Add(curNode);//加入到CloseList中

            //遍历当前节点的周边节点
            List<Node> sruuroundNodes = GetGroundNodes(curNode);
            for (int i = 0; i < sruuroundNodes.Count; i++) {
                Node snode = sruuroundNodes[i];
                if (closeList.Contains(snode)) {//如果当前节点已经在CloseList中则不处理 继续
                    continue;
                }
                if (!openList.Contains(snode))
                {//如果不在OpenList中
                    snode.parent = curNode; //则将节点的父亲设置为当前节点
                    CalcFGH(snode, end);//计算节点的FGH
                    openList.Add(snode); //加入到OpenList
                }
                else {//如果已经在OpenList中
                    float G = CalcG(snode, curNode);
                    if (G < curNode.G) {
                        snode.parent = curNode;
                        snode.G = G;
                        snode.F = snode.G + snode.H;
                    }
                }
            }
            if (openList.Contains(end)) {
                break;
            }
        }

        return PathList(start, end);
    }

    List<Node> PathList(Node start,  Node end) {
        List<Node> ret = new List<Node>();
        while (end.parent != null) {
            if (end == start) break;
            ret.Add(end);
            end = end.parent;
            
        }
        return ret;
    }

    private float CalcG(Node snode, Node curNode)
    {
        return Vector2.Distance(snode.pos, curNode.pos) + curNode.G;
    }

    private void CalcFGH(Node snode, Node end)
    {
        float h = Mathf.Abs(end.x - snode.x) + Mathf.Abs(end.y - snode.y);
        float g;
        if (snode.parent == null)
        {
            g = 0;
        }
        else
        {
            float distance = Vector3.Distance(snode.parent.pos, snode.pos);
            float distanceZ = Mathf.RoundToInt(distance);
            if (distance - distanceZ != 0)
            {
                g = distance * 1.4f + snode.parent.G;
            }
            else
            {
                if (snode.parent.x == snode.x && snode.parent.x != snode.y)
                {
                    g = distance * 1 + snode.parent.G;
                }
                else
                {
                    g = distance * 1 + snode.parent.G;
                }
            }
        }
        snode.G = g;
        snode.H = h;
        snode.F = g + h;
    }

    private List<Node> GetGroundNodes(Node node)
    {
        Node up = null, down = null, left = null, right = null, upleft = null, upright = null, leftdown = null, rightdown = null;
        List<Node> retList = new List<Node>();
        if(node.y + 1 < size) up = nodes[node.x, node.y + 1];
        if(node.y - 1 >= 0) down = nodes[node.x, node.y - 1];
        if(node.x - 1 >= 0) left = nodes[node.x - 1, node.y];
        if(node.x + 1 < size) right = nodes[node.x + 1, node.y];
        if (node.x - 1 >= 0 && node.y + 1 < size) upleft = nodes[node.x - 1, node.y + 1];
        if(node.x + 1 < size && node.y + 1 < size) upright = nodes[node.x + 1, node.y + 1];
        if (node.x - 1 >= 0 && node.y - 1 >= 0)  leftdown = nodes[node.x - 1, node.y - 1];
        if(node.x + 1 < size && node.y - 1 >= 0) rightdown = nodes[node.x + 1, node.y - 1];
        if (up != null) retList.Add(up);
        if (down != null) retList.Add(down);
        if (left != null) retList.Add(left);
        if (right != null) retList.Add(right);
        if (upleft != null) retList.Add(upleft);
        if (upright != null) retList.Add(upright);
        if (leftdown != null) retList.Add(leftdown);
        if (rightdown != null) retList.Add(rightdown);
        return retList;

    }

    private Node GetMinF(List<Node> nodes)
    {
        Node ret = null;
        float miniF = float.MaxValue;
        for (int i = 0; i < nodes.Count; i++) {
            if (nodes[i].F <= miniF) {
                miniF = nodes[i].F;
                ret = nodes[i];
            }
        }
        return ret;
    }

    private void OnDrawGizmos()
    {
        Vector3 size = new Vector3(0.9f, 0.1f, 0.9f);
        for (int i = 0; i < nodes.GetLength(0); i++) {
            for (int j = 0; j < nodes.GetLength(1); j++) {
                if (nodes[i, j].inList)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.white;
                Gizmos.DrawCube(nodes[i, j].pos, size);
            }
            //
        }
    }

}
