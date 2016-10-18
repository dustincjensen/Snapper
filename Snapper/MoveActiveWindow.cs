using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Windows.Input;
using Hotkeys;

namespace Snapper
{
    public static class MoveActiveWindow
    {
        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up, or top-level window. 
        /// These windows are ordered according to their appearance on the screen. 
        /// The topmost window receives the highest rank and is the first window in the Z order.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order.</param>
        /// <param name="x">New left side.</param>
        /// <param name="y">New top side.</param>
        /// <param name="cx">New width.</param>
        /// <param name="cy">New height.</param>
        /// <param name="wFlags">The window sizing and positioning flags.</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);


        private static class SWP
        {
            public static int NOSIZE = 1;
            public static int SHOWWINDOW = 0x0040;
        }

        public static void ToggleWindowMonitor()
        {
            IntPtr handle = GetForegroundWindow();
            if (handle == IntPtr.Zero)
            {
                // TODO: setup exception handling
                return;
            }

            var allScreens = Screen.AllScreens;
            var currentScreen = Screen.FromHandle(handle);
            Rectangle workingArea = currentScreen.WorkingArea;
            var activeWindow = _GetActiveWindowDimensions(handle);

            var minX = 0;
            var maxX = 0;
            foreach (var screen in allScreens)
            {
                Rectangle wa = screen.WorkingArea;
                if (wa.X < minX)
                {
                    minX = wa.X;
                }
                if (wa.X + wa.Width > maxX)
                {
                    maxX = wa.X + wa.Width;
                }
            }

            // TODO test with 3 monitors...
            // Where 0 is primary...
            // [-2, -1,  0]
            // [-1,  0,  1]
            // [ 0,  1,  2]
            var newX = (activeWindow.X+7) + workingArea.Width < maxX
                ? activeWindow.X + workingArea.Width
                : activeWindow.X - workingArea.Width;

            SetWindowPos(handle, 0, newX, activeWindow.Y, 0,0, SWP.NOSIZE | SWP.SHOWWINDOW);
        }

        public static void MoveWindow(GlobalHotKey hotKey)
        {
            IntPtr handle = GetForegroundWindow();
            if (handle == IntPtr.Zero)
            {
                // TODO: setup exception handling
                return;
            }            

            Rectangle workingArea = Screen.FromHandle(handle).WorkingArea;
            var windowDimension = _GetActiveWindowDimensions(handle);
            var positionDimension = _GetWindowPositionPlusDimensions(hotKey, workingArea, windowDimension);
                                   
            SetWindowPos(handle, 0, 
                positionDimension.Left - 7, 
                positionDimension.Top, 
                positionDimension.Width + 14,
                positionDimension.Height + 7, 
                SWP.SHOWWINDOW);
        }

        private static Rectangle _GetActiveWindowDimensions(IntPtr handle)
        {
            Rectangle dim;
            if (!GetWindowRect(handle, out dim))
            {
                throw new Exception("Failed to retrieve current window dimensions.");
            }

            return new Rectangle
            {
                X = dim.Left,
                Y = dim.Top,
                Width = dim.Right - (dim.Left*2),
                Height = dim.Bottom - (dim.Top*2)
            };
        }

        struct PositionDimension
        {
            public int Top;
            public int Left;
            public int Width;
            public int Height;
        }

        private static PositionDimension _GetWindowPositionPlusDimensions(GlobalHotKey hotKey, Rectangle workingArea, Rectangle windowDimensions)
        {
            var positionDimension = new PositionDimension
            {
                Top = _GetTop(hotKey, workingArea),
                Height = _GetHeight(hotKey, workingArea),
                // adding the workingArea.X handles multi monitors situations.
                Left = _GetLeft(hotKey, workingArea, windowDimensions) + workingArea.X,
                Width = _GetWidth(hotKey, workingArea, windowDimensions)
            };
            return positionDimension;
        }

        private static int _GetTop(GlobalHotKey hotKey, Rectangle workingArea)
        {
            if (hotKey.Key == Key.NumPad1 || hotKey.Key == Key.NumPad2 || hotKey.Key == Key.NumPad3)
            {
                return workingArea.Height/2;
            }
            return 0;
        }

        private static int _GetHeight(GlobalHotKey hotKey, Rectangle workingArea)
        {
            if (hotKey.Key == Key.NumPad4 || hotKey.Key == Key.NumPad5 || hotKey.Key == Key.NumPad6)
            {
                return workingArea.Height;
            }
            return workingArea.Height / 2;            
        }

        private static int _GetLeft(GlobalHotKey hotKey, Rectangle workingArea, Rectangle windowDimensions)
        {
            var third = workingArea.Width / 3;
            var half = workingArea.Width / 2;

            if (hotKey.Key == Key.NumPad1 || hotKey.Key == Key.NumPad4 || hotKey.Key == Key.NumPad7)
            {
                return 0;
            }
            if (hotKey.Key == Key.NumPad2 || hotKey.Key == Key.NumPad5 || hotKey.Key == Key.NumPad8)
            {
                if (windowDimensions.Width >= workingArea.Width)
                {
                    return third;
                }
                if (windowDimensions.Width < half)
                {
                    return third/2;
                }
                return 0;
            }
            if (hotKey.Key == Key.NumPad3 || hotKey.Key == Key.NumPad6 || hotKey.Key == Key.NumPad9)
            {
                if (windowDimensions.Width >= third*2)
                {
                    return third*2;
                }
                if (windowDimensions.Width < half)
                {
                    return half;
                }
                return third;
            }

            return -1;
        }

        private static int _GetWidth(GlobalHotKey hotKey, Rectangle workingArea, Rectangle windowDimensions)
        {
            var third = workingArea.Width / 3;
            var half = workingArea.Width / 2;

            if (hotKey.Key == Key.NumPad2 || hotKey.Key == Key.NumPad5 || hotKey.Key == Key.NumPad8)
            {            
                if (windowDimensions.Width >= workingArea.Width)
                {
                    return third;
                }
                if (windowDimensions.Width < half)
                {
                    return third*2;
                }
                return workingArea.Width;
            }


            if (windowDimensions.Width >= third * 2)
            {
                return third;
            }
            if (windowDimensions.Width < half)
            {
                return half;
            }
            return third * 2;            
        }
    }
}
