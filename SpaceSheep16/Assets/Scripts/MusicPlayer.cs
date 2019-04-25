using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{

    [SerializeField] public float volumeControl = 1f;

    [SerializeField] GameObject musicPlayer;
    [SerializeField] Button musicOnButton;
    [SerializeField] Button musicOffButton;
    [SerializeField] Button effectsOnButton;
    [SerializeField] Button effectsOffButton;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetVolumeControl()
    {
        return volumeControl;
    }


    public void MusiconClicked()
    {
        musicPlayer.GetComponent<AudioSource>().mute = false;
        musicOnButton.GetComponent<Button>().interactable = false;
        musicOffButton.GetComponent<Button>().interactable = true;
    }

    public void MusicoffClicked()
    {
        musicPlayer.GetComponent<AudioSource>().mute = true;
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
