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
    float playDialogSpeed = 0.1f;
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
                    if (dialogStats.afterEffect)
                    {
                        StoryLine.storyLine.storyCase = dialogStats.effectIndex;
                        //Storyline case run the index
                        StartCoroutine(StoryLine.storyLine.Story());
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }
                else
                {
                    StartCoroutine(DialogMethod(dialogStats.charTextHolder, dialogStats.charName, dialogStats.charRole));
                }
            }
            else
            {
                playDialogSpeed = 0;
            }
        }
        if (Input.GetButtonUp("Press[E]"))
        {
            if (!dialogDone)
            {
                playDialogSpeed = 0.1f;
            }
        }
	}
    public IEnumerator DialogMethod(List<string> dialog, string characterName, string characterRole, bool afterEffect = false, int effectIndex = 0)
    {
        if (firstDialog)
        {
            dialogStats.charTextHolder = dialog;
            dialogStats.afterEffect = afterEffect;
            dialogStats.effectIndex = effectIndex;
            dialogStats.charName = characterName;
            dialogStats.charRole = "-" + characterRole;
            charName.GetComponent<Text>().text = dialogStats.charName;
            charRole.GetComponent<Text>().text = dialogStats.charRole;
            dialogNum = 0;
            firstDialog = false;
        }
        dialogDone = false;
        dialogText.GetComponent<Text>().text = null;
        for (int i = 0; i < dialog[dialogNum].Length; i++)
        {
            dialogText.GetComponent<Text>().text += dialogStats.charTextHolder[dialogNum][i];
            yield return new WaitForSeconds(playDialogSpeed);
        }
        dialogNum++;
        dialogDone = true;
    }
    [System.Serializable]
    public class DialogBackup
    {
        public List<string> charTextHolder = new List<string>();
        public bool afterEffect;
        public int effectIndex;
        public string charName;
        public string charRole;
    }
}
