using System.Diagnostics;
using System.Text;
using WindowCapture.Core.Common;
using static WindowCapture.Core.Common.NativeInterop;

namespace WindowCapture.Core;
public class WindowManager
{
    private readonly Dictionary<IWindowHost, IntPtr> _hostAppHandles = new();

    private const int SW_SHOW = 5;
    private const int SW_HIDE = 0;
    private const uint WM_CLOSE = 0x0010;
    private const int WM_SYSCOMMAND = 0x0112;
    private const int SC_RESTORE = 0xF120;

    // ── Private helpers ────────────────────────────────────────────────

    private string GetWindowTitle(IntPtr hWnd)
    {
        var sb = new StringBuilder(256);
        GetWindowText(hWnd, sb, sb.Capacity);
        return sb.ToString();
    }

    private IntPtr GetMainWindowHandle(int processId)
    {
        IntPtr found = IntPtr.Zero;
        EnumWindows((hWnd, _) =>
        {
            GetWindowThreadProcessId(hWnd, out uint pid);
            if (pid == processId && IsWindowVisible(hWnd))
            {
                found = hWnd;
                return false;
            }
            return true;
        }, IntPtr.Zero);
        return found;
    }

    private List<IntPtr> GetOpenApplicationHandles()
    {
        var handles = new List<IntPtr>();
        EnumWindows((hWnd, _) =>
        {
            if (IsWindowVisible(hWnd)) handles.Add(hWnd);
            return true;
        }, IntPtr.Zero);
        return handles;
    }

    private bool WaitForMainWindow(Process process, int timeout = 10000)
    {
        int waited = 0;
        while (process.MainWindowHandle == IntPtr.Zero)
        {
            if (waited >= timeout) return false;
            Thread.Sleep(500);
            waited += 500;
            process.Refresh();
        }
        return true;
    }

    private void ResizeToHost(IWindowHost host, IntPtr appHandle)
    {
        if (appHandle != IntPtr.Zero)
            MoveWindow(appHandle, 0, 0, host.ClientWidth, host.ClientHeight, true);
    }

    // ── Public API ─────────────────────────────────────────────────────

    /// <summary>Returns all visible top-level windows keyed by title.</summary>
    public Dictionary<string, IntPtr> GetOpenApplications()
    {
        var result = new Dictionary<string, IntPtr>();
        foreach (IntPtr handle in GetOpenApplicationHandles())
        {
            string title = GetWindowTitle(handle);
            if (string.IsNullOrEmpty(title)) continue;
            if (result.ContainsKey(title)) title += " (2)";
            result[title] = handle;
        }
        return result;
    }

    /// <summary>
    /// Launches an executable, waits for its window, then embeds it into <paramref name="host"/>.
    /// Returns true on success. <paramref name="exePath"/> is the full path to the .exe.
    /// </summary>
    public bool LaunchAndDock(string exePath, IWindowHost host)
    {
        if (string.IsNullOrWhiteSpace(exePath)) return false;

        try
        {
            Process process = Process.Start(exePath);
            if (!WaitForMainWindow(process)) return false;

            IntPtr appHandle = GetMainWindowHandle(process.Id);
            if (appHandle == IntPtr.Zero) return false;

            SetParent(appHandle, host.Handle);
            ShowWindow(appHandle, SW_SHOW);
            ResizeToHost(host, appHandle);
            _hostAppHandles[host] = appHandle;

            host.Label = Path.GetFileNameWithoutExtension(exePath);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Embeds an already-running window (by handle) into <paramref name="host"/>.</summary>
    public void EmbedExisting(IntPtr appHandle, IWindowHost host)
    {
        SetParent(appHandle, host.Handle);
        ShowWindow(appHandle, SW_SHOW);
        ResizeToHost(host, appHandle);
        _hostAppHandles[host] = appHandle;
    }

    /// <summary>Resizes the embedded app to fill its host (call on host resize).</summary>
    public void ResizeAndDock(IWindowHost host)
    {
        if (_hostAppHandles.TryGetValue(host, out IntPtr handle) && handle != IntPtr.Zero)
            ResizeToHost(host, handle);
    }

    /// <summary>Detaches the embedded app from <paramref name="host"/> and restores it as a free window.</summary>
    public void Undock(IWindowHost host)
    {
        if (!_hostAppHandles.TryGetValue(host, out IntPtr handle) || handle == IntPtr.Zero) return;
        _hostAppHandles.Remove(host);
        UndockHandle(handle);
    }

    /// <summary>Detaches a window directly by handle.</summary>
    public void UndockHandle(IntPtr handle)
    {
        SetParent(handle, IntPtr.Zero);
        ShowWindow(handle, SW_SHOW);
        MoveWindow(handle, 100, 100, 800, 600, true);
        SendMessage(handle, (uint)WM_SYSCOMMAND, (IntPtr)SC_RESTORE, IntPtr.Zero);
    }

    /// <summary>Undocks all embedded apps.</summary>
    public void UndockAll()
    {
        foreach (IntPtr handle in _hostAppHandles.Values)
            if (handle != IntPtr.Zero) UndockHandle(handle);
        _hostAppHandles.Clear();
    }

    /// <summary>Undocks and closes the embedded app in <paramref name="host"/>.</summary>
    public void CloseEmbedded(IWindowHost host)
    {
        if (!_hostAppHandles.TryGetValue(host, out IntPtr handle) || handle == IntPtr.Zero) return;
        UndockHandle(handle);
        SendMessage(handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        _hostAppHandles.Remove(host);
    }

    /// <summary>Closes all embedded apps and clears the registry.</summary>
    public void CloseAll()
    {
        foreach (IntPtr handle in _hostAppHandles.Values)
        {
            if (handle == IntPtr.Zero) continue;
            UndockHandle(handle);
            SendMessage(handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
        _hostAppHandles.Clear();
    }

    public void ReleaseCapture() => NativeInterop.ReleaseCapture();

    public void SendTitleBarDragMessage(IntPtr hWnd)
        => SendMessage(hWnd, 0x0112, 0xf012, 0); // WM_SYSCOMMAND / SC_MOVE+2
}
