// jmneto - Floating Clock
// Jan 2021

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FloatingClock
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Initialize Window
            InitializeComponent();

            // Set start position
            this.Top = 10;
            this.Left = (int)(SystemParameters.PrimaryScreenWidth / 8 - this.Width / 2);

            // Avoid Docking
            this.ResizeMode = System.Windows.ResizeMode.NoResize;

            // Starts Clock
            timertick(null, null);

            // Starts Timer
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(timertick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        // Close Clock
        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        // Drag Clock
        private void OnMouseButtonLeftDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            DragMove();
        }

        // Print Clock
        private void timertick(object sender, EventArgs e)
        {
            // Make it TopMost - I do this here because I noticed that sometimes it loses the TopMost property
            this.Topmost = true;

            DateTime currentTime = DateTime.Now;

            // Digital Info
            txtClock.Text = DateTime.Now.ToString("hh:mm:ss tt\ndddd MMM,dd");

            // Analog Clock
            double hourAngle = (currentTime.Hour % 12 + currentTime.Minute / 60.0) * 30;
            double minuteAngle = (currentTime.Minute + currentTime.Second / 60.0) * 6;
            double secondAngle = currentTime.Second * 6;

            hourHand.RenderTransform = new RotateTransform(hourAngle, 100, 100);
            minuteHand.RenderTransform = new RotateTransform(minuteAngle, 100, 100);
            secondHand.RenderTransform = new RotateTransform(secondAngle, 100, 100);

            // Pixel Saver
            this.Left += dir;
            if (++cntMove >= 60)
            {
                dir *= -1;
                cntMove = 0;
            }
        }

        // Magic to allow the clock to be moved outside the screen boundaries
        // thanks to https://stackoverflow.com/revisions/331251/2
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public UInt32 flags;
        };


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        // Clock Pixel Saving Movement
        double dir = 1;
        int cntMove = 0;
    }
}