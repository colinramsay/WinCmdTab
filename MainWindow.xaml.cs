using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyboardHook _kh;

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            _kh = new KeyboardHook(this, VirtualKeyCodes.A, ModifierKeyCodes.Alt);
            _kh.Triggered += () => DoWork();


            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);


        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(KeyboardState.AnyKeyPressed());
            if (!KeyboardState.AnyKeyPressed())
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


        private void DoWork()
        {
            Console.WriteLine("press %o" + WinAPI.ApplicationIsActivated(Process.GetCurrentProcess().Id));
            if (!WinAPI.ApplicationIsActivated(Process.GetCurrentProcess().Id))
            {
                this.Show();
                this.Activate();

                lbTodoList.SelectedIndex = 1;

                //lbTodoList.ItemsSource = DesktopWindow.GetAll();

                dispatcherTimer.Start();
            }
            else
            {
                //Send(Key.Tab);
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
        public void Send(Key key)
        {
            Console.WriteLine("send");
            if (Keyboard.PrimaryDevice != null)
            {

                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {

                    var e = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    };
                    InputManager.Current.ProcessInput(e);

                    // Note: Based on your requirements you may also need to fire events for:
                    // RoutedEvent = Keyboard.PreviewKeyDownEvent
                    // RoutedEvent = Keyboard.KeyUpEvent
                    // RoutedEvent = Keyboard.PreviewKeyUpEvent
                }
            }
        }
        private void lbTodoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // if (lbTodoList.SelectedItem != null)
            // {
            //     var proc = (lbTodoList.SelectedItem as DesktopWindow);
            //     WinAPI.SetForegroundWindow(proc.ProcessId);
            // }

        }

    }
}
