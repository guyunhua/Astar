using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Node {
    public int x;
    public int y;
    public Vector3 pos;
    public Node parent = null;
    public float F;
    public float G;
    public float H;
    public bool inList = false;

    public Node(int x, int y) {
        this.x = x;
        this.y = y;
        pos = new Vector3(x, 0, y);
    }
}
