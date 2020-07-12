using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSlotController : MonoBehaviour
{
    //Instances
    [SerializeField]
    MonsterController monsterInSlot;
    [SerializeField]
    ActionController actionInSlot;
    [SerializeField]
    SpriteRenderer canInteractFeeback;
    [SerializeField]
    SpriteRenderer isAllowedFeedback;
    [SerializeField]
    SpriteRenderer interogationPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (GameManager.instance.currentState == GameManager.GameState.SelectMonsters && monsterInSlot == null && GameManager.instance.objectHolded != null)
            {
                DisplayCanInteract(false);

                monsterInSlot = GameManager.instance.objectHolded.GetComponent<MonsterController>();
                monsterInSlot.PlaceMonsterInSlot(this);

                GameManager.instance.DropObject();
            }
            else if (GameManager.instance.currentState == GameManager.GameState.Fight && actionInSlot == null && GameManager.instance.objectHolded != null)
            {
                DisplayCanInteract(false);

                actionInSlot = GameManager.instance.objectHolded.GetComponent<ActionController>();
                actionInSlot.PlaceActionInSlot(this);

                GameManager.instance.DropObject();
            }
        }
    }

    public void DisplayCanInteract(bool value)
    {
        if(GameManager.instance.currentState == GameManager.GameState.SelectMonsters)
        {
            if (monsterInSlot == null)
            {
                canInteractFeeback.enabled = value;
                interogationPoint.enabled = value;
                isAllowedFeedback.enabled = false;
            }
        }
        else if (GameManager.instance.currentState == GameManager.GameState.Fight)
        {
            if (actionInSlot == null)
            {
                canInteractFeeback.enabled = value;
                isAllowedFeedback.enabled = false;
            }
        }
    }

    public void DisplayCanPlace()
    {
        if (GameManager.instance.currentState == GameManager.GameState.SelectMonsters)
        {
            if (monsterInSlot == null)
            {
                canInteractFeeback.enabled = false;
                isAllowedFeedback.enabled = true;
            }
        }
        else if (GameManager.instance.currentState == GameManager.GameState.Fight)
        {
            if (actionInSlot == null)
            {
                canInteractFeeback.enabled = false;
                isAllowedFeedback.enabled = true;
            }
        }
    }

    public bool IsMonsterInSlot()
    {
        if (monsterInSlot != null)
            return true;

        return false;
    }

    public bool IsActionInSlot()
    {
        if (actionInSlot != null)
            return true;

        return false;
    }

    public bool TriggerAction()
    {
        if (monsterInSlot == null)
            return false;

        monsterInSlot.TriggerAction(actionInSlot.type);
        actionInSlot.gameObject.SetActive(false);

        return true;
    }
}
