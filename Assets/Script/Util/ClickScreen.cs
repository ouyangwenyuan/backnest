using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClickScreen : MonoBehaviour {

    public Ball ball;

    public EventSystem eventSystem;
    public GraphicRaycaster graphicRaycaster;

    public bool no_click = false;

    public delegate void Click();
    public Click OnClick; 

    GameObject ui;
    GameObject root;

    void Awake() {
        ball = GameObject.Find("GameStage/Ball").GetComponent<Ball>();
        string stage = SceneManager.GetActiveScene().name.ToString();

        if (stage.Equals("Stage_Newbies"))
        {
            ui = (GameObject)Instantiate(Resources.Load("Prefabs/2DCanvasNewbies"));
            root = GameObject.Find("UI");
        }else{
            ui = (GameObject)Instantiate(Resources.Load("Prefabs/2DCanvas"));
            root = GameObject.Find("UI");

            Camera camera = GameObject.Find("UI/UICamera").GetComponent<Camera>();

            Canvas canvas = ui.GetComponent<Canvas>();
            canvas.worldCamera = camera;
        }

        ui.transform.parent = root.transform;
        graphicRaycaster = ui.GetComponent<GraphicRaycaster>();        
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
    void Update()
    {
        if (no_click)
        {
            return;
        }

        if (CheckGuiRaycastObjects())
        {
            return;
        }

        if(Input.GetMouseButtonDown(0)||(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))  
        {
            if(OnClick != null){
                OnClick();
            }

            ball.Change();
        }
    }

    bool CheckGuiRaycastObjects()
    {
	    PointerEventData eventData = new PointerEventData(eventSystem);
	    eventData.pressPosition = Input.mousePosition;
	    eventData.position = Input.mousePosition;

	    List<RaycastResult> list = new List<RaycastResult>();

        if (graphicRaycaster != null)
        {
	        graphicRaycaster.Raycast(eventData, list);
        }

	    //Debug.Log(list.Count);
	    return list.Count > 0;
    }
}
