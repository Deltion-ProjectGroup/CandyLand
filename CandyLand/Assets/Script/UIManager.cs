using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject healthBar;
    public GameObject[] questStuff;
    public AnimationClip[] UIAnims;
    public static UIManager uiManager;
    public delegate void questOptions();
    public questOptions acceptQuest;
    public questOptions cancelQuest;
    public questOptions completeQuest;
	// Use this for initialization
	void Start () {
        uiManager = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void RefreshHealth()
    {
        healthBar.GetComponent<Image>().fillAmount = (1 / GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().maxHealth) * GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().health;
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
}
