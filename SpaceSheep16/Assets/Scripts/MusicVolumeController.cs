using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeController : MonoBehaviour
{

    [SerializeField] public Slider musicSlider;


    public void Start()
    {
        musicSlider.value = PlayerPrefsController.GetMusicVolume();

    }

    private void Update()
    {
        var musicPlayer = FindObjectOfType<MusicPlayer>();
        if(musicPlayer && musicSlider)
        {
            musicPlayer.SetVolume(musicSlider.value);
        }
        else
        {
            Debug.LogWarning("No music player found");
        }
    }

    public void SaveandExit()
    {
        PlayerPrefsController.SetMusicVolume(musicSlider.value);
        FindObjectOfType<Level>().LoadStartMenu();
    }

}
