using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : Interactable
{
    public GameObject craftingUI;
    public GameObject[] craftingElements; //To find the components in the UI that will show the cost etc. later
    public GameObject requiredItemImg;
    public static Crafting crafting;
	// Use this for initialization
	void Start ()
    {
        crafting = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    public override void Interact(GameObject interactor)
    {
        craftingUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
