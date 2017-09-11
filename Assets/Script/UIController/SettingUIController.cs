using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUIController : MonoBehaviour
{

    public GameObject go_sprite;
    public Text text;

    Image sprite;

	// Use this for initialization
	void Start () {
        text.text = PlayerData.GetInstance().GetGold().ToString();

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
