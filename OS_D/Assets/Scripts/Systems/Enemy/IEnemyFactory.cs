
using UnityEngine;

namespace Game
{
    public interface IEnemyFactory
    {
        void Load();
        void Create(Vector2 at);
    }
}