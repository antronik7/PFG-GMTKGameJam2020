using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    //Instances
    [SerializeField]
    Transform playerDestination;
    [SerializeField]
    public MonsterSlotController[] monsterSlots;


    //Awake is always called before any Start functions
    void Awake()
    {
        PrepareRoom();
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

    void PrepareRoom()
    {
        GameManager.instance.SetPlayerDestination(playerDestination.position);
        GameManager.instance.SetCurrentRoom(this);

        int randomNbrMonsters = Random.Range(1, 3);

        for (int i = 0; i < randomNbrMonsters; ++i)
        {
            monsterSlots[i].gameObject.SetActive(true);
        }
    }

    public void ActivateRoom()
    {
        for (int i = 0; i < monsterSlots.Length; ++i)
        {
            monsterSlots[i].DisplayCanInteract(true);
        }
    }

    public void ShowAvailabelSlots()
    {
        for (int i = 0; i < monsterSlots.Length; ++i)
        {
            monsterSlots[i].DisplayCanPlace();
        }
    }

    public void ShowEmptylSlots()
    {
        for (int i = 0; i < monsterSlots.Length; ++i)
        {
            monsterSlots[i].DisplayCanInteract(true);
        }
    }

    public bool CheckIfAllMonsterPlaced()
    {
        for (int i = 0; i < monsterSlots.Length; ++i)
        {
            if (monsterSlots[i].gameObject.activeSelf == true && monsterSlots[i].IsMonsterInSlot() == false)
                return false;
        }

        return true;
    }

    public bool CheckIfAllActionPlaced()
    {
        for (int i = 0; i < monsterSlots.Length; ++i)
        {
            if (monsterSlots[i].gameObject.activeSelf == true && monsterSlots[i].IsActionInSlot() == false)
                return false;
        }

        return true;
    }
}
