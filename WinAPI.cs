using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class WinAPI
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    //Used to get Handle for Foreground Window
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr GetForegroundWindow();

    //Used to get ID of any Window
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
    public delegate bool WindowEnumProc(IntPtr hwnd, IntPtr lparam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc callback, IntPtr lParam);

    public static int GetWindowProcessId(IntPtr hwnd)
    {
        int pid;
        GetWindowThreadProcessId(hwnd, out pid);
        return pid;
    }

    public static IntPtr GetforegroundWindow()
    {
        return GetForegroundWindow();
    }

    /// <summary>Returns true if the current application has focus, false otherwise</summary>
    public static bool ApplicationIsActivated(int procId)
    {
        var activatedHandle = GetForegroundWindow();
        if (activatedHandle == IntPtr.Zero)
        {
            Console.WriteLine("No window is activated");
            return false;       // No window is currently activated
        }
;
        int activeProcId;
        GetWindowThreadProcessId(activatedHandle, out activeProcId);

        return activeProcId == procId;
    }
}