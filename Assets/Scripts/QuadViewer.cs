#define IS_Y_AXIS_HEIGHT
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuadViewer : MonoBehaviour
{
    private TextMeshPro tmp;
    object quadTree;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        GetComponentInChildren<MeshRenderer>().material.color = new Color(Random.Range(0,1f), Random.Range(0, 1f), Random.Range(0, 1f), 0.3f);
    }

    public void SetQuadTree<T>(QuadTreeNode<T> qtn)
    {
        quadTree = qtn;
        transform.localScale = new Vector3(qtn.boundary.width, qtn.boundary.height, 1);

#if IS_Y_AXIS_HEIGHT
        transform.position = new Vector3(qtn.boundary.x, qtn.boundary.y);
#else
        transform.position = new Vector3(qtn.boundary.x, 0, qtn.boundary.y);
#endif


        tmp.text = qtn.data.ToString();
    }
}
