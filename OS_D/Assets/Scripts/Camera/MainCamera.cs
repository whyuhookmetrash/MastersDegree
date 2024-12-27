using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform playerTransform;
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    
    void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 newPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }
}
