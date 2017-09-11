using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour {

    public Text text;

    void Awake() {
        PlayerData.GetInstance();
    }

	// Use this for initialization
	void Start () {
        text.text = PlayerData.GetInstance().GetGold().ToString();
	}
	
	// Update is called once per frame
	//void Update () {
		
	
    //}

    public GameObject Panel_Setting;
    public GameObject Panel_Shop;

    public GameObject Button;
    public GameObject Image_GameName;

    public void ClearScreen(bool b) {

        if(b){
            Button.transform.localPosition = new Vector3(1000f, Button.transform.localPosition.y, Button.transform.localPosition.z);
            Image_GameName.transform.localPosition = new Vector3(1000f, Image_GameName.transform.localPosition.y, Image_GameName.transform.localPosition.z); 
        }else{
            Button.transform.localPosition = new Vector3(0f, Button.transform.localPosition.y, Button.transform.localPosition.z);
            Image_GameName.transform.localPosition = new Vector3(0f, Image_GameName.transform.localPosition.y, Image_GameName.transform.localPosition.z); 
        }
    }

    public void OnPlay() 
    {
        //Debug.Log("Play");
        bool b = PlayerData.GetInstance().GetNewbieDone();
        if (b)
        {
            SceneManager.LoadScene("SelectLevels");
        }
        else {
            SceneManager.LoadScene("Stage_Newbies");
        }
        
    }

    public void OnSet()
    {
        ClearScreen(true);
        
        //Debug.Log("Set");

        Panel_Setting.SetActive(true);
    }

    public void OnShop()
    {
        ClearScreen(true);

        //Debug.Log("Shop");

        Panel_Shop.SetActive(true);
    }

    /*
    public void OnGift()
    {
        Debug.Log("Gift");
    }
    */
    public void OnFBShare()
    {
        Debug.Log("OnFBShare");
    }

    public void OnRank()
    {
        Debug.Log("Rank");
    }

    public void OnComment()
    {
        Debug.Log("Comment");
    }

    public void OnMusicOn() {
        //Debug.Log("Music");

        PlayerData.GetInstance().ChangeMusicOn();
    }

    public void OnNewbies()
    {
        //Debug.Log("Newbies");

        SceneManager.LoadScene("Stage_Newbies");
    }

    public void OnPanelSettingBack()
    {
        //Debug.Log("Back");

        ClearScreen(false);

        Panel_Setting.SetActive(false);
    }

    public void OnPanelShopBack()
    {
        //Debug.Log("Back");

        ClearScreen(false);

        Panel_Shop.SetActive(false);
    }

    public void OnRestore() {
 
    }
}
