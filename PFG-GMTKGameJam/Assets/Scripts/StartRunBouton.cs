using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRunBouton : MonoBehaviour
{
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
            GameManager.instance.StartRun();
            transform.parent.gameObject.SetActive(false);
        }
    }
}
