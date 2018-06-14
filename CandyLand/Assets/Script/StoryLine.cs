using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryLine : MonoBehaviour {
    public int storyCase;
    public Dialogs[] dialogs;
    public static StoryLine storyLine;
    public GameObject Mayor;
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
                Mayor.AddComponent<NPC>();
                Mayor.GetComponent<NPC>().chars.charName= Chars.Names.Frans;
                Mayor.GetComponent<NPC>().chars.charRole = Chars.Roles.Mayor;
                Mayor.GetComponent<NPC>().hasStoryEffect = true;
                Mayor.GetComponent<NPC>().storyEffectIndex = 4;
                Mayor.GetComponent<NPC>().dialogText = dialogs[0].dialogText;

                //Cam changes towards the tutorial place, Mayors says you can get up trough there
                //Talk with mayor is on NPC script #3.5
                break;
            case 4:
                Destroy(Mayor.GetComponent<NPC>());
                Mayor.AddComponent<CollectionQuest>();
                //Mayor.GetComponent<CollectionQuest>().item == the item
                Mayor.GetComponent<CollectionQuest>().questName = "Get some stuff";
                Mayor.GetComponent<CollectionQuest>().questType = Quest.questTypes.Collect;
                Mayor.GetComponent<CollectionQuest>().questDialog = "Hey, I really need you to collect some recources for me before we can even try to chop wood... Would you be so kind to go find me some, they are probably nearby";
                //Mayor.GetComponent<CollectionQuest>().requiredItems
                //Mayor.GetComponent<CollectionQuest>().rewards
                Mayor.GetComponent<CollectionQuest>().hasStoryEffect = true;
                Mayor.GetComponent<CollectionQuest>().storyEffectIndex = 5;
                break;
            case 5:
                Destroy(Mayor.GetComponent<CollectionQuest>());
                Mayor.AddComponent<NPC>();
                Mayor.GetComponent<NPC>().chars.charName = Chars.Names.Frans;
                Mayor.GetComponent<NPC>().chars.charRole = Chars.Roles.Mayor;
                Mayor.GetComponent<NPC>().hasStoryEffect = true;
                Mayor.GetComponent<NPC>().storyEffectIndex = 6;
                Mayor.GetComponent<NPC>().dialogText = dialogs[0].dialogText;
                // Collection quest turns into NPC text, follow him text
                break;
            case 6:
                Destroy(Mayor.GetComponent<NPC>());
                Mayor.AddComponent<LocationQuest>();
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = false;
                Mayor.GetComponent<LocationQuest>().AcceptQuest();
                Mayor.GetComponent<LocationQuest>().questName = "Follow the Mayor";
                Mayor.GetComponent<LocationQuest>().questDialog = "Good job following me here!";
                Mayor.GetComponent<LocationQuest>().questRequirement = "Follow the Mayor to the blacksmith.";
                Mayor.GetComponent<LocationQuest>().questType = Quest.questTypes.Find;
                Mayor.GetComponent<LocationQuest>().hasStoryEffect = true;
                Mayor.GetComponent<LocationQuest>().storyEffectIndex = 7;
                //With a for loop add the coördinates to the point
                //NPC text turns into a location quest, wait with this until he is at the blacksmith
                //Mayor moves towards the blacksmith
                break;
            case 7:
                //After collecting the quest turn him into an AI again, he has to go away, he asks you to make the axe, work and come back later.
                UIManager.uiManager.Dialog(dialogs[0].dialogText, Chars.Names.Frans.ToString(), Chars.Roles.Mayor.ToString());
                //Mayor goes away
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
