﻿using System.Collections;
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
    int backUpHealth = 100;
	// Use this for initialization
	void Awake () {
        uiManager = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health -= 10;
            RefreshHealth();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health += 40;
            RefreshHealth();
        }
	}
    public void Dialog(List<string> dialogText, string charName, string charRole , bool hasAfterEffect = false, int effectIndexNum = 0, bool nextIndexIsDialog = false)
    {
        dialogUI.SetActive(true);
        dialogUI.GetComponent<Dialog>().startDialog(dialogText, charName, charRole, hasAfterEffect, effectIndexNum, nextIndexIsDialog);
    }
    public void RefreshHealth()
    {
        StartCoroutine(RefreshHP());
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
    IEnumerator RefreshHP()
    {
        yield return new WaitForEndOfFrame();
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health < backUpHealth)
        {
            for(int i = 0; healthBar.GetComponent<Image>().fillAmount > ((float)1 / GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().maxHealth) * GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().health; i++)
            {
                healthBar.GetComponent<Image>().fillAmount -= 0.003f;
                yield return new WaitForEndOfFrame();
            }
            print("Hi");
        }
        else
        {
            while (healthBar.GetComponent<Image>().fillAmount < ((float)1 / GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().maxHealth) * GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().health)
            {
                healthBar.GetComponent<Image>().fillAmount += 0.003f;
                yield return new WaitForEndOfFrame();
            }
        }
        backUpHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health;
        //healthBar.GetComponent<Image>().fillAmount = (1 / GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().maxHealth) * GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().health;
    }
}
