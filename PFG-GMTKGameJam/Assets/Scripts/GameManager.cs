using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Walking, SelectMonsters, Fight, TriggerAction };
    public enum ActionType { Attack, Block, Heal };

    public static GameManager instance = null;

    //Instances
    [SerializeField]
    CharacterController theCharacter;
    [SerializeField]
    GameObject[] monsters;
    [SerializeField]
    DeckController monsterDeck;
    [SerializeField]
    DeckController actionDeck;
    [SerializeField]
    GameObject fightText;

    //Values
    [SerializeField]
    int nbrMonstersInPool;
    [SerializeField]
    int nbrActionsInPool;

    //Variables
    public GameState currentState;
    public GameState previousState;
    public Vector3 playerDestination;
    public GameObject objectHolded = null;
    GameObject[] monstersPool;
    RoomController currentRoom;

    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartRun();
    }

    // Update is called once per frame
    void Update()
    {

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

    void StartRun()
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

        Debug.Log("2");
        actionDeck.DrawFromDeck();
    }

    IEnumerator TriggerAction()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < currentRoom.monsterSlots.Length; i++)
        {
            if (currentRoom.monsterSlots[i].TriggerAction())
                yield return new WaitForSeconds(1f);
        }

        //Action du joueur ici...
        yield return new WaitForSeconds(1f);

        ChangeState(GameState.Fight);
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
