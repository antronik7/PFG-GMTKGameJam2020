using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    //Values
    [SerializeField]
    int baseHP;
    [SerializeField]
    int baseAtkValue;
    [SerializeField]
    int baseExpValue;
    [SerializeField]
    float speedAttack;
    [SerializeField]
    TMPro.TextMeshPro textHP;
    [SerializeField]
    GameObject hearth;
    bool isBlocking = true;

    //Components
    Collider2D myCollider;

    //Variables
    bool isInHand = false;
    bool isHolded = false;
    MonsterSlotController currentSlot;
    float speed;
    Transform destination;
    Vector3 previousPosition;
    bool isAttacking = false;
    bool isBacking = false;
    int handIndex;
    int maxHP;
    int atkValue;
    public int currentHP;
    public int currentExpValue;


    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.currentState == GameManager.GameState.SelectMonsters)
        {
            if (isInHand)
            {
                // Move our position a step closer to the target.
                float step = speedAttack * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, destination.position, step);
            }
            else if (isHolded)
            {
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            }
        }
        else if(GameManager.instance.currentState == GameManager.GameState.TriggerAction)
        {
            if (isAttacking)
            {
                // Move our position a step closer to the target.
                float step = speedAttack * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.playerDestination, step);

                if (Vector3.Distance(transform.position, GameManager.instance.playerDestination) < 0.001f)
                {
                    GameManager.instance.theCharacter.TakeDamage(atkValue);
                    isAttacking = false;
                    isBacking = true;
                }
            }
            else if (isBacking)
            {
                // Move our position a step closer to the target.
                float step = speedAttack * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, previousPosition, step);

                if (Vector3.Distance(transform.position, previousPosition) < 0.001f)
                {
                    isBacking = false;
                }
            }
        }

        if(GameManager.instance.currentState == GameManager.GameState.SelectMonsters)
        {
            textHP.gameObject.SetActive(false);
            hearth.gameObject.SetActive(false);
        }
        else
        {
            if(isInHand == false)
            {
                textHP.gameObject.SetActive(true);
                hearth.gameObject.SetActive(true);

                textHP.text = currentHP + "/" + maxHP;
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(GameManager.instance.objectHolded == null)
            {
                GameManager.instance.objectHolded = gameObject;
                isHolded = true;
                isInHand = false;
                myCollider.enabled = false;
                GameManager.instance.PickUpObject();
            }
        }
    }

    public void ReplaceInHand()
    {
        isHolded = false;
        isInHand = true;
        transform.position = destination.position;
        myCollider.enabled = true;
    }

    public void PlaceMonsterInSlot(MonsterSlotController newSlot)
    {
        isHolded = false;
        currentSlot = newSlot;
        transform.position = newSlot.transform.position;
        transform.parent = null;
        GameManager.instance.objectHolded = null;
        GameManager.instance.monsterDeck.RemoveFromHandSlot(handIndex);

        if (GameManager.instance.CheckIfAllMonsterArePlaced())
            GameManager.instance.ChangeState(GameManager.GameState.Fight);
    }

    public void DrawMonster(Vector3 startPositoin, Transform handPosition, float moveSpeed, Transform parent, int handSlotIndex)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = startPositoin;
        gameObject.transform.parent = parent;
        destination = handPosition;
        speed = moveSpeed;
        isInHand = true;
        handIndex = handSlotIndex;
        gameObject.GetComponent<Collider2D>().enabled = true;

        //RESET EVERYTHING HERE
        maxHP = (int)(baseHP * (1f + (0.25f * GameManager.instance.gameLevel)));
        Debug.Log(maxHP);
        atkValue = (int)(baseAtkValue * (1 + (0.25f * GameManager.instance.gameLevel)));
        currentHP = maxHP;
        currentExpValue = (int)(baseExpValue * (1 + (0.25f * GameManager.instance.gameLevel)));
    }

    public void TriggerAction(GameManager.ActionType type)
    {
        isBlocking = false;

        if (type == GameManager.ActionType.Attack)
        {
            previousPosition = transform.position;
            isAttacking = true;
        }
        else if (type == GameManager.ActionType.Block)
        {
            isBlocking = true;
        }
    }

    public void TakeDamage(int damageValue)
    {
        if (isBlocking)
        {
            return;
        }

        currentHP -= damageValue;
    }
}
