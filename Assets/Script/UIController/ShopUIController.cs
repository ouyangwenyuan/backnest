using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour {

    public GameObject[] avatars;

    public Text text;

    int avatar_id = 0;
    List<int> avatars_list = new List<int>();

    public int[] costs;

	// Use this for initialization
	void Start () {
        avatar_id = PlayerData.GetInstance().GetAvatarID();
        avatars_list = PlayerData.GetInstance().GetAvatarsList();

        int length = avatars.Length;

        for (int i = 1; i <= length; i++ )
        {
            bool b = avatars_list.Contains(i);

            //if (i == avatar_id)

            ChangeAvatarStatus(avatars[i-1], b, i == avatar_id);
        }

        text.text = PlayerData.GetInstance().GetGold().ToString();
            
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}


    void ChangeAvatarStatus(GameObject go, bool unlock,bool select) {
        AvatarItem item = go.GetComponentInChildren<AvatarItem>();
        //Button button = go.GetComponentInChildren<Button>();



        item.ChangeStatus(unlock);
        item.OnSelect(select);
    }

    void ClearAllSelectImage() {
        foreach (int i in avatars_list) {
            GameObject go = avatars[i - 1];

            AvatarItem item = go.GetComponentInChildren<AvatarItem>();
            item.OnSelect(false);
        }
    }

    public void OnRandom() {

        int id = Random.Range(1, avatars_list.Count + 1);

        OnChangeAvatar(id);
    }

    public void OnChangeAvatar(int id) {
        if (avatar_id == id)
            return;

        ClearAllSelectImage();

        avatar_id = id;

        PlayerData.GetInstance().UpdateAvatarId(id);

        foreach (int i in avatars_list)
        {
            if(i == id){
                GameObject go = avatars[i - 1];

                AvatarItem item = go.GetComponentInChildren<AvatarItem>();
                item.OnSelect(true);
            }
        }
    }

    public void OnUnLockAvatar(int id) {
        int gold = PlayerData.GetInstance().GetGold();

        int cost = costs[id - 1];

        if(gold >= cost){
            PlayerData.GetInstance().SetGold(-cost);

            text.text = PlayerData.GetInstance().GetGold().ToString();

            PlayerData.GetInstance().AddAvatarId(id);
            //avatars_list.Add(id);

            ChangeAvatarStatus(avatars[id-1], true, false);
        }
    }
}
