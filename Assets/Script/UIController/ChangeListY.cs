using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeListY : MonoBehaviour {

    

	// Use this for initialization
	void Start () {

        int level_done_num = PlayerData.GetInstance().GetLevelDoneNum();

        float x = gameObject.GetComponent<RectTransform>().localPosition.x;
        float y = gameObject.GetComponent<RectTransform>().localPosition.y - level_done_num * 200;
        float z = gameObject.GetComponent<RectTransform>().localPosition.z;

        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(x,y,z);
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
