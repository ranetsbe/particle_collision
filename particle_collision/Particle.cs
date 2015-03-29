using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace particle_collision
{
    class Particle : Collidable
    {
        public Color color;
        public Rectangle rect;

        public Particle(Vector position, Vector velocity, int radius) 
            : base(position, velocity, radius)
        {
            color = Color.Black;
        }


        // return the velocity vector of this instance after colliding with b
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public override Vector collisionResponse(Collidable b)
        {
            double m1 = this.mass;
            double m2 = b.mass;
            Vector vai = new Vector(this.velocity);
            Vector vbi = new Vector(b.velocity);

            // vector along direction of impact from center of this particle to b 
            Vector n = Vector.unit(Vector.sub(this.position, b.position));
            Vector nrev = Vector.scalarMult(n, -1.0);

            double nv1 = Vector.dot(nrev, vai);
            Vector v1n = Vector.scalarMult(nrev, nv1);
            Vector v1t = Vector.sub(vai, v1n);

            double nv2 = Vector.dot(n, vbi);
            Vector v2n = Vector.scalarMult(n, nv2);
            Vector v2t = Vector.sub(v2n, v2n);

            Vector v1n_1 = Vector.scalarMult(v1n, (m1 - m2) / (m1 + m2));
            Vector v1n_2 = Vector.scalarMult(v2n, (2 * m2) / (m1 + m2));
            Vector v1f = Vector.add(v1n_1, v1n_2);

            return Vector.add(v1f, v1t);
        }

        // return the time t when this instance and b will collide
        // t will be max double if vectors are non-intersecting
        // known bug (should not be an issue normally)
        //   if two collidables are touching (ie c1.radius + c2.radius is exactly equal to dist(c1,c2)) 
        //   then t is equal to the distance between the two objects
        //   could check for this condition but it would be expensive
        public override double computeCollisionTime(Collidable b)
        {
            Vector r1 = this.position;
            Vector v1 = this.velocity;
            Vector r2 = b.position;
            Vector v2 = b.velocity;

            Vector dv = Vector.sub(v1, v2);
            Vector dr = Vector.sub(r1, r2);
            Vector drUnit = Vector.unit(dr);

            double a = dv.magnitude();
            a = a * a;
            double d = 2 * Vector.dot(dv, dr);
            double c = dr.magnitude();
            double r = this.radius + b.radius;

            if (c < r)
            {
                Console.WriteLine("c < r: c = " + c.ToString() + " r = " + r.ToString());
                return Double.MaxValue;
            }

            c = c * c - r * r;
            double discriminant = d * d - 4 * a * c;
            if (discriminant < 0)
            {
                Console.WriteLine("disc < 0: disc = " + discriminant.ToString());
                return Double.MaxValue;
            }
            double sqrt_disc = Math.Sqrt(discriminant);
            double s1 = (-d + sqrt_disc) / (2 * a);
            double s2 = (-d - sqrt_disc) / (2 * a);
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
            Console.Write("computeCollision: error: no result");
            return Double.MaxValue;
        }

        // Paint ourselves with the specified Graphics object
        public void Draw(Graphics graphics)
        {
            Rectangle rect = new Rectangle((int)position.x, (int)position.y, (int)radius, (int)radius);
            SolidBrush brush = new SolidBrush(Color.LightGray);
            graphics.FillEllipse(brush, rect);
            brush.Dispose();
        }
    }
}
