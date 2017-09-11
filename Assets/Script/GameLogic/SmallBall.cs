using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBall : MonoBehaviour {

    //GameObjectPool pool;

	// Use this for initialization
	void Start () {
        //StartCoroutine(wait(2));
	}

    void OnEnable() {
        StartCoroutine(wait(2));
    }

    IEnumerator wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);

        GameObjectPool.instance.Recycle(gameObject);

        //Destroy(gameObject);
    }  

	// Update is called once per frame
	//void Update () {
	//	
	//}
}
