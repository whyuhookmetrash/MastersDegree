
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyBase : Actor
    {
        private Rigidbody2D rb;

        [HideInInspector]
        public Vector2 velocity = Vector2.zero;

        private void Awake()
        {
            this.rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            this.rb.MovePosition(this.rb.position + this.velocity * Time.fixedDeltaTime);
        }


    }
}
 
