using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace WinCmdTab
{
    public class Processes : ObservableCollection<DesktopWindow>
    {
        public Processes() : base(DesktopWindow.GetAll())
        {

        }
    }
}
