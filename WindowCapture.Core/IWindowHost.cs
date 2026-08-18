namespace WindowCapture.Core;

/// <summary>
/// Represents a UI container that can host an embedded Win32 window.
/// Implemented by WinForms (ControlWindowHost) and WPF (HwndWindowHost).
/// </summary>
public interface IWindowHost
{
    /// <summary>The Win32 HWND of the host container.</summary>
    IntPtr Handle { get; }

    /// <summary>Client area width in pixels.</summary>
    int ClientWidth { get; }

    /// <summary>Client area height in pixels.</summary>
    int ClientHeight { get; }

    /// <summary>Display label (tab title, window title, etc.).</summary>
    string Label { get; set; }
}