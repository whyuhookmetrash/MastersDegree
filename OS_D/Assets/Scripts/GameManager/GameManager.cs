
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform playerTransform; //GameManager.Instance.playerTransform;
    private Player player;

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

        // test
        //IModifier mod = new Modifiers.MaxHP(5);
        //player.SetModifier(mod);
        //player.RemoveModifier(mod);

    }

    public void DealDamageToPlayer(int value) //test
    {
        DamageInfo damage = new DamageInfo();
        damage.damageValue = value;
        damage.damageType = DamageType.Physical;
        player.TakeDamage(damage);
    }

    void Update()
    {
       
    }
}
