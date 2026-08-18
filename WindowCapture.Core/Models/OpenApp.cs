namespace WindowCapture.Core.Models;
public class OpenApp
{
    public IntPtr Handle { get; set; }
    public string Title { get; set; }

    public override string ToString() => Title;
}