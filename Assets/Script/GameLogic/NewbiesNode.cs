using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewbiesNode : MonoBehaviour {

    public int step;
    public ClickScreen cs;

    BoxCollider box_collider;

    GameObject image;
//    Gameobject image2;
//    Gameobject image3;
//    Gameobject image4;


	// Use this for initialization
	void Start () {
		box_collider = GetComponent<BoxCollider>();
        
        image = GameObject.Find("UI/2DCanvasNewbies(Clone)/Tips" + step.ToString());
 //       image2 = GameObject.Find("UI/2DCanvasNewbies(Clone)/Tips2");
 //       image3 = GameObject.Find("UI/2DCanvasNewbies(Clone)/Tips3");
 //       image4 = GameObject.Find("UI/2DCanvasNewbies(Clone)/Tips4");

        ClearImage();

	}
	
	// Update is called once per frame
	//void Update () {
		
	//}


    void ClearImage() {
        image.SetActive(false);
//        image2.SetActive(false);
//        image3.SetActive(false);
//        image4.SetActive(false);
    }

    void ShowImage() {
        //if(s == 1){
            image.SetActive(true);
        //}else if(s == 2){
        //    image2.SetActive(true);
        //}else if(s == 3){
        //    image3.SetActive(true);
        //}else if (s == 4){
        //    image4.SetActive(true);
        //}
    }


    void OnClick() {
        Time.timeScale = 1f;
        cs.no_click = true;

        cs.OnClick = null;

        ClearImage();
    }

    void OnDestroy() {
        //Time.timeScale = 1f;
    }


    void OnTriggerEnter(Collider collider)
    {
        //进入触发器执行的代码
        if (collider.gameObject.tag.Equals("Ball"))
        {
            //Debug.Log("Name : " + collider.gameObject.name);

            Time.timeScale = 0f;
            cs.no_click = false;

            box_collider.enabled = false;

            cs.OnClick = OnClick;

            ShowImage();
        }
    }
}
