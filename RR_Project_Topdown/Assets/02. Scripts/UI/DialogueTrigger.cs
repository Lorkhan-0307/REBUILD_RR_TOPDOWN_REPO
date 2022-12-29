using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public string _CSVFileName = "Dialogue03";
    public GameObject dialogueBox;
    public void TriggerDialogue()
    {
        dialogueBox.SetActive(true);
        //FindObjectOfType<DialogueManager>().StartDialogue();
        Parse();
    }

    public void Parse()
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);
        if(csvData == null)
        {
            Debug.LogError("CANNOT READ CSV!");
        }    
        else
        {
            string[] data = csvData.text.Split(new char[] { '\n' });
            Debug.Log($"Data Length is {data.Length}");
            
            for (int i = 1; i < data.Length-1; )
            {
                string[] row = data[i].Split(new char[] { ',' });
                Dialogue _dialogue = new Dialogue();

                _dialogue.name = row[1];

                List<string> _dialogueList = new List<string>();
                //Debug.Log($"row is {row[2]}");


                do
                {
                    _dialogueList.Add(row[2]);
                    
                    if (++i < data.Length-1)
                    {
                        row = data[i].Split(new char[] { ',' });
                    }
                    else
                    {
                        break;
                    }
                }
                while (row[0].ToString() == "");

                _dialogue.emotion = row[3];

                _dialogue.sentences = _dialogueList.ToArray();

                dialogueList.Add(_dialogue);
            }
            if(dialogueList != null)
                FindObjectOfType<DialogueManager>().GetDialogue(dialogueList);
        }
        

        //return dialogueList.ToArray();
    }
}
