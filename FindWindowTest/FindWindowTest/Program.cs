using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FindWindowTest
{
    class Program
    {
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
        private static extern int GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        static void Main(string[] args)
        {
            RECT rectangle = new RECT();
            IntPtr hwnd = FindWindow(null, "Google+ Hangouts - Mozilla Firefox");
            while (true)
            {
                GetWindowRect(hwnd, ref rectangle);
                Console.WriteLine("Top left = " + rectangle.Left + ", " + rectangle.Top);
                Console.WriteLine("Bottom right = " + rectangle.Right + ", " + rectangle.Bottom);
                Console.In.Read();
            }
        }
    }
}
