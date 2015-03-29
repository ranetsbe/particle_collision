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
using System.Timers;

namespace particle_collision
{
    public partial class Form1 : Form
    {
        private readonly int MAX_PARTICLES = 1000;
        private readonly int N_WALLS = 4;

        // thread for physics simulation
        private BackgroundWorker bw = new BackgroundWorker();

        // heap
        private HeapPriorityQueue<CollisionInfo> heap;
        private List<Particle> particles;

        // timer to refresh graphics
        private System.Windows.Forms.Timer RefreshTimer = new System.Windows.Forms.Timer();
        private readonly int REFRESH_TIMER_INTERVAL = 5; // 10 ms or 100hz

        // keeps track of the current simulation time in ms
        private System.Timers.Timer globalTimer;
        private readonly double GLOBAL_TIMER_INTERVAL = 1.0; // 1 ms
        private double globalTime = 0.0;

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

            // setup background worker
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            // setup global timer
            globalTimer = new System.Timers.Timer(GLOBAL_TIMER_INTERVAL);
            globalTimer.Elapsed += new System.Timers.ElapsedEventHandler(globalTimerCallBack);
            globalTimer.AutoReset = true;

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

        private void globalTimerCallBack(object sender, EventArgs e)
        {
            globalTime += GLOBAL_TIMER_INTERVAL;
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
            foreach (Particle p in particles)
            {
                p.position = Vector.add(p.position, Vector.scalarMult(p.velocity, 1 / GLOBAL_TIMER_INTERVAL));
                p.Draw(e.Graphics);
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
                globalTime = 0.0;
                globalTimer.Start();
                bw.RunWorkerAsync();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (bw.WorkerSupportsCancellation == true)
            {
                bw.CancelAsync();
                globalTimer.Stop();
                globalTime = 0.0;
            }
        }
    }
}
