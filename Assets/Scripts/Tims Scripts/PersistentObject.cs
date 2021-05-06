﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour {
    public List<string> avoidScenes = new List<string>();
    private GameObject[] perObjs;
    private GameObject[] player;
 


    [SerializeField] UnityEngine.Audio.AudioMixer p_mixer;    
    float tmp;

    // Use this for initialization
    void Awake () {
        perObjs = GameObject.FindGameObjectsWithTag("Persistent");
        player = GameObject.FindGameObjectsWithTag("Player");

        if (perObjs != null && perObjs.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }

        SetupAudioMixer();
	}

    void SetupAudioMixer()
    {
        if (p_mixer)
        {
            if (PlayerPrefs.HasKey(Slime_Const.Volume_Master))
            {
                tmp = PlayerPrefs.GetFloat(Slime_Const.Volume_Master);
                p_mixer.SetFloat(Slime_Const.Volume_Master, tmp);
            }
            if (PlayerPrefs.HasKey(Slime_Const.Volume_Music))
            {
                tmp = PlayerPrefs.GetFloat(Slime_Const.Volume_Music);
                p_mixer.SetFloat(Slime_Const.Volume_Music, tmp);
            }
            if (PlayerPrefs.HasKey(Slime_Const.Volume_Sfx))
            {
                tmp = PlayerPrefs.GetFloat(Slime_Const.Volume_Sfx);
                p_mixer.SetFloat(Slime_Const.Volume_Sfx, tmp);
            }
        }
    }

    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
    {
        if (perObjs != null && avoidScenes.Contains(arg1.name))
        {
            for (int i = 0; i < perObjs.Length; i++)
            {
                Destroy(perObjs[i]);
            }
        }
    }
}
