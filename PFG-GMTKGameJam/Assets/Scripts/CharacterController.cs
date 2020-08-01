using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //Values
    [SerializeField]
    int maxHP;
    [SerializeField]
    int maxEXP;
    [SerializeField]
    int level;
    [SerializeField]
    int atkValue;
    [SerializeField]
    float speed;
    [SerializeField]
    float speedAttack;
    [SerializeField]
    ActionController[] actions;
    [SerializeField]
    TMPro.TextMeshPro textHP;
    [SerializeField]
    TMPro.TextMeshPro textEXP;
    [SerializeField]
    GameObject hearth;

    //Components
    Animator myAnimator;

    //Variables
    int currentActionIndex;
    bool isAttacking = false;
    bool isBacking = false;
    MonsterSlotController currentTarget;
    int currentHP;
    int currentEXP = 0;
    bool isBlocking = true;


    //Awake is always called before any Start functions
    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        currentHP = maxHP;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState == GameManager.GameState.Walking)
        {
            Walk();
        }
        else if (GameManager.instance.currentState == GameManager.GameState.TriggerAction || GameManager.instance.currentState == GameManager.GameState.Win)
        {
            if (isAttacking)
            {
                Debug.Log("asdfasdf");
                // Move our position a step closer to the target.
                float step = speedAttack * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, step);

                if (Vector3.Distance(transform.position, currentTarget.transform.position) < 0.001f)
                {
                    currentTarget.monsterInSlot.TakeDamage(atkValue);

                    if (currentTarget.monsterInSlot.currentHP <= 0)
                    {
                        GetExp(currentTarget.monsterInSlot.currentExpValue);
                        currentTarget.killMonster();

                        if (GameManager.instance.currentRoom.CheckIfAllMonsterAreDead())
                            GameManager.instance.ChangeState(GameManager.GameState.Win);
                    }

                    isAttacking = false;
                    isBacking = true;
                }
            }
            else if (isBacking)
            {
                // Move our position a step closer to the target.
                float step = speedAttack * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.playerDestination, step);

                if (Vector3.Distance(transform.position, GameManager.instance.playerDestination) < 0.001f)
                {
                    isBacking = false;
                }
            }
        }

        if(GameManager.instance.currentState == GameManager.GameState.Walking)
        {
            textHP.gameObject.SetActive(false);
            textEXP.gameObject.SetActive(false);
            hearth.gameObject.SetActive(false);
        }
        else
        {
            textHP.gameObject.SetActive(true);
            textEXP.gameObject.SetActive(true);
            hearth.gameObject.SetActive(true);

            textHP.text = currentHP + "/" + maxHP;
            textEXP.text = "LV " + level + " " + currentEXP + "/" + maxEXP; 
        }
    }

    void Walk()
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.playerDestination, step);

        if (Vector3.Distance(transform.position, GameManager.instance.playerDestination) < 0.001f)
            GameManager.instance.ChangeState(GameManager.GameState.SelectMonsters);
    }

    public void StartAnimation(string triggerName)
    {
        myAnimator.SetTrigger(triggerName);
    }

    public void GetAction()
    {
        isBlocking = false;
        int randomIndex = Random.Range(0, actions.Length);

        currentActionIndex = randomIndex;
        actions[currentActionIndex].gameObject.SetActive(true);

        if (actions[currentActionIndex].type == GameManager.ActionType.Block)
            isBlocking = true;
    }

    public void TriggerAction()
    {
        if (actions[currentActionIndex].type == GameManager.ActionType.Attack)
        {
            isAttacking = true;
            currentTarget = GameManager.instance.currentRoom.GetFirstValidMonsterSlot();
        }

        actions[currentActionIndex].gameObject.SetActive(false);
    }

    public void TakeDamage(int damageValue)
    {
        if(isBlocking)
        {
            return;
        }

        currentHP -= damageValue;
        StartAnimation("Hit");

        if (currentHP <= 0)
            GameManager.instance.ChangeState(GameManager.GameState.Lose);
    }

    public void GetExp(int expValue)
    {
        currentEXP += expValue;

        if (currentEXP >= maxEXP)
            levelUp();
    }

    public void levelUp()
    {
        int remainingExp = currentEXP - maxEXP;

        currentEXP = 0;
        maxEXP = (int)(maxEXP * 1.5f);
        ++level;
        maxHP += Random.Range(10, 26);
        currentHP = maxHP;
        atkValue += Random.Range(1, 6);
    }
}
