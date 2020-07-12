using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    //Instances
    [SerializeField]
    GameObject chest;
    [SerializeField]
    TMPro.TextMeshPro textChest;
    [SerializeField]
    GameObject[] handSlots = new GameObject[3];
    [SerializeField]
    GameObject[] handSlotsPosition = new GameObject[3];

    //Values
    [SerializeField]
    bool isActionDeck = false;
    [SerializeField]
    float timeBeforeDrawing;
    [SerializeField]
    float timeBetweenDraw;
    [SerializeField]
    float drawSpeed;

    //Components
    Animator myAnimator;

    //Variables
    public List<GameObject> deck = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActionDeck)
            textChest.text = deck.Count + "/" + GameManager.instance.GetNumberActionInPool();
        else
            textChest.text = deck.Count + "/" + GameManager.instance.GetNumberMonsterInPool();
    }

    public void Show()
    {
        myAnimator.SetTrigger("Up");
    }

    public void Hide()
    {
        myAnimator.SetTrigger("Down");
    }

    public void DrawFromDeck()
    {
        StartCoroutine(Drawing());
    }

    IEnumerator Drawing()
    {
        yield return new WaitForSeconds(timeBeforeDrawing);

        for (int i = 0; i < handSlots.Length; i++)
        {
            if (handSlots[i] == null)
            {
                handSlots[i] = deck[0];

                if(GameManager.instance.currentState == GameManager.GameState.SelectMonsters)
                {
                    deck[i].GetComponent<MonsterController>().DrawMonster(chest.transform.position, handSlotsPosition[i].transform.position, drawSpeed, transform);
                }
                else
                {
                    deck[i].GetComponent<ActionController>().DrawAction(chest.transform.position, handSlotsPosition[i].transform.position, drawSpeed, transform);
                }

                deck.RemoveAt(0);

                //Check if deck is not empty...

                if (i < handSlots.Length - 1)
                    yield return new WaitForSeconds(timeBetweenDraw);
            }
        }
    }

    public void ShuffleDeck()
    {
        if(isActionDeck == false)
        {
            deck.Clear();

            for (int i = 0; i < GameManager.instance.GetNumberMonsterInPool(); ++i)
            {
                deck.Add(GameManager.instance.GetMonsterFromPool(i));
            }
        }

        for (int i = 0; i < handSlots.Length; ++i)
        {
            if (handSlots[i] != null)
                deck.Remove(handSlots[i]);
        }

        //Shuffle Deck
        for (int i = 0; i < deck.Count; i++)
        {
            GameObject temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
}
