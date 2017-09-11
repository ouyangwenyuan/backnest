using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetGiftMonoHandler : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {
        text.text = PlayerData.GetInstance().GetCountDownGold().ToString();
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}

    public void OnDead() {
        GameObject.Destroy(gameObject);
    }
}
