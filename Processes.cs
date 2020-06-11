using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace MyWPFApp
{
    public class Processes : ObservableCollection<DesktopWindow>
    {
        private Process _realProcess;

        public Processes() : base(DesktopWindow.GetAll())
        {

        }

        private Process GetRealProcess(Process foregroundProcess)
        {
            WinAPI.EnumChildWindows(foregroundProcess.MainWindowHandle, ChildWindowCallback, IntPtr.Zero);
            return _realProcess;
        }

        private bool ChildWindowCallback(IntPtr hwnd, IntPtr lparam)
        {
            var process = Process.GetProcessById(WinAPI.GetWindowProcessId(hwnd));
            if (process.ProcessName != "ApplicationFrameHost")
            {
                _realProcess = process;
            }
            return true;
        }
    }
}
