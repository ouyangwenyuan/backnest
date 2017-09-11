using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BUFF_TYPE
{
    BUFF_TYPE_SHIELD,
    BUFF_TYPE_GOLD,
    BUFF_TYPE_SPEED_UP,
    BUFF_TYPE_LIGHTNING,
    BUFF_TYPE_OUTSIDE,
    BUFF_TYPE_INSIDE,
    BUFF_TYPE_REVERSE,
    BUFF_TYPE_SUCCESS,
    BUFF_TYPE_MAX,
}

public class Buff : MonoBehaviour {

    public BUFF_TYPE buff_type = BUFF_TYPE.BUFF_TYPE_MAX;
    public float valid_time = 5f;
    public float speed_multiple = 2;

    public int gold_num = 1;

    Vector3 v3_backup;

    
    Vector3 v3_target;
    Vector3 v3;

    float TIME_LENGTH = CommonData.gold_fly_time;

    float stepX = 1f;
    float stepY = 1f;

    float len_x = 0f;
    float len_y = 0f;

    bool fly = false;

    float time = 0f;
    

	// Use this for initialization
	void Start () {
        v3_backup = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.RESET_OBSTACLE_BUFF] += Reset;

        if (buff_type == BUFF_TYPE.BUFF_TYPE_GOLD) { 
            Camera uicamera = GameObject.Find("UI/UICamera").GetComponent<Camera>();
            Transform t = GameObject.Find("UI/2DCanvas(Clone)/Gold").transform;  
            //Canvas canvas = GameObject.Find("UI/2DCanvas").GetComponent<Canvas>();

            float x=uicamera.WorldToScreenPoint(t.position).x;  
            float y=uicamera.WorldToScreenPoint(t.position).y;

            //v3_target = GetWorldPosFromUIPos(canvas, t);

            v3_target = new Vector3(x,y,0);
        }
	}
	
	// Update is called once per frame
	void Update () {
        
        if (fly) {
            time += Time.deltaTime;

            if (time >= TIME_LENGTH)
            {
                fly = false;
                transform.position = new Vector3(transform.position.x, transform.position.y - 1000f, transform.position.z);

                Event_ChangeGoldNum event_gold = new Event_ChangeGoldNum();
                event_gold.gold_num = PlayerData.GetInstance().GetGold(); ;
                CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.CHANGE_GOLD_NUM](event_gold);
            }
            else
            {
                //float x = transform.position.x + (stepX * Time.deltaTime);
                //float y = transform.position.y + (stepY * Time.deltaTime);



                float x = v3.x + (stepX * Time.deltaTime);
                float y = v3.y + (stepY * Time.deltaTime);

                v3.x = x;
                v3.y = y;

                Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();

                float x1 = camera.ScreenToWorldPoint(new Vector3(x, y, 0f)).x;
                float y1 = camera.ScreenToWorldPoint(new Vector3(x, y, 0f)).y;

                transform.position = new Vector3(x1, y1, transform.position.z);

            }
        }
        
	}

    void OnDestroy()
    {
        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.RESET_OBSTACLE_BUFF] -= Reset;
    }

    void Reset(CustomEventData d)
    {
        //if (tag.Equals("Ball"))
        {
            transform.position = new Vector3(v3_backup.x, v3_backup.y, v3_backup.z);
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider == null || collider.gameObject == null)
        {
            return;
        }

        GameObject go = collider.gameObject;
        string tag = go.tag;

        if (tag.Equals("Ball") && buff_type != BUFF_TYPE.BUFF_TYPE_SUCCESS && buff_type != BUFF_TYPE.BUFF_TYPE_GOLD)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y -1000f, transform.position.z);
        }

        
        if (tag.Equals("Ball") && buff_type == BUFF_TYPE.BUFF_TYPE_GOLD)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y - 1000f, transform.position.z);

            fly = true;

            Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();

            float x = camera.WorldToScreenPoint(transform.position).x;
            float y = camera.WorldToScreenPoint(transform.position).y;

            v3 = new Vector3(x, y, 0);

            float x_target = v3_target.x;
            float y_target = v3_target.y;

            len_x = (x_target - x);
            len_y = (y_target - y);

            stepX = len_x / TIME_LENGTH;
            stepY = len_y / TIME_LENGTH;


            time = 0f;	
        }
        
    }

    Vector3 GetWorldPosFromUIPos(Canvas canvas, Transform t)
    {
        Vector3 scr = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, t.position);

        if (Camera.main != null)
        {
            scr.z = 0;//Camera.main.transform.position.z;
            Vector3 result = Camera.main.ScreenToWorldPoint(scr);

            return result;
        }

        return Vector3.zero;
    }
}
