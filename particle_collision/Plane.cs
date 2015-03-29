using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace particle_collision
{
    class Plane : Collidable
    {
        public Plane(Vector position, Vector velocity) : base(position, velocity, 0.0) { }

        // planes are static, so return the original vector
#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public override Vector collisionResponse(Collidable b)
        {
            return this.velocity;
        }
    }
}
