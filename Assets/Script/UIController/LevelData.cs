using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelData : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int level_done_id = PlayerData.GetInstance().GetLevelDoneNum();

        if (id <= level_done_id)
        {
            audio.enabled = true;

            text_done.text = id.ToString();

            GO_Done.SetActive(true);

            int star = 0;
            float best_time = 0;

            PlayerData.GetInstance().GetLevelDoneStarNBestTime(id , out star, out best_time);

            for (int i = 1; i <= stars.Length; i++) {
                if (i <= star)
                {
                    stars[i - 1].SetActive(true);
                }
                else {
                    stars[i - 1].SetActive(false);
                }
            }
        }
        else if (id - level_done_id == 1)
        {
            text_now.text = id.ToString();

            audio.enabled = true;

            GO_New.SetActive(true);
        }
        else {
            is_lock = true;

            audio.enabled = false;

            GO_Lock.SetActive(true);
        }
	}

    // Update is called once per frame
    //void Update () {

    //}

    //public Text text;
    //public int star_num;
    bool is_lock = false;

    public Text text_done;
    public Text text_now;

    public GameObject GO_Done;
    public GameObject GO_New;
    public GameObject GO_Lock;

    public GameObject[] stars;

    public AudioSource audio;

    public int id;

    public void OnClick() {
        if(!is_lock){
            SceneManager.LoadScene("Stage" + id.ToString());
        }else{
            //Debug.Log("Lock");
        }
    }
}
