using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    [SerializeField] string csv_FileName;
    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();

    public static bool isFinish = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DialogueTrigger trigger = GetComponent<DialogueTrigger>();
            /*Dialogue[] dialogues = trigger.Parse();
            for(int i=0; i<dialogues.Length; i++)
            {

            }*/
        }
    }
}
