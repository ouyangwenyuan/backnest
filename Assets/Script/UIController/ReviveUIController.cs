using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveUIController : MonoBehaviour {

    public GameObject GO_ADS;
    public GameObject GO_GOLD;
    public GameObject GO_FINISH;

    public Text text;
    public Text text_finish; // 完成度

    public Text text_gold;

    public Image ads_cd;
    public Image gold_cd;

    public float cd = 3;

    public AudioSource audio;

    bool no_click = false;

    bool show_ads_cd = false;
    bool show_gold_cd = false;

    float step = 0;

    AudioClipSet audioclip_set;

    // Use this for initialization
    void Start () {
    }

    int revive_num  = 0 ;

    void Init() {
        revive_num++;

        text.text = (25*Math.Pow(2,revive_num)).ToString();
        text_finish.text = GameObject.Find("GameStage").GetComponent<GameStage>().GetFinish().ToString();
        audioclip_set = GameObject.Find("AudioClipSet").GetComponent<AudioClipSet>();
        step = 1 / cd;
    }

	// Update is called once per frame
	void Update () {
        if (show_ads_cd) {


            bool b = ShowImageCD(ads_cd , step*Time.deltaTime);

            if (b) {
                show_ads_cd = false;
                no_click = true;

                AudioSourcesManager.GetInstance().Play(audio, audioclip_set.a_lose);

                GO_ADS.SetActive(false);
                GO_GOLD.SetActive(false);
                GO_FINISH.SetActive(true);
            }
        }

        if (show_gold_cd) {
            bool b = ShowImageCD(gold_cd, step * Time.deltaTime);

            if (b)
            {
                show_gold_cd = false;
                no_click = true;

                AudioSourcesManager.GetInstance().Play(audio, audioclip_set.a_lose);

                GO_ADS.SetActive(false);
                GO_GOLD.SetActive(false);
                GO_FINISH.SetActive(true);
            }
        }
	}

    bool ShowImageCD(Image image , float amount)
    {
        image.fillAmount -= amount;

        if (image.fillAmount <= 0)
            return true;

        return false;
    }

    void OnEnable() {
        Init();

        if (PlayerData.GetInstance().GetFirstAds() == 0) {
            if (GO_ADS != null)
                GO_ADS.SetActive(true);

            show_ads_cd = true;

            if (ads_cd != null)
                ads_cd.fillAmount = 1;

            no_click = false;

            AudioSourcesManager.GetInstance().Play(audio, audioclip_set!= null ?audioclip_set.a_timeup:null, true);

            return;
        }

        int gold = PlayerData.GetInstance().GetGold();

        if (gold >= CommonData.resurgence_num)
        {
            if (GO_GOLD != null)
                GO_GOLD.SetActive(true);

            show_gold_cd = true;

            if (gold_cd != null)
                gold_cd.fillAmount = 1;

            no_click = false;

            AudioSourcesManager.GetInstance().Play(audio, audioclip_set!=null?audioclip_set.a_timeup:null, true);

            return;
        }

        AudioSourcesManager.GetInstance().Play(audio, audioclip_set!=null?audioclip_set.a_lose:null);

        if (GO_FINISH != null)
            GO_FINISH.SetActive(true);
    }

    void OnDisable() {
        GO_ADS.SetActive(false);
        GO_GOLD.SetActive(false);
        GO_FINISH.SetActive(false);

        no_click = true;

        step = 0;

        show_ads_cd = false;
        show_gold_cd = false;
    }

    public void OnClickAds() {
        if (!no_click) {
            Ball ball = GameObject.Find("GameStage/Ball").GetComponent<Ball>();
            ball.OnResurgence();
            PlayerData.GetInstance().SetFirstAds(1);

            AudioSourcesManager.GetInstance().Stop(audio);

            gameObject.SetActive(false);
        }
    }

    public void OnClickGold() {
        if (!no_click)
        {
            Ball ball = GameObject.Find("GameStage/Ball").GetComponent<Ball>();
            ball.OnResurgence();

            PlayerData.GetInstance().SetGold(-/*Common.resurgence_num*/(int)(25 * Math.Pow(2, revive_num)));

            text_gold.text = PlayerData.GetInstance().GetGold().ToString();

            AudioSourcesManager.GetInstance().Stop(audio);

            gameObject.SetActive(false);
        }
    }
}