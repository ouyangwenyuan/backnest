using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSourceMonoHandler : MonoBehaviour {

    AudioSource audio;
    int id;
	void Awake () {
        audio = GetComponent<AudioSource>();

        if (audio != null)
        {
            bool on = PlayerData.GetInstance().GetMusicOn();

            //audio.enabled = on;

            audio.mute = !on;

            /*
            if (!AudioSourcesManager.GetInstance().listener_list.ContainsKey(SceneManager.GetActiveScene().name)){
                AudioSourcesManager.GetInstance().listener_list.Add(SceneManager.GetActiveScene().name, listener);
            }
            */

            id = AudioSourcesManager.GetInstance().AddAudioSource(audio);
        }

	}

    void OnEnable() {
        
    }

    void OnDestroy() {
        if (audio != null)
        {
            AudioSourcesManager.GetInstance().RemoveAudioSource(id);
        }
    }

	// Update is called once per frame
	//void Update () {
		
	//}
}
