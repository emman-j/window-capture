using WindowCapture.Core;
namespace WindowCapture.WinForms;

/// <summary>
/// Adapts a WinForms <see cref="Control"/> to <see cref="IWindowHost"/>.
/// </summary>
internal class ControlWindowHost : IWindowHost
{
    private readonly Control _control;

    public ControlWindowHost(Control control) => _control = control;

    public IntPtr Handle => _control.Handle;
    public int ClientWidth => _control.ClientSize.Width;
    public int ClientHeight => _control.ClientSize.Height;

    public string Label
    {
        get => _control.Text;
        set => _control.Text = value;
    }

    // Convenience: get the underlying control if needed in WinForms-only code
    public Control Control => _control;
}
