using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace particle_collision
{
    class CollisionInfo : PriorityQueueNode
    {
        public Collidable c1;                   // collidable 1
        public Collidable c2;                   // collidable 2
        public long collisionTime;              // time when c1 and c2 will collide
        public long steppingTime;               // time when collisionTime was computed
        public Vector c1_target;                // position at which c1 will collide with c2
        public Vector c2_target;                // position at which c2 will collide with c1

        public CollisionInfo(Collidable c1, Collidable c2)
        {
            this.c1 = c1;
            this.c2 = c2;
            collisionTime = 0;
            steppingTime = 0;
        }

        // return the time that c1 and c2 will collide.
        // **note** that for a particle to collide with a plane, c1 must be the plane
        public double computeCollision(long globalTime)
        {
            steppingTime = 0;
            collisionTime = c1.computeCollisionTime(c2);
            c1_target = c1.targetPosition(collisionTime);
            c2_target = c2.targetPosition(collisionTime);
            if (collisionTime + collisionTime < 0)
            {
                return long.MaxValue;
            }
            return globalTime + collisionTime;
        }
    }
}
