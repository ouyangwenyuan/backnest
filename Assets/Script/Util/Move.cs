using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
	}

    void OnTriggerEnter(Collider collider)
    {
        //进入触发器执行的代码

        Debug.Log("Name : " + collider.gameObject.name);
    }
}