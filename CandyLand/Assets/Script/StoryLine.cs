﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryLine : MonoBehaviour
{
    public bool destroyCam;
    public int storyCase;
    public Dialogs[] dialogs;
    public PosCordinates[] locQuestPositions;
    public static StoryLine storyLine;
    public Blueprint[] blueprints;
    public List<ColQuestReq> questRequirementList = new List<ColQuestReq>();
    public GameObject Mayor;
    public GameObject Hunter;
    public GameObject Miner;
    public GameObject hotBar;
    public Item[] itemList;
    public GameObject storyCam;
    // Use this for initialization
    void Awake()
    {
        storyLine = this;
    }
    public void Update()
    {
        if (!destroyCam)
        {
            if (storyCam == null)
            {
                destroyCam = true;
                storyCase = 3;
                StartCoroutine(Story());
            }
        }
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
                UIManager.uiManager.Dialog(dialogs[1].dialogText, UIManager.uiManager.charNames[0], UIManager.uiManager.charRoles[0], true, 1, true);
                yield return new WaitForEndOfFrame(); // wait till anim is over + 1 sec or so
                break;
            case 1:
                //Mayor talks to you, change the camera towards him, play anim if he has one
                yield return new WaitForEndOfFrame();
                UIManager.uiManager.Dialog(dialogs[17].dialogText, UIManager.uiManager.charNames[0], UIManager.uiManager.charRoles[0]);
                storyCam.SetActive(true);
                hotBar.SetActive(false);
                storyCam.GetComponent<Animation>().Play();
                Destroy(storyCam, storyCam.GetComponent<Animation>().clip.length);
                break;
            case 3:
                hotBar.SetActive(true);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UnFreeze();
                Mayor.AddComponent<NPC>();
                NPC mayorNPC = Mayor.GetComponent<NPC>();
                Mayor.GetComponent<NPC>().item = itemList[2];
                //mayorNPC.item = itemList[2];
                mayorNPC.characterName = UIManager.uiManager.charNames[0];
                mayorNPC.role = UIManager.uiManager.charRoles[0];
                mayorNPC.hasStoryEffect = true;
                mayorNPC.storyEffectIndex = 4;
                mayorNPC.dialogText = dialogs[3].dialogText;

                //Cam changes towards the tutorial place, Mayors says you can get up trough there
                //Talk with mayor is on NPC script #3.5
                break;
            case 4:
                //UIManager.uiManager.dialogUI.SetActive(false);
                Destroy(Mayor.GetComponent<NPC>());
                Mayor.AddComponent<CollectionQuest>();
                CollectionQuest mayorColl = Mayor.GetComponent<CollectionQuest>();
                mayorColl.item = itemList[2];
                mayorColl.questName = "Get some stuff";
                mayorColl.questType = Quest.questTypes.Collect;
                mayorColl.questDialog = "Hey, I really need you to collect some recources for me. Would you be so kind to go find me some, they are probably nearby";
                mayorColl.requiredItems = questRequirementList[0].requiredItems;
                mayorColl.rewards = questRequirementList[0].questRewards;
                mayorColl.hasStoryEffect = true;
                mayorColl.storyEffectIndex = 5;
                break;
            case 5:
                Destroy(Mayor.GetComponent<CollectionQuest>());
                UIManager.uiManager.Dialog(dialogs[5].dialogText, UIManager.uiManager.charNames[0], UIManager.uiManager.charRoles[0], true, 6);
                // Collection quest turns into Dialog text, follow him text
                break;
            case 6:
                //UIManager.uiManager.dialogUI.SetActive(false);
                Mayor.AddComponent<NPCFollow>();
                Mayor.GetComponent<NPCFollow>().targetLocation = locQuestPositions[0].movePosNPC;
                StartCoroutine(Mayor.GetComponent<NPCFollow>().StartMove(1));
                Mayor.AddComponent<LocationQuest>();
                LocationQuest mayorLoc = Mayor.GetComponent<LocationQuest>();
                mayorLoc.questPositions = locQuestPositions[0].coördinates;
                mayorLoc.item = itemList[2];
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = false;
                mayorLoc.AcceptQuest();
                //Mayor.GetComponent<LocationQuest>().item = 
                mayorLoc.questName = "Follow the Mayor";
                mayorLoc.questDialog = "Good job following me here!";
                mayorLoc.questRequirement = "Follow the Mayor to the blacksmith.";
                mayorLoc.questType = Quest.questTypes.Find;
                mayorLoc.hasStoryEffect = true;
                mayorLoc.storyEffectIndex = 7;
                mayorLoc.rewards = questRequirementList[1].questRewards;
                //NPC text turns into a location quest, wait with this until he is at the blacksmith
                //Mayor moves towards the blacksmith
                break;
            case 7:
                Destroy(Mayor.GetComponent<NPCFollow>());
                Destroy(Mayor.GetComponent<SphereCollider>());
                //After collecting the quest turn him into an AI again, he has to go away, he asks you to make the axe, work and come back later.
                Destroy(Mayor.GetComponent<LocationQuest>());
                UIManager.uiManager.Dialog(dialogs[7].dialogText, UIManager.uiManager.charNames[0], UIManager.uiManager.charRoles[0], true, 8);
                //Mayor goes away
                break;
            case 8:
                Mayor.AddComponent<NPCFollow>();
                Mayor.GetComponent<NPCFollow>().needsPlayer = false;
                Mayor.GetComponent<NPCFollow>().targetLocation = locQuestPositions[1].movePosNPC;
                StartCoroutine(Mayor.GetComponent<NPCFollow>().StartMove(0));
                Crafting.crafting.AddBlueprint(blueprints[4]);
                break;
            //play Mayor leaving anim

            case 9:
                Vector3 playerPos;
                playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                playerPos.z += 3f;
                UIManager.uiManager.Dialog(dialogs[8].dialogText, UIManager.uiManager.charNames[1], UIManager.uiManager.charRoles[1], true, 10);
                Destroy(Mayor.GetComponent<NPCFollow>());
                Destroy(Mayor.GetComponent<SphereCollider>());
                Hunter.AddComponent<NPCFollow>();
                Hunter.GetComponent<NPCFollow>().needsPlayer = false;
                Hunter.GetComponent<NPCFollow>().targetLocation = playerPos; // leave 2 free
                StartCoroutine(Hunter.GetComponent<NPCFollow>().StartMove(0));
                // Happened after first craft
                break;
            case 10:
                //Hunter leaving anim
                Hunter.GetComponent<NPCFollow>().targetLocation = locQuestPositions[3].movePosNPC; // leave 3 free
                StartCoroutine(Hunter.GetComponent<NPCFollow>().StartMove(0));
                goto case 11;
            case 11:
                Destroy(Hunter.GetComponent<NPCFollow>(), 0.1f);
                Destroy(Hunter.GetComponent<SphereCollider>());
                Mayor.AddComponent<NPC>();
                NPC mayorNPC2 = Mayor.GetComponent<NPC>();
                mayorNPC2.item = itemList[2];
                mayorNPC2.characterName = UIManager.uiManager.charNames[0];
                mayorNPC2.role = UIManager.uiManager.charRoles[0];
                mayorNPC2.hasStoryEffect = true;
                mayorNPC2.storyEffectIndex = 12;
                mayorNPC2.dialogText = dialogs[9].dialogText;
                break;
            case 12:
                //Mayor moves towards location
                Destroy(Mayor.GetComponent<NPC>());
                Mayor.AddComponent<NPCFollow>();
                Mayor.GetComponent<NPCFollow>().needsPlayer = false;
                Mayor.GetComponent<NPCFollow>().targetLocation = locQuestPositions[4].movePosNPC; // leave 4 free
                StartCoroutine(Mayor.GetComponent<NPCFollow>().StartMove(1));
                Mayor.AddComponent<LocationQuest>();
                LocationQuest mayorLoc2 = Mayor.GetComponent<LocationQuest>();
                mayorLoc2.questPositions = locQuestPositions[4].coördinates;
                mayorLoc2.item = itemList[2];
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = false;
                mayorLoc2.AcceptQuest();
                mayorLoc2.questName = "Follow the Mayor";
                mayorLoc2.questDialog = "Thanks for following me, could you gather me some recources now?";
                mayorLoc2.questRequirement = "Follow the Mayor towards a safe farming spot";
                mayorLoc2.questType = Quest.questTypes.Find;
                mayorLoc2.hasStoryEffect = true;
                mayorLoc2.storyEffectIndex = 13;
                mayorLoc2.rewards = questRequirementList[2].questRewards;
                break;
            case 13:
                Destroy(Mayor.GetComponent<LocationQuest>());
                Destroy(Mayor.GetComponent<NPCFollow>());
                Destroy(Mayor.GetComponent<SphereCollider>());
                Mayor.AddComponent<CollectionQuest>();
                CollectionQuest mayorCol2 = Mayor.GetComponent<CollectionQuest>();
                mayorCol2.item = itemList[2];
                mayorCol2.questName = "Chop it";
                mayorCol2.questType = Quest.questTypes.Collect;
                mayorCol2.questDialog = "Since we have finally arrived here, could you collect some candywood, I would really appreciate it if you would.";
                mayorCol2.requiredItems = questRequirementList[3].requiredItems;
                mayorCol2.rewards = questRequirementList[3].questRewards;
                mayorCol2.hasStoryEffect = true;
                mayorCol2.storyEffectIndex = 14;
                break;
            case 14:
                Destroy(Mayor.GetComponent<CollectionQuest>());
                UIManager.uiManager.Dialog(dialogs[16].dialogText, UIManager.uiManager.charNames[0], UIManager.uiManager.charRoles[0]);
                Crafting.crafting.AddBlueprint(blueprints[0]);
                // Dialog that you got a blueprint also from him.
                //Add the blueprint also in this case.
                // He'll send a miner he says
                break;
            case 15:
                UIManager.uiManager.Dialog(dialogs[10].dialogText, UIManager.uiManager.charNames[2], UIManager.uiManager.charRoles[2], true, 16);
                Miner.AddComponent<NPCFollow>();
                NPCFollow minerFollow = Miner.GetComponent<NPCFollow>();
                minerFollow.needsPlayer = false;
                playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                playerPos.z += 3f;
                minerFollow.targetLocation = playerPos;
                StartCoroutine(minerFollow.StartMove(1));
                break;
            case 16:
                // MAYOR HAS TO BECOME THE MINER MODEL!!!!
                //Miner runs
                NPCFollow minerFollow2 = Miner.GetComponent<NPCFollow>();
                minerFollow2.needsPlayer = false;
                minerFollow2.targetLocation = locQuestPositions[6].movePosNPC;
                StartCoroutine(minerFollow2.StartMove(1));
                Miner.AddComponent<LocationQuest>();
                LocationQuest minerLoc = Miner.GetComponent<LocationQuest>();
                minerLoc.questPositions = locQuestPositions[6].coördinates;
                minerLoc.item = itemList[2];
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = false;
                minerLoc.AcceptQuest();
                minerLoc.questName = "Follow the Miner";
                minerLoc.questDialog = "Hey good job following me.. Let's start mining!";
                minerLoc.questRequirement = "Follow the Miner towards the mining spot";
                minerLoc.questType = Quest.questTypes.Find;
                minerLoc.hasStoryEffect = true;
                minerLoc.storyEffectIndex = 17;
                minerLoc.rewards = questRequirementList[4].questRewards;
                break;
            case 17:
                Destroy(Miner.GetComponent<NPCFollow>());
                Destroy(Hunter.GetComponent<SphereCollider>());
                Destroy(Miner.GetComponent<LocationQuest>());
                UIManager.uiManager.Dialog(dialogs[11].dialogText, UIManager.uiManager.charNames[2], UIManager.uiManager.charRoles[2], true, 18);
                break;
            case 18:
                // MAYOR HAS TO BECOME THE MINER MODEL!!!!
                Miner.AddComponent<CollectionQuest>();
                CollectionQuest minerCol = Miner.GetComponent<CollectionQuest>();
                minerCol.item = itemList[2];
                minerCol.questName = "Mine Away";
                minerCol.questType = Quest.questTypes.Collect;
                minerCol.questDialog = "Please bring me some of these materials from the mine";
                minerCol.requiredItems = questRequirementList[5].requiredItems;
                minerCol.rewards = questRequirementList[5].questRewards;
                minerCol.hasStoryEffect = true;
                minerCol.storyEffectIndex = 19;
                break;
            case 19:
                Crafting.crafting.AddBlueprint(blueprints[3]);
                Destroy(Miner.GetComponent<CollectionQuest>());
                Hunter.AddComponent<NPCFollow>();
                NPCFollow hunterFollow = Hunter.GetComponent<NPCFollow>();
                hunterFollow.needsPlayer = false;
                playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                playerPos.z += 3f;
                hunterFollow.targetLocation = playerPos;
                StartCoroutine(hunterFollow.StartMove(1));
                UIManager.uiManager.Dialog(dialogs[12].dialogText, UIManager.uiManager.charNames[1], UIManager.uiManager.charRoles[1], true, 20);
                break;
            case 20:
                NPCFollow hunterFollow2 = Hunter.GetComponent<NPCFollow>();
                hunterFollow2.targetLocation = locQuestPositions[8].movePosNPC;
                StartCoroutine(hunterFollow2.StartMove(1));
                //Hunter leaving anim
                goto case 21;

            case 21:
                //MAYOR IS HUNTER
                Hunter.AddComponent<CollectionQuest>();
                CollectionQuest hunterCol = Hunter.GetComponent<CollectionQuest>();
                hunterCol.item = itemList[2];
                hunterCol.questName = "HouseParty";
                hunterCol.questType = Quest.questTypes.Collect;
                hunterCol.questDialog = "Did you bring my materials?";
                hunterCol.requiredItems = questRequirementList[6].requiredItems;
                hunterCol.rewards = questRequirementList[6].questRewards;
                hunterCol.hasStoryEffect = true;
                hunterCol.storyEffectIndex = 22;
                //Add the quest component to the hunter..
                //Add without the player noticing that you can now forge a sword.
                break;
            case 22:
                Crafting.crafting.AddBlueprint(blueprints[1]);
                Crafting.crafting.AddBlueprint(blueprints[2]);
                Destroy(Hunter.GetComponent<NPCFollow>());
                Destroy(Hunter.GetComponent<SphereCollider>());
                Destroy(Hunter.GetComponent<CollectionQuest>());
                UIManager.uiManager.Dialog(dialogs[13].dialogText, UIManager.uiManager.charNames[1], UIManager.uiManager.charRoles[1], true, 25);
                //Hunter tells you about how forging swords works and who you are
                //Also tells you that he forgot his bag and asks you to retreive it.
                break;
            case 23:
                /*Hunter.AddComponent<CollectionQuest>();
                CollectionQuest hunterCol2 = Hunter.GetComponent<CollectionQuest>();
                hunterCol2.item = itemList[2];
                hunterCol2.questName = "Bag-O-Blueprints";
                hunterCol2.questType = Quest.questTypes.Collect;
                hunterCol2.questDialog = "Please find my bag of blueprints";
                hunterCol2.requiredItems = questRequirementList[7].requiredItems;
                hunterCol2.rewards = questRequirementList[7].questRewards;
                hunterCol2.hasStoryEffect = true;
                hunterCol2.storyEffectIndex = 24;*/
                //Adds locationQuest to the bag
                goto case 24;
            case 24:
                //Destroy(Hunter.GetComponent<CollectionQuest>());
                UIManager.uiManager.Dialog(dialogs[14].dialogText, UIManager.uiManager.charNames[1], UIManager.uiManager.charRoles[1], true, 25);
                //Hunter tells you where the final boss is and how to get to it.. But that you need something stronger
                break;
            case 25:
                Hunter.AddComponent<NPC>();
                Hunter.GetComponent<NPC>().item = itemList[2];
                Hunter.GetComponent<NPC>().characterName = UIManager.uiManager.charNames[1];
                Hunter.GetComponent<NPC>().role = UIManager.uiManager.charRoles[1];
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
