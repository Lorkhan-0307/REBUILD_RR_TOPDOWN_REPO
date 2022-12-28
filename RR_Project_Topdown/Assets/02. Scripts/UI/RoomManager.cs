using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
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

    [SerializeField] private GameObject SceneLoader;
    [SerializeField] private Animator Transition;
    [SerializeField] private float transitionTime = 4f;
    private GameObject Player;
    [System.Serializable]
    public class StartPositionArray
    {
        public List<Transform> StartPosition = new List<Transform>();
    }

    public StartPositionArray startPositionArray;

    //[SerializeField] private int LastStage = 3;
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
        potals[currentStage].SetActive(false);
        potals.RemoveAt(currentStage);

        if (startPositionArray.StartPosition.Count == 0)
        {
            Debug.Log("BossRoom");
            return;
        }

        StartCoroutine(ChangeStage());

        int randomIndex = Random.Range(0, startPositionArray.StartPosition.Count);
        Player.transform.position = startPositionArray.StartPosition[randomIndex].position;
        startPositionArray.StartPosition.RemoveAt(randomIndex);

        enemySpawnPoolController.spawnPoints.RemoveAt(currentStage);
        enemySpawnPoolController.spawnCountList.RemoveAt(currentStage);
        enemySpawnPoolController.stageNum = randomIndex;
        currentStage = randomIndex;


    }

    private void SetActivePotal(object sender, EnemySpawnPoolController.OnSetActivePotalEventArgs e)
    {
        //Debug.Log(stageNum);
        potals[currentStage].SetActive(true);
    }

    private IEnumerator ChangeStage()
    {
        SceneLoader.SetActive(true);
        Transition.SetBool("Start", true);

        yield return new WaitForSeconds(transitionTime);
        SceneLoader.SetActive(false);

    }
}
