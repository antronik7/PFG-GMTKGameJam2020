using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Walking, SelectMonsters, Fight };

    public static GameManager instance = null;

    public CharacterController theCharacter;

    //Variables
    public GameState currentState;
    public Vector3 playerDestination;

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

    void StartRun()
    {
        RoomGeneratorController.instance.GenerateRoom();
        ChangeState(GameState.Walking);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        if (currentState == GameState.Walking)
        {
            theCharacter.StartAnimation("Walk");  
        }
        else if (currentState == GameState.SelectMonsters)
        {
            theCharacter.StartAnimation("Idle");
        }
    }

    public void SetPlayerDestination(Vector2 newDestination)
    {
        playerDestination = newDestination;
    }
}
