using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    static bool AudioBegin = false;
    void Awake()
    {
        if (!AudioBegin)
        {
            audioSource.Play();
            DontDestroyOnLoad(gameObject);
            AudioBegin = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            if (audioSource.isPlaying) {
                audioSource.Pause();
            } else {
                audioSource.Play();
            }
        }
    }
}
