using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WindowCapture.Core;
using WindowCapture.Core.Common;
namespace WindowCapture.WPF;

public class WpfClient : INotifyPropertyChanged
{
    private DockingMode _dockingMode;

    private readonly TabControl _tabControl;
    private readonly Panel _windowPanel;   // StackPanel/WrapPanel for window-mode buttons
    private readonly Panel _contentArea;   // Grid/Canvas for floating child windows

    public readonly WindowManager WindowManager;

    // Tracks which HwndWindowHost belongs to which TabItem
    private readonly Dictionary<TabItem, HwndWindowHost> _tabHosts = new();
    private readonly List<HwndWindowHost> _windowHosts = new();

    public TabItem SelectedTab => _tabControl.SelectedItem as TabItem;

    public event PropertyChangedEventHandler PropertyChanged;

    public DockingMode DockingMode
    {
        get => _dockingMode;
        set
        {
            if (_dockingMode == value) return;
            _dockingMode = value;
            NotifyPropertyChanged();
            ApplyDockingMode(value);
        }
    }

    public WpfClient(TabControl tabControl, Panel windowPanel, Panel contentArea)
    {
        _tabControl = tabControl;
        _windowPanel = windowPanel;
        _contentArea = contentArea;
        WindowManager = new WindowManager();

        _tabControl.SelectionChanged += OnTabSelectionChanged;
        DockingMode = DockingMode.Tab;
    }
    public TabItem AddTab(string header = "New tab")
    {
        var host = new HwndWindowHost();
        var content = new Grid();
        content.Children.Add(host);

        var tab = new TabItem
        {
            Header = header,
            Content = content
        };

        // Wire label changes back to the tab header
        host.LabelChanged += (_, label) => tab.Header = label;

        // Insert before the "+" tab (always last)
        int insertAt = Math.Max(0, _tabControl.Items.Count - 1);
        _tabControl.Items.Insert(insertAt, tab);
        _tabHosts[tab] = host;

        _tabControl.SelectedItem = tab;
        return tab;
    }

    public void CloseTab(TabItem tab)
    {
        if (_tabHosts.TryGetValue(tab, out HwndWindowHost host))
        {
            WindowManager.CloseEmbedded(host);
            _tabHosts.Remove(tab);
        }
        _tabControl.Items.Remove(tab);
    }

    private void OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectedTab == null) return;

        // Resize embedded app to fill the newly visible tab
        if (_tabHosts.TryGetValue(SelectedTab, out HwndWindowHost host))
            WindowManager.ResizeAndDock(host);

        // "+" tab selected → add a real tab and launch
        if (SelectedTab.Header?.ToString() == "+")
            LaunchAndDock();
    }
    public void LaunchAndDock()
    {
        string path = PickExecutable();
        if (path == null) return;

        if (DockingMode == DockingMode.Tab)
        {
            TabItem tab = AddTab("Loading…");
            HwndWindowHost host = _tabHosts[tab];

            // Host must be loaded/measured before the HWND is valid
            host.Loaded += (_, _) =>
            {
                bool ok = WindowManager.LaunchAndDock(path, host);
                if (!ok) CloseTab(tab);
            };
        }
    }

    public void EmbedExisting(IntPtr appHandle, string title)
    {
        if (DockingMode != DockingMode.Tab || SelectedTab == null) return;
        if (!_tabHosts.TryGetValue(SelectedTab, out HwndWindowHost host)) return;
        WindowManager.EmbedExisting(appHandle, host);
        SelectedTab.Header = title;
    }

    public void Undock()
    {
        if (SelectedTab == null || !_tabHosts.TryGetValue(SelectedTab, out HwndWindowHost host)) return;
        WindowManager.Undock(host);
    }

    public void UndockAll() => WindowManager.UndockAll();

    public void TabResized(TabItem tab)
    {
        if (_tabHosts.TryGetValue(tab, out HwndWindowHost host))
            WindowManager.ResizeAndDock(host);
    }

    public void Close()
    {
        WindowManager.CloseAll();
        Application.Current.Shutdown();
    }
    private string PickExecutable()
    {
        var dlg = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Select Application",
            Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*"
        };
        return dlg.ShowDialog() == true ? dlg.FileName : null;
    }

    private void ApplyDockingMode(DockingMode mode)
    {
        _tabControl.Visibility = mode == DockingMode.Tab ? Visibility.Visible : Visibility.Collapsed;
        _windowPanel.Visibility = mode == DockingMode.Window ? Visibility.Visible : Visibility.Collapsed;
    }
    public void RegisterTab(TabItem tab, HwndWindowHost host)
    => _tabHosts[tab] = host;

    public void TabSelected(TabItem tab)
    {
        if (_tabHosts.TryGetValue(tab, out HwndWindowHost? host))
            WindowManager.ResizeAndDock(host);
    }

    public void EmbedExisting(IntPtr handle, TabItem tab, string title)
    {
        if (_tabHosts.TryGetValue(tab, out HwndWindowHost? host))
            WindowManager.EmbedExisting(handle, host);
    }

    public void CloseWindowHost(HwndWindowHost host)
    {
        WindowManager.CloseEmbedded(host);
        _windowHosts.Remove(host);
    }

    public void CanvasResized(System.Windows.Controls.Canvas canvas)
    {
        // Optionally resize any maximized child windows to fill the canvas
    }

    private void NotifyPropertyChanged([CallerMemberName] string prop = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}
