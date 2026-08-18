using WindowCapture.Core;
namespace WindowCapture.WinForms;
internal class TabManager
{
    private readonly TabControl _tabControl;
    private readonly WindowManager _windowManager;

    public TabPage SelectedTab { get; private set; }

    public TabManager(TabControl tabControl, WindowManager windowManager)
    {
        _tabControl = tabControl;
        _windowManager = windowManager;
        SelectedTab = _tabControl.TabPages[0];

        _tabControl.SelectedIndexChanged += OnSelectedIndexChanged;
        _tabControl.DoubleClick += OnDoubleClick;
    }

    private void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (sender is not TabControl tc) return;

        SelectedTab = tc.SelectedTab;

        if (SelectedTab != null)
            _windowManager.ResizeAndDock(new ControlWindowHost(SelectedTab));

        // Last tab ("+") → add a real tab and launch
        if (tc.SelectedIndex == tc.TabCount - 1)
        {
            AddNewTab("+");
            // Caller is responsible for triggering LaunchAndDock via the Client
            NewTabRequested?.Invoke(this, new TabEventArgs(SelectedTab));
        }
    }

    private void OnDoubleClick(object sender, EventArgs e)
    {
        if (SelectedTab == null) return;
        _windowManager.CloseEmbedded(new ControlWindowHost(SelectedTab));
        _tabControl.TabPages.Remove(SelectedTab);
        SelectedTab = GetSecondToLastTab();
        if (SelectedTab != null) _tabControl.SelectedTab = SelectedTab;
    }

    public void AddNewTab(string tabName)
    {
        var newTab = new TabPage { Text = tabName };
        _tabControl.TabPages.Add(newTab);

        TabPage secondToLast = GetSecondToLastTab();
        if (secondToLast != null)
        {
            secondToLast.Text = $"New tab {_tabControl.TabPages.Count - 1}";
            secondToLast.BackColor = Color.FromArgb(56, 57, 60);
        }
    }

    public TabPage GetSecondToLastTab()
    {
        int count = _tabControl.TabPages.Count;
        return count >= 2 ? _tabControl.TabPages[count - 2] : null;
    }

    /// <summary>Fired when the "+" tab is clicked and a new tab has been created.</summary>
    public event EventHandler<TabEventArgs> NewTabRequested;
}

public class TabEventArgs : EventArgs
{
    public TabPage Tab { get; }
    public TabEventArgs(TabPage tab) => Tab = tab;
}
