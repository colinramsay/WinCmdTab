using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Gma.System.MouseKeyHook;

namespace WinCmdTab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Hook.GlobalEvents().OnCombination(new Dictionary<Combination, Action>
            {
                {Combination.FromString("Alt+A"), () => {
                    Console.WriteLine("Press detected (forward)");
                    HotkeyPressed(1);
                }},
                {Combination.FromString("Alt+Shift+A"), () => {
                    Console.WriteLine("Press detected (backward)");
                    HotkeyPressed(-1);
                }},
            });

            this.KeyUp += new KeyEventHandler(onKeyUp);

            // Required to prevent application playing a sound due to unhandled keypress
            // https://github.com/colinramsay/WinCmdTab/issues/8
            this.KeyDown += new KeyEventHandler((object sender, KeyEventArgs e) => e.Handled = true);
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Keyup");

            if (e.Key == Key.LeftAlt)
            {
                this.Hide();

                if (lbTodoList.SelectedItem != null)
                {
                    var proc = (lbTodoList.SelectedItem as DesktopWindow);
                    WinAPI.SetForegroundWindow(proc.ProcessId);

                    lbTodoList.ItemsSource = DesktopWindow.Promote(proc);
                    var view = CollectionViewSource.GetDefaultView(lbTodoList.ItemsSource);
                    view.Refresh();
                }
            }
        }

        private void HotkeyPressed(int direction)
        {
            var activated = WinAPI.ApplicationIsActivated(Process.GetCurrentProcess().Id);
            Console.WriteLine("Window activated? {0}.", activated);

            // If the window is not already visible:
            // - make it visble/foregrounded
            // - ensure the **second** item in the list (index = 1) is selected
            if (!activated)
            {
                this.Show();
                this.Activate();

                lbTodoList.SelectedIndex = 1;
            }
            // If the window is already visible:
            // - Move to next item in list
            // - Or, if at last item, wrap back round to the first item 
            else
            {
                if (lbTodoList.SelectedIndex == lbTodoList.Items.Count - 1)
                {
                    lbTodoList.SelectedIndex = 0;
                }
                else
                {
                    lbTodoList.SelectedIndex = lbTodoList.SelectedIndex + direction;
                }
            }
        }
    }
}
