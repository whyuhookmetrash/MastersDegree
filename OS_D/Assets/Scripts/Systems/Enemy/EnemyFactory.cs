
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class EnemyFactory: IEnemyFactory
    {
        private readonly DiContainer diContainer;
        private const string GUARDIAN = "Enemy/Enemy";
        public EnemyFactory(DiContainer diContainer)
        {
            this.diContainer = diContainer;
        }

        private Object guardianPrefab;


        void IEnemyFactory.Load() //TODO: сделать загрузку при инициализации ?
        {
            this.guardianPrefab = Resources.Load(GUARDIAN);
        }

        void IEnemyFactory.Create(Vector2 at)
        {
            this.diContainer.InstantiatePrefab(guardianPrefab, at, Quaternion.identity, null);
        }
    }
}