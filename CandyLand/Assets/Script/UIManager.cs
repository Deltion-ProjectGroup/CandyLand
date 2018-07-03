using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    bool isInGame;
    bool paused;
    public AudioClip[] music;
    public GameObject healthBar;
    public Sprite[] charNames;
    public Sprite[] charRoles;
    public GameObject[] questStuff;
    public GameObject dialogUI;
    public GameObject[] menuScreens;
    public AnimationClip[] UIAnims;
    public static UIManager uiManager;
    public delegate void questOptions();
    public questOptions acceptQuest;
    public questOptions cancelQuest;
    public questOptions completeQuest;
    AudioSource getSource;
	// Use this for initialization
	void Awake () {
        getSource = gameObject.GetComponent<AudioSource>();
        uiManager = this;
	}
    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Freeze();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            if(isInGame && GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canInteract)
            {
                PauseGame();
            }
        }
	}
    public void Dialog(List<string> dialogText, Sprite charName, Sprite charRole , bool hasAfterEffect = false, int effectIndexNum = 0, bool nextIndexIsDialog = false)
    {
        dialogUI.SetActive(true);
        dialogUI.GetComponent<Dialog>().startDialog(dialogText, charName, charRole, hasAfterEffect, effectIndexNum, nextIndexIsDialog);
    }
    public void RefreshHealth()
    {
        
    }
    public void AcceptQuest()
    {
        acceptQuest();
    }
    public void CancelQuest()
    {
        cancelQuest();
    }
    public void CompleteQuest()
    {
        completeQuest();
    }
    public void StartGame(AudioClip clickSound)
    {
        if (!isInGame)
        {
            getSource.pitch = Random.Range(0.95f, 2.05f);
            getSource.clip = clickSound;
            getSource.Play();
            isInGame = true;
            for (int i = 0; i < menuScreens.Length; i++)
            {
                menuScreens[i].SetActive(false);
            }
            Cursor.lockState = CursorLockMode.Locked;
            GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().clip = music[1];
            GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().Play();
            StartCoroutine(StoryLine.storyLine.Story());
        }
    }
    public void Options(AudioClip clickSound)
    {
        getSource.pitch = Random.Range(0.95f, 2.05f);
        getSource.clip = clickSound;
        getSource.Play();
        for (int i = 0; i < menuScreens.Length; i++)
        {
            menuScreens[i].SetActive(false);
        }
        menuScreens[0].SetActive(true);
        menuScreens[2].SetActive(true);
    }
    public void ExitGame(AudioClip clickSound)
    {
        getSource.clip = clickSound;
        getSource.Play();
        Application.Quit();
    }
    public void PauseGame()
    {
        if (paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UnFreeze();
            Time.timeScale = 1;
            paused = false;
            for(int i = 0; i < menuScreens.Length; i++)
            {
                menuScreens[i].SetActive(false);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Freeze();
            Time.timeScale = 0;
            paused = true;
            for(int i = 0; i < 2; i++)
            {
                menuScreens[i].SetActive(true);
            }
        }
    }
    public void ChangeQuality()
    {
        QualitySettings.SetQualityLevel((int)GameObject.FindGameObjectWithTag("QualitySlider").GetComponent<Slider>().value);
    }
    public void ChangeSensitivity()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().rotateMultiplier = GameObject.FindGameObjectWithTag("SensitivitySlider").GetComponent<Slider>().value;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camerah>().rotateMultiplier = GameObject.FindGameObjectWithTag("SensitivitySlider").GetComponent<Slider>().value / GameObject.FindGameObjectWithTag("SensitivitySlider").GetComponent<Slider>().maxValue;
    }
    public void FullScreen(AudioClip clickSound)
    {
        getSource.pitch = Random.Range(0.95f, 2.05f);
        getSource.clip = clickSound;
        getSource.Play();
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
            GameObject.FindGameObjectWithTag("ScreensizeButton").GetComponent<Text>().text = "Fullscreen";
        }
        else
        {
            Screen.fullScreen = true;
            GameObject.FindGameObjectWithTag("ScreensizeButton").GetComponent<Text>().text = "Windowed";
        }
    }
    public void Return(AudioClip clickSound)
    {
        getSource.pitch = Random.Range(0.95f, 2.05f);
        getSource.clip = clickSound;
        getSource.Play();
        for (int i = 0; i < menuScreens.Length; i++)
        {
            menuScreens[i].SetActive(false);
        }
        if (isInGame)
        {
            for (int i = 0; i < 2; i++)
            {
                menuScreens[i].SetActive(true);
            }
        }
        else
        {
            menuScreens[0].SetActive(true);
            menuScreens[3].SetActive(true);
        }
    }
}
