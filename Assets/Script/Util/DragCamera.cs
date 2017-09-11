using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        cam_transform = Camera.main.transform;

        x = cam_transform.position.x;
        z = cam_transform.position.z;
	}

    Transform cam_transform;

    float x = 0f;
    float z = 0f;

    float move_y = 0f;
    float pre_y = 0f;
    float now_y = 0f;

    bool follow = false;

	// Update is called once per frame
	void Update () {
        /*
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Debug.Log(Input.GetTouch(0).position.y.ToString());
        }*/

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            //Debug.Log("Down : " + Input.mousePosition.y.ToString());


            pre_y = Input.mousePosition.y;
            //pre_y = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z)).y;

            follow = true;
        }

        if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            //Debug.Log("Move : " + Input.mousePosition.y.ToString());
            now_y = Input.mousePosition.y;
            //now_y = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z)).y;

            move_y = (now_y - pre_y) / 10f;

            pre_y = now_y;

            //OnUpdate();

            //MoveCamera(move_y);
        }

        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            //Debug.Log("Up : " + Input.mousePosition.y.ToString());

            //MoveCamera(2f);

            follow = false;

            move_y = 0f;
            pre_y = 0f;
            now_y = 0f;
        }
	}

    void LateUpdate() {
        if(follow){
            Vector3 v_target = new Vector3(x, cam_transform.position.y + move_y, z);

            cam_transform.position = v_target;
        }
    }

    void MoveCamera(float y)
    {
        Vector3 v_target = new Vector3(x, cam_transform.position.y + y, z);

        Camera.main.transform.DOMove(v_target, 0.1f);
    }
}
