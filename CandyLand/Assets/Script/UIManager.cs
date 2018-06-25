using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject healthBar;
    public GameObject[] questStuff;
    public GameObject dialogUI;
    public AnimationClip[] UIAnims;
    public static UIManager uiManager;
    public delegate void questOptions();
    public questOptions acceptQuest;
    public questOptions cancelQuest;
    public questOptions completeQuest;
	// Use this for initialization
	void Awake () {
        uiManager = this;
	}
	
	// Update is called once per frame
	void Update () {

	}
    public void Dialog(List<string> dialogText, string charName, string charRole , bool hasAfterEffect = false, int effectIndexNum = 0, bool nextIndexIsDialog = false)
    {
        dialogUI.SetActive(true);
        dialogUI.GetComponent<Dialog>().startDialog(dialogText, charName, charRole, hasAfterEffect, effectIndexNum, nextIndexIsDialog);
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
