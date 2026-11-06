using System.Windows;
using System.Windows.Input;
namespace WindowCapture.WPF;
public partial class TabRenameWindow : Window
{
    public string TabName
    {
        get => NameBox.Text;
        set => NameBox.Text = value;
    }

    public TabRenameWindow() => InitializeComponent();

    private void Save_Click(object sender, RoutedEventArgs e) => DialogResult = true;
    private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    private void NameBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) DialogResult = true;
    }
}
