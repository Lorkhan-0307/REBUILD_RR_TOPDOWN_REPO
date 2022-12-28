using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialogueBox;

    private Queue<string> _sentences;
    private int lineNum = 0;

    private List<Dialogue> dialogues;

    void Start()
    {
        _sentences = new Queue<string>();
        dialogues = new List<Dialogue>();
        
    }

    public void GetDialogue(List<Dialogue> _dialogueList)
    {
        dialogues = _dialogueList;
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

        string _sentence = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(_sentence));
        //dialogueText.text = _sentence;
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
    }

    public void FetchNextDialogue()
    {
        if(lineNum < dialogues.Count)
        {
            if (dialogues[lineNum].name != "")
                nameText.text = dialogues[lineNum].name;

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
