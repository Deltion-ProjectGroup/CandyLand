using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : Interactable {
    GameObject interactorr;
    public Rewards[] rewards;
    public enum questTypes { Collect, Find}
    public questTypes questType;
    public int currencyReward;
    public string questName;
    public string completionMsg;
    public string progressMsg;
    public string questDialog;
    public bool completed;
    public bool inProgress;
    public bool hasItems;
    public bool collected;
    public bool hasStoryEffect = false;
    public int storyEffectIndex;
    public static bool interactedQuest;
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}
    public virtual void checkIfComplete()
    {

    }
    public virtual void AcceptQuest()
    {
        print("ACCEPTED");
        checkIfComplete();
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = true;
            inProgress = true;
            checkIfComplete();
            RefreshButtons();
        }
    }
    public virtual void CancelQuest()
    {
        print("CANCELLED");
        if (inProgress)
        {
            inProgress = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = false;
            RefreshButtons();
        }
        if (completed)
        {
            inProgress = false;
            completed = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = false;
            RefreshButtons();
        }
    }
    public virtual void CollectQuest()
    {
        print("COLLECTED");
        for (int i = 0; i < rewards.Length; i++)
        {
            Inventory.instance.Add(rewards[i].itemRewards, rewards[i].rewardAmt, false);
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = false;
        collected = true;
        if (hasStoryEffect)
        {
            StoryLine.storyLine.storyCase = storyEffectIndex;
            StoryLine.storyLine.Story();
        }
        RefreshButtons();
    }
    public override void Interact(GameObject interactor)
    {
        interactedQuest = true;
        interactorr = interactor;
        Cursor.lockState = CursorLockMode.None;
        UIManager.uiManager.acceptQuest = AcceptQuest;
        UIManager.uiManager.cancelQuest = CancelQuest;
        UIManager.uiManager.completeQuest = CollectQuest;
        UIManager.uiManager.questStuff[UIManager.uiManager.questStuff.Length - 1].SetActive(true);
        Inventory.instance.InventoryOnOff();
        UIManager.uiManager.questStuff[0].GetComponent<Text>().text = questName;
        UIManager.uiManager.questStuff[1].GetComponent<Text>().text = questType.ToString();
        UIManager.uiManager.questStuff[2].GetComponent<Text>().text = questDialog;
        if(rewards.Length > 1)
        {
            UIManager.uiManager.questStuff[4].GetComponent<Text>().text = "QUEST REWARDS";
        }
        else
        {
            UIManager.uiManager.questStuff[4].GetComponent<Text>().text = "QUEST REWARD";
        }
        UIManager.uiManager.questStuff[5].GetComponent<Text>().text = null;
        for (int i = 0; i < rewards.Length; i++)
        {
            UIManager.uiManager.questStuff[5].GetComponent<Text>().text += rewards[i].rewardAmt + " * " + rewards[i].itemRewards.itemName + "\n";
        }
        RefreshButtons();
    }
    public void RefreshButtons()
    {
        for (int q = 6; q < 9; q++)
        {
            UIManager.uiManager.questStuff[q].SetActive(false);
        }
        if (!inProgress && !collected && !completed)
        {
            UIManager.uiManager.questStuff[6].SetActive(true);
        }
        else
        {
            if (!collected)
            {
                UIManager.uiManager.questStuff[7].SetActive(true);
            }
            if (completed && !collected)
            {
                UIManager.uiManager.questStuff[8].SetActive(true);
            }
        }
    }
    public IEnumerator Completemsg(string questName)
    {
        UIManager.uiManager.questStuff[9].SetActive(true);
        UIManager.uiManager.questStuff[10].GetComponent<Text>().text = "Quest: " + questName;
        UIManager.uiManager.questStuff[9].GetComponent<Animation>().Play("QuestCompleteAnim");
        yield return new WaitForEndOfFrame();
        UIManager.uiManager.questStuff[9].SetActive(false);
    }
    [System.Serializable]
    public class Rewards
    {
        public Item itemRewards;
        public int rewardAmt;
    }
}
