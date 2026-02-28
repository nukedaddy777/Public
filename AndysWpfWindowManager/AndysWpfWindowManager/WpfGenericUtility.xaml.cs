//JJ                                   Copyright Richard Andrew Holland 2026 All Rights Reserved
/* 
 * The code creates a windows for dialogs and windows that are not tied to the main window and block operations. 
 * WpfGenericUtility provides a framework onto which one places page content with a fixed height for
 * titles and window management. These can be loaded into and called from GenericUtilityFactory based on content to allow someone to
 * quickly access and focus on a window somewhere in the system if the page content is the same. 
 */
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AndysWpfWindowManager
{
    //These delegates are used to provide hot keys and mouse events
    public delegate void GenericDialogSubmitProcessDelegates(object content, WpfGenericDialog window);
    public delegate void HotKeyDelegates(object sender, KeyEventArgs e);
    public delegate void PreviewMouseDelegates(object sender, System.Windows.Input.MouseWheelEventArgs e);

    /// <summary>
    /// Interaction logic for WpfGenericUtility.xaml
    /// </summary>
    public partial class WpfGenericUtility : Window
    {
        public HotKeyDelegates? HotKey_Pressed { get; set; } = null;
        public PreviewMouseDelegates? MouseWheel_Scroll { get; set; } = null;

        public WpfGenericUtility(string title)
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            txtBoxTitle.Text = title;
            ResizeMode = ResizeMode.CanResizeWithGrip;  
        }

        public WpfGenericUtility(string title, object contentObj)
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            FrameContentObj = contentObj;
            txtBoxTitle.Text = title;
            ResizeMode = ResizeMode.CanResizeWithGrip;  // ← Add this line
        }

        public string DisplayTitle
        {
            get { return txtBoxTitle.Text; }
            set
            {
                txtBoxTitle.Text = value;
                Title = value;
            }
        }
        public Object FrameContentObj
        {
            get { return this.UtilityContentFrame.Content; }
            set { this.UtilityContentFrame.Content = value; }
        }

        public StackPanel AccessStackPanel
        {
            get { return this.stackPanel; }
            set { this.stackPanel = value; }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the DPI scale factor
            if (PresentationSource.FromVisual(this) is HwndSource source)
            {
                Matrix transformToDevice = source.CompositionTarget.TransformToDevice;
                double dpiScaleX = transformToDevice.M11;
                double dpiScaleY = transformToDevice.M22;

                // Now you have the DPI scale factors (e.g., 1.0, 1.25, 1.5)
                // You can use these values to adjust element sizes, positions, or apply LayoutTransforms.
                // For example, to adjust a control's width:
                // myControl.Width = originalWidth * dpiScaleX;
            }
        }

        // You can also handle the DpiChanged event (available in .NET 4.6.2 and later)
        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            base.OnDpiChanged(oldDpi, newDpi);

            // Perform adjustments based on the new DPI scale
            // For example, update LayoutTransform or recalculate sizes.
            // double scaleFactorX = newDpi.DpiScaleX;
            // double scaleFactorY = newDpi.DpiScaleY;
        }


        #region Windows Control Buttons Top Bar
        public void WpfGenericUtility_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else WindowState = WindowState.Maximized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            HotKey_Pressed?.Invoke(sender, e);
        }

        private void ScrollActivated(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            MouseWheel_Scroll?.Invoke(sender, e);
        }

    } //class WpfGenericUtility
}
//SDG