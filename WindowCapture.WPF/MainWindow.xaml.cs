using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowCapture.Core.Common;
using WindowCapture.Core.Models;
namespace WindowCapture.WPF;

public partial class MainWindow : Window
{
    private readonly WpfClient _client;

    public MainWindow()
    {
        InitializeComponent();
        _client = new WpfClient(MainTabControl, WindowSidebarPanel, WindowCanvas);
        AddPlusTab();
    }

    // ── Tab helpers ────────────────────────────────────────────────────

    private void AddPlusTab()
    {
        var plusTab = new TabItem { Header = CreatePlusHeader() };
        MainTabControl.Items.Add(plusTab);
    }

    private TabItem AddNamedTab(string header = "New tab")
    {
        var host = new HwndWindowHost();
        var grid = new Grid();
        grid.Children.Add(host);

        var tab = new TabItem
        {
            Content = grid,
            Tag = host
        };

        // Set header after tab is constructed so the close button captures the right reference
        tab.Header = CreateClosableHeader(header, tab);
        _client.RegisterTab(tab, host);

        int insertAt = Math.Max(0, MainTabControl.Items.Count - 1);
        MainTabControl.Items.Insert(insertAt, tab);
        MainTabControl.SelectedItem = tab;
        return tab;
    }

    private UIElement CreateClosableHeader(string title, TabItem tab)
    {
        var panel = new StackPanel { Orientation = Orientation.Horizontal };

        var label = new TextBlock
        {
            Text = title,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 6, 0)
        };

        var closeBtn = new Button
        {
            Content = "✕",
            Width = 16,
            Height = 16,
            Padding = new Thickness(0),
            FontSize = 10,
            VerticalAlignment = VerticalAlignment.Center,
            // Flat appearance without overriding Fluent styles
            Background = System.Windows.Media.Brushes.Transparent,
            BorderThickness = new Thickness(0)
        };

        closeBtn.Click += (_, _) =>
        {
            if (tab != null) CloseTab(tab);
        };

