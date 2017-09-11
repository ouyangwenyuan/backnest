using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccessUIController : MonoBehaviour {

    public GameObject[] stars;

    public Text text_time;
    public Text text_best_time;

    public AudioSource audio;

    // Use this for initialization
    //void Start () {

    //}

    // Update is called once per frame
    //void Update () {

    //}

    int star_num = 0;


    public void Show(int _star_num , string time, string best_time) {
        star_num = _star_num;

        /*
        int length = stars.Length;
        //int num = 1;

        for (int i = 0; i < length; i++)
        {
            if (i < star_num)
            {
                stars[i].SetActive(true);
            }
            else
            {
                stars[i].SetActive(false);
            }
        }
        */

        Invoke("ShowStar1", 0.5f);
        Invoke("ShowStar2", 1f);
        Invoke("ShowStar3", 1.5f);

        text_time.text = time;
        text_best_time.text = best_time;
    }

    void ShowStar1() {
        int i = 0;

        if (i < star_num)
        {
            stars[i].SetActive(true);

            audio.Play();
        }
        else
        {
            stars[i].SetActive(false);
        }
    }

    void ShowStar2() {
        int i = 1;

        if (i < star_num)
        {
            stars[i].SetActive(true);

            audio.Play();
        }
        else
        {
            stars[i].SetActive(false);
        }
    }

    void ShowStar3 ()
    {
        int i = 2;

        if (i < star_num)
        {
            stars[i].SetActive(true);

            audio.Play();
        }
        else
        {
            stars[i].SetActive(false);
        }
    }
}
