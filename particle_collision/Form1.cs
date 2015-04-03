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

        // thread for physics simulation
        private BackgroundWorker bw = new BackgroundWorker();

        // Collidables
        private HeapPriorityQueue<CollisionInfo> heap;
        private CollisionInfo[,] collisionData;
        private List<Particle> particles;
        private int n_particles = 1;

        // timer to refresh graphics
        private System.Windows.Forms.Timer RefreshTimer = new System.Windows.Forms.Timer();
        private readonly int REFRESH_TIMER_INTERVAL = 40; // 30hz

        // keeps track of the current simulation time
        Stopwatch globalTime;

        // statistics
        Stopwatch statsTime;
        private long n_collisions = 0;
        private long delayTime = 0;
        private long calcTime = 0;


        public Form1()
        {
            InitializeComponent();

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
            if (n_collisions > 0)
            {
                delayValue.Text = (delayTime / n_collisions).ToString() + " ms";
                computeValue.Text = (calcTime / n_collisions).ToString() + " ms";
                collisionsValue.Text = n_collisions.ToString();
            }
        }

        // initialize particles with random properties
        private void initParticles()
        {
            Random rng = new Random();
            System.Console.WriteLine(string.Format("initParticles: n_particles = {0}", n_particles));
            for (int i = 0; i < n_particles; i++)
            {
                int radius = rng.Next(1, 10);
                //int radius = (int)numericUpDown2.Value;
                Size panelSize = collisionPanel.Size;
                Vector pos = new Vector(rng.Next(radius, panelSize.Width - radius), rng.Next(radius, panelSize.Height - radius));
                Vector vel = new Vector(rng.Next(0, 100), rng.Next(0, 100));
                Particle p = new Particle(pos, vel, radius, i);
                switch(rng.Next(0, 10)){
                    case 0: p.color = Color.Lime; break;
                    case 1: p.color = Color.Blue; break;
                    case 2: p.color = Color.Red; break;
                    case 3: p.color = Color.Orange; break;
                    case 4: p.color = Color.Green; break;
                    case 5: p.color = Color.Yellow; break;
                    case 6: p.color = Color.Violet; break;
                    case 7: p.color = Color.Teal; break;
                    case 8: p.color = Color.DarkOrange; break;
                    case 9: p.color = Color.DimGray; break;
                    case 10: p.color = Color.Coral; break;
                }
                particles.Add(p);
            }

            // hard coded walls
            Plane top = new Plane(new Vector(1, 1), new Vector(0, 1), n_particles);
            Plane left = new Plane(new Vector(499, 1), new Vector(-1, 0), n_particles + 1);
            Plane bottom = new Plane(new Vector(1, 499), new Vector(0, -1), n_particles + 2);
            Plane right = new Plane(new Vector(1, 1), new Vector(1, 0), n_particles + 3);

            // create collisionInfo objects for each unique pair of Collidables (n choose 2 total)
            // keep refrences to the CollisionInfos, only half the array is used
            collisionData = new CollisionInfo[n_particles + 4, n_particles + 4];
            for (int i = 0; i < n_particles; i++)
            {
                CollisionInfo info;
                // each particle can collide with each other particle
                Collidable c1 = particles[i];
                for (int j = i + 1; j < n_particles; j++)
                {
                    Collidable c2 = particles[j];
                    info = new CollisionInfo(c1, c2);
                    collisionData[i, j] = info;
                    heap.Enqueue(info, info.computeCollision(0));
                }
                // each particle can collide with a wall
                info = new CollisionInfo(top, c1);
                collisionData[i, n_particles] = info;
                heap.Enqueue(info, info.computeCollision(0));
                info = new CollisionInfo(left, c1);
                collisionData[i, n_particles + 1] = info;
                heap.Enqueue(info, info.computeCollision(0));
                info = new CollisionInfo(bottom, c1);
                collisionData[i, n_particles + 2] = info;
                heap.Enqueue(info, info.computeCollision(0));
                info = new CollisionInfo(right, c1);
                collisionData[i, n_particles + 3] = info;
                heap.Enqueue(info, info.computeCollision(0));
            }
        }

        // update the steppingTime and position of all particles
        private void stepParticles(long t)
        {
            foreach (Particle p in particles)
            {
                p.position = p.targetPosition(t - p.steppingTime);
                p.steppingTime = t;
            }
        }

        // update the collisionInfos for collidable with id1 O(nlg(n))
        private void updateCollisionInfos(int cid, long currentTime)
        {
            if (cid < n_particles) {
                Collidable c = particles[cid];
                // update all CollisionInfos prior to cid
                for (int i = 0; i < cid; i++)
                {
                    CollisionInfo ci = collisionData[i, cid];
                    heap.UpdatePriority(ci, ci.computeCollision(currentTime));
                }
                for (int i = cid + 1; i < n_particles + 4; i++)
                {
                    CollisionInfo ci = collisionData[cid, i];
                    heap.UpdatePriority(ci, ci.computeCollision(currentTime));
                }
            }
        }

        // update all collision infos O(n^2lg(n))
        private void updateCollisionInfos(long currentTime)
        {
            for (int i = 0; i < n_particles; i++)
            {
                for (int j = i + 1; j < n_particles + 4; j++)
                {
                    CollisionInfo c = collisionData[i, j];
                    heap.UpdatePriority(c, c.computeCollision(currentTime));
                }
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
            while (true)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    CollisionInfo c = heap.First;
                    Collidable c1 = c.c1;
                    Collidable c2 = c.c2;
                    long collisionTime = c.collisionTime;
                    long originalTime = c.steppingTime;
                    long currentTime = globalTime.ElapsedMilliseconds;
                    int delay = (int)(collisionTime + originalTime - currentTime);
                    //if (delay > 0)
                    //{
                    //    System.Threading.Thread.Sleep(delay);
                    //    delayTime += delay;
                    //}
                    while (true)
                    {
                        if (collisionTime + originalTime <= globalTime.ElapsedMilliseconds)
                            break;
                    }

                    n_collisions += 1;
                    globalTime.Stop();
                    long statsTimeStart = statsTime.ElapsedMilliseconds;
                    // update collidable positions
                    currentTime = globalTime.ElapsedMilliseconds;
                    stepParticles(currentTime);
                    c1.setPosition(c.c1_target, currentTime);
                    c2.setPosition(c.c2_target, currentTime);
                    Collidable.doCollision(c.c1, c.c2);
                    updateCollisionInfos(c1.id, currentTime);
                    updateCollisionInfos(c2.id, currentTime);
                    calcTime += statsTime.ElapsedMilliseconds - statsTimeStart;
                    globalTime.Start();
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
            heap.Clear();
            delayTime = 0;
            calcTime = 0;
            n_collisions = 0;
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

            // setup graphics refresh rate
            RefreshTimer.Tick += new EventHandler(RefreshTimerCallBack);
            RefreshTimer.Interval = REFRESH_TIMER_INTERVAL;
            RefreshTimer.Start();
        }

        private void collisionPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (globalTime != null)
            {
                long currentTime = globalTime.ElapsedMilliseconds;
                foreach (Particle p in particles)
                {
                    p.Draw(e.Graphics, currentTime);
                }
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (bw.IsBusy != true)
            {
                numericUpDown1.Enabled = false;
                initParticles();
                globalTime = Stopwatch.StartNew();
                statsTime = Stopwatch.StartNew();
                bw.RunWorkerAsync();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (bw.WorkerSupportsCancellation == true)
            {
                bw.CancelAsync();
                globalTime.Reset();
                statsTime.Reset();
                numericUpDown1.Enabled = true;
            }
        }

        // 15% 6RHRCEFLVP9ZCXFA8VV4
        private void numericUpDown1_ValueChanged(object sender, EventArgs e) 
        {
            n_particles = (int)numericUpDown1.Value;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e) { }
    }
}
