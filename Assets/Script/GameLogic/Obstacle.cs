using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    public enum OBSTACLE_TYPE
    {
        NORMAL,
        HIDE,
        CIRCLE
    };

    public OBSTACLE_TYPE type = OBSTACLE_TYPE.NORMAL;

    public GameObject center_point;
    public float angle_speed;
    public float begin_time;
    public float time1;
    public float time2;

    //MeshRenderer mr;
    BoxCollider bc;
    SpriteRenderer sr;

    bool is_show = true;

    Vector3 v3_backup;

    public GameObject go;

	// Use this for initialization
    void Start()
    {
        if (center_point != null /*&& go.transform.position != null*/)
        {
            Vector3 v21 = center_point.transform.position - transform.position;
            Vector3 v31 = go.transform.position - transform.position;

            float angle = Vector2.Angle(v21, v31);

            //Debug.Log("Angle : " + angle.ToString());

            if (v21.x < 0)
                angle = 360 - angle;

            transform.Rotate(Vector3.forward, angle, Space.World);
        }

        if (type == OBSTACLE_TYPE.HIDE)
        {
            //mr = GetComponent<MeshRenderer>();
            bc = GetComponent<BoxCollider>();
            sr = GetComponentInChildren<SpriteRenderer>();

            Invoke("Begin", begin_time);
        }

        v3_backup = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.RESET_OBSTACLE_BUFF] += Reset;
    }

    void OnDestroy() {
        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.RESET_OBSTACLE_BUFF] -= Reset;
    }

    void Reset(CustomEventData d)
    {
        if (tag.Equals("Ball"))
        {
            transform.position = new Vector3(v3_backup.x, v3_backup.y, v3_backup.z);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider == null || collider.gameObject == null)
        {
            return;
        }

        GameObject go = collider.gameObject;
        string tag = go.tag;

        if (tag.Equals("Ball"))
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y - 1000f, transform.position.z);
        }
    }

    void Begin() {

        ChangeStatus(null);

        int id;
        TimerManager.GetInstance().CreateTimer(
            out id,
            begin_time,
            ChangeStatus,//Action<object> callback, 
            null,
            TimerType.UnityTime,
            true,
            true,
            time1,
            time2
        ).Start();
    }

    void ChangeStatus(object o) {
        is_show = !is_show;

        if (is_show)
        {
            //if(mr != null){
            //    mr.enabled = true;
            //}
            sr.enabled = true;
            bc.enabled = true; 
        }else{
            //if (mr != null)
            //{
            //    mr.enabled = false;
            //}

            sr.enabled = false;
            bc.enabled = false;        
        }
    }

	// Update is called once per frame
	void Update () {
	    if (type == OBSTACLE_TYPE.CIRCLE)
        {
            if(center_point != null){
                transform.RotateAround(center_point.transform.position, Vector3.forward, angle_speed * Time.deltaTime);
            }
        }	
    }
}
