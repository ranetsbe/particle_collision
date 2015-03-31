using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace particle_collision
{
    class Collidable : PriorityQueueNode
    {
        public int id;
        public Vector position { get; set; }    // x,y position
        public Vector velocity { get; set; }    // velocity vector
        public double radius { get; set; }      // radius
        public double mass { get; set; }        // mass is radius^2
        public long steppingTime { get; set; }  // the most recent time that position changed

        // constructor
        public Collidable(Vector p, Vector v, double r, double m, int n)
        {
            velocity = v;
            position = p;
            radius = r;
            mass = m;
            id = n;
            steppingTime = 0;
        }

        // sets the position and stepping time
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void setPosition(Vector p, long t)
        {
            steppingTime = t;
            position = p;
        }

        // return the velocity vector of this instance after colliding with b
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public virtual Vector collisionResponse(Collidable b)
        {
            return new Vector();
        }

        // return the time t when this instance and b will collide
        // non-overriden virtual method returns double.MaxValue
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public virtual long computeCollisionTime(Collidable c2)
        {
            return long.MaxValue;
        }

        // return the position of this instance after t milliseconds
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public virtual Vector targetPosition(long t)
        {
            return position;
        }

        // compute the final velocities of a and b after colliding
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void doCollision(Collidable a, Collidable b)
        {
            Vector vaf = a.collisionResponse(b);
            Vector vbf = b.collisionResponse(a);
            a.velocity = vaf;
            b.velocity = vbf;
        }
        
    }
}
