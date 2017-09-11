using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingEffect : MonoBehaviour {
    public bool loading = false;
    public Texture loadingTexture;
    private float rotAngle = 0.0f;
    public float size = 70.0f;
    public float rotSpeed = 300.0f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (loading)
        {
            rotAngle += rotSpeed * Time.deltaTime;
        }
	}
    void OnGUI()
    {
        if (loading)
        {
            Vector2 pivot = new Vector2(Screen.width / 2, Screen.height / 2);
            GUIUtility.RotateAroundPivot(rotAngle % 360, pivot);
            GUI.DrawTexture(new Rect((Screen.width - size) / 2, (Screen.height - size) / 2, size, size), loadingTexture); 
        }
    }
}
