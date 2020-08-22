#define IS_Y_AXIS_HEIGHT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * this quad tree purpose, save point
 * and get data from that point contains T
 * T could be anything 
 */

public unsafe class QuadTreeNode<T>
{
    public static QuadTreeNode<T> Root;
    public const int MaxCapacity = 4;

    public QuadTreeNode<T> Parent;
    public T data;
    public uint currentCapacity = 0;

    QuadTreeNode<T> parent;

    //childrens
    QuadTreeNode<T> childLeftTop;
    QuadTreeNode<T> childRightTop;
    QuadTreeNode<T> childLeftDown;
    QuadTreeNode<T> childRightDown;

    public Rect boundary = new Rect();

    public QuadTreeNode<T> WhereIsIn(Vector2 pos)
    {
        var x = pos.x / boundary.width;
        var y = pos.y / boundary.height;
        Debug.LogError("index : " + (x + y * 2));
        return childLeftDown;
    }

    

    private bool IsIn(Vector2 pos)
    {
        return boundary.Contains(pos);
    }

    private void Subdivide()
    {
        //when over capacity
    }

    private void SetUpViewer()
    {
        var qv_prefab = Resources.Load("QuadViewer") as GameObject;
        var qv_instance = GameObject.Instantiate(qv_prefab);
        var qv = qv_instance.GetComponent<QuadViewer>();
        qv.SetQuadTree(this);
    }

    public QuadTreeNode(QuadTreeNode<T> _parent, Vector3 pos, Vector2 size, T _data)
    {
        parent = _parent;
#if IS_Y_AXIS_HEIGHT
        pos.z = pos.y;
        pos.y = 0;
#endif  
        if(_parent != null)
        {
            size = _parent.boundary.size * 0.5f;
        }

        boundary = new Rect(pos, size);
        data = _data;
        SetUpViewer();

        childLeftDown = new QuadTreeNode<T>(this, 0);
        childRightDown = new QuadTreeNode<T>(this, 1);
        childLeftTop = new QuadTreeNode<T>(this, 2);
        childRightTop = new QuadTreeNode<T>(this, 3);
    }

    //make this for atom children
    public QuadTreeNode(QuadTreeNode<T> _parent, int index)
    {
        const float constantQuadSizeRate = 0.25f;
        parent = _parent;
        boundary.width = _parent.boundary.width * 0.5f;
        boundary.height = _parent.boundary.width * 0.5f;

        switch(index)
        {
            case 0://left down
                boundary.x = _parent.boundary.x - _parent.boundary.width * constantQuadSizeRate;
                boundary.y = _parent.boundary.y - _parent.boundary.height * constantQuadSizeRate;
                break;
            case 1://right down
                boundary.x = _parent.boundary.x + _parent.boundary.width * constantQuadSizeRate;
                boundary.y = _parent.boundary.y - _parent.boundary.height * constantQuadSizeRate;
                break;
            case 2://left up
                boundary.x = _parent.boundary.x - _parent.boundary.width * constantQuadSizeRate;
                boundary.y = _parent.boundary.y + _parent.boundary.height * constantQuadSizeRate;
                break;
            case 3://right up
                boundary.x = _parent.boundary.x + _parent.boundary.width * constantQuadSizeRate;
                boundary.y = _parent.boundary.y + _parent.boundary.height * constantQuadSizeRate;
                break;
        }
        SetUpViewer();
    }

public static Vector2 RootSize;
    public static void Init(Vector3 rootCenterPos, Vector2 size, T rootData)
    {
        RootSize = size;
        Root = new QuadTreeNode<T>(null, rootCenterPos, size, rootData);
    }

    public static void Insert(Vector3 position, T data)
    {        
        var node = QuadTreeNode<T>.Root;
        QuadTreeNode<T> parent = node?.Parent;
        Vector3 size = QuadTreeNode<T>.RootSize;        
        do
        {            
            if (node == null)
            {
                node = new QuadTreeNode<T>(parent, position, size, data);
                break;
            }
            else
            {
                parent = node.WhereIsIn(position);
                node = null;                
            }            
        } while (true);

        

    }
}
