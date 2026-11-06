using System.Windows;
using WindowCapture.Core;
using WindowCapture.Core.Models;
namespace WindowCapture.WPF;
public partial class OpenAppPickerWindow : Window
{
    public OpenApp? SelectedApp { get; private set; }

    public OpenAppPickerWindow(WindowManager windowManager)
    {
        InitializeComponent();

        var apps = windowManager.GetOpenApplications();
        foreach (var kvp in apps)
            AppListBox.Items.Add(new OpenApp { Handle = kvp.Value, Title = kvp.Key });
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        SelectedApp = AppListBox.SelectedItem as OpenApp;
        DialogResult = SelectedApp != null;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;

    private void AppListBox_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (AppListBox.SelectedItem is OpenApp app)
        { SelectedApp = app; DialogResult = true; }
    }
}
