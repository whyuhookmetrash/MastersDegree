using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Rect Bounds;
    public List<Vector2> Doors;
    public List<Room> Connections;

    void Start()
    {
        // Инициализация Rect Bounds
        Bounds = new Rect(transform.position, transform.localScale);
        // Инициализация Doors
        // ... (Добавьте код для задания позиций дверей)

        // Инициализация Connections
        // ... (Добавьте код для обновления списка связей)
    }

}
