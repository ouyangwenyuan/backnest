using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public GameObject go1;
    public GameObject go3;

	// Use this for initialization
	void Start () {

        Vector3 v21 = go1.transform.position - transform.position;
        Vector3 v31 = go3.transform.position - transform.position;

        float angle = Vector2.Angle(v21, v31);

        Debug.Log("Angle : " + angle.ToString());

        if (v21.x < 0)
            angle = 360 - angle;
        transform.Rotate(Vector3.forward , angle , Space.World);
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}
}
