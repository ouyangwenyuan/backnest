using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAvatar : MonoBehaviour {

    public SpriteRenderer sr;

    public ParticleSystem[] tails;
    public ParticleSystem[] explodes;
    public TrailRenderer[] renders;
    int i;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();

        i = PlayerData.GetInstance().GetAvatarID();

        Texture2D texture2d = (Texture2D)Resources.Load("Textures/ball" + i.ToString() );//更换为红色主题英雄角色图片  
        Sprite sp = Sprite.Create(texture2d, sr.sprite.textureRect, new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值  
        sr.sprite = sp;

        ShowTail();
	}
	
    public void ClearTails(){
        foreach(ParticleSystem tail in tails){
            tail.Stop();
        }

        foreach (TrailRenderer traill in renders)
        {
            traill.enabled = false;
        }

    }

    void ShowTail() {
        tails[i-1].Play();

        renders[i - 1].enabled = true;
}

void StopTail() {
        tails[i - 1].Stop();

        renders[i - 1].enabled = false;
    }

    public void ShowBody() {
        ShowTail();
        sr.enabled = true; 
    }

    public void HideBody()
    {
        StopTail();
        sr.enabled = false;
    }

    public void Explode() {
        if (explodes[i - 1].isPlaying) {
            explodes[i - 1].Stop();
        }

        explodes[i - 1].Play();
    }

	// Update is called once per frame
	//void Update () {
		
	//}
}
