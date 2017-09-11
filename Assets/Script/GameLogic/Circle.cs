using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CIRCLE_TYPE {
    CIRCLE_TYPE_NORMAL,
    CIRCLE_TYPE_SAVE,
    CIRCLE_TYPE_FOLLOW_BALL,
    CIRCLE_TYPE_CIRCLE,
    CIRCLE_TYPE_END,
}

public class Circle : MonoBehaviour {

    public float radius;

    public MeshRenderer mr;

    public CIRCLE_TYPE type = CIRCLE_TYPE.CIRCLE_TYPE_NORMAL;

    public GameObject center_point;
    public float angle_speed;

    public int circle_id = 0;

    public ParticleSystem particle_system; 

    //public float radius_inside;
    //public float radius_outside; 

	// Use this for initialization
	void Start () {
        mr = GetComponent<MeshRenderer>();		
	}
	
	// Update is called once per frame
	void Update () {
        if (type == CIRCLE_TYPE.CIRCLE_TYPE_CIRCLE)
        {
            if (center_point != null)
            {
                transform.RotateAround(center_point.transform.position, Vector3.forward, angle_speed * Time.deltaTime);
            }
        }	
	}
}
