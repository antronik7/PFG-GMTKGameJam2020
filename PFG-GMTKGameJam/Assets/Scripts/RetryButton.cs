using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshPro textScore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textScore.text = "Monsters Killed : " + GameManager.instance.nbrMonsterKill + "\n" + "Rooms Cleared : " + GameManager.instance.nbrRoomCleared;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
