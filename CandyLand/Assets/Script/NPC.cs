using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable {
    public List<string> dialogText = new List<string>();
    public StoryLine.Chars chars;
    public bool hasStoryEffect;
    public int storyEffectIndex;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void Interact(GameObject interactor)
    {
        UIManager.uiManager.Dialog(dialogText, chars.charName.ToString(), chars.charRole.ToString(), hasStoryEffect, storyEffectIndex);
    }
}
