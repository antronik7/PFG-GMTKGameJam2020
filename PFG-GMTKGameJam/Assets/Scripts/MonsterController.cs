using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    //Values
    [SerializeField]
    float speedAttack;

    //Components
    Collider2D myCollider;

    //Variables
    bool isInHand = false;
    bool isHolded = false;
    MonsterSlotController currentSlot;
    float speed;
    Vector3 destination;
    Vector3 previousPosition;
    bool isAttacking = false;
    bool isBacking = false;

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
                transform.position = Vector3.MoveTowards(transform.position, destination, step);
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
                    Debug.Log("ici");
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
        transform.position = destination;
        myCollider.enabled = true;
    }

    public void PlaceMonsterInSlot(MonsterSlotController newSlot)
    {
        isHolded = false;
        currentSlot = newSlot;
        transform.position = newSlot.transform.position;
        transform.parent = null;
        GameManager.instance.objectHolded = null;

        if (GameManager.instance.CheckIfAllMonsterArePlaced())
            GameManager.instance.ChangeState(GameManager.GameState.Fight);
    }

    public void DrawMonster(Vector3 startPositoin, Vector3 handPosition, float moveSpeed, Transform parent)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = startPositoin;
        gameObject.transform.parent = parent;
        destination = handPosition;
        speed = moveSpeed;
        isInHand = true;
    }

    public void TriggerAction(GameManager.ActionType type)
    {
        if (type == GameManager.ActionType.Attack)
        {
            previousPosition = transform.position;
            isAttacking = true;
        }
    }
}
