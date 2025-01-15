
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform playerTransform; //GameManager.Instance.playerTransform;
    public Player player;

    public GameObject weaponPrefab1; //test
    public GameObject weaponPrefab2; //test

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
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        //test
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.EquipWeapon(weaponPrefab1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.EquipWeapon(weaponPrefab2);
        }
    }
}
