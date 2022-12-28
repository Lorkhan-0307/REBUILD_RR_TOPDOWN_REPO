using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Text nameText;
    private Text dialogueText;
    private Image image;
    [Header("Dialogues")]
    public GameObject dialogueBox;



    [Header("NPC Dialogues")]
    public GameObject nPCDialogueBox;
    public Text nPCNameText;
    public Text nPCDialogueText;
    public Image nPCImage;

    [Header("Player Dialogues")]
    public GameObject playerDialogueBox;
    public Text playerNameText;
    public Text playerDialogueText;
    public Image playerImage;

    public List<string> playerList;
    private bool playerCheck;

    private Queue<string> _sentences;
    private int lineNum = 0;

    private List<Dialogue> dialogues;
    
    [SerializeField] PauseManager pauseManager;

    void Start()
    {
        _sentences = new Queue<string>();
        dialogues = new List<Dialogue>();
    }

    public void GetDialogue(List<Dialogue> _dialogueList)
    {
        dialogues = _dialogueList;
        pauseManager.PauseGame();
        StartDialogue();
    }

    public void StartDialogue()
    {
        FetchNextDialogue();
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (_sentences.Count ==0)
        {
            FetchNextDialogue();
        }

        if(_sentences.Count != 0)
        {
            string _sentence = _sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(_sentence));
        }
    }


    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        Debug.Log("End Conversation");
        dialogueBox.SetActive(false);
        pauseManager.ResumeGame();
    }

    public void FetchNextDialogue()
    {
        if(lineNum < dialogues.Count)
        {
            if (dialogues[lineNum].name != "")
            {
                //playerCheck = Array.Exists(playerList, character => character == dialogues[lineNum].name);
                playerCheck = playerList.Contains(dialogues[lineNum].name);
                if (playerCheck)
                {
                    nameText = playerNameText;
                    dialogueText = playerDialogueText;
                    image = playerImage;
                    playerDialogueBox.SetActive(true);
                    nPCDialogueBox.SetActive(false);
                }
                else
                {
                    nameText = nPCNameText;
                    dialogueText = nPCDialogueText;
                    image = nPCImage;
                    playerDialogueBox.SetActive(false);
                    nPCDialogueBox.SetActive(true);
                }

                nameText.text = dialogues[lineNum].name;
            }
                

            //if(_sentences == null)
              //_sentences = new Queue<string>();

            _sentences.Clear();

            foreach (string sentence in dialogues[lineNum].sentences)
            {
                _sentences.Enqueue(sentence);
            }
            lineNum += 1;
        }
        else
        {
            EndDialogue();
        }
        
    }
}
