using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClearLocalSave : MonoBehaviour {

	// Use this for initialization
	//void Start () {
		
	//}
	
	// Update is called once per frame
	//void Update () {
		
	//}

    [MenuItem("LocalSave/Clear")]
    public static void Clear() {
        PlayerPrefs.DeleteAll();
    }
}
