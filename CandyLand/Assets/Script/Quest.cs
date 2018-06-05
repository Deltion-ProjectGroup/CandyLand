using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : Interactable {
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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AcceptQuest()
    {
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = true;
            inProgress = true;
        }
    }
    public void CancelQuest()
    {
        if (inProgress)
        {
            inProgress = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest = false;
        }
    }
    public virtual void CollectQuest()
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            Inventory.instance.Add(rewards[i].itemRewards, rewards[i].rewardAmt, false);
        }
    }
    public override void Interact(GameObject interactor)
    {
        UIManager.uiManager.questStuff[UIManager.uiManager.questStuff.Length - 1].SetActive(true);
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
        for(int q  = 6; q < 9; q++)
        {
            UIManager.uiManager.questStuff[q].SetActive(false);
        }
        if(!inProgress && !completed)
        {
            UIManager.uiManager.questStuff[6].SetActive(true);
        }
        else
        {
            if (inProgress)
            {
                UIManager.uiManager.questStuff[7].SetActive(true);
            }
            else
            {
                if (completed && inProgress)
                {
                    UIManager.uiManager.questStuff[8].SetActive(true);
                }
            }
        }
    }
    [System.Serializable]
    public class Rewards
    {
        public Item itemRewards;
        public int rewardAmt;
    }
}
