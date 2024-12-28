using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform playerTransform; //GameManager.Instance.playerTransform;
    private PlayerController player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerTransform = GameObject.FindWithTag("Player").transform;    
    }

    public void DealDamageToPlayer(int value)
    {
        player.healthPoints -= value;
        Debug.Log(player.healthPoints);
    }

    void Update()
    {
       
    }
}
