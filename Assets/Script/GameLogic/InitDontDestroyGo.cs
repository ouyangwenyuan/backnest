using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitDontDestroyGo : MonoBehaviour {

    public GameObject Go_Music;
    public GameObject Go_AudioClipSet;

    void Awake() {
        DontDestroyOnLoad(Go_Music);
        DontDestroyOnLoad(Go_AudioClipSet);
		//PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Main");
    }

	// Use this for initialization
	//void Start () {
		
	//}
	
	// Update is called once per frame
	//void Update () {
		
	//}
}
