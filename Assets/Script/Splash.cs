using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {

    public Animator animator;
	// Use this for initialization
	void Start () {
        Invoke("Load", 2.3f);
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Load() {
        SceneManager.LoadScene("Init");
    }
}
