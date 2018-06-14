using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryLine : MonoBehaviour {
    public int storyCase;
    public Dialogs[] dialogs;
    public static StoryLine storyLine;
	// Use this for initialization
	void Awake () {
        storyLine = this;
    }
    private void Start()
    {
        StartCoroutine(Story());
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(Story());
        }
    }
    public IEnumerator Story()
    {
        switch (storyCase)
        {
            case 0:
                //start animation of waking up
                yield return new WaitForSeconds(0); // wait till anim is over + 1 sec or so
                storyCase++;
                goto case 1;
            case 1:
                //Mayor talks to you, change the camera towards him, play anim if he has one
                UIManager.uiManager.Dialog(dialogs[storyCase].dialogText, Chars.Names.Frank.ToString(), Chars.Roles.Mayor.ToString(), true, 3);
                break;
            case 3:
                UIManager.uiManager.Dialog(dialogs[storyCase].dialogText, Chars.Names.Frank.ToString(), Chars.Roles.Mayor.ToString());
                print("SHIT");
                //Cam changes towards the tutorial place, Mayors says you can get up trough there
                //Talk with mayor is on NPC script #3.5
                break;
            case 4:
                //Mayor gets Collection Quest
                break;
            case 5:
                // Collection quest turns into NPC text, follow him text
                break;
            case 6:
                //NPC text turns into a location quest, wait with this until he is at the blacksmith
                //Mayor moves towards the blacksmith
                break;
            case 7:
                //After collecting the quest turn him into an NPC again, he has to go away, he asks you to make the axe, work and come back later.
                break;
            case 8:
                //play Mayor leaving anim
                //After crafting the first item case 9 triggers #8.5
                break;
            case 9:
                //
                break;

        }
    }
    [System.Serializable]
    public class Dialogs
    {
        public List <string> dialogText = new List<string>();
        public AnimationClip anim;
    }
    [System.Serializable]
    public class Chars
    {
        public enum Roles {Mayor, Hunter, Miner, Villager, Slave, Unknown }
        public Roles charRole;
        public enum Names { Frank, Ashley, Pim, Maurits, Frans, Unknown}
        public Names charName;
    }
}
