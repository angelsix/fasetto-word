﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Fasetto.Word
{
    /// <summary>
    /// The dock position of the window
    /// </summary>
    public enum WindowDockPosition
    {
        /// <summary>
        /// Not docked
        /// </summary>
        Undocked = 0,
        /// <summary>
        /// Docked to the left of the screen
        /// </summary>
        Left = 1,
        /// <summary>
        /// Docked to the right of the screen
        /// </summary>
        Right = 2,
        /// <summary>
        /// Docked to the top/bottom of the screen
        /// </summary>
        TopBottom = 3,
        /// <summary>
        /// Docked to the top-left of the screen
        /// </summary>
        TopLeft = 4,
        /// <summary>
        /// Docked to the top-right of the screen
        /// </summary>
        TopRight = 5,
        /// <summary>
        /// Docked to the bottom-left of the screen
        /// </summary>
        BottomLeft = 6,
        /// <summary>
        /// Docked to the bottom-right of the screen
        /// </summary>
        BottomRight = 7,
    }


    /// <summary>
    /// Fixes the issue with Windows of Style <see cref="WindowStyle.None"/> covering the taskbar
    /// </summary>
    public class WindowResizer
    {
        #region Private Members

        /// <summary>
        /// The window to handle the resizing for
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// The last calculated available screen size
        /// </summary>
        private Rect mScreenSize = new Rect();

        /// <summary>
        /// How close to the edge the window has to be to be detected as at the edge of the screen
        /// </summary>
        private int mEdgeTolerance = 8;

        /// <summary>
        /// The transform matrix used to convert WPF sizes to screen pixels
        /// </summary>
        private DpiScale? mMonitorDpi;

        /// <summary>
        /// The last screen the window was on
        /// </summary>
        private IntPtr mLastScreen;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition mLastDock = WindowDockPosition.Undocked;

        #endregion

        #region DLL Imports

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

        #endregion

        #region Public Events

        /// <summary>
        /// Called when the window dock position changes
        /// </summary>
        public event Action<WindowDockPosition> WindowDockChanged = (dock) => { };

        #endregion

        #region Public Properties

        /// <summary>
        /// The size and position of the current monitor the window is on
        /// </summary>
        public Rectangle CurrentMonitorSize { get; set; } = new Rectangle();

        /// <summary>
        /// The size and position of the current screen in relation to the multi-screen desktop
        /// For example a second monitor on the right will have a Left position of
        /// the X resolution of the screens on the left
        /// </summary>
        public Rect CurrentScreenSize => mScreenSize;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="window">The window to monitor and correctly maximize</param>
        /// <param name="adjustSize">The callback for the host to adjust the maximum available size if needed</param>
        public WindowResizer(Window window)
        {
            mWindow = window;

            // Listen out for source initialized to setup
            mWindow.SourceInitialized += Window_SourceInitialized;

            // Monitor for edge docking
            mWindow.SizeChanged += Window_SizeChanged;
            mWindow.LocationChanged += Window_LocationChanged;
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Initialize and hook into the windows message pump
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SourceInitialized(object sender, System.EventArgs e)
        {
            // Get the handle of this window
            var handle = (new WindowInteropHelper(mWindow)).Handle;
            var handleSource = HwndSource.FromHwnd(handle);

            // If not found, end
            if (handleSource == null)
                return;

            // Hook into it's Windows messages
            handleSource.AddHook(WindowProc);
        }

        #endregion

        #region Edge Docking

        /// <summary>
        /// Monitor for moving of the window and constantly check for docked positions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            Window_SizeChanged(null, null);
        }

        /// <summary>
        /// Monitors for size changes and detects if the window has been docked (Aero snap) to an edge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Cannot calculate size until we know monitor scale
            if (mMonitorDpi == null)
                return;

            // Get window rectangle
            var top = mWindow.Top;
            var left = mWindow.Left;
            var bottom = top + mWindow.Height;
            var right = left + mWindow.Width;

            // Get window position/size in device pixels
            var windowTopLeft = new Point(left * mMonitorDpi.Value.DpiScaleX, top * mMonitorDpi.Value.DpiScaleX);
            var windowBottomRight = new Point(right * mMonitorDpi.Value.DpiScaleX, bottom * mMonitorDpi.Value.DpiScaleX);

            // Check for edges docked
            var edgedTop = windowTopLeft.Y <= (mScreenSize.Top + mEdgeTolerance) && windowTopLeft.Y >= (mScreenSize.Top - mEdgeTolerance);
            var edgedLeft = windowTopLeft.X <= (mScreenSize.Left + mEdgeTolerance) && windowTopLeft.X >= (mScreenSize.Left - mEdgeTolerance);
            var edgedBottom = windowBottomRight.Y >= (mScreenSize.Bottom - mEdgeTolerance) && windowBottomRight.Y <= (mScreenSize.Bottom + mEdgeTolerance);
            var edgedRight = windowBottomRight.X >= (mScreenSize.Right - mEdgeTolerance) && windowBottomRight.X <= (mScreenSize.Right + mEdgeTolerance);

            // Get docked position
            var dock = WindowDockPosition.Undocked;

            // Left docking
            if (edgedTop && edgedBottom && edgedLeft)
                dock = WindowDockPosition.Left;
            // Right docking
            else if (edgedTop && edgedBottom && edgedRight)
                dock = WindowDockPosition.Right;
            // Top/bottom
            else if (edgedTop && edgedBottom)
                dock = WindowDockPosition.TopBottom;
            // Top-left
            else if (edgedTop && edgedLeft)
                dock = WindowDockPosition.TopLeft;
            // Top-right
            else if (edgedTop && edgedRight)
                dock = WindowDockPosition.TopRight;
            // Bottom-left
            else if (edgedBottom && edgedLeft)
                dock = WindowDockPosition.BottomLeft;
            // Bottom-right
            else if (edgedBottom && edgedRight)
                dock = WindowDockPosition.BottomRight;

            // None
            else
                dock = WindowDockPosition.Undocked;

            // If dock has changed
            if (dock != mLastDock)
                // Inform listeners
                WindowDockChanged(dock);

            // Save last dock position
            mLastDock = dock;
        }

        #endregion

        #region Windows Message Pump

        /// <summary>
        /// Listens out for all windows messages for this window
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                // Handle the GetMinMaxInfo of the Window
                case 0x0024:/* WM_GETMINMAXINFO */
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (IntPtr)0;
        }

        #endregion

        /// <summary>
        /// Get the min/max window size for this window
        /// Correctly accounting for the taskbar size and position
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        private void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            // Get the point position to determine what screen we are on
            GetCursorPos(out POINT lMousePosition);

            // Now get the current screen
            var lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST);
            var lPrimaryScreen = MonitorFromPoint(new POINT(0,0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);

            // Try and get the current screen information
            var lCurrentScreenInfo = new MONITORINFO();
            if (GetMonitorInfo(lCurrentScreen, lCurrentScreenInfo) == false)
                return;

            // Try and get the primary screen information
            var lPrimaryScreenInfo = new MONITORINFO();
            if (GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) == false)
                return;

            // If this has changed from the last one, update the transform
            if (lCurrentScreen != mLastScreen || mMonitorDpi == null)
                mMonitorDpi = VisualTreeHelper.GetDpi(mWindow);

            // Store last know screen
            mLastScreen = lCurrentScreen;

            // Get work area sizes and rations
            var currentX = lCurrentScreenInfo.RCWork.Left - lCurrentScreenInfo.RCMonitor.Left;
            var currentY = lCurrentScreenInfo.RCWork.Top - lCurrentScreenInfo.RCMonitor.Top;
            var currentWidth = (lCurrentScreenInfo.RCWork.Right - lCurrentScreenInfo.RCWork.Left);
            var currentHeight = (lCurrentScreenInfo.RCWork.Bottom - lCurrentScreenInfo.RCWork.Top);
            var currentRatio = (float)currentWidth / (float)currentHeight;

            var primaryX = lPrimaryScreenInfo.RCWork.Left - lPrimaryScreenInfo.RCMonitor.Left;
            var primaryY = lPrimaryScreenInfo.RCWork.Top - lPrimaryScreenInfo.RCMonitor.Top;
            var primaryWidth = (lPrimaryScreenInfo.RCWork.Right - lPrimaryScreenInfo.RCWork.Left);
            var primaryHeight = (lPrimaryScreenInfo.RCWork.Bottom - lPrimaryScreenInfo.RCWork.Top);
            var primaryRatio = (float)primaryWidth / (float)primaryHeight;

            // Get min/max structure to fill with information
            var lMmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // NOTE: rcMonitor is the monitor size
            //       rcWork is the available screen size (so the area inside the taskbar start menu for example)

            // Size size limits (used by Windows when maximized)
            // relative to 0,0 being the current screens top-left corner
            //
            //  - Position
            lMmi.PointMaxPosition.X = currentX;
            lMmi.PointMaxPosition.Y = currentY;
            //
            // - Size
            lMmi.PointMaxSize.X = currentWidth;
            lMmi.PointMaxSize.Y = currentHeight;

            //
            // BUG: 
            // NOTE: I've noticed a bug which I think is Windows itself
            //       If your non-primary monitor has a greater width than your primary
            //       (or possibly due to the screen ratio's being different)
            //       then setting the max X on the monitor to the correct value causes
            //       it to scale wrong. 
            //
            //       The fix seems to be to set the max width only (height is fine)
            //       to that of the primary monitor, not the current monitor
            //        
            //       However, 1 pixel different and the size goes totally wrong again
            //       so the fix doesn't work when the taskbar is on the left or right
            //

            // Set monitor size
            CurrentMonitorSize = new Rectangle(lMmi.PointMaxPosition.X, lMmi.PointMaxPosition.Y, lMmi.PointMaxSize.X + lMmi.PointMaxPosition.X, lMmi.PointMaxSize.Y + lMmi.PointMaxPosition.Y);

            // Set min size
            var minSize = new Point(mWindow.MinWidth * mMonitorDpi.Value.DpiScaleX, mWindow.MinHeight * mMonitorDpi.Value.DpiScaleX);
            lMmi.PointMinTrackSize.X = (int)minSize.X;
            lMmi.PointMinTrackSize.Y = (int)minSize.Y;

            // Store new size
            mScreenSize = new Rect(lCurrentScreenInfo.RCWork.Left, lCurrentScreenInfo.RCWork.Top, lMmi.PointMaxSize.X, lMmi.PointMaxSize.Y);

            // Now we have the max size, allow the host to tweak as needed
            Marshal.StructureToPtr(lMmi, lParam, true);
        }

        /// <summary>
        /// Gets the current cursor position in screen coordinates relative to an entire multi-desktop position
        /// </summary>
        /// <returns></returns>
        public Point GetCursorPosition()
        {
            // Get mouse position
            GetCursorPos(out POINT lMousePosition);

            // Apply DPI scaling
            return new Point(lMousePosition.X / mMonitorDpi.Value.DpiScaleX, lMousePosition.Y / mMonitorDpi.Value.DpiScaleY);
        }
    }

    #region DLL Helper Structures

    enum MonitorOptions : uint
    {
        MONITOR_DEFAULTTONULL = 0x00000000,
        MONITOR_DEFAULTTOPRIMARY = 0x00000001,
        MONITOR_DEFAULTTONEAREST = 0x00000002
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
#pragma warning disable IDE1006 // Naming Styles
        public int CBSize = Marshal.SizeOf(typeof(MONITORINFO));
        public Rectangle RCMonitor = new Rectangle();
        public Rectangle RCWork = new Rectangle();
        public int DWFlags = 0;
#pragma warning restore IDE1006 // Naming Styles
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
#pragma warning disable IDE1006 // Naming Styles
        public int Left, Top, Right, Bottom;
#pragma warning restore IDE1006 // Naming Styles

        public Rectangle(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
#pragma warning disable IDE1006 // Naming Styles
        public POINT PointReserved;
        public POINT PointMaxSize;
        public POINT PointMaxPosition;
        public POINT PointMinTrackSize;
        public POINT PointMaxTrackSize;
#pragma warning restore IDE1006 // Naming Styles
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        /// <summary>
        /// x coordinate of point.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public int X;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// y coordinate of point.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public int Y;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// Construct a point of coordinates (x,y).
        /// </summary>
        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    #endregion
}