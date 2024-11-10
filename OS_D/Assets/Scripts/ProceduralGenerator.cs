using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public int numRooms = 10;
    public int maxAttempts = 100;

    private List<Room> rooms = new List<Room>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        // Создание начальной комнаты
        CreateRoom(new Vector2(-10, -10));
        CreateRoom(new Vector2(10, -10));
        CreateRoom(new Vector2(-10, 10));
        CreateRoom(new Vector2(10, 10));

        // Повторение для создания остальных комнат
        for (int i = 0; i < numRooms - 4; i++) // Изменили условие цикла
        {
            // Поиск свободного места для новой комнаты
            Vector2 newRoomPos = FindFreeSpaceForRoom();

            // Создание новой комнаты, если есть место
            if (newRoomPos != Vector2.zero)
            {
                CreateRoom(newRoomPos);
            }
            else
            {
                Debug.LogWarning("Не удалось найти место для новой комнаты.");
            }
        }
    }

    Vector2 FindFreeSpaceForRoom()
    {
        // Реализация функции поиска свободного места для новой комнаты
        // (Проверка пересечения с существующими комнатами)
        // Возвращает Vector2 с позицией для новой комнаты, или Vector2.zero, если места нет
        if (rooms.Count > 1)
        {

            for (int i = 0; i < maxAttempts; i++)
            {
                // Случайные направления для новой комнаты
                int direction = Random.Range(0, 4);
                Vector2 offset = Vector2.zero;

                switch (direction)
                {
                    case 0: offset = Vector2.right; break; // Вправо
                    case 1: offset = Vector2.left; break; // Влево
                    case 2: offset = Vector2.up; break; // Вверх
                    case 3: offset = Vector2.down; break; // Вниз
                }

                // Проверка, свободно ли место для новой комнаты
                bool isFree = true;
                foreach (Room room in rooms)
                {
                    if (room.Bounds.Overlaps(new Rect(room.Bounds.center + offset, room.Bounds.size)))
                    {
                        isFree = false;
                        break;
                    }
                }

                if (isFree)
                {
                    return rooms[-1].Bounds.center + offset;
                }
            }
        }

        return Vector2.zero; // Не найдено свободного места
    }

    void CreateRoom(Vector2 position)
    {
        // Создание новой комнаты с помощью Instantiate
        GameObject newRoom = Instantiate(roomPrefab, position, Quaternion.identity);
        Room room = newRoom.GetComponent<Room>();

        // Обновление списка комнат
        rooms.Add(room);
        Debug.Log(rooms);

        if (rooms.Count > 2)
        {
            // Создание коридора между новой и старой комнатами
            CreateCorridor(rooms[rooms.Count], room);

            // Обновление связей между комнатами
            // ... (Реализуйте код для определения связей между комнатами)
        }
    }
    void CreateCorridor(Room room1, Room room2)
    {
        Vector2 room1Center = room1.Bounds.center;
        Vector2 room2Center = room2.Bounds.center;

        // Выбор случайного направления для коридора
        int direction = Random.Range(0, 2);
        if (direction == 0)
        {
            // Коридор по горизонтали
            GameObject corridor = new GameObject("Corridor");
            LineRenderer lineRenderer = corridor.AddComponent<LineRenderer>(); // Инициализация LineRenderer

            // Установка позиций для коридора
            lineRenderer.SetPosition(0, room1Center);
            lineRenderer.SetPosition(1, room2Center);
        }
        else
        {
            // Коридор по вертикали
            GameObject corridor = new GameObject("Corridor");
            LineRenderer lineRenderer = corridor.AddComponent<LineRenderer>(); // Инициализация LineRenderer

            // Установка позиций для коридора
            lineRenderer.SetPosition(0, room1Center);
            lineRenderer.SetPosition(1, room2Center);
        }
    }
}