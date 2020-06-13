﻿using System;
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
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            Hook.GlobalEvents().OnCombination(new Dictionary<Combination, Action>
            {
                {Combination.FromString("Alt+A"), HotkeyPressed},
            });
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Any key pressed? {0}", Keyboard.IsKeyUp(Key.LeftAlt));
            if (Keyboard.IsKeyUp(Key.LeftAlt))
            {
                Console.WriteLine("no keys pressed");
                this.Hide();
                dispatcherTimer.Stop();

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

        private void HotkeyPressed()
        {
            Console.WriteLine("Press detected. Window activated? {0}", WinAPI.ApplicationIsActivated(Process.GetCurrentProcess().Id));

            // If the window is not already visible:
            // - make it visble/foregrounded
            // - ensure the **second** item in the list (index = 1) is selected
            if (!WinAPI.ApplicationIsActivated(Process.GetCurrentProcess().Id))
            {
                this.Show();
                this.Activate();

                lbTodoList.SelectedIndex = 1;

                dispatcherTimer.Start();
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
                    lbTodoList.SelectedIndex++;
                }
            }
        }
    }
}
