using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public GameObject charName;
    public GameObject charRole;
    public GameObject dialogText;
    public DialogBackup dialogStats;
    bool dialogDone = true;
    bool firstDialog = true;
    int dialogNum;
    float playDialogSpeed = 0;
    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Press[E]"))
        {
            if (dialogDone)
            {
                if(dialogStats.charTextHolder.Count == dialogNum)
                {
                    firstDialog = true;
                    dialogNum = 0;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UnFreeze();
                    if (dialogStats.afterEffect)
                    {
                        StoryLine.storyLine.storyCase = dialogStats.effectIndex;
                        //Storyline case run the index
                        StartCoroutine(StoryLine.storyLine.Story());
                        if (!dialogStats.nextIndexIsDialog)
                        {
                            gameObject.SetActive(false);
                            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canInteract = true;
                        }
                    }
                    else
                    {
                        gameObject.SetActive(false);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canInteract = true;
                    }
                }
                else
                {
                    startDialog(dialogStats.charTextHolder, dialogStats.charName, dialogStats.charRole, dialogStats.afterEffect, dialogStats.effectIndex, dialogStats.nextIndexIsDialog);
                }
            }
            else
            {
                //First dialog text won't do its thing, if it is played normally the rest will work also.. 
                if(dialogNum >= 0)
                {
                    StopAllCoroutines();
                    print(dialogNum);
                    dialogText.GetComponent<Text>().text = dialogStats.charTextHolder[dialogNum];
                    dialogNum++;
                    dialogDone = true;
                }
            }
        }
	}
    public void startDialog(List<string> dialogs, string charNames, string charRoles, bool hasAfterEffect = false, int effectNum = 0, bool nextIsDialog = false)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Freeze();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canInteract = false;
        StartCoroutine(DialogMethod(dialogs, charNames, charRoles, hasAfterEffect, effectNum, nextIsDialog));
    }
    IEnumerator DialogMethod(List<string> dialog, string characterName, string characterRole, bool afterEffect = false, int effectIndex = 0, bool nextIndexIsDialog = false)
    {
        dialogDone = false;
        if (firstDialog)
        {
            dialogStats.charTextHolder = dialog;
            dialogStats.afterEffect = afterEffect;
            dialogStats.effectIndex = effectIndex;
            dialogStats.charName = characterName;
            dialogStats.charRole = "-" + characterRole;
            dialogStats.nextIndexIsDialog = nextIndexIsDialog;
            charName.GetComponent<Text>().text = dialogStats.charName;
            charRole.GetComponent<Text>().text = dialogStats.charRole;
            dialogNum = 0;
            firstDialog = false;
        }
        dialogText.GetComponent<Text>().text = null;
        for (int i = 0; i < dialog[dialogNum].Length; i++)
        {
            dialogText.GetComponent<Text>().text += dialogStats.charTextHolder[dialogNum][i];
            yield return new WaitForSeconds(0.1f);
        }
        dialogNum++;
        dialogDone = true;
    }
    [System.Serializable]
    public class DialogBackup
    {
        public List<string> charTextHolder = new List<string>();
        public bool afterEffect;
        public bool nextIndexIsDialog;
        public int effectIndex;
        public string charName;
        public string charRole;
    }
}
