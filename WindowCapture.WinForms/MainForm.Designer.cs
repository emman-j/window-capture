namespace WindowCapture.WinForms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            WindowViewPanel = new Panel();
            panel1 = new Panel();
            SidePanel = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label2 = new Label();
            tabPage2 = new TabPage();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            WindowsModeToolStripMenuItem = new ToolStripMenuItem();
            TabsModeToolStripMenuItem = new ToolStripMenuItem();
            dockmodeToolStripMenuItem1 = new ToolStripMenuItem();
            chToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            undockAllAppToolStripMenuItem = new ToolStripMenuItem();
            undockAppToolStripMenuItem1 = new ToolStripMenuItem();
            dockOpenAppToolStripMenuItem = new ToolStripMenuItem();
            openAppToolStripMenuItem = new ToolStripMenuItem();
            dockToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            menuToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            menuStrip1 = new MenuStrip();
            panel2 = new Panel();
            panel3 = new Panel();
            WindowViewPanel.SuspendLayout();
            SidePanel.SuspendLayout();
            tabControl1.SuspendLayout();
            menuStrip1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // WindowViewPanel
            // 
            WindowViewPanel.Controls.Add(panel1);
            WindowViewPanel.Controls.Add(SidePanel);
            WindowViewPanel.Location = new Point(223, 163);
            WindowViewPanel.Name = "WindowViewPanel";
            WindowViewPanel.Size = new Size(415, 317);
            WindowViewPanel.TabIndex = 11;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(56, 57, 60);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(134, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(281, 317);
            panel1.TabIndex = 6;
            // 
            // SidePanel
            // 
            SidePanel.Controls.Add(flowLayoutPanel1);
            SidePanel.Controls.Add(label2);
            SidePanel.Dock = DockStyle.Left;
            SidePanel.Location = new Point(0, 0);
            SidePanel.Name = "SidePanel";
            SidePanel.Size = new Size(134, 317);
            SidePanel.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 15);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(134, 302);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(56, 15);
            label2.TabIndex = 0;
            label2.Text = "Windows";
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.FromArgb(56, 57, 60);
            tabPage2.BorderStyle = BorderStyle.Fixed3D;
            tabPage2.Location = new Point(4, 27);
            tabPage2.Margin = new Padding(4, 3, 4, 3);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4, 3, 4, 3);
            tabPage2.Size = new Size(324, 185);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "+";
            // 
            // tabControl1
            // 
            tabControl1.Appearance = TabAppearance.Buttons;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(437, 47);
            tabControl1.Margin = new Padding(4, 3, 4, 3);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(332, 216);
            tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.FromArgb(56, 57, 60);
            tabPage1.Location = new Point(4, 27);
            tabPage1.Margin = new Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4, 3, 4, 3);
            tabPage1.Size = new Size(324, 185);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "New tab";
            // 
            // WindowsModeToolStripMenuItem
            // 
            WindowsModeToolStripMenuItem.BackColor = Color.FromArgb(43, 43, 43);
            WindowsModeToolStripMenuItem.CheckOnClick = true;
            WindowsModeToolStripMenuItem.ForeColor = Color.FromArgb(209, 209, 209);
            WindowsModeToolStripMenuItem.Name = "WindowsModeToolStripMenuItem";
            WindowsModeToolStripMenuItem.Size = new Size(157, 22);
            WindowsModeToolStripMenuItem.Text = "&Windows Mode";
            // 
            // TabsModeToolStripMenuItem
            // 
            TabsModeToolStripMenuItem.BackColor = Color.FromArgb(43, 43, 43);
            TabsModeToolStripMenuItem.Checked = true;
            TabsModeToolStripMenuItem.CheckOnClick = true;
            TabsModeToolStripMenuItem.CheckState = CheckState.Checked;
            TabsModeToolStripMenuItem.ForeColor = Color.FromArgb(209, 209, 209);
            TabsModeToolStripMenuItem.Name = "TabsModeToolStripMenuItem";
            TabsModeToolStripMenuItem.Size = new Size(157, 22);
            TabsModeToolStripMenuItem.Text = "&Tabs Mode";
            // 
            // dockmodeToolStripMenuItem1
            // 
            dockmodeToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { TabsModeToolStripMenuItem, WindowsModeToolStripMenuItem });
            dockmodeToolStripMenuItem1.ForeColor = Color.FromArgb(209, 209, 209);
            dockmodeToolStripMenuItem1.Name = "dockmodeToolStripMenuItem1";
            dockmodeToolStripMenuItem1.Size = new Size(97, 26);
            dockmodeToolStripMenuItem1.Text = "Docking &Mode";
            // 
            // chToolStripMenuItem
            // 
            chToolStripMenuItem.BackColor = Color.FromArgb(43, 43, 43);
            chToolStripMenuItem.ForeColor = Color.FromArgb(209, 209, 209);
            chToolStripMenuItem.Name = "chToolStripMenuItem";
            chToolStripMenuItem.Size = new Size(171, 22);
            chToolStripMenuItem.Text = "&Change Tab Name";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { chToolStripMenuItem });
            editToolStripMenuItem.ForeColor = Color.FromArgb(209, 209, 209);
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 26);
            editToolStripMenuItem.Text = "&Edit";
            // 
            // undockAllAppToolStripMenuItem
            // 
            undockAllAppToolStripMenuItem.BackColor = Color.FromArgb(43, 43, 43);
            undockAllAppToolStripMenuItem.ForeColor = Color.FromArgb(209, 209, 209);
            undockAllAppToolStripMenuItem.Name = "undockAllAppToolStripMenuItem";
            undockAllAppToolStripMenuItem.Size = new Size(158, 22);
            undockAllAppToolStripMenuItem.Text = "U&ndock All App";
            // 
            // undockAppToolStripMenuItem1
            // 
            undockAppToolStripMenuItem1.BackColor = Color.FromArgb(43, 43, 43);
            undockAppToolStripMenuItem1.ForeColor = Color.FromArgb(209, 209, 209);
            undockAppToolStripMenuItem1.Name = "undockAppToolStripMenuItem1";
            undockAppToolStripMenuItem1.Size = new Size(158, 22);
            undockAppToolStripMenuItem1.Text = "&Undock App";
            // 
            // dockOpenAppToolStripMenuItem
            // 
            dockOpenAppToolStripMenuItem.BackColor = Color.FromArgb(43, 43, 43);
            dockOpenAppToolStripMenuItem.ForeColor = Color.FromArgb(209, 209, 209);
            dockOpenAppToolStripMenuItem.Name = "dockOpenAppToolStripMenuItem";
            dockOpenAppToolStripMenuItem.Size = new Size(158, 22);
            dockOpenAppToolStripMenuItem.Text = "&Dock Open App";
            // 
            // openAppToolStripMenuItem
            // 
            openAppToolStripMenuItem.BackColor = Color.FromArgb(43, 43, 43);
            openAppToolStripMenuItem.ForeColor = Color.FromArgb(209, 209, 209);
            openAppToolStripMenuItem.Name = "openAppToolStripMenuItem";
            openAppToolStripMenuItem.Size = new Size(158, 22);
            openAppToolStripMenuItem.Text = "&Open App";
            // 
            // dockToolStripMenuItem
            // 
            dockToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openAppToolStripMenuItem, dockOpenAppToolStripMenuItem, undockAppToolStripMenuItem1, undockAllAppToolStripMenuItem });
            dockToolStripMenuItem.ForeColor = Color.FromArgb(209, 209, 209);
            dockToolStripMenuItem.Name = "dockToolStripMenuItem";
            dockToolStripMenuItem.Size = new Size(46, 26);
            dockToolStripMenuItem.Text = "&Dock";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.BackColor = Color.FromArgb(43, 43, 43);
            exitToolStripMenuItem.ForeColor = Color.FromArgb(209, 209, 209);
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(128, 22);
            exitToolStripMenuItem.Text = "&Exit";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.BackColor = Color.FromArgb(43, 43, 43);
            openToolStripMenuItem.ForeColor = Color.FromArgb(209, 209, 209);
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(128, 22);
            openToolStripMenuItem.Text = "&Open App";
            // 
            // menuToolStripMenuItem
            // 
            menuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, exitToolStripMenuItem });
            menuToolStripMenuItem.ForeColor = Color.FromArgb(209, 209, 209);
            menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            menuToolStripMenuItem.Size = new Size(50, 26);
            menuToolStripMenuItem.Text = "&Menu";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Right;
            label1.Font = new Font("Nirmala UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(209, 209, 209);
            label1.Location = new Point(775, 0);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(25, 30);
            label1.TabIndex = 0;
            label1.Text = "X";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            menuStrip1.AutoSize = false;
            menuStrip1.BackColor = Color.FromArgb(43, 43, 43);
            menuStrip1.Dock = DockStyle.Fill;
            menuStrip1.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem, dockToolStripMenuItem, editToolStripMenuItem, dockmodeToolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 2, 0, 2);
            menuStrip1.Size = new Size(251, 30);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(43, 43, 43);
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(4, 3, 4, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 30);
            panel2.TabIndex = 9;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(43, 43, 43);
            panel3.Controls.Add(menuStrip1);
            panel3.Dock = DockStyle.Left;
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(4, 3, 4, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(251, 30);
            panel3.TabIndex = 10;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(WindowViewPanel);
            Controls.Add(tabControl1);
            Controls.Add(panel2);
            Name = "MainForm";
            Text = "Form1";
            WindowViewPanel.ResumeLayout(false);
            SidePanel.ResumeLayout(false);
            SidePanel.PerformLayout();
            tabControl1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel WindowViewPanel;
        private Panel panel1;
        private Panel SidePanel;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label2;
        private TabPage tabPage2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private ToolStripMenuItem WindowsModeToolStripMenuItem;
        private ToolStripMenuItem TabsModeToolStripMenuItem;
        private ToolStripMenuItem dockmodeToolStripMenuItem1;
        private ToolStripMenuItem chToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem undockAllAppToolStripMenuItem;
        private ToolStripMenuItem undockAppToolStripMenuItem1;
        private ToolStripMenuItem dockOpenAppToolStripMenuItem;
        private ToolStripMenuItem openAppToolStripMenuItem;
        private ToolStripMenuItem dockToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem menuToolStripMenuItem;
        private Label label1;
        private MenuStrip menuStrip1;
        private Panel panel2;
        private Panel panel3;
    }
}
