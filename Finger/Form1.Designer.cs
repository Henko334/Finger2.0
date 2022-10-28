
namespace Finger
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picFPImg = new System.Windows.Forms.PictureBox();
            this.employeesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enrollEmployeeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFPImg)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(139)))), ((int)(((byte)(185)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(557, 36);
            this.label1.TabIndex = 781;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(139)))), ((int)(((byte)(185)))));
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.employeesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(775, 29);
            this.menuStrip1.TabIndex = 783;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fileToolStripMenuItem.Image = global::Finger.Properties.Resources.streamlinehq_interface_file_text_interface_essential_20;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(62, 25);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(139)))), ((int)(((byte)(185)))));
            this.closeToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 26);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(139)))), ((int)(((byte)(185)))));
            this.exitToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(218, 473);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(557, 36);
            this.panel2.TabIndex = 786;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(139)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.picFPImg);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(218, 480);
            this.panel1.TabIndex = 788;
            // 
            // picFPImg
            // 
            this.picFPImg.BackColor = System.Drawing.SystemColors.Control;
            this.picFPImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picFPImg.Location = new System.Drawing.Point(21, 61);
            this.picFPImg.Name = "picFPImg";
            this.picFPImg.Size = new System.Drawing.Size(125, 125);
            this.picFPImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFPImg.TabIndex = 780;
            this.picFPImg.TabStop = false;
            // 
            // employeesToolStripMenuItem
            // 
            this.employeesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enrollEmployeeToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.employeesToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.employeesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.employeesToolStripMenuItem.Image = global::Finger.Properties.Resources.streamlinehq_interface_user_circle_single_interface_essential_20;
            this.employeesToolStripMenuItem.Name = "employeesToolStripMenuItem";
            this.employeesToolStripMenuItem.Size = new System.Drawing.Size(113, 25);
            this.employeesToolStripMenuItem.Text = "Employees";
            this.employeesToolStripMenuItem.Click += new System.EventHandler(this.employeesToolStripMenuItem_Click);
            this.employeesToolStripMenuItem.MouseLeave += new System.EventHandler(this.employeesToolStripMenuItem_MouseLeave);
            // 
            // enrollEmployeeToolStripMenuItem
            // 
            this.enrollEmployeeToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(139)))), ((int)(((byte)(185)))));
            this.enrollEmployeeToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.enrollEmployeeToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.enrollEmployeeToolStripMenuItem.Image = global::Finger.Properties.Resources.streamlinehq_interface_user_add_interface_essential_20;
            this.enrollEmployeeToolStripMenuItem.Name = "enrollEmployeeToolStripMenuItem";
            this.enrollEmployeeToolStripMenuItem.Size = new System.Drawing.Size(180, 26);
            this.enrollEmployeeToolStripMenuItem.Text = "Enroll";
            this.enrollEmployeeToolStripMenuItem.Click += new System.EventHandler(this.enrollEmployeeToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(139)))), ((int)(((byte)(185)))));
            this.removeToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.removeToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.removeToolStripMenuItem.Image = global::Finger.Properties.Resources.streamlinehq_interface_user_delete_interface_essential_20;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(180, 26);
            this.removeToolStripMenuItem.Text = "Remove";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 509);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Home";
            this.Load += new System.EventHandler(this.FingerPrintControl_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFPImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox picFPImg;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem employeesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enrollEmployeeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
    }
}

