using System.Runtime.InteropServices;
using System.Windows.Interop;
using WindowCapture.Core;

namespace WindowCapture.WPF;
public class HwndWindowHost : HwndHost, IWindowHost
{
    private IntPtr _childHandle = IntPtr.Zero;
    private string _label = string.Empty;

    public IntPtr Handle => _hwnd;
    public int ClientWidth => (int)ActualWidth;
    public int ClientHeight => (int)ActualHeight;

    public string Label
    {
        get => _label;
        set { _label = value; LabelChanged?.Invoke(this, value); }
    }

    public event EventHandler<string> LabelChanged;

    private IntPtr _hwnd;

    protected override HandleRef BuildWindowCore(HandleRef hwndParent)
    {
        // Create a plain child Win32 window to act as the container HWND.
        // The embedded app will be SetParent'd into this.
        _hwnd = CreateWindowEx(
            0, "static", "",
            WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN,
            0, 0, (int)Width, (int)Height,
            hwndParent.Handle,
            IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

        return new HandleRef(this, _hwnd);
    }

    protected override void DestroyWindowCore(HandleRef hwnd)
    {
        DestroyWindow(hwnd.Handle);
    }

    // Win32 needed for the container window itself
    private const int WS_CHILD = 0x40000000;
    private const int WS_VISIBLE = 0x10000000;
    private const int WS_CLIPCHILDREN = 0x02000000;

    [DllImport("user32.dll")]
    private static extern IntPtr CreateWindowEx(
        int dwExStyle, string lpClassName, string lpWindowName, int dwStyle,
        int x, int y, int nWidth, int nHeight,
        IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

    [DllImport("user32.dll")]
    private static extern bool DestroyWindow(IntPtr hwnd);
}
