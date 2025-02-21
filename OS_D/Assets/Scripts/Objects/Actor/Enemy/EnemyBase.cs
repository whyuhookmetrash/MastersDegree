
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyBase : Actor, IGameFixedTickable
    {
        private Rigidbody2D rb;

        [HideInInspector]
        public Vector2 velocity = Vector2.zero;

        private void Awake()
        {
            this.rb = GetComponent<Rigidbody2D>();
        }

        public virtual void FixedTick(float deltaTime)
        {
            this.rb.MovePosition(this.rb.position + this.velocity * deltaTime);
        }


    }
}
 
