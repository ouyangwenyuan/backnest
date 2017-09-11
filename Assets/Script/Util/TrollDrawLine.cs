using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TrollDrawLine : MonoBehaviour {
    public int pointCount = 10;
    private float radius;
    
    private float angle;
    private List<Vector3> points=new List<Vector3>();
    private LineRenderer renderer = null;//new LineRenderer();

    private bool rendering = true;  //用于标识是否显示

    // Use this for initialization
    void Start () {
        radius = GetComponent<Circle>().radius;


        angle = 360f / pointCount;
        renderer = GetComponent<LineRenderer>();

        if(!renderer)
        {
            Debug.LogError("LineRender is NULL!");
        }
    }

    void CalculationPoints()
    {
        /*
        Vector3 v = transform.position + transform.forward * radius;

        points.Add(v);

        Quaternion r = transform.rotation;

        for(int i=1;i<pointCount;i++)
        {
            Quaternion q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y - (angle * i), r.eulerAngles.z);
            //Quaternion q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z - (angle * i));
            v = transform.position + (q * Vector3.forward) * radius;
            points.Add(v);
        }
        */


        Vector3 v = transform.position + transform.up * radius;

        points.Add(v);

        Quaternion r = transform.rotation;

        for (int i = 1; i < pointCount; i++)
        {
            Quaternion q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z - (angle * i));
            //Quaternion q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z - (angle * i));
            v = transform.position + (q * Vector3.up) * radius;
            points.Add(v);
        }

    }

    void DrowPoints()
    {
        for(int i=0;i<points.Count;i++)
        {
          //  Debug.DrawLine(transform.position, points[i], Color.green);
            renderer.SetPosition(i, points[i]);  //把所有点添加到positions里
        }

        if (points.Count > 0)   //这里要说明一下，因为圆是闭合的曲线，最后的终点也就是起点，
            renderer.SetPosition(pointCount, points[0]);
    }

    void ClearPoints()
    {
        points.Clear();  ///清除所有点
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.D))   //按下D键显示
        {
            rendering = true;
        }

        if(Input.GetKeyUp(KeyCode.D))//放开D键不显示</span><br>
        {
            rendering = false;
        }

        if(rendering)
        {
            //renderer.SetVertexCount(pointCount + 1);  ///这里是设置圆的点数，加1是因为加了一个终点（起点）
            renderer.positionCount = pointCount + 1;

            CalculationPoints();
            DrowPoints();
        }
        else
        {
            //renderer.SetVertexCount(0);//

            renderer.positionCount = 0;
        }

        ClearPoints();
    }
}