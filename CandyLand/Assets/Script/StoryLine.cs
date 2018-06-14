using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryLine : MonoBehaviour {
    public int storyCase;
    public Dialogs[] dialogs;
    public PosCordinates[] locQuestPositions;
    public static StoryLine storyLine;
    public List<ColQuestReq> questRequirementList = new List<ColQuestReq>();
    public GameObject Mayor;
    public Item[] itemList;
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
    [System.Serializable]
    public class PosCordinates
    {
        public float[] coördinates;
    }
    [System.Serializable]
    public class ColQuestReq
    {
        public List<Quest.Rewards> questRewards = new List<Quest.Rewards>();
        public List<CollectionQuest.RequiredStuff> requiredItems = new List<CollectionQuest.RequiredStuff>();
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
                UIManager.uiManager.dialogUI.SetActive(false);
                Mayor.AddComponent<NPC>();
                Mayor.GetComponent<NPC>().item = itemList[2];
                Mayor.GetComponent<NPC>().characterName = Chars.Names.Frans.ToString();
                Mayor.GetComponent<NPC>().role = Chars.Roles.Mayor.ToString();
                Mayor.GetComponent<NPC>().hasStoryEffect = true;
                Mayor.GetComponent<NPC>().storyEffectIndex = 4;
                Mayor.GetComponent<NPC>().dialogText = dialogs[3].dialogText;

                //Cam changes towards the tutorial place, Mayors says you can get up trough there
                //Talk with mayor is on NPC script #3.5
                break;
            case 4:
                UIManager.uiManager.dialogUI.SetActive(false);
                Destroy(Mayor.GetComponent<NPC>());
                Mayor.AddComponent<CollectionQuest>();
                Mayor.GetComponent<CollectionQuest>().item = itemList[2];
                Mayor.GetComponent<CollectionQuest>().questName = "Get some stuff";
                Mayor.GetComponent<CollectionQuest>().questType = Quest.questTypes.Collect;
                Mayor.GetComponent<CollectionQuest>().questDialog = "Hey, I really need you to collect some recources for me. Would you be so kind to go find me some, they are probably nearby";
                Mayor.GetComponent<CollectionQuest>().requiredItems = questRequirementList[0].requiredItems;
                Mayor.GetComponent<CollectionQuest>().rewards = questRequirementList[0].questRewards;
                Mayor.GetComponent<CollectionQuest>().hasStoryEffect = true;
                Mayor.GetComponent<CollectionQuest>().storyEffectIndex = 5;
                break;
            case 5:
                Destroy(Mayor.GetComponent<CollectionQuest>());
                Mayor.AddComponent<NPC>();
                Mayor.GetComponent<NPC>().item = itemList[2];
                Mayor.GetComponent<NPC>().characterName = Chars.Names.Frans.ToString();
                Mayor.GetComponent<NPC>().role = Chars.Roles.Mayor.ToString();
                Mayor.GetComponent<NPC>().hasStoryEffect = true;
                Mayor.GetComponent<NPC>().storyEffectIndex = 6;
                Mayor.GetComponent<NPC>().dialogText = dialogs[5].dialogText;
                // Collection quest turns into NPC text, follow him text
                break;
            case 6:
                UIManager.uiManager.dialogUI.SetActive(false);
                Destroy(Mayor.GetComponent<NPC>());
                Mayor.AddComponent<LocationQuest>();
                Mayor.GetComponent<LocationQuest>().questPositions = locQuestPositions[0].coördinates;
                Mayor.GetComponent<LocationQuest>().item = itemList[2];
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = false;
                Mayor.GetComponent<LocationQuest>().AcceptQuest();
                //Mayor.GetComponent<LocationQuest>().item = 
                Mayor.GetComponent<LocationQuest>().questName = "Follow the Mayor";
                Mayor.GetComponent<LocationQuest>().questDialog = "Good job following me here!";
                Mayor.GetComponent<LocationQuest>().questRequirement = "Follow the Mayor to the blacksmith.";
                Mayor.GetComponent<LocationQuest>().questType = Quest.questTypes.Find;
                Mayor.GetComponent<LocationQuest>().hasStoryEffect = true;
                Mayor.GetComponent<LocationQuest>().storyEffectIndex = 7;
                Mayor.GetComponent<LocationQuest>().rewards = questRequirementList[1].questRewards;
                //NPC text turns into a location quest, wait with this until he is at the blacksmith
                //Mayor moves towards the blacksmith
                break;
            case 7:
                //After collecting the quest turn him into an AI again, he has to go away, he asks you to make the axe, work and come back later.
                UIManager.uiManager.Dialog(dialogs[7].dialogText, Chars.Names.Frans.ToString(), Chars.Roles.Mayor.ToString());
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
