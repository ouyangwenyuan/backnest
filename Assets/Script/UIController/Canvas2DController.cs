using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Canvas2DController : MonoBehaviour
{

    public GameObject[] stars;
    public Text label_Gold;
    public GameObject Sun;

    public GameObject ui_pause;
    public GameObject ui_success;
    public GameObject ui_fail;

    GameStage game_stage;

    ClickScreen click_screen;

	// Use this for initialization
	void Start () {

        GameObject game_stage_go = GameObject.Find("GameStage");
        game_stage = game_stage_go.GetComponent<GameStage>();

        click_screen = GameObject.Find("Main Camera").GetComponent<ClickScreen>();

        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.CHANGE_LIFE_NUM] += ChangeStarNum;
        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.CHANGE_GOLD_NUM] += ChangeGoldNum;
        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.GAME_RESULT] += ShowGameResult;

        label_Gold.text = PlayerData.GetInstance().GetGold().ToString();

        ShowStar(game_stage.life_num);
	}

    void ShowStar(int star_num) {
        int length = stars.Length;
        int num = 1;

        for (int i = 0; i < length; i++)
        {
            if (num <= star_num-1)
            {
                stars[i].SetActive(true);
            }
            else {
                stars[i].SetActive(false);
            }

            num++;
        }
    }

	// Update is called once per frame
	//void Update () {
		
	//}

    void ShowGameResult(CustomEventData ced)
    {
        Event_GameResult e = (Event_GameResult)ced;

        if (e.result)
        {
            if (ui_success != null)
            {
                int star = 0;

                if (game_stage.life_num > 3) {
                    star = 3;
                }
                else if (game_stage.life_num > 1)
                {
                    star = 2;
                }else{
                    star = 1;
                }

                if(star > game_stage.star_num){
                    game_stage.star_num = star;
                }

                //if(){

                //}

                float time = Time.timeSinceLevelLoad;

                if (game_stage.new_level)
                {
                    game_stage.best_time = time;

                    PlayerData.GetInstance().AddLevelData(game_stage.level_id,game_stage.star_num,game_stage.best_time);
                }
                else {

                    if(game_stage.best_time > time)
                        game_stage.best_time = time;

                    PlayerData.GetInstance().UpdateLevelData(game_stage.level_id, game_stage.star_num, game_stage.best_time);
                }

                ui_success.SetActive(true);

                SuccessUIController controller = ui_success.GetComponent<SuccessUIController>();

                controller.Show(star, StringUtil.TimeFormat(time), StringUtil.TimeFormat(game_stage.best_time));
            }
        }
        else {
            if (ui_fail != null)
            {
                ui_fail.SetActive(true);


            }
        }
    }

    void OnDestroy() {
        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.CHANGE_LIFE_NUM] -= ChangeStarNum;
        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.CHANGE_GOLD_NUM] -= ChangeGoldNum;
        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.GAME_RESULT] -= ShowGameResult;
    }

    void ChangeStarNum(CustomEventData ced) {
        Event_ChangeLifeNum e = (Event_ChangeLifeNum)ced;

        ShowStar(e.life_num);
    }

    void ChangeGoldNum(CustomEventData ced)
    {
        Event_ChangeGoldNum e = (Event_ChangeGoldNum)ced;

        label_Gold.text = e.gold_num.ToString();
        //PlayerData.GetInstance().SetGold(e.gold_num);
    }

    public void OnPause() {
        //Debug.Log("OnPause");

        click_screen.no_click = true;

        Time.timeScale = 0f;
        if (ui_pause != null)
        {
            ui_pause.SetActive(true);
        }
    }

    public void OnPauseUIResume()
    {
        //Debug.Log("OnPause");

        click_screen.no_click = false;

        Time.timeScale = 1f;

        if (ui_pause != null)
        {
            ui_pause.SetActive(false);
        }
    }

    public void OnRestart()
    {
        //Debug.Log("OnPause");

        Time.timeScale = 1f;
        //ui_pause.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExit()
    {
        //Debug.Log("OnPause");

        Time.timeScale = 1f;

        SceneManager.LoadScene("SelectLevels");
    }

    public void OnPauseUIMusicOn()
    {
        PlayerData.GetInstance().ChangeMusicOn();
    }

    public void OnGift()
    {
        //Debug.Log("OnGift");
    }

    public void OnResurgence(int type) {

        if (type == 1) // ads
        {

        }
        else if (type == 2)//Gold
        {
            /*
            int gold = PlayerData.GetInstance().GetGold();

            if (gold >= Common.resurgence_num)
            {
                Debug.Log("OnResurgence");
            }
            */

        }
    }

    public void OnBack2Main()
    {
        Time.timeScale = 1f;

        //bool b = PlayerData.GetInstance().GetNewbieDone();
        //if (b)
        {
            SceneManager.LoadScene("Main");
        }
        //else {
        //    PlayerData.GetInstance().SetNewbieDone(true);
        //    SceneManager.LoadScene("SelectLevels");
        //}
    }

    public void OnFaceBook() {
        Debug.Log("Facebook");
    }
}
