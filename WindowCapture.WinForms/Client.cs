using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindowCapture.Core;
using WindowCapture.Core.Common;
namespace WindowCapture.WinForms;

internal class Client : INotifyPropertyChanged
{
    private DockingMode _dockingMode;

    private readonly TabControl _tabControl;
    private readonly Panel _contentPanel;   // fills the content area
    private readonly Panel _windowViewPanel;
    private readonly FlowLayoutPanel _flowLayout;

    public readonly WindowManager WindowManager;
    public readonly TabManager TabManager;

    public Form SelectedChildForm { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public DockingMode DockingMode
    {
        get => _dockingMode;
        set
        {
            if (_dockingMode == value) return;
            _dockingMode = value;
            NotifyPropertyChanged();
            ApplyDockingMode(_dockingMode);
        }
    }

    public Client(TabControl tabControl, Panel contentPanel,
                  Panel windowViewPanel, FlowLayoutPanel flowLayout)
    {
        _tabControl = tabControl;
        _contentPanel = contentPanel;
        _windowViewPanel = windowViewPanel;
        _flowLayout = flowLayout;

        WindowManager = new WindowManager();
        TabManager = new TabManager(tabControl, WindowManager);

        // Wire new-tab event so the client can open the file dialog
        TabManager.NewTabRequested += (_, e) =>
            LaunchAndDockIntoTab(e.Tab);

        DockingMode = DockingMode.Tab;
    }

    // ── Docking mode ───────────────────────────────────────────────────

    private void ApplyDockingMode(DockingMode mode)
    {
        void Show(Control c, DockStyle dock = DockStyle.Fill) { c.Visible = true; c.Dock = dock; }
        void Hide(Control c) { c.Dock = DockStyle.None; c.Visible = false; }

        if (mode == DockingMode.Tab)
        {
            Hide(_windowViewPanel);
            Show(_tabControl);
        }
        else if (mode == DockingMode.Window)
        {
            Hide(_tabControl);
            Show(_windowViewPanel);
        }
    }

    // ── Tab mode ───────────────────────────────────────────────────────

    private void LaunchAndDockIntoTab(TabPage tab)
    {
        string path = PickExecutable();
        if (path == null) return;
        WindowManager.LaunchAndDock(path, new ControlWindowHost(tab));
    }

    public void LaunchAndDock()
    {
        if (DockingMode == DockingMode.Tab)
        {
            if (TabManager.SelectedTab == null) return;
            string path = PickExecutable();
            if (path == null) return;
            WindowManager.LaunchAndDock(path, new ControlWindowHost(TabManager.SelectedTab));
        }
        else if (DockingMode == DockingMode.Window)
        {
            Form child = CreateChildForm();
            _contentPanel.Controls.Add(child);
            child.Show();

            string path = PickExecutable();
            if (path == null) { child.Close(); return; }

            WindowManager.LaunchAndDock(path, new ControlWindowHost(child));
            AddSidebarButton(child);
            WindowManager.ResizeAndDock(new ControlWindowHost(child));
            SelectedChildForm = child;
        }
    }

    public void EmbedSelectedApp(IntPtr appHandle, string appTitle)
    {
        if (DockingMode == DockingMode.Tab && TabManager.SelectedTab != null)
        {
            WindowManager.EmbedExisting(appHandle, new ControlWindowHost(TabManager.SelectedTab));
            TabManager.SelectedTab.Text = appTitle;
        }
        else if (DockingMode == DockingMode.Window && SelectedChildForm != null)
        {
            WindowManager.EmbedExisting(appHandle, new ControlWindowHost(SelectedChildForm));
            SelectedChildForm.Text = appTitle;
        }
    }

    public void Undock()
    {
        if (DockingMode == DockingMode.Tab && TabManager.SelectedTab != null)
            WindowManager.Undock(new ControlWindowHost(TabManager.SelectedTab));
        else if (DockingMode == DockingMode.Window && SelectedChildForm != null)
            WindowManager.Undock(new ControlWindowHost(SelectedChildForm));
    }

    public void UndockAll() => WindowManager.UndockAll();

    public void FormResized()
    {
        if (DockingMode == DockingMode.Tab && TabManager.SelectedTab != null)
            WindowManager.ResizeAndDock(new ControlWindowHost(TabManager.SelectedTab));
    }

    public void TitleBarMouseDown(Control control)
    {
        Form parent = control.Parent?.FindForm();
        if (parent == null) return;
        WindowManager.ReleaseCapture();
        WindowManager.SendTitleBarDragMessage(parent.Handle);
    }

    public void Close()
    {
        WindowManager.CloseAll();
        Application.Exit();
    }

    // ── Helpers ────────────────────────────────────────────────────────

    private string PickExecutable()
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Select Application",
            Filter = "Executable files (*.exe)|*.exe|Shortcuts (*.lnk)|*.lnk|All files (*.*)|*.*"
        };
        return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : null;
    }

    private Form CreateChildForm()
    {
        var child = new Form
        {
            TopLevel = false,
            FormBorderStyle = FormBorderStyle.Sizable,
            Size = new System.Drawing.Size(400, 200),
            Location = new System.Drawing.Point(20, 20)
        };

        child.MouseDown += (_, _) => { child.BringToFront(); SelectedChildForm = child; };
        child.Move += (_, _) => { child.BringToFront(); SelectedChildForm = child; };

        child.FormClosing += (_, _) =>
            WindowManager.CloseEmbedded(new ControlWindowHost(child));

        child.Resize += (_, _) =>
        {
            WindowManager.ResizeAndDock(new ControlWindowHost(child));
            if (child.WindowState == FormWindowState.Minimized) child.Hide();
            else child.Show();
        };

        return child;
    }

    private void AddSidebarButton(Form form)
    {
        var btn = new Button { Text = form.Text, Width = 125, Tag = form };

        btn.Click += (_, _) =>
        {
            if (form.WindowState == FormWindowState.Minimized)
            { form.WindowState = FormWindowState.Maximized; form.Show(); }
            else
            { form.WindowState = FormWindowState.Minimized; form.Hide(); }
        };

        form.FormClosed += (_, _) => _flowLayout.Controls.Remove(btn);
        _flowLayout.Controls.Add(btn);
    }

    private void NotifyPropertyChanged([CallerMemberName] string prop = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}
