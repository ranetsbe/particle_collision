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

namespace particle_collision
{
    public partial class Form1 : Form
    {
        public readonly int MAX_PARTICLES = 10000;
        // does physics simulation in background
        private BackgroundWorker bw = new BackgroundWorker();

        // heap
        private HeapPriorityQueue<CollisionInfo> heap;
        private List<Particle> particles;


        // timer to refresh graphics
        Timer timer = new Timer();

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

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            base.OnPaint(e);
        }

        private void TimerCallBack(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        // GUI code

        private void Form1_Load(object sender, EventArgs e) 
        {
            // graphics
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty |
                           BindingFlags.Instance | BindingFlags.NonPublic,
                           null, panel1, new object[] { true });
            timer.Enabled = true;
            timer.Interval = 100; // 100ms
            timer.Tick += new EventHandler(TimerCallBack);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Particle p in particles)
            {
                p.Draw(e.Graphics);
            }
            //panel1.Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {}
        private void numericUpDown2_ValueChanged(object sender, EventArgs e) {}

        private void startButton_Click(object sender, EventArgs e)
        {
            if (bw.IsBusy != true)
            {
                initParticles();
                bw.RunWorkerAsync();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (bw.WorkerSupportsCancellation == true)
            {
                bw.CancelAsync();
            }
        }

        // initialize particles with random positions and velocities
        private void initParticles()
        {
            Random rng = new Random();
            int n_particles = (int)numericUpDown1.Value;
            System.Console.WriteLine(string.Format("initParticles: n_particles = {0}", n_particles));
            for (int i = 0; i < n_particles; i++)
            {
                int radius = (int)numericUpDown2.Value;
                Size panelSize = panel1.Size;
                Vector pos = new Vector(rng.Next(radius, panelSize.Width - radius), rng.Next(radius, panelSize.Height - radius));
                Vector vel = new Vector(rng.Next(0, 100)/13.0, rng.Next(0, 100)/13.0);
                particles.Add(new Particle(pos, vel, radius));
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Particle p = this.particles[0];
            while(true)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    System.Threading.Thread.Sleep(500);
                    //worker.ReportProgress((i * 10));
                }
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                this.tbProgress.Text = "Canceled!";
            }

            else if (!(e.Error == null))
            {
                this.tbProgress.Text = ("Error: " + e.Error.Message);
            }

            else
            {
                this.tbProgress.Text = "Done!";
            }
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.tbProgress.Text = (e.ProgressPercentage.ToString() + "%");
        }
    }
}
