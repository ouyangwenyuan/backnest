using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {
    static PlayerData player_data;

    public static PlayerData GetInstance() {
        if (player_data == null)
        {
            player_data = new PlayerData();
            player_data.LoadData();
        }

        return player_data;
    }

    private int gold = 0;
    private bool music_on = true;
    private bool newbie_done = false;
    private bool ads_done = false;

    private List<int> avatars = new List<int>();
    private int avatar_id = 0;
    private int avatars_count = 0;

    private int level_done_num = 0;
    private List<int> levels_star = new List<int>();
    private List<float> levels_best_time = new List<float>();

    private int first_revive = 0; // 还没有失败过 0 失败过了 1 

    public string count_down = "";    //开始的时间戳
    //public int count_down_b = 0; //是否正在计时中

    public int count_down_id;
    public int count_down_time;
    public int count_down_gold;

    private void LoadData() {
        first_revive = PlayerPrefs.GetInt("FIRST_ADS");

        gold = PlayerPrefs.GetInt("GOLD");
        music_on = PlayerPrefs.GetInt("MUSIC_ON") == 0?true:false;
        newbie_done = PlayerPrefs.GetInt("NEWBIE_DONE") == 1 ? true : false;
        ads_done = PlayerPrefs.GetInt("ADS_DONE") == 1 ? true : false;

        avatar_id = PlayerPrefs.GetInt("CLOTH_ID");
        avatars_count = PlayerPrefs.GetInt("CLOTHS_COUNT");

        for (int i = 1; i <= avatars_count; i++ )
        {
            int id = PlayerPrefs.GetInt("CLOTH_" + i.ToString());
            avatars.Add(id);
        }

        if (avatar_id == 0)  // 第一次游戏 默认
        {
            UpdateAvatarId(1);
            AddAvatarId(1);
        }

        level_done_num = PlayerPrefs.GetInt("LEVEL_DONE_NUM");

        for (int i = 1; i <= level_done_num; i++)
        {
            int star = PlayerPrefs.GetInt("LEVEL_STAR_" + i.ToString());
            float time = PlayerPrefs.GetFloat("LEVEL_BEST_TIME_" + i.ToString());

            levels_star.Add(star);
            levels_best_time.Add(time);
        }

        count_down = PlayerPrefs.GetString("COUNT_DOWN");

        count_down_id =  PlayerPrefs.GetInt("COUNT_DOWN_ID");;
        count_down_time = PlayerPrefs.GetInt("COUNT_DOWN_TIME");;
        count_down_gold = PlayerPrefs.GetInt("COUNT_DOWN_GOLD"); ;

        //count_down_b = PlayerPrefs.GetInt("COUNT_DOWN_B");

        if (count_down.Equals(""))
        {
            SetCountDown(DateTime.Now.ToString());

            count_down_id = 1;

            CommonData.GetGiftTime(count_down_id , out count_down_time , out count_down_gold);

            SetCountDownID(count_down_id);
            SetCountDownTime(count_down_time);
            SetCountDownGold(count_down_gold);
        }
    }

    public void Clear() { 
        gold = 0;
        music_on = true;
        newbie_done = false;
        ads_done = false;

        avatars.Clear();
        avatar_id = 0;
        avatars_count = 0;

        level_done_num = 0;
        levels_star.Clear();
        levels_best_time.Clear();

        count_down = "";
        //count_down_b = 0;
    }

    public int GetFirstAds()
    {
        return first_revive;
    }

    public void SetFirstAds(int s)
    {
        first_revive = s;

        PlayerPrefs.SetInt("FIRST_ADS", s);
    }

    public string GetCountDown() {
        return count_down;
    }

    public void SetCountDown(string s) {
        count_down = s;

        PlayerPrefs.SetString("COUNT_DOWN", s);
    }

    public int GetCountDownID()
    {
        return count_down_id;
    }

    public void SetCountDownID(int b)
    {
        if (b > 6) {
            b = 6;
        }

        count_down_id = b;

        PlayerPrefs.SetInt("COUNT_DOWN_ID", b);
    }

    public int GetCountDownTime()
    {
        return count_down_time;
    }

    public void SetCountDownTime(int b)
    {
        count_down_time = b;

        PlayerPrefs.SetInt("COUNT_DOWN_TIME", b);
    }

    public int GetCountDownGold()
    {
        return count_down_gold;
    }

    public void SetCountDownGold(int b)
    {
        count_down_gold = b;

        PlayerPrefs.SetInt("COUNT_DOWN_GOLD", b);
    }

    public void ChangeMusicOn() {
        music_on = music_on ? false : true;

        SetMusicOn(music_on);

        AudioSourcesManager.GetInstance().ChangeStatus(music_on);
    }



    public int GetGold()
    {
        return gold;
    }

    public void SetGold(int num ) {

        gold += num;

        PlayerPrefs.SetInt("GOLD",gold);
    }

    public void SetMusicOn(bool done)
    {
        music_on = done;

        PlayerPrefs.SetInt("MUSIC_ON", music_on?0:1);
    }

    public bool GetMusicOn() {
        return music_on;
    }

    public bool GetNewbieDone() {
        return newbie_done;
    }

    public void SetNewbieDone(bool done)
    {
        newbie_done = done;

        PlayerPrefs.SetInt("NEWBIE_DONE", newbie_done ? 1 : 0);
    }

    public void SetAdsDone(bool done)
    {
        ads_done = done;

        PlayerPrefs.SetInt("ADS_DONE", ads_done ? 1 : 0);
    }

    public void UpdateAvatarId(int id)
    {
        avatar_id = id;

        PlayerPrefs.SetInt("CLOTH_ID", id);
    }


    public void AddAvatarId(int id)
    {
        avatars.Add(id);
        avatars_count += 1;

        PlayerPrefs.SetInt("CLOTH_" + id.ToString(), id);
        PlayerPrefs.SetInt("CLOTHS_COUNT", avatars_count);
    }

    public int GetAvatarID() {
        return avatar_id;
    }

    public List<int> GetAvatarsList(){
        return avatars;
    }

    public void UpdateLevelData(int level_id , int level_star , float level_best_time) {

        levels_star[level_id - 1] = level_star;
        levels_best_time[level_id - 1] = level_best_time;

        //PlayerPrefs.SetInt("LEVEL_" + level_id.ToString(), level_id);
        PlayerPrefs.SetInt("LEVEL_STAR_" + level_id.ToString(), level_star);
        PlayerPrefs.SetFloat("LEVEL_BEST_TIME_" + level_id.ToString(), level_best_time);
    }

    public void AddLevelData(int level_id, int level_star, float level_best_time)
    {
        level_done_num += 1;

        levels_star.Add(level_star);
        levels_best_time.Add(level_best_time);

        PlayerPrefs.SetInt("LEVEL_DONE_NUM", level_done_num);
        UpdateLevelData(level_id, level_star, level_best_time);
    }

    public int GetLevelDoneNum() {
        return level_done_num;
    }

    public void GetLevelDoneStarNBestTime(int id , out int star , out float best_time ) {
        star = 0;
        best_time = 0;

        if (id <= level_done_num && id > 0)
        {
            star = 0;
            best_time = 0;
        }

        star = levels_star[id-1];
        best_time = levels_best_time[id-1];
    }

    public int GetAllStars() {
        int sum = 0;

        foreach (int i in levels_star) {
            sum += i;
        }

        return sum;
    }
}
