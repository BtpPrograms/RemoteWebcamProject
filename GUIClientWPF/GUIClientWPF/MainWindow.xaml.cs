using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace GUIClientWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UdpClient client;
        Window controlWindow;
        IntPtr hwnd;
        RECT rectangle;

        public delegate void followDelegate();

        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, ref RECT rectangle);

        public MainWindow()
        {
            InitializeComponent();
            client = new UdpClient("76.190.248.85");
            controlWindow = this;

            hwnd = FindWindow(null, "Google+ Hangouts - Mozilla Firefox");

            rectangle = new RECT();

            startFollowWindowLoop();

        }

        private void Click(object sender, RoutedEventArgs e)
        {
            var clickedElement = sender as Control;
            client.sendMessage(clickedElement.Tag.ToString());
        }

        private void followWindowLoop()
        {
            hwnd = FindWindow(null, "Google+ Hangouts - Mozilla Firefox");
            GetWindowRect(hwnd, ref rectangle);
            controlWindow.Left = rectangle.Right - controlWindow.Width - 100;
            controlWindow.Top = rectangle.Bottom - controlWindow.Height - 100;

            this.Dispatcher.BeginInvoke(new followDelegate(followWindowLoop), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
        }

        private void startFollowWindowLoop()
        {
            this.Dispatcher.BeginInvoke(new followDelegate(followWindowLoop), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
        }
    }
}
