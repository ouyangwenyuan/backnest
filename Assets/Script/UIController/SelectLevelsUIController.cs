using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelsUIController : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {
        text.text = PlayerData.GetInstance().GetGold().ToString();
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}

    public void OnBack() {
        SceneManager.LoadScene("Main");
    }
}
