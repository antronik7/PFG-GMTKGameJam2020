using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //Values
    [SerializeField]
    float speed;

    //Components
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState == GameManager.GameState.Walking)
            Walk();
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
}
