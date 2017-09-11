using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickAdsController : MonoBehaviour {

    public int gold;
    //public int dollar;

    public Text text;

    // Use this for initialization
    //void Start () {

    //}

    // Update is called once per frame
    //void Update () {

    //}

    public void OnClick()
    {
       PlayerData.GetInstance().SetGold(gold);

       text.text = PlayerData.GetInstance().GetGold().ToString();
    }
}
