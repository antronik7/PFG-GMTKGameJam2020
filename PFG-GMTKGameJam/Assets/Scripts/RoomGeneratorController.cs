using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneratorController : MonoBehaviour
{
    //Instances
    [SerializeField]
    GameObject corridor;
    [SerializeField]
    GameObject room;

    //Values
    [SerializeField]
    Vector2 startPosition;
    [SerializeField]
    float corridorSize;
    [SerializeField]
    float roomSize;
    [SerializeField]
    int nbrCorridorsToSpawn;

    //Variables
    Vector2 currentPosition;

    public static RoomGeneratorController instance = null;

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
        currentPosition = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateRoom()
    {
        for (int i = 0; i < nbrCorridorsToSpawn; ++i)
        {
            Instantiate(corridor, currentPosition, Quaternion.identity);
            currentPosition += Vector2.right * corridorSize;
        }

        currentPosition += (Vector2.right * (roomSize / 2)) - (Vector2.right * corridorSize / 2);
        Instantiate(room, currentPosition, Quaternion.identity);

        currentPosition += (Vector2.right * (roomSize / 2)) + (Vector2.right * corridorSize / 2);
    }
}
