using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace particle_collision
{
    class Collidable : PriorityQueueNode
    {
        public Vector position { get; set; }    // x,y position
        public Vector velocity { get; set; }    // velocity vector
        public double radius { get; set; }      // radius
        public double mass { get; set; }        // mass is radius^2

        // init collidable with position, velocity and radius
        public Collidable(Vector p, Vector v, double r)
        {
            velocity = v;
            position = p;
            radius = r;
            mass = r * r;
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
        public virtual double computeCollisionTime(Collidable c2)
        {
            return double.MaxValue;
        }

        // compute the final velocities of a and b after colliding
        public static void doCollision(Collidable a, Collidable b)
        {
            Vector vaf = a.collisionResponse(b);
            Vector vbf = b.collisionResponse(a);
            a.velocity = vaf;
            b.velocity = vbf;
        }
        
    }
}
