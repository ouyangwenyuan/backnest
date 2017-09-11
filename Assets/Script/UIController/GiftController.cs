using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftController : MonoBehaviour {

    public Text text_timer;
    public Image image;
    Button button;

    string count_down;

    public int duration = 5000;
    public int gold = 1000;

    bool show_time = false;

    long seconds_done;
    long seconds_now;

    long seconds_diff;

    float timer = 0;

    public GameObject particle;

	// Use this for initialization
	//void Start ()
    void OnEnable()
    {
        duration = PlayerData.GetInstance().GetCountDownTime();
        gold = PlayerData.GetInstance().GetCountDownGold();

        button = GetComponent<Button>();

        count_down = PlayerData.GetInstance().GetCountDown();

        if (count_down.Equals("Done"))
        {
            //可以领
            Done();

            return;
        }

        DateTime date = Convert.ToDateTime(count_down);

        seconds_done = date.Ticks / 10000000 + duration;
        seconds_now = DateTime.Now.Ticks / 10000000;

        if (seconds_now >= seconds_done) //计时完成
        {
            Done(true);

            return;
        }
        else {

            //{
            seconds_diff = seconds_done - seconds_now;

            show_time = true;

            button.interactable = false;

            if (particle != null)
                particle.SetActive(false);

            //}

        }


	}

    void Done(bool save = false) {
        button.interactable = true;

        if (particle != null)
            particle.SetActive(true);

        text_timer.enabled = false;
        if(image != null)
            image.enabled = false; 

        if(save){
            PlayerData.GetInstance().SetCountDown("Done");
        }
    }

	// Update is called once per frame
	void Update () {
        if (show_time)
        {
            timer += Time.deltaTime;

            if(timer > 1)
            {
                timer = 0;

                seconds_diff--;
            }

            text_timer.text = UIUtil.ShowTime(seconds_diff);

            if(seconds_diff <= 0){
                show_time =false;

                timer = 0;

                Done(true);
            }
        }
	}

    public void GetGift() {

        GameObject go_gift = (GameObject)Instantiate(Resources.Load("Prefabs/GetGift"));
        go_gift.transform.parent = transform;
        go_gift.transform.localPosition = new Vector3(0f, 0f, 0f);
        go_gift.transform.localScale = new Vector3(1f, 1f, 1f);

        PlayerData.GetInstance().SetGold(gold);
        PlayerData.GetInstance().SetCountDown(DateTime.Now.ToString());

        //////////////////////////////////////
        int id = PlayerData.GetInstance().GetCountDownID();

        id++;

        PlayerData.GetInstance().SetCountDownID(id);

        CommonData.GetGiftTime(id,out duration , out gold);

        PlayerData.GetInstance().SetCountDownTime(duration);
        PlayerData.GetInstance().SetCountDownGold(gold);

        //////////////////////////////////////

        seconds_diff = duration;

        show_time = true;

        button.interactable = false;

        if(particle != null)
            particle.SetActive(false);

        text_timer.enabled = true;
        if (image != null)
            image.enabled = true; 
    }

}
