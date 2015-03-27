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
            rect = new Rectangle((int)position.x, (int)position.y, radius, radius);
        }

        // Paint ourselves with the specified Graphics object
        public void Draw(Graphics graphics)
        {
            using (Pen pen = new Pen(color, 1))
                graphics.DrawEllipse(pen, rect);
        }
    }
}
