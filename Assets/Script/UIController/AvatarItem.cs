using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarItem : MonoBehaviour {

    public int cost = 0;

    public GameObject go_unlock;
    public GameObject go_lock;

    public Image image;

    public void ChangeStatus(bool b) {
        if(b){
            go_unlock.SetActive(true);
            go_lock.SetActive(false);
        }else{
            go_unlock.SetActive(false);
            go_lock.SetActive(true);
        }
    }

    public void OnSelect(bool b) {
        image.enabled = b;
    }

	// Use this for initialization
	//void Start () {
		
	//}
	
	// Update is called once per frame
	//void Update () {
		
	//}
}
