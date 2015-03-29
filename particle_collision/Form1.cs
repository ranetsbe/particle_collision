using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace particle_collision
{
    public partial class Form1 : Form
    {
        private readonly int MAX_PARTICLES = 10000;
        private readonly int N_WALLS = 4;

        // thread for physics simulation
        private BackgroundWorker bw = new BackgroundWorker();

        // heap
        private HeapPriorityQueue<CollisionInfo> heap;
        private List<Particle> particles;

        // timer to refresh graphics
        private System.Windows.Forms.Timer RefreshTimer = new System.Windows.Forms.Timer();
        private readonly int REFRESH_TIMER_INTERVAL = 10; // 60hz

        // keeps track of the current simulation time
        private long globalTime = 0;
        Stopwatch stopwatch;


        public Form1()
        {
            InitializeComponent();

            // simple testing
            Particle a = new Particle(new Vector(0, 0), new Vector(0.32, 0.5), 10);
            Collidable b = new Particle(new Vector(20, 0), new Vector(-0.5, -0.554), 10);
            double t = a.computeCollisionTime(b);
            Collidable.doCollision(a, b);
            System.Console.WriteLine("t = " + t.ToString());
            System.Console.WriteLine("a: pos = (" + a.position.x.ToString() + ", " + a.position.y.ToString() + ") | vel = <" + a.velocity.x.ToString() + ", " + a.velocity.y.ToString() + ">");
            System.Console.WriteLine("b: pos = (" + b.position.x.ToString() + ", " + b.position.y.ToString() + ") | vel = <" + b.velocity.x.ToString() + ", " + b.velocity.y.ToString() + ">");
            System.Console.WriteLine("sw frequency: " + Stopwatch.Frequency.ToString());

            // setup background worker
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            // initialize particles
            particles = new List<Particle>();

            // initialize priority queue
            int heapsize = (MAX_PARTICLES + 4) * (MAX_PARTICLES + 4);
            heap = new HeapPriorityQueue<CollisionInfo>(heapsize);
        }

        private void RefreshTimerCallBack(object sender, EventArgs e)
        {
            collisionPanel.Invalidate();
        }

        // initialize particles with random properties
        private void initParticles()
        {
            Random rng = new Random();
            int n_particles = (int)numericUpDown1.Value;
            System.Console.WriteLine(string.Format("initParticles: n_particles = {0}", n_particles));
            for (int i = 0; i < n_particles; i++)
            {
                int radius = (int)numericUpDown2.Value;
                Size panelSize = collisionPanel.Size;
                Vector pos = new Vector(rng.Next(radius, panelSize.Width - radius), rng.Next(radius, panelSize.Height - radius));
                Vector vel = new Vector(rng.Next(0, 100) / 13.0, rng.Next(0, 100) / 13.0);
                particles.Add(new Particle(pos, vel, radius));
            }

            // hard coded walls
            Plane top = new Plane(new Vector(0, 0), new Vector(1, 0));
            Plane left = new Plane(new Vector(0, 0), new Vector(0, 1));
            Plane bottom = new Plane(new Vector(500, 500), new Vector(-1, 0));
            Plane right = new Plane(new Vector(500, 500), new Vector(0, -1));

            // create collisionInfo objects for each unique pair of Collidables (n choose 2)
            ulong n_ci = 0;
            for (int i = 0; i < n_particles; i++)
            {
                CollisionInfo info;
                // each particle can collide with each other particle
                Collidable c1 = particles[i];
                for (int j = i + 1; j < n_particles; j++)
                {
                    n_ci += 1;
                    Collidable c2 = particles[j];
                    info = new CollisionInfo(c1, c2);
                    heap.Enqueue(info, info.computeCollision());
                }
                // each particle can collide with a wall
                info = new CollisionInfo(c1, top);
                heap.Enqueue(info, info.computeCollision());
                info = new CollisionInfo(c1, left);
                heap.Enqueue(info, info.computeCollision());
                info = new CollisionInfo(c1, bottom);
                heap.Enqueue(info, info.computeCollision());
                info = new CollisionInfo(c1, right);
                heap.Enqueue(info, info.computeCollision());
                n_ci += 4;
            }
            System.Console.WriteLine("n choose 2: " + n_ci.ToString());

        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (true)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                System.Console.WriteLine("Worker stopped.");
            }

            else if (!(e.Error == null))
            {
                System.Console.WriteLine("Error: " + e.Error.Message);
            }

            else
            {
                System.Console.WriteLine("Worker finished.");
            }
            // remove particles
            particles.Clear();
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            System.Console.WriteLine(e.ProgressPercentage.ToString());
        }

        /**** GUI code ****/

        private void Form1_Load(object sender, EventArgs e)
        {
            // graphics
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty |
                           BindingFlags.Instance | BindingFlags.NonPublic,
                           null, collisionPanel, new object[] { true });

            // setup refresh timer
            RefreshTimer.Tick += new EventHandler(RefreshTimerCallBack);
            RefreshTimer.Interval = REFRESH_TIMER_INTERVAL;
            RefreshTimer.Start();
        }

        private void collisionPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (stopwatch != null)
            {
                long currentTime = stopwatch.ElapsedMilliseconds;
                foreach (Particle p in particles)
                {
                    long timeDiff = currentTime - p.steppingTime;
                    //System.Console.WriteLine("timediff = " + timeDiff.ToString() + " | " + (1000/timeDiff).ToString() + "Hz");
                    //System.Console.WriteLine("particle speed = " +  p.velocity.magnitude().ToString());
                    p.steppingTime = currentTime;
                    p.position = Vector.add(p.position, Vector.scalarMult(p.velocity, timeDiff / 1000.0));
                    p.Draw(e.Graphics);
                }
            }
        }

        // 15% 6RHRCEFLVP9ZCXFA8VV4
        private void numericUpDown1_ValueChanged(object sender, EventArgs e) { }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e) { }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (bw.IsBusy != true)
            {
                initParticles();
                globalTime = 0;
                stopwatch = Stopwatch.StartNew();
                bw.RunWorkerAsync();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (bw.WorkerSupportsCancellation == true)
            {
                bw.CancelAsync();
                stopwatch.Stop();
                globalTime = 0;
            }
        }
    }
}
