using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace particle_collision
{
    class Collidable : PriorityQueueNode
    {
        public Vector velocity { get; set; }    // vector
        public Vector position { get; set; }    // position
        public double radius { get; set; }      // radius

        // init collidable with position, velocity and radius
        public Collidable(Vector p, Vector v, double r)
        {
            velocity = v;
            position = p;
            radius = r;
        }

        // return the time t when c1 and c2 collide
        // t will be max double if c1 and c2 will not collide
        public static double computeCollision(Collidable c1, Collidable c2)
        {
            Vector r1 = c1.position;
            Vector v1 = c1.velocity;
            Vector r2 = c2.position;
            Vector v2 = c2.velocity;

            Vector dv = Vector.sub(v1, v2);
            Vector dr = Vector.sub(r1, r2);
            Vector drUnit = Vector.unit(dr);

            double a = dv.magnitude();
            a = a * a;
            double b = 2 * Vector.dot(dv, dr);
            double c = dr.magnitude();
            double r = c1.radius + c2.radius;

            if (c < r)
            {
                return Double.MaxValue;
            }

            c = c * c - r * r;
            double discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                return Double.MaxValue;
            }
            double s1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            double s2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            if (s1 > 0 && s2 > 0)
            {
                return (s1 <= s2) ? s1 : s2;
            }
            else if (s1 > 0)
            {
                return s1;
            }
            else if (s2 > 0)
            {
                return s2;
            }
            Console.Write("Hello from computeCollision");
            return Double.MaxValue;
        }
    }
}
