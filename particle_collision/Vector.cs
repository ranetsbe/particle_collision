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
            x = 0;
            y = 0;
        }

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // dot product
        public static double dot(Vector a, Vector b)
        {
            double res = 0;
            res += a.x * b.x;
            res += a.y * b.y;
            return res;
        }

        // vector addition a + b
        public static Vector add(Vector a, Vector b)
        {
            Vector res = new Vector();
            res.x = a.x + b.x;
            res.y = a.y + b.y;
            return res;
        }

        // vector subtraction a - b
        public static Vector sub(Vector a, Vector b)
        {
            Vector res = new Vector();
            res.x = a.x - b.x;
            res.y = a.y - b.y;
            return res;
        }

        // return vector component multiply a*b
        public static Vector mult(Vector a, Vector b)
        {
            Vector res = new Vector();
            res.x = a.x * b.x;
            res.y = a.y * b.y;
            return res;
        }

        // return normal vector
        public static Vector norm(Vector a)
        {
            Vector res = new Vector();
            res.x = -a.y;
            res.y = a.x;
            return res;
        }

        // return unit vector
        public static Vector unit(Vector a)
        {
            Vector res = new Vector();
            double mag = a.magnitude();
            res.x = a.x / mag;
            res.y = a.y / mag;
            return res;
        }
        
        // return vector magnitude
        public double magnitude() {
            double res = x*x;
            res += y*y;
            return res;
        }

        // scalar multiply
        public void scalarMult(double c)
        {
            x *= c;
            y *= c;
        }
    }
}
