namespace particle_collision
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.collisionPanel = new System.Windows.Forms.Panel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.collisionsLabel = new System.Windows.Forms.Label();
            this.delayLabel = new System.Windows.Forms.Label();
            this.computeLabel = new System.Windows.Forms.Label();
            this.collisionsValue = new System.Windows.Forms.Label();
            this.delayValue = new System.Windows.Forms.Label();
            this.computeValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // collisionPanel
            // 
            this.collisionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.collisionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.collisionPanel.ForeColor = System.Drawing.SystemColors.Control;
            this.collisionPanel.Location = new System.Drawing.Point(104, 12);
            this.collisionPanel.Name = "collisionPanel";
            this.collisionPanel.Size = new System.Drawing.Size(500, 500);
            this.collisionPanel.TabIndex = 0;
            this.collisionPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.collisionPanel_Paint);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.BackColor = System.Drawing.Color.Silver;
            this.numericUpDown1.Location = new System.Drawing.Point(12, 28);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(78, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Particles";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.BackColor = System.Drawing.Color.Silver;
            this.numericUpDown2.Location = new System.Drawing.Point(12, 67);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(78, 20);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Radius";
            // 
            // startButton
            // 
            this.startButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.startButton.Location = new System.Drawing.Point(12, 460);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(78, 23);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(12, 489);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(78, 23);
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // collisionsLabel
            // 
            this.collisionsLabel.AutoSize = true;
            this.collisionsLabel.Location = new System.Drawing.Point(9, 280);
            this.collisionsLabel.Name = "collisionsLabel";
            this.collisionsLabel.Size = new System.Drawing.Size(52, 13);
            this.collisionsLabel.TabIndex = 7;
            this.collisionsLabel.Text = "collisions:";
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(9, 319);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(59, 13);
            this.delayLabel.TabIndex = 8;
            this.delayLabel.Text = "delay avg: ";
            // 
            // computeLabel
            // 
            this.computeLabel.AutoSize = true;
            this.computeLabel.Location = new System.Drawing.Point(9, 358);
            this.computeLabel.Name = "computeLabel";
            this.computeLabel.Size = new System.Drawing.Size(65, 13);
            this.computeLabel.TabIndex = 9;
            this.computeLabel.Text = "ms/collision:";
            // 
            // collisionsValue
            // 
            this.collisionsValue.AutoSize = true;
            this.collisionsValue.Location = new System.Drawing.Point(9, 293);
            this.collisionsValue.Name = "collisionsValue";
            this.collisionsValue.Size = new System.Drawing.Size(13, 13);
            this.collisionsValue.TabIndex = 10;
            this.collisionsValue.Text = "0";
            // 
            // delayValue
            // 
            this.delayValue.AutoSize = true;
            this.delayValue.Location = new System.Drawing.Point(9, 332);
            this.delayValue.Name = "delayValue";
            this.delayValue.Size = new System.Drawing.Size(13, 13);
            this.delayValue.TabIndex = 11;
            this.delayValue.Text = "0";
            // 
            // computeValue
            // 
            this.computeValue.AutoSize = true;
            this.computeValue.Location = new System.Drawing.Point(9, 371);
            this.computeValue.Name = "computeValue";
            this.computeValue.Size = new System.Drawing.Size(13, 13);
            this.computeValue.TabIndex = 12;
            this.computeValue.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(616, 524);
            this.Controls.Add(this.computeValue);
            this.Controls.Add(this.delayValue);
            this.Controls.Add(this.collisionsValue);
            this.Controls.Add(this.computeLabel);
            this.Controls.Add(this.delayLabel);
            this.Controls.Add(this.collisionsLabel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.collisionPanel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Particle Collisions";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel collisionPanel;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label collisionsLabel;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.Label computeLabel;
        private System.Windows.Forms.Label collisionsValue;
        private System.Windows.Forms.Label delayValue;
        private System.Windows.Forms.Label computeValue;
    }
}

