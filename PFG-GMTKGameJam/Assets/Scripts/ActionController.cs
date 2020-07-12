using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    //Values
    [SerializeField]
    public GameManager.ActionType type;

    //Components
    Collider2D myCollider;

    //Variables
    bool isInHand = false;
    bool isHolded = false;
    MonsterSlotController currentSlot;
    float speed;
    Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState == GameManager.GameState.Fight)
        {
            if (isInHand)
            {
                // Move our position a step closer to the target.
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, destination, step);
            }
            else if (isHolded)
            {
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.instance.objectHolded == null)
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

    public void PlaceActionInSlot(MonsterSlotController newSlot)
    {
        isHolded = false;
        currentSlot = newSlot;
        transform.position = newSlot.transform.position + (Vector3.up * 0.96f);
        transform.parent = null;
        GameManager.instance.objectHolded = null;

        if (GameManager.instance.CheckIfAllActionsArePlaced())
            GameManager.instance.ChangeState(GameManager.GameState.TriggerAction);
    }

    public void DrawAction(Vector3 startPositoin, Vector3 handPosition, float moveSpeed, Transform parent)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = startPositoin;
        gameObject.transform.parent = parent;
        destination = handPosition;
        speed = moveSpeed;
        isInHand = true;
    }
}
