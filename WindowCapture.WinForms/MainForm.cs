using WindowCapture.Core.Common;

namespace WindowCapture.WinForms;
public partial class MainForm : Form
{
    private Client client;

    public MainForm()
    {
        InitializeComponent();

        this.DoubleBuffered = true;
        this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        this.SetStyle(ControlStyles.ResizeRedraw, true);
        this.Text = string.Empty;
        this.ControlBox = false;

        client = new Client
        (
            tabControl: tabControl1,
            contentPanel: panel1,
            windowViewPanel: WindowViewPanel,
            flowLayout: flowLayoutPanel1
        );

        panel2.MouseDown += TitleBarPanel_MouseDown;
        label1.Click += ExitButton_Click;
        exitToolStripMenuItem.Click += ExitButton_Click;
        openToolStripMenuItem.Click += openToolStripMenuItem_Click;
        openAppToolStripMenuItem.Click += openToolStripMenuItem_Click;
        dockOpenAppToolStripMenuItem.Click += dockOpenAppToolStripMenuItem_Click;
        undockAppToolStripMenuItem1.Click += undockAppToolStripMenuItem1_Click;
        undockAllAppToolStripMenuItem.Click += undockAllAppToolStripMenuItem_Click;
        chToolStripMenuItem.Click += chToolStripMenuItem_Click;
        TabsModeToolStripMenuItem.Click += DockingModeToolStripMenuItem_Click;
        WindowsModeToolStripMenuItem.Click += DockingModeToolStripMenuItem_Click;
    }

    private void Main_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (components != null)
        {
            components.Dispose();
        }
        client?.Close();
    }
    private void Main_SizeChanged(object sender, EventArgs e)
    {
        client?.FormResized();
    }

    private void TitleBarPanel_MouseDown(object sender, MouseEventArgs e)
    {
        client?.TitleBarMouseDown((Control)sender);
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
        client.LaunchAndDock();
    }
    private void dockOpenAppToolStripMenuItem_Click(object sender, EventArgs e)
    {
        //client?.SelectOpenApp();
    }
    private void undockAppToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        client.Undock();
    }
    private void undockAllAppToolStripMenuItem_Click(object sender, EventArgs e)
    {
        client.UndockAll();
    }
    private void chToolStripMenuItem_Click(object sender, EventArgs e)
    {
        //client?.ChangeTabName();
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void DockingModeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;

        // Get the parent menu of the clicked item
        ToolStripItemCollection menuItems = clickedItem.GetCurrentParent().Items;

        // Uncheck all menu items in the same group
        foreach (ToolStripItem item in menuItems)
        {
            if (item is ToolStripMenuItem menuItem)
            {
                menuItem.Checked = false;
            }
        }

        if (clickedItem == TabsModeToolStripMenuItem)
            client.DockingMode = DockingMode.Tab;
        else if (clickedItem == WindowsModeToolStripMenuItem)
            client.DockingMode = DockingMode.Window;

        clickedItem.Checked = true;
    }
}
