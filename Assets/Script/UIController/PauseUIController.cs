using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUIController : MonoBehaviour {

    public Text text;

    public GameObject go_sprite;
    Image sprite;

	// Use this for initialization
	void Start () {

        string scene_name = SceneManager.GetActiveScene().name;

        scene_name = scene_name.Substring(5);

        //if (scene_name.Equals("_"))
        //    return;

        //int level_id = int.Parse(scene_name);

        text.text = scene_name;

        sprite = go_sprite.GetComponent<Image>();

        ChangeImage();
    }
	
	// Update is called once per frame
	//void Update () {
		
	//}

    void ChangeImage() {
        bool b = PlayerData.GetInstance().GetMusicOn();

        if (b)
        {
            sprite.overrideSprite = Resources.Load("Textures/Pause/button_music_on", typeof(Sprite)) as Sprite;
        }
        else
        {
            sprite.overrideSprite = Resources.Load("Textures/Pause/button_music_off", typeof(Sprite)) as Sprite;
        }
 
    }

    public void OnClick() {
        PlayerData.GetInstance().ChangeMusicOn();

        ChangeImage();
    }

}
