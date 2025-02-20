
using Zenject;

namespace Game.Enemy
{
    public sealed class AIService: IFixedTickable
    {
        private readonly EnemyBase enemy;
        private readonly Player player;
        public AIService(EnemyBase enemy, Player player)
        {
            this.enemy = enemy;
            this.player = player;
        }

        void IFixedTickable.FixedTick() 
        {
            this.enemy.velocity = (this.player.transform.position - this.enemy.transform.position).normalized * 2f;
        }

    }
}

