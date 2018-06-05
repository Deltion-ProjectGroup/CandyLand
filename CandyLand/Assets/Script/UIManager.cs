using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject healthBar;
    public GameObject[] questStuff;
    public UIManager uiManager;
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
}
