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
        public Color color = Color.Black;


        public Particle(Vector position, Vector velocity, int radius, int n)
            : base(position, velocity, radius, radius*radius, n) { }


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

            if (m2 == 0.0)
            {
                Vector vaf = vai;
                if (vbi.x != 0)
                {
                    vaf.x *= -1;
                }
                else
                {
                    vaf.y *= -1;
                }
                return vaf;
            }

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
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public override long computeCollisionTime(Collidable b)
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
                return long.MaxValue;
            }

            c = c * c - r * r;
            double discriminant = d * d - 4 * a * c;
            if (discriminant < 0)
            {
                return long.MaxValue;
            }
            double sqrt_disc = Math.Sqrt(discriminant);
            long s1 = (long)((-d + sqrt_disc) / (2 * a) * 1000);
            long s2 = (long)((-d - sqrt_disc) / (2 * a) * 1000);
            if (s1 > 0 && s2 > 0)
            {
                if (s1 <= s2)
                {
                    return s1;
                }
                return s2; 
            }
            else if (s1 > 0)
            {
                return s1;
            }
            else if (s2 > 0)
            {
                return s2;
            }
            return long.MaxValue;
        }

        // return the position of this object after t milliseconds
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public override Vector targetPosition(long t)
        {
            double timeScalar = t / 1000.0;
            return Vector.add(position, Vector.scalarMult(velocity, timeScalar));
        }

        // Paint ourselves with the specified Graphics object
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Draw(Graphics graphics, long globalTime)
        {
            double timeScalar = (globalTime - steppingTime) / 1000.0;
            int x = (int)(position.x + velocity.x * timeScalar);
            int y = (int)(position.y + velocity.y * timeScalar);
            Rectangle rect = new Rectangle(x, y, (int)radius*2, (int)radius*2);
            SolidBrush brush = new SolidBrush(Color.Blue);
            graphics.FillEllipse(brush, rect);
            brush.Dispose();
        }
    }
}
