using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Walking, SelectMonsters, Fight, TriggerAction, Win, Lose, Start };
    public enum ActionType { Attack, Block, Heal };

    public static GameManager instance = null;

    public int nbrRoomCleared = 0;
    public int nbrMonsterKill = 0;

    //Instances
    [SerializeField]
    public CharacterController theCharacter;
    [SerializeField]
    GameObject[] monsters;
    [SerializeField]
    public GameObject[] actions;
    [SerializeField]
    public DeckController monsterDeck;
    [SerializeField]
    public DeckController actionDeck;
    [SerializeField]
    GameObject fightText;
    [SerializeField]
    TMPro.TextMeshPro TxtDiff;
    [SerializeField]
    TMPro.TextMeshPro TxtDiffLevel;
    [SerializeField]
    GameObject gameOverScreen;

    //Values
    [SerializeField]
    int nbrMonstersInPool;
    [SerializeField]
    int nbrActionsInPool;

    //Variables
    public GameState currentState = GameState.Start;
    public GameState previousState;
    public Vector3 playerDestination;
    public GameObject objectHolded = null;
    GameObject[] monstersPool;
    public RoomController currentRoom;
    public int gameLevel = 0;
    float gametime;

    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != GameState.Start)
        {
            gametime += Time.deltaTime;
            if(gametime >= 30f)
            {
                gameLevel++;
                gametime = 0f;
            }

            TxtDiff.text = "Difficulty :";
            TxtDiffLevel.text = "LV " + (gameLevel + 1) + " " + ((int)gametime) + "/30";
        }
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (objectHolded != null)
            {
                if(currentState == GameState.SelectMonsters)
                {
                    objectHolded.GetComponent<MonsterController>().ReplaceInHand();
                }
                else
                {
                    objectHolded.GetComponent<ActionController>().ReplaceInHand();
                }

                objectHolded = null;
                DropObject();
            }
        }
    }

    public void StartRun()
    {
        RoomGeneratorController.instance.GenerateRoom();
        GenerateMonsters();
        ChangeState(GameState.Walking);
    }

    public void ChangeState(GameState newState)
    {
        previousState = currentState;
        currentState = newState;

        if (currentState == GameState.Walking)
        {
            theCharacter.StartAnimation("Walk");
        }
        else if (currentState == GameState.SelectMonsters)
        {
            theCharacter.StartAnimation("Idle");
            currentRoom.ActivateRoom();
            monsterDeck.Show();
            monsterDeck.DrawFromDeck();
        }
        else if (currentState == GameState.Fight)
        {
            Debug.Log("1");
            StartCoroutine(TransitionToFight());
        }
        else if (currentState == GameState.TriggerAction)
        {
            StartCoroutine(TriggerAction());
        }
        else if (currentState == GameState.Win)
        {
            nbrRoomCleared++;
            StopAllCoroutines();
            StartCoroutine(Winning());
        }
        else if (currentState == GameState.Lose)
        {
            actionDeck.Hide();
            StopAllCoroutines();
            gameOverScreen.SetActive(true);
        }
    }

    IEnumerator TransitionToFight()
    {
        if(previousState != GameState.TriggerAction)
        {
            monsterDeck.Hide();
            yield return new WaitForSeconds(0.5f);
            fightText.GetComponent<Animator>().SetTrigger("Fight");
            actionDeck.Show();
            yield return new WaitForSeconds(0.6f);
        }

        theCharacter.GetAction();
        actionDeck.DrawFromDeck();
    }

    IEnumerator TriggerAction()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < currentRoom.monsterSlots.Length; i++)
        {
            if (currentRoom.monsterSlots[i].TriggerAction())
                yield return new WaitForSeconds(0.5f);
        }

        theCharacter.TriggerAction();
        yield return new WaitForSeconds(0.5f);

        currentRoom.ShowEmptylSlots();
        Debug.Log("ICI");

        ChangeState(GameState.Fight);
    }

    IEnumerator Winning()
    {
        yield return new WaitForSeconds(0.5f);
        actionDeck.Hide();
        RoomGeneratorController.instance.GenerateRoom();
        ChangeState(GameState.Walking);
    }

    public void SetPlayerDestination(Vector2 newDestination)
    {
        playerDestination = newDestination;
    }

    public void SetCurrentRoom(RoomController newRoom)
    {
        currentRoom = newRoom;
    }

    private void GenerateMonsters()
    {
        Vector3 monsterSpawnPosition = new Vector3(-100f, -100f, 0f);
        Vector3 monsterSpawnRotation = new Vector3(0f, 180f, 0f);
        monstersPool = new GameObject[nbrMonstersInPool];

        for (int i = 0; i < nbrMonstersInPool; ++i)
        {
            int randomMonsterIndex = Random.Range(0, monsters.Length);
            GameObject monster = Instantiate(monsters[randomMonsterIndex], monsterSpawnPosition, Quaternion.identity);
            monster.transform.eulerAngles = monsterSpawnRotation;
            monstersPool[i] = monster;
            monster.SetActive(false);
        }

        monsterDeck.ShuffleDeck();
    }

    public int GetNumberMonsterInPool ()
    {
        return nbrMonstersInPool;
    }

    public int GetNumberActionInPool()
    {
        return nbrActionsInPool;
    }

    public GameObject GetMonsterFromPool(int index)
    {
        return monstersPool[index];
    }

    public void PickUpObject()
    {
        currentRoom.ShowAvailabelSlots();
    }

    public void DropObject()
    {
        currentRoom.ShowEmptylSlots();
    }

    public bool CheckIfAllMonsterArePlaced()
    {
        return currentRoom.CheckIfAllMonsterPlaced();
    }

    public bool CheckIfAllActionsArePlaced()
    {
        return currentRoom.CheckIfAllActionPlaced();
    }
}
