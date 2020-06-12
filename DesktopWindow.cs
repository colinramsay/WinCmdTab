using System;
using System.Collections.Generic;
using System.Linq;

namespace WinCmdTab
{
    public class DesktopWindow
    {
        private static IList<DesktopWindow> _all;

        public string Name { get; set; }
        public IntPtr ProcessId { get; set; }

        public bool IsForeground { get; set; }


        public override string ToString() => Name;

        public static IList<DesktopWindow> GetAll()
        {
            var processes = System.Diagnostics.Process.GetProcesses();
            var usefulProcesses = processes.Where(x => x.MainWindowHandle != IntPtr.Zero && !x.ProcessName.EndsWith("Host"));//.Select(x => GetRealProcess(x)).GroupBy(x => x.ProcessName);

            _all = usefulProcesses.Select(proc => new DesktopWindow { Name = proc.ProcessName, ProcessId = proc.MainWindowHandle, IsForeground = WinAPI.ApplicationIsActivated(proc.Id) }).ToList();

            return _all;
        }

        public static IList<DesktopWindow> Promote(DesktopWindow dw)
        {
            Console.WriteLine("Promoting " + dw.Name);
            _all.Remove(dw);
            _all.Insert(0, dw);

            return _all;
        }
    }
}
