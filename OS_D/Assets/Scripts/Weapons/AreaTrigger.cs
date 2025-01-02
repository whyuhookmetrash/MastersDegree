using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    private List<GameObject> areaTriggers;

    private void Start()
    {
        areaTriggers = new List<GameObject>();
    }
    public List<GameObject> GetTriggers() 
    {
        return areaTriggers;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        areaTriggers.Add(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        areaTriggers.Remove(other.gameObject);
    }
}
