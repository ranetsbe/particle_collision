using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace particle_collision
{
    class CollisionInfo : PriorityQueueNode
    {
        public Collidable c1;               // collidable 1
        public Collidable c2;               // collidable 2
        public double collisionTime = 0.0;  // time when c1 and c2 will collide
        public Vector c1_target;            // position at which c1 will collide with c2
        public Vector c2_target;            // position at which c2 will collide with c1


        public CollisionInfo(Collidable c1, Collidable c2)
        {
            this.c1 = c1;
            this.c2 = c2;
        }

        public double computeCollision()
        {
            collisionTime = c1.computeCollisionTime(c2);
            if (collisionTime != double.MaxValue)
            {
                c1_target = Vector.add(c1.position, Vector.scalarMult(c1.velocity, collisionTime));
                c2_target = Vector.add(c2.position, Vector.scalarMult(c2.velocity, collisionTime));
            }
            return collisionTime;
        }
    }
}
