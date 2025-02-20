
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class EnemySpawner: MonoBehaviour
    {
        
        private IEnemyFactory enemyFactory;

        [Inject]
        public void Constract(IEnemyFactory enemyFactory)
        {
            this.enemyFactory = enemyFactory;
        }

        private void Start()
        {
            enemyFactory.Load();
            enemyFactory.Create(transform.position);
            enemyFactory.Create(transform.position);
        }

    }
}