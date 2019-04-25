using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicVolumeController : MonoBehaviour
{

    [SerializeField] float volumeControl = 1f;

    [SerializeField] Button musicOnButton;
    [SerializeField] Button musicOffButton;
    [SerializeField] Button effectsOnButton;
    [SerializeField] Button effectsOffButton;

    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1) //GetType takes the type of the class, means that MusicVolumeController for this script.
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }


    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }



    public float GetVolumeControl()
    {
        return volumeControl;
    }


    public void MusiconClicked()
    {
        FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().mute = false;
        musicOnButton.GetComponent<Button>().interactable = false;
        musicOffButton.GetComponent<Button>().interactable = true;
    }

    public void MusicoffClicked()
    {
        FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().mute = true;
        musicOffButton.GetComponent<Button>().interactable = false;
        musicOnButton.GetComponent<Button>().interactable = true;
    }

    public void EffectsonClicked()
    {
        volumeControl = 1f;
        effectsOnButton.GetComponent<Button>().interactable = false;
        effectsOffButton.GetComponent<Button>().interactable = true;
    }

    public void EffectsoffClicked()
    {
        volumeControl = 0f;
        effectsOffButton.GetComponent<Button>().interactable = false;
        effectsOnButton.GetComponent<Button>().interactable = true;
    }

}
