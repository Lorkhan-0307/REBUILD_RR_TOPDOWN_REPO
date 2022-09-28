using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    /*[SerializeField] private int totalRoom = 3;
    [SerializeField] private GameObject[] roomPositions;
    [SerializeField] private GameObject Player;
    private bool[] isRoomClear;
    private int ranNum;
    private int count = 1;

    // Start is called before the first frame update
    private void Awake()
    {
        isRoomClear = new bool[totalRoom];
        isRoomClear[0] = true;

        for (int i = 1; i < totalRoom; i++)
        {
            isRoomClear[i] = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log($"{count}, {isRoomClear[0]}, {isRoomClear[1]}, {isRoomClear[2]}");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CheckRoomClear();
        }
     }

    private void Interact()
    {
        if(count < totalRoom)
        {
            isRoomClear[ranNum] = true;
            Player.transform.position = roomPositions[ranNum].transform.position;
            count++;
        }
        else
        {
            //Teleport to Boss Room
            Debug.Log("Teleport Boss Room");
        }
    }

    private void CheckRoomClear()
    {
        while (true)
        {
            ranNum = Random.Range(1, totalRoom);

            if (isRoomClear[ranNum]) continue;

            else
            {
                break;
            }
        }
    }*/

    public static RoomManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RoomManager>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("RoomManager");
                    instance = instanceContainer.AddComponent<RoomManager>();
                }
            }
            return instance;
        }
    }

    private static RoomManager instance;

    [SerializeField] private GameObject Player;

    [System.Serializable]
    public class StartPositionArray
    {
        public List<Transform> StartPosition = new List<Transform>();
    }

    public StartPositionArray startPositionArray;

    [SerializeField] private int LastStage = 3;
    [SerializeField] EnemySpawnPoolController enemySpawnPoolController;
    [SerializeField] private List<GameObject> potals;

    private int currentStage = 0;
    private int stageNum;

    private void Awake()
    {
        /*for(int i = 0; i < potals.Count; i++)
        {
            potals[i].SetActive(false);
        }*/
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        enemySpawnPoolController.OnSetActivePotal += SetActivePotal;
    }

    public void NextStage()
    {
        //현재 스테이지의 포탈 off
        potals[stageNum].SetActive(false);

        currentStage++;
        Debug.Log(currentStage);

        if (currentStage > LastStage)
        {
            Debug.Log("BossRoom");
            return;
        }

        else
        {
            int randomIndex = Random.Range(0, startPositionArray.StartPosition.Count);
            Debug.Log(randomIndex);
            Player.transform.position = startPositionArray.StartPosition[randomIndex].position;
            startPositionArray.StartPosition.RemoveAt(randomIndex);
        }
    }

    private void SetActivePotal(object sender, EnemySpawnPoolController.OnSetActivePotalEventArgs e)
    {
        stageNum = e.stageNum;
        //Debug.Log(stageNum);
        potals[e.stageNum].SetActive(true);
    }
}
