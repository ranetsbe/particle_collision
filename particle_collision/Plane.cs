using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace particle_collision
{
    // a plane is defined by a point of origin and a normal vector
    class Plane : Collidable
    {
        public Plane(Vector position, Vector normal, int n) : base(position, normal, 1.0, 0, n) { }

        // planes are static, so return the original vector
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public override Vector collisionResponse(Collidable b)
        {
            return new Vector(velocity);
        }

        // return the time when b will collide with this plane
        // return long.MaxValue if b will never collide
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public override long computeCollisionTime(Collidable b)
        {
            Vector n = this.velocity;   // plane normal
            Vector p1 = this.position;  // plane position
            Vector v = b.velocity;      // object velocity
            Vector p2 = b.position;     // object position
            double r = b.radius;        // object radius

            double dist = Vector.dot(Vector.sub(p2, p1), n);
            if (dist < r)
            {
                double adjDist = Math.Abs(r - dist);
                Vector diff = Vector.scalarMult(n, adjDist + 1);
                p2.x = p2.x + diff.x;
                p2.y = p2.y + diff.y;
            }

            // if the dot product of b's velocity and the plane's normal is zero
            // then the vectors are parallel
            double vdotn = Vector.dot(v, n);
            if (Vector.dot(v, n) == 0.0)
            {
                return long.MaxValue;
            }

            Vector nMult = Vector.scalarMult(n, r);
            Vector num2 = Vector.add(p1, nMult);
            Vector num = Vector.sub(num2, p2);
            long t = (long)(Vector.dot(num, n) / Vector.dot(v, n) * 1000.0);
            if (t <= 0)
            {
                return long.MaxValue;
            }
            return t;
        }
    }
}
