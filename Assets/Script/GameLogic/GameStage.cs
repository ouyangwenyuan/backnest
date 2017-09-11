using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStage : MonoBehaviour {

    public int life_num = 0;
    public int star_num = 0;
    public float best_time = 0;
    //public int time_now = 0;

    public int level_id = 0;
    public bool new_level = false;

    public int save_num = 5;        //需要完成多少存储点 
    public int save_done = 0;

    public string GetFinish() {
        float result = (float)(save_done + 1) / (float)save_num;

        result *= 100f;

        return ((int)result).ToString() + "%";
    }

    void Awake() {
        Reset();

        string scene_name = SceneManager.GetActiveScene().name;

        scene_name = scene_name.Substring(5);

        if (scene_name.Equals("_Newbies"))
            return;

        level_id = int.Parse(scene_name);
        //Debug.Log(level_id.ToString());

        int level_done_num = PlayerData.GetInstance().GetLevelDoneNum();

        if (level_id > level_done_num)
        {
            new_level = true;
        }
        else {
            PlayerData.GetInstance().GetLevelDoneStarNBestTime(level_id , out star_num , out best_time);
        }
    }

	// Use this for initialization
	void Start () {
        //star_num = Common.star_num;

        //Reset();
	}

    public void Reset() {
        life_num = CommonData.life_num;
    }

	// Update is called once per frame
	void Update () {
        TimerManager.GetInstance().OnUpdate();
	}

    void OnDestroy() {
        TimerManager.GetInstance().OnDestroy();
    }
}
