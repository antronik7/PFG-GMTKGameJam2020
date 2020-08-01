using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Instances
    [SerializeField]
    Transform playerPosition;

    //Values
    [SerializeField]
    float offset;

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState == GameManager.GameState.TriggerAction || GameManager.instance.currentState == GameManager.GameState.Win)
            return;

        // Define a target position above and behind the target transform
        Vector3 targetPosition = playerPosition.position + (Vector3.right * offset);
        targetPosition = new Vector3(targetPosition.x, 0f, -10f);

        if (targetPosition.x < 0f)
            targetPosition = new Vector3(0f, 0f, -10f);

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
