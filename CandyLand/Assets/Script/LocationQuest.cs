using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationQuest : Quest {
    public string questRequirement;
    public float[] questPositions = new float[4];
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}
    public override void AcceptQuest()
    {
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasQuest)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().movementDelegate += checkIfComplete;
        }
        base.AcceptQuest();
    }
    public override void CancelQuest()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().movementDelegate -= checkIfComplete;
        base.CancelQuest();
    }
    public override void CollectQuest()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().movementDelegate -= checkIfComplete;
        base.CollectQuest();
        if (hasStoryEffect)
        {
            StoryLine.storyLine.storyCase = storyEffectIndex;
            Inventory.instance.OnTab();
            StartCoroutine(StoryLine.storyLine.Story());
        }
    }
    public override void Interact(GameObject interactor)
    {
        UIManager.uiManager.questStuff[3].GetComponent<Text>().text = null;
        UIManager.uiManager.questStuff[3].GetComponent<Text>().text = questRequirement;
        base.Interact(interactor);
    }
    public override void checkIfComplete()
    {
        if (!completed)
        {
            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            if (playerPos.z < questPositions[0] && playerPos.z > questPositions[2])
            {
                if (playerPos.x < questPositions[1] && playerPos.x > questPositions[3])
                {
                    inProgress = false;
                    completed = true;
                    StartCoroutine(CompleteMsg(questName));
                    print("Completed");
                }
            }
        }
    }
    public IEnumerator CompleteMsg(string questName)
    {
        UIManager.uiManager.questStuff[9].SetActive(true);
        UIManager.uiManager.questStuff[10].GetComponent<Text>().text = "Quest: " + questName;
        UIManager.uiManager.questStuff[9].GetComponent<Animation>().Play();
        yield return new WaitForSeconds(UIManager.uiManager.questStuff[9].GetComponent<Animation>().clip.length);
        UIManager.uiManager.questStuff[9].SetActive(false);
    }
}
