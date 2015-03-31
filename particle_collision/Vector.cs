using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace particle_collision
{
    /// <summary>
    /// simple 2d vector with operations
    /// </summary>
    class Vector
    {
        public double x { get; set; }
        public double y { get; set; }

        public Vector()
        {
            x = 0.0;
            y = 0.0;
        }

        // implicit
        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // copy constructor
        public Vector(Vector copy)
        {
            x = copy.x;
            y = copy.y;
        }

        // dot product
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double dot(Vector a, Vector b)
        {
            double res = 0;
            res += a.x * b.x;
            res += a.y * b.y;
            return res;
        }

        // vector addition a + b
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Vector add(Vector a, Vector b)
        {
            Vector res = new Vector();
            res.x = a.x + b.x;
            res.y = a.y + b.y;
            return res;
        }

        // vector subtraction a - b
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Vector sub(Vector a, Vector b)
        {
            Vector res = new Vector();
            res.x = a.x - b.x;
            res.y = a.y - b.y;
            return res;
        }

        // return vector component multiply a*b
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Vector mult(Vector a, Vector b)
        {
            Vector res = new Vector();
            res.x = a.x * b.x;
            res.y = a.y * b.y;
            return res;
        }

        // return new vector a * c
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Vector scalarMult(Vector a, double c)
        {
            Vector res = new Vector(a);
            res.scalarMult(c);
            return res;
        }

        // return normal vector
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Vector normal(Vector a)
        {
            Vector res = new Vector();
            res.x = -a.y;
            res.y = a.x;
            return res;
        }

        // return unit vector
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Vector unit(Vector a)
        {
            if (a == null)
                return null;
            Vector res = new Vector();
            double mag = a.magnitude();
            res.x = a.x / mag;
            res.y = a.y / mag;
            return res;
        }

        // return vector magnitude
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public double magnitude() {
            double res = x*x;
            res += y*y;
            return Math.Sqrt(res);
        }

        // scalar multiply
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void scalarMult(double c)
        {
            x *= c;
            y *= c;
        }
    }
}
