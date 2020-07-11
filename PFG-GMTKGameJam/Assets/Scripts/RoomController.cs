using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    //Instances
    [SerializeField]
    Transform playerDestination;

    //Awake is always called before any Start functions
    void Awake()
    {
        GameManager.instance.SetPlayerDestination(playerDestination.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetPlayerDestination()
    {
        return playerDestination.position;
    }
}
