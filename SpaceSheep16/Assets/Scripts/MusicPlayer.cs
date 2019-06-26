using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{

    AudioSource audioSource;

    void Awake()
    {
        SetUpSingleton();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefsController.GetMusicVolume();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1) //GetType takes the type of the class, means that MusicPlayer for this script.
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource)
        {
            audioSource.volume = volume;
        }
    }


}
