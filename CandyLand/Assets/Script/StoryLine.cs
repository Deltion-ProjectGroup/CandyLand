using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryLine : MonoBehaviour
{
    public int storyCase;
    public Dialogs[] dialogs;
    public PosCordinates[] locQuestPositions;
    public static StoryLine storyLine;
    public List<ColQuestReq> questRequirementList = new List<ColQuestReq>();
    public GameObject Mayor;
    public GameObject Hunter;
    public GameObject Miner;
    public Item[] itemList;
    // Use this for initialization
    void Awake()
    {
        storyLine = this;
    }
    private void Start()
    {
        StartCoroutine(Story());
    }
    // Update is called once per frame
    void Update()
    {

    }
    [System.Serializable]
    public class PosCordinates
    {
        public float[] coördinates;
        public Vector3 movePosNPC;
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
                // UIManager.uiManager.dialogUI.SetActive(false);
                Mayor.AddComponent<NPC>();
                Mayor.GetComponent<NPC>().item = itemList[2];
                Mayor.GetComponent<NPC>().characterName = Chars.Names.Frank.ToString();
                Mayor.GetComponent<NPC>().role = Chars.Roles.Mayor.ToString();
                Mayor.GetComponent<NPC>().hasStoryEffect = true;
                Mayor.GetComponent<NPC>().storyEffectIndex = 4;
                Mayor.GetComponent<NPC>().dialogText = dialogs[3].dialogText;

                //Cam changes towards the tutorial place, Mayors says you can get up trough there
                //Talk with mayor is on NPC script #3.5
                break;
            case 4:
                //UIManager.uiManager.dialogUI.SetActive(false);
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
                UIManager.uiManager.Dialog(dialogs[5].dialogText, Chars.Names.Frans.ToString(), Chars.Roles.Mayor.ToString(), true, 6);
                // Collection quest turns into Dialog text, follow him text
                break;
            case 6:
                //UIManager.uiManager.dialogUI.SetActive(false);
                Mayor.AddComponent<NPCFollow>();
                Mayor.GetComponent<NPCFollow>().targetLocation = locQuestPositions[0].movePosNPC;
                StartCoroutine(Mayor.GetComponent<NPCFollow>().StartMove());
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
                Destroy(Mayor.GetComponent<NPCFollow>());
                Destroy(Mayor.GetComponent<SphereCollider>());
                //After collecting the quest turn him into an AI again, he has to go away, he asks you to make the axe, work and come back later.
                Destroy(Mayor.GetComponent<LocationQuest>());
                UIManager.uiManager.Dialog(dialogs[7].dialogText, Chars.Names.Frans.ToString(), Chars.Roles.Mayor.ToString(), true, 8);
                //Mayor goes away
                break;
            case 8:
                Mayor.AddComponent<NPCFollow>();
                Mayor.GetComponent<NPCFollow>().needsPlayer = false;
                Mayor.GetComponent<NPCFollow>().targetLocation = locQuestPositions[1].movePosNPC;
                StartCoroutine(Mayor.GetComponent<NPCFollow>().StartMove());
                break;
            //play Mayor leaving anim

            case 9:
                UIManager.uiManager.Dialog(dialogs[8].dialogText, Chars.Names.Pim.ToString(), Chars.Roles.Hunter.ToString(), true, 10);
                Destroy(Mayor.GetComponent<NPCFollow>());
                Destroy(Mayor.GetComponent<SphereCollider>());
                Hunter.AddComponent<NPCFollow>();
                Hunter.GetComponent<NPCFollow>().needsPlayer = false;
                Hunter.GetComponent<NPCFollow>().targetLocation = locQuestPositions[0].movePosNPC; // leave 2 free
                StartCoroutine(Mayor.GetComponent<NPCFollow>().StartMove());
                // Happened after first craft
                break;
            case 10:
                //Hunter leaving anim
                Destroy(Mayor.GetComponent<NPCFollow>());
                Destroy(Mayor.GetComponent<SphereCollider>());
                Hunter.AddComponent<NPCFollow>();
                Hunter.GetComponent<NPCFollow>().needsPlayer = false;
                Hunter.GetComponent<NPCFollow>().targetLocation = locQuestPositions[1].movePosNPC; // leave 3 free
                StartCoroutine(Mayor.GetComponent<NPCFollow>().StartMove());
                goto case 11;
            case 11:
                Destroy(Mayor.GetComponent<NPCFollow>());
                Destroy(Mayor.GetComponent<SphereCollider>());
                Mayor.AddComponent<NPC>();
                Mayor.GetComponent<NPC>().item = itemList[2];
                Mayor.GetComponent<NPC>().characterName = Chars.Names.Frans.ToString();
                Mayor.GetComponent<NPC>().role = Chars.Roles.Mayor.ToString();
                Mayor.GetComponent<NPC>().hasStoryEffect = true;
                Mayor.GetComponent<NPC>().storyEffectIndex = 12;
                Mayor.GetComponent<NPC>().dialogText = dialogs[9].dialogText;
                break;
            case 12:
                //Mayor moves towards location
                Destroy(Mayor.GetComponent<NPC>());
                Mayor.AddComponent<NPCFollow>();
                Mayor.GetComponent<NPCFollow>().needsPlayer = false;
                Mayor.GetComponent<NPCFollow>().targetLocation = locQuestPositions[1].movePosNPC; // leave 3 free
                Mayor.AddComponent<LocationQuest>();
                Mayor.GetComponent<LocationQuest>().questPositions = locQuestPositions[0].coördinates;
                Mayor.GetComponent<LocationQuest>().item = itemList[2];
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = false;
                Mayor.GetComponent<LocationQuest>().AcceptQuest();
                Mayor.GetComponent<LocationQuest>().questName = "Follow the Mayor";
                Mayor.GetComponent<LocationQuest>().questDialog = "Thanks for following me, could you gather me some recources now?";
                Mayor.GetComponent<LocationQuest>().questRequirement = "Follow the Mayor towards a safe farming spot";
                Mayor.GetComponent<LocationQuest>().questType = Quest.questTypes.Find;
                Mayor.GetComponent<LocationQuest>().hasStoryEffect = true;
                Mayor.GetComponent<LocationQuest>().storyEffectIndex = 13;
                Mayor.GetComponent<LocationQuest>().rewards = questRequirementList[2].questRewards;
                break;
            case 13:
                Destroy(Mayor.GetComponent<LocationQuest>());
                Destroy(Mayor.GetComponent<NPCFollow>());
                Destroy(Mayor.GetComponent<SphereCollider>());
                Mayor.AddComponent<CollectionQuest>();
                Mayor.GetComponent<CollectionQuest>().item = itemList[2];
                Mayor.GetComponent<CollectionQuest>().questName = "GET (TO) DA CHOPPA";
                Mayor.GetComponent<CollectionQuest>().questType = Quest.questTypes.Collect;
                Mayor.GetComponent<CollectionQuest>().questDialog = "Since we have finally arrived here, could you collect some candywood, I would really appreciate it if you would.";
                Mayor.GetComponent<CollectionQuest>().requiredItems = questRequirementList[3].requiredItems;
                Mayor.GetComponent<CollectionQuest>().rewards = questRequirementList[3].questRewards;
                Mayor.GetComponent<CollectionQuest>().hasStoryEffect = true;
                Mayor.GetComponent<CollectionQuest>().storyEffectIndex = 14;
                break;
            case 14:
                Destroy(Mayor.GetComponent<CollectionQuest>());
                // Dialog that you got a blueprint also from him.
                //Add the blueprint also in this case.
                // He'll send a miner he says
                goto case 15;
            case 15:
                UIManager.uiManager.Dialog(dialogs[10].dialogText, Chars.Names.Ashley.ToString(), Chars.Roles.Miner.ToString(), true, 16);
                break;
            case 16:
                // MAYOR HAS TO BECOME THE MINER MODEL!!!!
                //Miner runs
                Miner.AddComponent<LocationQuest>();
                Miner.GetComponent<LocationQuest>().questPositions = locQuestPositions[0].coördinates;
                Miner.GetComponent<LocationQuest>().item = itemList[2];
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = false;
                Miner.GetComponent<LocationQuest>().AcceptQuest();
                Miner.GetComponent<LocationQuest>().questName = "Follow the Miner";
                Miner.GetComponent<LocationQuest>().questDialog = "Hey good job following me.. Let's start mining!";
                Miner.GetComponent<LocationQuest>().questRequirement = "Follow the Miner towards the mining spot";
                Miner.GetComponent<LocationQuest>().questType = Quest.questTypes.Find;
                Miner.GetComponent<LocationQuest>().hasStoryEffect = true;
                Miner.GetComponent<LocationQuest>().storyEffectIndex = 17;
                Miner.GetComponent<LocationQuest>().rewards = questRequirementList[4].questRewards;
                break;
            case 17:
                Destroy(Miner.GetComponent<LocationQuest>());
                UIManager.uiManager.Dialog(dialogs[11].dialogText, Chars.Names.Ashley.ToString(), Chars.Roles.Miner.ToString(), true, 18);
                break;
            case 18:
                // MAYOR HAS TO BECOME THE MINER MODEL!!!!
                Miner.AddComponent<CollectionQuest>();
                Miner.GetComponent<CollectionQuest>().item = itemList[2];
                Miner.GetComponent<CollectionQuest>().questName = "Mine Away";
                Miner.GetComponent<CollectionQuest>().questType = Quest.questTypes.Collect;
                Miner.GetComponent<CollectionQuest>().questDialog = "Please bring me some of these materials from the mine";
                Miner.GetComponent<CollectionQuest>().requiredItems = questRequirementList[5].requiredItems;
                Miner.GetComponent<CollectionQuest>().rewards = questRequirementList[5].questRewards;
                Miner.GetComponent<CollectionQuest>().hasStoryEffect = true;
                Miner.GetComponent<CollectionQuest>().storyEffectIndex = 19;
                break;
            case 19:
                Destroy(Miner.GetComponent<CollectionQuest>());
                UIManager.uiManager.Dialog(dialogs[12].dialogText, Chars.Names.Pim.ToString(), Chars.Roles.Hunter.ToString(), true, 20);
                break;
            case 20:
                //Hunter leaving anim
                goto case 21;

            case 21:
                //MAYOR IS HUNTER
                Hunter.AddComponent<CollectionQuest>();
                Hunter.GetComponent<CollectionQuest>().item = itemList[2];
                Hunter.GetComponent<CollectionQuest>().questName = "HouseParty";
                Hunter.GetComponent<CollectionQuest>().questType = Quest.questTypes.Collect;
                Hunter.GetComponent<CollectionQuest>().questDialog = "Did you bring my materials?";
                Hunter.GetComponent<CollectionQuest>().requiredItems = questRequirementList[6].requiredItems;
                Hunter.GetComponent<CollectionQuest>().rewards = questRequirementList[6].questRewards;
                Hunter.GetComponent<CollectionQuest>().hasStoryEffect = true;
                Hunter.GetComponent<CollectionQuest>().storyEffectIndex = 22;
                //Add the quest component to the hunter..
                //Add without the player noticing that you can now forge a sword.
                break;
            case 22:
                Destroy(Hunter.GetComponent<CollectionQuest>());
                UIManager.uiManager.Dialog(dialogs[13].dialogText, Chars.Names.Pim.ToString(), Chars.Roles.Hunter.ToString(), true, 23);
                //Hunter tells you about how forging swords works and who you are
                //Also tells you that he forgot his bag and asks you to retreive it.
                break;
            case 23:
                Hunter.AddComponent<CollectionQuest>();
                Hunter.GetComponent<CollectionQuest>().item = itemList[2];
                Hunter.GetComponent<CollectionQuest>().questName = "Bag-O-Blueprints";
                Hunter.GetComponent<CollectionQuest>().questType = Quest.questTypes.Collect;
                Hunter.GetComponent<CollectionQuest>().questDialog = "Please find my bag of blueprints";
                Hunter.GetComponent<CollectionQuest>().requiredItems = questRequirementList[7].requiredItems;
                Hunter.GetComponent<CollectionQuest>().rewards = questRequirementList[7].questRewards;
                Hunter.GetComponent<CollectionQuest>().hasStoryEffect = true;
                Hunter.GetComponent<CollectionQuest>().storyEffectIndex = 24;
                //Adds locationQuest to the bag
                break;
            case 24:
                Destroy(Hunter.GetComponent<CollectionQuest>());
                UIManager.uiManager.Dialog(dialogs[14].dialogText, Chars.Names.Pim.ToString(), Chars.Roles.Hunter.ToString(), true, 25);
                //Hunter tells you where the final boss is and how to get to it.. But that you need something stronger
                break;
            case 25:
                Hunter.AddComponent<NPC>();
                Hunter.GetComponent<NPC>().item = itemList[2];
                Hunter.GetComponent<NPC>().characterName = Chars.Names.Pim.ToString();
                Hunter.GetComponent<NPC>().role = Chars.Roles.Hunter.ToString();
                Hunter.GetComponent<NPC>().hasStoryEffect = false;
                Hunter.GetComponent<NPC>().dialogText = dialogs[15].dialogText;
                break;
        }
    }
    [System.Serializable]
    public class Dialogs
    {
        public List<string> dialogText = new List<string>();
        public AnimationClip anim;
    }
    [System.Serializable]
    public class Chars
    {
        public enum Roles { Mayor, Hunter, Miner, Villager, Slave, Unknown }
        public Roles charRole;
        public enum Names { Frank, Ashley, Pim, Maurits, Frans, Unknown }
        public Names charName;
    }
}