        panel.Children.Add(label);
        panel.Children.Add(closeBtn);
        return panel;
    }

    private UIElement CreatePlusHeader()
    {
        return new TextBlock { Text = "+", Padding = new Thickness(4, 0, 4, 0) };
    }

    /// <summary>Updates the text label inside a closable tab header.</summary>
    public void RenameTabHeader(TabItem tab, string newTitle)
    {
        if (tab.Header is StackPanel panel && panel.Children[0] is TextBlock tb)
            tb.Text = newTitle;
    }

    private void CloseTab(TabItem tab)
    {
        _client.CloseTab(tab);
        MainTabControl.Items.Remove(tab);

        // Always keep at least the "+" tab
        if (MainTabControl.Items.Count == 1)
        {
            // Only "+" remains — nothing to select
            MainTabControl.SelectedIndex = 0;
        }
    }

    private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (MainTabControl.SelectedItem is not TabItem selected) return;

        // "+" tab selected → defer the insertion until after layout completes
        if (selected.Header is TextBlock { Text: "+" })
        {
            Dispatcher.InvokeAsync(() =>
            {
                MainTabControl.SelectionChanged -= MainTabControl_SelectionChanged;

                TabItem newTab = AddNamedTab();
                OpenAppIntoTab(newTab);

                MainTabControl.SelectionChanged += MainTabControl_SelectionChanged;

            }, System.Windows.Threading.DispatcherPriority.Loaded);

            return;
        }

        _client.TabSelected(selected);
    }

    private void OpenApp_Click(object sender, RoutedEventArgs e)
    {
        if (_client.DockingMode == DockingMode.Tab)
        {
            TabItem tab = AddNamedTab();
            OpenAppIntoTab(tab);
        }
        else
        {
            OpenAppWindowMode();
        }
    }

    private void DockOpenApp_Click(object sender, RoutedEventArgs e)
    {
        // Show picker of already-running apps
        var picker = new OpenAppPickerWindow(_client.WindowManager);
        if (picker.ShowDialog() != true || picker.SelectedApp == null) return;

        OpenApp app = picker.SelectedApp;

        if (_client.DockingMode == DockingMode.Tab)
        {
            TabItem tab = AddNamedTab(app.Title);
            _client.EmbedExisting(app.Handle, tab, app.Title);
            RenameTabHeader(tab, app.Title);
        }
        else
        {
            OpenChildWindow(embedHandle: app.Handle, title: app.Title);
        }
    }

    private void UndockApp_Click(object sender, RoutedEventArgs e) => _client.Undock();
    private void UndockAll_Click(object sender, RoutedEventArgs e) => _client.UndockAll();

    private void RenameTab_Click(object sender, RoutedEventArgs e)
    {
        if (_client.DockingMode != DockingMode.Tab) return;
        if (MainTabControl.SelectedItem is not TabItem tab) return;

        string current = tab.Header is StackPanel p && p.Children[0] is TextBlock tb
            ? tb.Text : "New tab";

        var dlg = new TabRenameWindow { TabName = current };
        if (dlg.ShowDialog() != true) return;

        RenameTabHeader(tab, dlg.TabName);
    }

    private void DockingMode_Click(object sender, RoutedEventArgs e)
    {
        // Mutual exclusion
        TabModeMenuItem.IsChecked = sender == TabModeMenuItem;
        WindowModeMenuItem.IsChecked = sender == WindowModeMenuItem;

        _client.DockingMode = sender == TabModeMenuItem
            ? DockingMode.Tab
            : DockingMode.Window;

        MainTabControl.Visibility = _client.DockingMode == DockingMode.Tab
            ? Visibility.Visible : Visibility.Collapsed;
        WindowModeGrid.Visibility = _client.DockingMode == DockingMode.Window
            ? Visibility.Visible : Visibility.Collapsed;
    }

    private void Exit_Click(object sender, RoutedEventArgs e) => Close();

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        => _client.Close();

    // ── Window mode ────────────────────────────────────────────────────

    private void OpenAppWindowMode()
    {
        string? path = PickExecutable();
        if (path == null) return;
        OpenChildWindow(exePath: path);
    }

    private void OpenChildWindow(string? exePath = null, IntPtr embedHandle = default, string title = "Window")
    {
        var host = new HwndWindowHost { Width = 400, Height = 300 };
        var border = new System.Windows.Controls.Border
        {
            BorderThickness = new Thickness(1),
            BorderBrush = SystemColors.ControlDarkBrush,
            Width = 420,
            Height = 320,
            Tag = host
        };

        // Title bar for the floating child
        var titleBar = new TextBlock { Text = title, Padding = new Thickness(4, 2, 4, 2), Cursor = Cursors.SizeAll };
        var closeBtn = new Button { Content = "✕", Width = 24, Height = 24, BorderThickness = new Thickness(0), Background = System.Windows.Media.Brushes.Transparent };
        var titleRow = new DockPanel();
        DockPanel.SetDock(closeBtn, Dock.Right);
        titleRow.Children.Add(closeBtn);
        titleRow.Children.Add(titleBar);

        var stack = new StackPanel();
        stack.Children.Add(titleRow);
        stack.Children.Add(host);
        border.Child = stack;

        // Drag to move
        Point dragStart = default;
        bool dragging = false;
        titleBar.MouseLeftButtonDown += (_, me) => { dragging = true; dragStart = me.GetPosition(WindowCanvas); titleBar.CaptureMouse(); };
        titleBar.MouseMove += (_, me) =>
        {
            if (!dragging) return;
            Point pos = me.GetPosition(WindowCanvas);
            double left = Canvas.GetLeft(border) + (pos.X - dragStart.X);
            double top = Canvas.GetTop(border) + (pos.Y - dragStart.Y);
            Canvas.SetLeft(border, left);
            Canvas.SetTop(border, top);
            dragStart = pos;
        };
        titleBar.MouseLeftButtonUp += (_, _) => { dragging = false; titleBar.ReleaseMouseCapture(); };

        closeBtn.Click += (_, _) =>
        {
            _client.CloseWindowHost(host);
            WindowCanvas.Children.Remove(border);
            RemoveSidebarButton(border);
        };

        Canvas.SetLeft(border, 20);
        Canvas.SetTop(border, 20);
        WindowCanvas.Children.Add(border);

        // Embed after host HWND is ready
        host.Loaded += (_, _) =>
        {
            if (exePath != null)
                _client.WindowManager.LaunchAndDock(exePath, host);
            else if (embedHandle != default)
                _client.WindowManager.EmbedExisting(embedHandle, host);

            host.LabelChanged += (_, lbl) =>
            {
                titleBar.Text = lbl;
                UpdateSidebarButtonLabel(border, lbl);
            };
        };

        AddSidebarButton(title, border);
    }

    private void WindowCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        => _client.CanvasResized(WindowCanvas);

    // ── Sidebar (window mode) ──────────────────────────────────────────

    private void AddSidebarButton(string title, UIElement target)
    {
        var btn = new Button
        {
            Content = title,
            Width = 120,
            Margin = new Thickness(4),
            Tag = target
        };

        btn.Click += (_, _) =>
        {
            if (target is FrameworkElement fe)
            {
                bool isVisible = fe.Visibility == Visibility.Visible;
                fe.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
            }
        };

        WindowSidebarPanel.Children.Add(btn);
    }

    private void RemoveSidebarButton(UIElement target)
    {
        for (int i = WindowSidebarPanel.Children.Count - 1; i >= 0; i--)
        {
            if (WindowSidebarPanel.Children[i] is Button btn && btn.Tag == target)
            { WindowSidebarPanel.Children.RemoveAt(i); break; }
        }
    }

    private void UpdateSidebarButtonLabel(UIElement target, string label)
    {
        foreach (UIElement child in WindowSidebarPanel.Children)
            if (child is Button btn && btn.Tag == target) { btn.Content = label; break; }
    }

    // ── Helpers ────────────────────────────────────────────────────────

    private void OpenAppIntoTab(TabItem tab)
    {
        string? path = PickExecutable();
        if (path == null) { CloseTab(tab); return; }

        if (tab.Tag is HwndWindowHost host)
        {
            host.Loaded += (_, _) =>
            {
                bool ok = _client.WindowManager.LaunchAndDock(path, host);
                if (!ok) Application.Current.Dispatcher.Invoke(() => CloseTab(tab));

                host.LabelChanged += (_, lbl) => RenameTabHeader(tab, lbl);
            };
        }
    }

    private string? PickExecutable()
    {
        var dlg = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Select Application",
            Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*"
        };
        return dlg.ShowDialog() == true ? dlg.FileName : null;
    }
}