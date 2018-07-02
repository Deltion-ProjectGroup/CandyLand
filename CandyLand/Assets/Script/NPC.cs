using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable {
    public List<string> dialogText = new List<string>();
    public Sprite role;
    public Sprite characterName;
    public bool hasStoryEffect;
    public int storyEffectIndex;
    public bool nextIndexIsDialog;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void Interact(GameObject interactor)
    {
        UIManager.uiManager.Dialog(dialogText, characterName, role, hasStoryEffect, storyEffectIndex, nextIndexIsDialog);
    }
}
